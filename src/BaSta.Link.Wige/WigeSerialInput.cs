using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace BaSta.Link.Wige;

public class WigeSerialInput : TimeSyncTaskBase, ITimeSyncInputTask
{
    public const string TypeName = "WIGE";
    
    private static readonly Regex MinuteExpression = new("([0-9]*):");
    private static readonly Regex SecondExpression = new("([0-9]*)\\.|:([0-9]*)");
    private static readonly Regex MillisecondExpression = new("\\.([0-9]*)");

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
            BaudRate = settings.GetValue("BaudRate", int.Parse, 9600),
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

            string text = Encoding.ASCII.GetString(data);

            Debug.WriteLine($"Text received: {text}");

            // The received data may contain multiple frames, therefore iterate over data
            do
            {
                // Search for the first STX/0x02 byte and ETX/0x03 bytes
                int frameStart = Array.IndexOf(data, (byte) 0x02);
                int frameEnd = Array.IndexOf(data, (byte) 0x03);

                // Frame start and end should be available and appropriately placed
                if (frameStart < 0 || frameEnd < 0 || frameEnd < frameStart)
                    break;

                byte[] timeData = new byte[frameEnd - frameStart - 2];
                Array.Copy(data, frameStart + 1, timeData, 0, timeData.Length);

                byte[] remainingData = new byte[data.Length - frameEnd - 1];
                Array.Copy(data, frameEnd + 1, remainingData, 0, remainingData.Length);
                data = remainingData;

                string timeText = Encoding.ASCII.GetString(timeData);
                timeText = timeText.Split('|').FirstOrDefault();

                int ParseRegexValue(string text, Regex regex, int paddingLeft, int paddingRight)
                {
                    Match match = regex.Match(text);

                    if (match.Groups.Count <= 1)
                        return 0;

                    string matchText = match.Groups.Skip(1).Select(x => x.Value).Aggregate((a, b) => a + b);
                    matchText = matchText.PadLeft(paddingLeft, '0');
                    matchText = matchText.PadRight(paddingRight, '0');
                    return int.Parse(matchText);
                }

                int minutes = ParseRegexValue(timeText, MinuteExpression, 2, 0);
                int seconds = ParseRegexValue(timeText, SecondExpression, 2, 0);
                int milliseconds = ParseRegexValue(timeText, MillisecondExpression, 0, 3);

                _lastUsedTime = new TimeSpan(0, 0, minutes, seconds, milliseconds);

                //Debug.Write($"Received time: {_lastUsedTime}");

                StateChanged?.Invoke(this, new DataEventArgs<TimeSpan>(_lastUsedTime));
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

    public event EventHandler<DataEventArgs<TimeSpan>> StateChanged;
}