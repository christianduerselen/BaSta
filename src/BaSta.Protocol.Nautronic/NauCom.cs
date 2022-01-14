using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BaSta.Protocol.Nautronic.Driver;
using BaSta.Protocol.Nautronic.Extra;
using BaSta.Protocol.Nautronic.Services;

namespace BaSta.Protocol.Nautronic;

public class NauCom
{
    private static int CHANNEL_INDEX_OFFSET = 11;
    public string DetectedPort = (string)null;
    public NauBeeChannels WirelessChannel = NauBeeChannels.CHANNEL_0;
    private int oldChannel = -1;
    public int Group = (int)byte.MaxValue;
    public int wirelessDBM = 0;
    private int oldDBM = -1;
    public int PauseSending = 0;
    private Protocols Protocol;
    internal List<NauBee> NB;
    private Communication Com;
    private cBoard Board;
    internal int _teamDispLength;
    internal bool PassiveListenerMode = false;
    private iPacketBuilder PacketBuilder;
    public eConsoleLocalization ConsoleLocalization;
    private bool IsClosing = false;
    private Stopwatch TimerWatch = new Stopwatch();
    private Task TimerTask;
    private bool TimerTaskRunning;
    private bool TimerTaskEnabled;
    private bool TimerTaskPaused;
    private double TaskTimer_Interval = 100.0;

    public bool IsPassiveListenerMode() => this.PassiveListenerMode;

    public void CommunicationUpdateChannel(NauBeeChannels Channel)
    {
        this.WirelessChannel = Channel;
        this.Com.CommunicationUpdateChannel(this);
    }

    public void DebugCommandSetScore(int Score) => this.Board.SetScore(Score);

    public NauCom(
        Protocols p,
        string SerialPort = "ANY",
        NauBeeChannels nWirelessChannel = NauBeeChannels.CHANNEL_0,
        bool bPassiveListenerMode = false,
        int SelectedGroup = 15,
        int teamDispLength = 24)
    {
        this.Protocol = p;
        this.PassiveListenerMode = bPassiveListenerMode;
        this._teamDispLength = teamDispLength;
        this.Board = new cBoard(this);
        this.Com = new Communication(this.Board);
        if (this.Protocol == Protocols.NG12)
        {
            this.PacketBuilder = (iPacketBuilder)new PacketBuilderNG12(this, this.Board);
        }
        else
        {
            if (this.Protocol == Protocols.NG08Direct && !SttyExecution.IsPlatformCompatible())
                throw new PlatformNotSupportedException("This serial implementation only works on platforms with stty");
            this.PacketBuilder = (iPacketBuilder)new PacketBuilderNG08(this, this.Board);
            if (SelectedGroup > 15)
                throw new Exception("illegal group for NG08");
        }
        this.Group = SelectedGroup;
        this.WirelessChannel = nWirelessChannel;
        this.Com.CommunicationUpdateChannel(this);
        this.Com.CommunicationSetDBM(this);
        this.PacketBuilder.ClearFlags();
        this.NB = new List<NauBee>();
        if (SerialPort.ToUpper() != "ANY" && SerialPort.ToUpper() != "ALL")
        {
            this.NB.Add(new NauBee(SerialPort, this.Protocol));
        }
        else
        {
            foreach (string portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                try
                {
                    this.NB.Add(new NauBee(portName, this.Protocol));
                }
                catch (Exception ex)
                {
                }
            }
        }
        if (this.Protocol == Protocols.NG08Direct)
        {
            foreach (NauBee nauBee in this.NB)
                SttyExecution.CallStty("-F " + nauBee.ListeningPort.PortName + " inpck parmrk -parenb cmspar parodd");
        }
        if (this.NB.Count == 0)
            throw new Exception("No Given Com Ports Could be opened");
        foreach (NauBee nauBee in this.NB)
        {
            NauBee N = nauBee;
            N.NauBeeEvent += PacketBuilder.NauBeeEvent;
            if (PassiveListenerMode)
                N.DataRecieved += () => DataAccepted(N);
        }
        TaskTimerStart().GetAwaiter().GetResult();
    }

    private void DataAccepted(NauBee Sender)
    {
        lock (this.NB)
        {
            foreach (NauBee nauBee in this.NB)
            {
                if (nauBee != Sender)
                    nauBee.Dispose();
                nauBee.ClearDataRecievedEvents();
            }
            this.NB.Clear();
            this.NB.Add(Sender);
        }
        this.DetectedPort = Sender.ListeningPort.PortName;
        Action portScanCompleted = this.PortScanCompleted;
        if (portScanCompleted == null)
            return;
        portScanCompleted();
    }

    internal string HandleRightToLeft(IEnumerable<char> source)
    {
        string empty = string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (char ch in source)
        {
            if (this.IsRightToLeft(ch) || !string.IsNullOrEmpty(empty) && (char.IsDigit(ch) || char.IsWhiteSpace(ch)))
            {
                empty += ch.ToString();
            }
            else
            {
                if (empty != string.Empty)
                    stringBuilder.Insert(0, this.Reverse(empty));
                stringBuilder.Append(ch);
                empty = string.Empty;
            }
        }
        if (empty != string.Empty)
            stringBuilder.Insert(0, this.Reverse(empty));
        return stringBuilder.ToString();
    }

    internal bool IsRightToLeft(char character) => character >= 'Ԁ' && character <= '\u05FF' || character >= '\u0600' && character <= 'ۿ' || character >= 'ݐ' && character <= 'ݿ' || character >= 'ﭐ' && character <= 'ﰿ' || character >= 'ﹰ' && character <= 'ﻼ';

    private string Reverse(string source) => new string(((IEnumerable<char>)source.ToCharArray()).Reverse<char>().ToArray<char>());

    private async Task TaskTimerStart()
    {
        TimerTaskRunning = true;
        TimerTask = Task.Run(TaskTimer);
    }

    private void TaskTimerStop()
    {
        this.TimerTaskRunning = false;
        this.TimerTask = (Task)null;
    }

    public void TaskTimerPause() => this.TimerTaskPaused = true;

    public void TaskTimerResume() => this.TimerTaskPaused = false;

    private async Task TaskTimer()
    {
        this.TimerWatch.Restart();
        while (!this.IsClosing && this.TimerTaskRunning)
        {
            if (this.TimerTaskPaused)
            {
                await Task.Delay(10);
            }
            else
            {
                this.TaskTimer_Interval = this.Protocol != Protocols.NG12 ? (double)this.PacketBuilder.Run() : (this.PacketBuilder.Run() == 0 ? 40.0 : 30.0);
                while ((double)this.TimerWatch.ElapsedMilliseconds < this.TaskTimer_Interval)
                {
                    if (this.PassiveListenerMode)
                        await Task.Delay(1);
                    else
                        Thread.SpinWait(100);
                }
            }
            this.TimerWatch.Restart();
        }
    }

    public void Stop()
    {
        this.TaskTimerStop();
        this.Board.BoardTimer.Stop();
        foreach (NauBee nauBee in this.NB)
            nauBee.Dispose();
    }

    private void NauComTask(object sender)
    {
        this.TimerTaskEnabled = false;
        this.TaskTimer_Interval = (double)this.PacketBuilder.Run();
        if (this.IsClosing)
            return;
        this.TimerTaskEnabled = true;
    }

    public void SetDigit(
        int Adress,
        int value,
        bool flash = false,
        bool ExtraVisible = false,
        bool ExtraFlashing = false,
        PriorityLevels Prio = PriorityLevels.Normal)
    {
        T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(0U, (ushort)Adress);
        uint num = (uint)value;
        if (flash)
            num |= 16U;
        if (ExtraVisible)
            num |= 32U;
        if (ExtraFlashing)
            num |= 64U;
        tableIndex.priority = 1U;
        if ((int)tableIndex.datauint != (int)num)
            tableIndex.updates = 3U;
        tableIndex.datauint = num;
    }

    public void GetDigit(
        int Adress,
        out int value,
        out bool flash,
        out bool ExtraVisible,
        out bool ExtraFlashing)
    {
        T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(0U, (ushort)Adress);
        value = (int)tableIndex.datauint & 15;
        flash = (tableIndex.datauint & 16U) > 0U;
        ExtraVisible = (tableIndex.datauint & 32U) > 0U;
        ExtraFlashing = (tableIndex.datauint & 64U) > 0U;
    }

    public void SetDot(int Adress, int Bitfield, int FlashBitfield)
    {
        T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(1U, (ushort)Adress);
        if ((int)tableIndex.datauint != Bitfield)
            tableIndex.updates = 3U;
        if ((int)tableIndex.flashuint != FlashBitfield)
            tableIndex.updates = 3U;
        tableIndex.datauint = (uint)Bitfield;
        tableIndex.flashuint = (uint)FlashBitfield;
        tableIndex.priority = 1U;
    }

    public void GetDot(int Adress, out int Bitfield, out int FlashBitfield)
    {
        T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(1U, (ushort)Adress);
        Bitfield = (int)tableIndex.datauint;
        FlashBitfield = (int)tableIndex.flashuint;
    }

    public void SetRelay(int Adress, int Duration) => this.Board.Board_RelayUpdate(Adress, Duration);

    public void GetRelay(int Adress, out int Duration)
    {
        T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(2U, (ushort)Adress);
        Duration = (int)tableIndex.datauint;
    }

    public void SetText(int Adress, string Text)
    {
        Dictionary<int, char> conversionDictionary = NauASCIIDictionaries.WesternConversionDictionary;
        if (this.ConsoleLocalization == eConsoleLocalization.ARABIC)
            conversionDictionary = NauASCIIDictionaries.ArabicConversionDictionary;
        string text1 = Text.Aggregate("", (current, t) =>
        {
            char ch = (char) conversionDictionary.FirstOrDefault(x => x.Value == t).Key;
            if (ch == char.MinValue)
                ch = t;
            return current + ch;
        });
        Board.Board_TextLineUpdate((byte)Adress, text1);
    }

    public void ClearAll(int Value)
    {
        for (uint TableID = 0; TableID < 3U; ++TableID)
        {
            for (int index = 0; index < (int)byte.MaxValue; ++index)
            {
                T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(TableID, (ushort)index);
                int num;
                switch (TableID)
                {
                    case 0:
                        num = Value == (int)byte.MaxValue ? 1 : 0;
                        break;
                    case 2:
                        tableIndex.datauint = 0U;
                        goto label_8;
                    default:
                        num = 0;
                        break;
                }
                tableIndex.datauint = num == 0 ? (Value != 254 ? (uint)Value : (TableID != 0U ? 0U : 15U)) : 40U;
                label_8:
                tableIndex.flashuint = 0U;
                tableIndex.priority = 0U;
                tableIndex.updates = 0U;
            }
        }
        if (this.MCB_CLEAR_ALL == null)
            return;
        this.MCB_CLEAR_ALL((object)this, (EventArgs)null);
    }

    internal void SendGameLogUpdateEvent(int[] Data)
    {
        if (this.GameLogUpdate == null)
            return;
        this.GameLogUpdate(Data);
    }

    internal void SendNauBeeEvent(int[] data)
    {
        if (this.NauBeeEvent == null || !this.PassiveListenerMode)
            return;
        this.NauBeeEvent(data);
    }

    internal void SendNauBeeRemoteEvent(int[] data)
    {
        if (this.NauBeeRemoteEvent == null)
            return;
        this.NauBeeRemoteEvent(data[6], data[7]);
    }

    internal void SendNauBeeDigitDotRelayUpdateEvent(EventType Type, int[] Adresses)
    {
        if (this.NauBeeDigitDotRelayUpdate == null)
            return;
        this.NauBeeDigitDotRelayUpdate(Type, Adresses);
    }

    internal void SendNauBeeTextUpdateEvent(int adress, string text)
    {
        if (this.NauBeeTextUpdate == null)
            return;
        this.NauBeeTextUpdate(adress, text);
    }

    public event Action PortScanCompleted;

    public event EventHandler MCB_CLEAR_ALL;

    public event NauCom.NauBeeEventHandler NauBeeEvent;

    public event NauCom.NauBeeRemoteEventHandler NauBeeRemoteEvent;

    public event NauCom.NauBeeDataUpdateEventHandler NauBeeDigitDotRelayUpdate;

    public event NauCom.NauBeeTextUpdateEventHandler NauBeeTextUpdate;

    public event NauCom.GameLogUpdateEventHandler GameLogUpdate;

    public delegate void GameLogUpdateEventHandler(int[] Data);

    public delegate void NauBeeDataUpdateEventHandler(EventType Type, int[] Adresses);

    public delegate void NauBeeTextUpdateEventHandler(int Adress, string Text);

    public delegate void NauBeeEventHandler(int[] Data);

    public delegate void NauBeeRemoteEventHandler(int Adress, int Data);
}