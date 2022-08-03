using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Xunit;

namespace BaSta.Protocol.Stramatel.Tests;

public class StramatelMessageSenderTests
{
    [Fact]
    public void StramatelMessageSender_SendGameTimeZeroTest()
    {
        SerialPort port = new()
        {
            PortName = "COM6",
            BaudRate = 19200,
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            Handshake = Handshake.None
        };

        byte[] sendBytes =
        {
            0xF8,
            0x33,
            0x20,
            0x20, // Ball posession
            0x20, // Time digit 1
            0x20, // Time digit 2
            0x20, // Time digit 3
            0x20, // Time digit 4
            0x20, // Home points digit 1
            0x20, // Home points digit 2
            0x30, // Home points digit 3
            0x20, // Guest points digit 1
            0x20, // Guest points digit 2
            0x30, // Guest points digit 3
            0x31, // Period
            0x30, // Home fouls
            0x30, // Guest fouls
            0x30, // Home timeouts
            0x30, // Guest timeouts
            0x20, // Horn
            0x20, // Start/stop time
            0x20, // Timeout time digit 1
            0x30, // Home fouls player 1
            0x30, // Home fouls player 2
            0x30, // Home fouls player 3
            0x30, // Home fouls player 4
            0x30, // Home fouls player 5
            0x30, // Home fouls player 6
            0x30, // Home fouls player 7
            0x30, // Home fouls player 8
            0x30, // Home fouls player 9
            0x30, // Home fouls player 10
            0x30, // Home fouls player 11
            0x30, // Home fouls player 12
            0x30, // Guest fouls player 1
            0x30, // Guest fouls player 2
            0x30, // Guest fouls player 3
            0x30, // Guest fouls player 4
            0x30, // Guest fouls player 5
            0x30, // Guest fouls player 6
            0x30, // Guest fouls player 7
            0x30, // Guest fouls player 8
            0x30, // Guest fouls player 9
            0x30, // Guest fouls player 10
            0x30, // Guest fouls player 11
            0x30, // Guest fouls player 12
            0x20, // Timeout time digit 2
            0x20, // Timeout time digit 3
            0x20, // Shot clock digit 1
            0x20, // Shot clock digit 2
            0x20, // Shot clock horn
            0x20, // Start/stop shot clock
            0x31, // Show shot clock
            0x0D
        };

        port.Open();

        TimeSpan span = TimeSpan.FromSeconds(5);

        while (true)
        {
            string shotClockText = span.TotalSeconds.ToString().PadLeft(2, '0');
            byte[] shotClockBytes = Encoding.ASCII.GetBytes(shotClockText);
            sendBytes[48] = shotClockBytes[0];
            sendBytes[49] = shotClockBytes[1];

            if (span == TimeSpan.Zero)
                sendBytes[50] = 0x31;
            else
                sendBytes[50] = 0x20;

            port.Write(sendBytes, 0, sendBytes.Length);

            Thread.Sleep(100);
            
            if (span != TimeSpan.Zero)
                span -= TimeSpan.FromMilliseconds(100);
        }
    }
}