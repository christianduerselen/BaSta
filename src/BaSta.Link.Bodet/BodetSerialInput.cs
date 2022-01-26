using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace BaSta.Link.Bodet;

public class BodetSerialInput : TimeSyncTaskBase, ITimeSyncInputTask
{
    public const string TypeName = "Bodet";

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

            // The received data may contain multiple frames, therefore iterate over data
            do
            {
                // Search for the first SOH/0x01 byte and ETX/0x03 bytes
                int frameStart = Array.IndexOf(data, (byte) 0x01);
                int frameEnd = Array.IndexOf(data, (byte) 0x03);

                // Frame start and end should be available, appropriately placed and there should be an additional LRC byte
                if (frameStart < 0 || frameEnd < 0 || frameEnd < frameStart + 2 || frameEnd >= data.Length)
                    break;

                // Message data from address (byte #2) to ETX
                byte[] messageData = new byte[frameEnd - frameStart - 1];
                Array.Copy(data, frameStart + 1, messageData, 0, messageData.Length);
                // Message LRC is from byte #2 to 
                byte? messageLrc = data.Length < frameEnd ? data[frameEnd + 1] : null;
                
                // Remaining data starts after ETX (if LRC available it's included)
                byte[] remainingData = new byte[data.Length - frameEnd - 1];
                Array.Copy(data, frameEnd + 1, remainingData, 0, remainingData.Length);
                data = remainingData;

                // Verify LRC
                //if (messageLrc != CalculateLrc(messageData))
                //    continue;

                // Only message types 18 (main timer) and 36 (main timer tenth of second)
                bool isMessageType18 = messageData[3] == 0x31 && messageData[4] == 0x38;
                bool isMessageType36 = messageData[3] == 0x33 && messageData[4] == 0x36;

                if (!isMessageType18 && !isMessageType36)
                    continue;

                if (isMessageType18)
                {
                    string timeText = Encoding.ASCII.GetString(messageData, 7, 4);
                    if (timeText[2] == 'D')
                    {
                        timeText = timeText.Replace('D', '.');
                        double seconds = double.Parse(timeText, NumberStyles.Float, CultureInfo.InvariantCulture);
                        
                        _lastUsedTime = TimeSpan.FromSeconds(seconds);
                    }
                    else
                    {
                        int minutes = int.Parse(timeText.Substring(0, 2));
                        int seconds = int.Parse(timeText.Substring(2, 2));
                        _lastUsedTime = new TimeSpan(0, minutes, seconds);
                    }
                }

                if (isMessageType36)
                {
                    string timeText = Encoding.ASCII.GetString(messageData, 5, 3);
                    double seconds = double.Parse(timeText);
                    seconds /= 10;

                    _lastUsedTime = TimeSpan.FromSeconds(seconds);
                }

                Debug.WriteLine($"New time: {_lastUsedTime}");
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

    private byte CalculateLrc(byte[] messageData)
    {
        byte checkLrc = 0x00;
        
        foreach (byte messageByte in messageData)
            checkLrc ^= messageByte;
        
        checkLrc &= 0x7f;
        
        if (checkLrc < 0x20)
            checkLrc += 0x20;
        
        return checkLrc;
    }
}