using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using BaSta.TimeSync.Helper;

namespace BaSta.TimeSync.Input.Stramatel;

internal class StramatelSerialInput : TimeSyncTaskBase, ITimeSyncInputTask
{
    internal const string TypeName = "Stramatel";

    private readonly byte[] _receiveBuffer = new byte[ushort.MaxValue];
    private SerialPort _port;
    private TimeSpan _lastUsedTime;

    protected override void LoadSettings(ITimeSyncSettingsGroup settings)
    {
        _port = new SerialPort
        {
            PortName = settings.GetValue("PortName", s =>
            {
                if (string.IsNullOrWhiteSpace(s))
                    s = SerialPortSelector.Select(Name);

                if (!SerialPort.GetPortNames().Contains(s))
                    throw new Exception($"Serial port '{s}' not available on this system!");

                return s;
            }, null),
            BaudRate = settings.GetValue("BaudRate", int.Parse, 19200),
            Parity = settings.GetValue("Parity", Enum.Parse<Parity>, Parity.None),
            DataBits = settings.GetValue("DataBits", int.Parse, 8),
            StopBits = settings.GetValue("StopBits", Enum.Parse<StopBits>, StopBits.One),
            Handshake = settings.GetValue("Handshake", Enum.Parse<Handshake>, Handshake.None)
        };
    }

    protected override void OnStartSync()
    {
        _port.Open();
    }

    protected override void OnProcess(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Thread.Sleep(1);

            if (_port.BytesToRead <= 0)
                continue;

            int length = _port.Read(_receiveBuffer, 0, _receiveBuffer.Length);

            byte[] data = new byte[length];
            Array.Copy(_receiveBuffer, 0, data, 0, data.Length);

            // The received data may contain multiple frames, therefore iterate over data
            do
            {
                // Search for the first 0xF8 byte
                int frameStart = Array.IndexOf(data, (byte) 0xF8);

                // Frame start should be available, there should be 53 data bytes afterwards and 0x0D as the terminator
                if (frameStart < 0 || data.Length < frameStart + 54 || data[frameStart+53] != 0x0D)
                    break;

                byte[] timeData = new byte[4];
                Array.Copy(data, frameStart + 4, timeData, 0, timeData.Length);

                byte[] remainingData = new byte[data.Length - frameStart - 54];
                Array.Copy(data, frameStart + 54, remainingData, 0, remainingData.Length);
                data = remainingData;

                // Sanity check - bytes 4 to 8 should either be 0x20 (Space), 0x3A (:) or 0x30 (0) to 0x39 (8)
                bool CheckValueRange(params byte[] values)
                {
                    foreach (byte value in values)
                        if (value != 0x20 && value != 0x3A && (value < 0x30 || value > 0x39))
                            return false;
                    return true;
                }
                if (!CheckValueRange(timeData[0], timeData[1], timeData[2], timeData[3]))
                    continue;

                string timeText = Encoding.ASCII.GetString(timeData);

                // Everything after a ':' must be zeroed
                if (timeText.IndexOf(':') >= 0)
                    timeText = timeText.Substring(0, timeText.IndexOf(':') + 1).PadRight(4, '0');

                // If the last char is a space (0x20 | " "), shift the text by 2 digits because of sub-minute display
                if (timeText[^1] != ' ')
                    timeText += "  ";

                // If a character is a ':' this means first two digits are *10 minutes
                timeText = timeText.Replace(":", "00");

                // Pad to 7 digits (3x minutes, 2x seconds, 2x milliseconds)
                timeText = timeText.PadLeft(7, ' ');

                // Spaces are zeros
                timeText = timeText.Replace(' ', '0');

                Debug.WriteLine($"Received time: {timeText}");

                int minutes = int.Parse(timeText.Substring(0, 3));
                int seconds = int.Parse(timeText.Substring(3, 2));
                int milliseconds = int.Parse(timeText.Substring(5, 2)) * 10;
                _lastUsedTime = new TimeSpan(0, 0, minutes, seconds, milliseconds);

                Debug.WriteLine($"Received time: {_lastUsedTime}");
                StateChanged?.Invoke(this, null);
            } while (true);
        }
    }

    protected override void OnStopSync()
    {
        _port.Close();
    }

    public TimeSpan Pull()
    {
        return _lastUsedTime;
    }

    public event EventHandler StateChanged;
}