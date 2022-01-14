using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using BaSta.Protocol.Nautronic.Extra;

namespace BaSta.Protocol.Nautronic.Driver;

internal class NauBee
{
    private static byte SOF = 113;
    private static byte ESC = 123;
    private static byte ACK = 117;
    private List<byte> UART0txBuf;
    internal SerialPort ListeningPort;
    private bool SOFDetected;
    private Thread Poller;
    private bool IsClosing;
    private Protocols _protocol;
    private Stopwatch sw = new();
    private List<int> DataBuffer = new();
    private bool SOFFound;
    private int LastByte;
    private int ExpectedLen;
    private bool DoubleByte;
    private byte CRCM;
    private byte CRCX;
    private List<int> DirectConverterBytes = new();
    private bool NextIsParity;

    public event NauBeeEventHandler NauBeeEvent;

    public event Action DataRecieved;

    public void Dispose()
    {
        try
        {
            if (!ListeningPort.IsOpen)
                return;
            ListeningPort.ReceivedBytesThreshold = int.MaxValue;
            IsClosing = true;
            var num = 0;
            while (Poller.IsAlive)
            {
                if (num > 20)
                    try
                    {
                        Poller.Abort();
                    }
                    catch
                    {
                    }

                Thread.Sleep(20);
                ++num;
            }

            NauBeeEvent = (NauBeeEventHandler)null;
            if (ListeningPort.IsOpen)
                ListeningPort.Close();
        }
        catch
        {
        }
    }

    public NauBee(string COM, Protocols p)
    {
        _protocol = p;
        UART0txBuf = new List<byte>();
        switch (p)
        {
            case Protocols.NG08:
                ListeningPort = new SerialPort(COM, 57600, Parity.None, 8, StopBits.One);
                break;
            case Protocols.NG08Direct:
                ListeningPort = new SerialPort(COM, 57600, Parity.None, 8, StopBits.Two);
                break;
            default:
                ListeningPort = new SerialPort(COM, 115200, Parity.None, 8, StopBits.One);
                break;
        }

        ListeningPort.Handshake = Handshake.None;
        try
        {
            ListeningPort.Open();
            Log("Succesfully Opened: " + COM);
        }
        catch
        {
            Log("Failed to Open: " + COM);
        }

        Poller = new Thread(new ThreadStart(PollData))
        {
            IsBackground = true,
            Name = "PollData " + COM
        };
        Poller.Start();
    }

    private static int AddPacketByte(out List<byte> DataToAdd, byte dataByte, ref int Crc)
    {
        DataToAdd = new List<byte>();
        var num = 1;
        if ((int)dataByte == (int)SOF || (int)dataByte == (int)ESC)
        {
            num = 2;
            DataToAdd.Add(ESC);
            DataToAdd.Add((byte)((uint)dataByte ^ (uint)byte.MaxValue));
        }
        else
        {
            DataToAdd.Add(dataByte);
        }

        Crc = CRC.crc8(Crc, (int)dataByte);
        return num;
    }

    public void ClearDataRecievedEvents()
    {
        DataRecieved = (Action)null;
    }

    public void NauNetNG08TextSend(ref uint[] ReadBuffer, byte ResponseSize)
    {
        var maxValue = (int)byte.MaxValue;
        byte num1 = 0;
        var num2 = 0;
        UART0txBuf.Clear();
        UART0txBuf.Add(SOF);
        UART0txBuf.Add((byte)((uint)ResponseSize + 1U));
        List<byte> DataToAdd;
        for (var index = 0; index < (int)ResponseSize; ++index)
        {
            num2 += AddPacketByte(out DataToAdd, (byte)ReadBuffer[index], ref maxValue);
            UART0txBuf.AddRange((IEnumerable<byte>)DataToAdd);
        }

        for (var index = 0; index < (int)ResponseSize; ++index)
            num1 ^= UART0txBuf[2];
        var num3 = num2 + AddPacketByte(out DataToAdd, (byte)((uint)num1 | 128U), ref maxValue);
        UART0txBuf.AddRange((IEnumerable<byte>)DataToAdd);
        var num4 = num3 + AddPacketByte(out DataToAdd, (byte)maxValue, ref maxValue);
        UART0txBuf.AddRange((IEnumerable<byte>)DataToAdd);
        if (!ListeningPort.IsOpen)
            return;
        ListeningPort.Write(UART0txBuf.ToArray(), 0, UART0txBuf.Count);
    }

    public void NauBeeSend(ref uint[] ReadBuffer, byte ResponseSize)
    {
        var num1 = 0;
        var maxValue = (int)byte.MaxValue;
        try
        {
            int num2;
            if (_protocol == Protocols.NG12)
            {
                var num3 = 0;
                UART0txBuf.Clear();
                UART0txBuf.Add(byte.MaxValue);
                UART0txBuf.Add(SOF);
                UART0txBuf.Add(ResponseSize);
                List<byte> DataToAdd;
                for (var index = 0; index < (int)ResponseSize; ++index)
                {
                    num3 += AddPacketByte(out DataToAdd, (byte)ReadBuffer[index], ref maxValue);
                    UART0txBuf.AddRange((IEnumerable<byte>)DataToAdd);
                }

                num2 = num3 + AddPacketByte(out DataToAdd, (byte)maxValue, ref maxValue);
                UART0txBuf.AddRange((IEnumerable<byte>)DataToAdd);
                SOFDetected = false;
                if (!ListeningPort.IsOpen)
                    return;
                try
                {
                    ListeningPort.Write(UART0txBuf.ToArray(), 0, UART0txBuf.Count);
                }
                catch
                {
                    Dispose();
                }
            }
            else if (ListeningPort.IsOpen)
            {
                UART0txBuf.Clear();
                UART0txBuf.Add(SOF);
                UART0txBuf.Add((byte)((uint)ResponseSize + 1U));
                List<byte> DataToAdd;
                for (var index = 0; index < (int)ResponseSize; ++index)
                {
                    num1 += AddPacketByte(out DataToAdd, (byte)ReadBuffer[index], ref maxValue);
                    UART0txBuf.AddRange((IEnumerable<byte>)DataToAdd);
                }

                var num4 = (byte)ReadBuffer[0];
                if (UART0txBuf[2] < (byte)10 || UART0txBuf[2] >= (byte)70 && UART0txBuf[2] < (byte)90)
                {
                    for (var index = 1; index < (int)ResponseSize; ++index)
                        num4 ^= (byte)ReadBuffer[index];
                }
                else
                {
                    if (UART0txBuf[2] == (byte)128)
                        UART0txBuf[2] = (byte)0;
                    for (var index = 1; index < (int)ResponseSize; ++index)
                        num4 *= (byte)ReadBuffer[index];
                }

                var num5 = num1 + AddPacketByte(out DataToAdd, (byte)((uint)num4 | 128U), ref maxValue);
                UART0txBuf.AddRange((IEnumerable<byte>)DataToAdd);
                num2 = num5 + AddPacketByte(out DataToAdd, (byte)maxValue, ref maxValue);
                UART0txBuf.AddRange((IEnumerable<byte>)DataToAdd);
                ListeningPort.Write(UART0txBuf.ToArray(), 0, UART0txBuf.Count);
            }
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }
    }

    private void Log(string data)
    {
        lock (DataBuffer)
        {
            try
            {
                File.AppendAllText("log.txt", DateTime.Now.ToString() + ": " + data + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine((object)ex);
            }
        }

        Console.WriteLine(data);
    }

    private void PollData()
    {
        if (ListeningPort == null)
        {
            Log("PollData Started for port: NULL");
        }
        else
        {
            Log("PollData Started for port: " + ListeningPort.PortName);
            Log("Port " + ListeningPort.PortName + " Status: " + ListeningPort.IsOpen.ToString());
        }

        try
        {
            while (!IsClosing && ListeningPort.IsOpen)
                try
                {
                    while (!IsClosing && ListeningPort.BytesToRead > 0)
                    {
                        var DataByte = ListeningPort.ReadByte();
                        if (_protocol == Protocols.NG08)
                            RecieveByteNG08(DataByte);
                        else if (_protocol == Protocols.NG08Direct)
                            RecieveByteNG08Direct(DataByte);
                        else
                            RecieveByteNG12(DataByte);
                    }

                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Log("IO ERROR!!" + ex?.ToString());
                    Log("Thread Closing:" + IsClosing.ToString());
                    Log("Port " + ListeningPort.PortName + " Status: " + ListeningPort.IsOpen.ToString());
                }
        }
        catch (Exception ex)
        {
            Log("BAD IO ERROR!!" + ex?.ToString());
        }

        Log("COM " + ListeningPort.PortName + " CLOSED");
    }

    private void RecieveByteNG08Direct(int DataByte)
    {
        if (DirectConverterBytes.Count == 0 && DataByte == (int)byte.MaxValue)
        {
            DirectConverterBytes.Add(DataByte);
        }
        else if (DirectConverterBytes.Count == 1)
        {
            switch (DataByte)
            {
                case 0:
                    NextIsParity = true;
                    DirectConverterBytes.Clear();
                    break;
                case (int)byte.MaxValue:
                    DirectConverterBytes.Clear();
                    CheckAndAddByte((int)byte.MaxValue);
                    break;
                default:
                    Console.WriteLine("ERROR - middle of message?");
                    DirectConverterBytes.Clear();
                    Debugger.Break();
                    break;
            }
        }
        else if (DirectConverterBytes.Count >= 2)
        {
            Console.WriteLine("ERROR - end of message?");
            DirectConverterBytes.Clear();
        }
        else
        {
            CheckAndAddByte(DataByte);
        }
    }

    private void CheckAndAddByte(int DataByte)
    {
        var flag = false;
        var num = 0;
        for (var index = 0; index < 8; ++index)
            if ((DataByte & (1 << index)) > 0)
                ++num;
        if (num % 2 == 0)
            flag = true;
        if (!NextIsParity)
        {
            RecieveByteNG08(128 | (DataByte & 15), true);
            RecieveByteNG08(144 | (DataByte >> 4), true);
        }
        else if ((DataByte & 128) > 0)
        {
            RecieveByteNG08(160 | (DataByte & 15));
            RecieveByteNG08(176 | (DataByte >> 4));
        }
        else
        {
            RecieveByteNG08(DataByte);
        }

        NextIsParity = false;
    }

    private void RecieveByteNG08(int DataByte, bool Direct = false)
    {
        var flag = false;
        if (DoubleByte)
        {
            DoubleByte = false;
            if ((LastByte & 240) == 128 && (DataByte & 240) == 144)
            {
                SOFFound = true;
                flag = true;
                DataBuffer.Clear();
                var num = ((DataByte & 15) << 4) + (LastByte & 15);
                CRCM = (byte)((uint)(byte)LastByte * (uint)(byte)DataByte);
                CRCX = (byte)((uint)(byte)LastByte ^ (uint)(byte)DataByte);
                if (Direct)
                {
                    CRCM = (byte)num;
                    CRCX = (byte)num;
                }

                DataBuffer.Add(num);
            }
            else if ((LastByte & 240) == 160 && (DataByte & 240) == 176)
            {
                var num = (byte)(((DataByte & 15) << 4) + (LastByte & 15));
                if (((int)num | 128) == ((int)CRCM | 128) || (int)num == ((int)CRCX | 128))
                {
                    if (NauBeeEvent != null)
                        NauBeeEvent(DataBuffer.ToArray());
                    var dataRecieved = DataRecieved;
                    if (dataRecieved != null)
                        dataRecieved();
                }
                else
                {
                    Log(ListeningPort.PortName + " CRC ERROR!: " + string.Join(",",
                        DataBuffer.Select<int, string>((Func<int, string>)(x => x.ToString("X2")))));
                    Log("Expected CRCM:" + CRCM.ToString("X2") + " OR CRCX" + CRCX.ToString("X2") + " GOT: " +
                        num.ToString("X2"));
                }

                SOFFound = false;
            }
            else if (SOFFound)
            {
                Log(ListeningPort.PortName + " DOUBLE BYTE ERROR! Prev:" + LastByte.ToString("X2") + " Current: " +
                    DataByte.ToString("X2"));
            }
        }

        if (!flag && (DataByte & 240) == 128 || (DataByte & 240) == 160)
        {
            DoubleByte = true;
            flag = true;
        }

        if (!flag && SOFFound)
        {
            DataBuffer.Add(DataByte);
            CRCM *= (byte)DataByte;
            CRCX ^= (byte)DataByte;
        }

        LastByte = DataByte;
    }

    private void RecieveByteNG12(int DataByte)
    {
        if (DataByte == 113)
        {
            SOFFound = true;
            DataBuffer.Clear();
        }
        else if (SOFFound && DataByte != 123)
        {
            if (LastByte == 123)
                DataByte ^= (int)byte.MaxValue;
            if (DataBuffer.Count == 0)
                ExpectedLen = DataByte + 1;
            DataBuffer.Add(DataByte);
            if (DataBuffer.Count == ExpectedLen + 1)
            {
                var crc8in = (int)byte.MaxValue;
                int index;
                for (index = 1; index < DataBuffer.Count - 1; ++index)
                    crc8in = CRC.crc8(crc8in, DataBuffer[index]) & (int)byte.MaxValue;
                if (DataBuffer[index] == crc8in)
                {
                    DataBuffer.RemoveAt(DataBuffer.Count - 1);
                    if (NauBeeEvent != null)
                        NauBeeEvent(DataBuffer.ToArray());
                    if (DataBuffer[1] > 0)
                    {
                        var dataRecieved = DataRecieved;
                        if (dataRecieved != null)
                            dataRecieved();
                    }
                }

                SOFFound = false;
            }

            DataByte = 0;
        }

        LastByte = DataByte;
    }

    public delegate void NauBeeEventHandler(int[] Data);
}