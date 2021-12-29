using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using BaSta.TimeSync.Helper;

namespace BaSta.TimeSync.Input.Bodet;

internal class BodetSerialInput : TimeSyncTaskBase, ITimeSyncInputTask
{
    internal const string TypeName = "Bodet";

    private SerialPort _port;

    protected override void LoadSettings(ITimeSyncSettingsGroup settings)
    {
        // PortName=COM1
        // BaudRate=19200
        // # 0 = None | 1 = Odd | 2 = Even | 3 = Mark | 4 = Space
        // Parity=0
        // DataBits=8
        // # 1 = One | 2 = Two | 3 = OnePointFive
        // StopBits=1
        // # 0 = None | 1 = XOnXOff | 2 = RequestToSend | 3 = RequestToSendXOnXOff
        // Handshake=0

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
        throw new NotImplementedException();
    }

    protected override void OnProcess(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    protected override void OnStopSync()
    {
        throw new NotImplementedException();
    }

    public TimeSpan Pull()
    {
        throw new NotImplementedException();
    }

    public event EventHandler StateChanged;
}