using System.Collections.Generic;
using BaSta.Protocol.Nautronic.Driver;
using BaSta.Protocol.Nautronic.Extra;

namespace BaSta.Protocol.Nautronic.Services;

internal class PacketBuilderNG08 : iPacketBuilder
{
    internal uint[,] FlagList = new uint[3, 32];
    internal uint PacketSize;
    internal uint[] PacketBuffer = new uint[128];

    private readonly NauCom _instance;
    private cBoard _board;
    private int PacketBuilderRun_Step;
    private int[] pIndex = { 0, 1 };
    private int[] LastClockSecValue = new int[2];
    private int[] LastClock10SecValue = new int[2];
    private int[] VirtualSecCounter = new int[2];
    private eSports sportSelect = eSports.Basketball;
    private bool shotClockExtraDigitEnabled = true;
    private int[] SendFlags = new int[NG08Tables.NG08Entries];
    private int[] SendMergeFlags = new int[NG08Tables.NG08Entries];
    private static List<int> lastAdresses = new() { 0 };
    private byte[] commandBuffer = new byte[128];
    private int commandSize = 0;
    private int commandRetryCount = 3;
    private int[] SeqList = new int[40];
    private int SequenceNr = 0;
    private int WasMultiCast = 0;
    private int BlockNo = 0;
    private int TextNo = 0;
    private int TxtCommand = 0;
    private int TxtSeq = 0;
    private int TxtEffect = 0;
    private int FinishProgress = 0;
    private int FinishProgressStep = 0;
    private int TextSeqProgress = 0;

    public PacketBuilderNG08(NauCom instance, cBoard instance2)
    {
        _instance = instance;
        _board = instance2;
    }

    public int Run()
    {
        if (_instance.PassiveListenerMode)
            return 15;
            
        PacketSize = 0U;
        int num1 = 0;
        int num2 = 0;
        int index1 = 0;
        int num3 = 0;
        PacketBuilderRun_Step = (PacketBuilderRun_Step + 1) % 6;

        if (PacketBuilderRun_Step == 2 || PacketBuilderRun_Step == 4)
        {
            CheckCommandBufferNG08();
            if (PacketSize > 0U)
            {
                lock (_instance.NB)
                {
                    foreach (NauBee nauBee in _instance.NB)
                        nauBee.NauBeeSend(ref PacketBuffer, (byte) PacketSize);
                }
            }
        }

        if (PacketBuilderRun_Step == 0 || PacketBuilderRun_Step == 1)
        {
            index1 = 12;
            int num4 = PacketBuilderRun_Step == 0 ? 10 : 40;
            for (int index2 = 0; index2 < NG08Tables.NG08Entries; ++index2)
            {
                if (NG08Tables.NG08StandardBoardTable[index2].DestinationCommand == index1 && NG08Tables.NG08StandardBoardTable[index2].DestinationAddress == num4)
                {
                    if (this.PacketBuilderRun_Step == 0)
                        ++this.VirtualSecCounter[this.PacketBuilderRun_Step];
                    T_BOARD_ADDRESS tableIndex1 = this._board.Board_GetTableIndex((uint)NG08Tables.NG08StandardBoardTable[index2 + 3].OriginalTable, (ushort)NG08Tables.NG08StandardBoardTable[index2 + 3].OriginalIndex);
                    if ((long)this.LastClockSecValue[this.PacketBuilderRun_Step] == (long)tableIndex1.datauint)
                        num3 = 1;
                    this.LastClockSecValue[this.PacketBuilderRun_Step] = (int)tableIndex1.datauint;
                    T_BOARD_ADDRESS tableIndex2 = this._board.Board_GetTableIndex((uint)NG08Tables.NG08StandardBoardTable[index2 + 1].OriginalTable, (ushort)NG08Tables.NG08StandardBoardTable[index2 + 1].OriginalIndex);
                    if ((long)this.LastClock10SecValue[this.PacketBuilderRun_Step] != (long)tableIndex2.datauint)
                        num3 = 0;
                    this.LastClock10SecValue[this.PacketBuilderRun_Step] = (int)tableIndex2.datauint;
                    if (num3 > 0)
                    {
                        if (this.VirtualSecCounter[this.PacketBuilderRun_Step] < 10)
                            ++index2;
                        else
                            this.VirtualSecCounter[this.PacketBuilderRun_Step] = 0;
                    }
                    else
                        this.VirtualSecCounter[this.PacketBuilderRun_Step] = 0;
                    index1 = index2;
                    num1 = 1;
                    break;
                }
            }
        }
        else if (this.PacketBuilderRun_Step == 3 || this.PacketBuilderRun_Step == 2)
        {
            for (int index3 = 0; index3 < NG08Tables.NG08RelayEntries; ++index3)
            {
                int updates = (int)this._board.Board_GetTableIndex((uint)NG08Tables.NG08RelayTable[index3].OriginalTable, (ushort)NG08Tables.NG08RelayTable[index3].OriginalIndex).updates;
                if (updates > num1)
                {
                    num1 = updates;
                    index1 = index3;
                }
            }
            if ((uint)num1 > 0U)
            {
                T_BOARD_ADDRESS tableIndex = this._board.Board_GetTableIndex((uint)NG08Tables.NG08RelayTable[index1].OriginalTable, (ushort)NG08Tables.NG08RelayTable[index1].OriginalIndex);
                tableIndex.updates = 0U;
                this.PacketBuffer[(int)this.PacketSize++] = (uint)NG08Tables.NG08RelayTable[index1].DestinationAddress;
                this.PacketBuffer[(int)this.PacketSize++] = (uint)NG08Tables.NG08RelayTable[index1].DestinationCommand;
                this.PacketBuffer[(int)this.PacketSize++] = (uint)(NG08Tables.NG08RelayTable[index1].DotMergeAddress << 4) | (tableIndex.datauint + 5U) / 10U;
                lock (this._instance.NB)
                {
                    foreach (NauBee nauBee in this._instance.NB)
                        nauBee.NauBeeSend(ref this.PacketBuffer, (byte)this.PacketSize);
                }
                return 15;
            }
        }
        if (num1 == 0 && this.sportSelect == eSports.Basketball && this.shotClockExtraDigitEnabled)
        {
            index1 = 78;
            for (int Index = 0; Index < NG08Tables.NG08Entries; ++Index)
            {
                if (NG08Tables.NG08StandardBoardTable[Index].OriginalIndex == index1)
                {
                    int num5 = (int)this._board.Board_GetTableIndex((uint)NG08Tables.NG08StandardBoardTable[Index].OriginalTable, (ushort)NG08Tables.NG08StandardBoardTable[Index].OriginalIndex).updates;
                    if (!this.PacketSplitterIsIndexValid(Index))
                        num5 = 0;
                    if (num5 > 0)
                    {
                        index1 = Index;
                        num1 = 1;
                        break;
                    }
                }
            }
        }
        if (num1 == 0)
        {
            for (int Index = this.pIndex[1]; Index < NG08Tables.NG08Entries; ++Index)
            {
                if (NG08Tables.NG08StandardBoardTable[Index].OriginalIndex < 12 || NG08Tables.NG08StandardBoardTable[Index].OriginalIndex > 15)
                {
                    T_BOARD_ADDRESS tableIndex3 = this._board.Board_GetTableIndex((uint)NG08Tables.NG08StandardBoardTable[Index].OriginalTable, (ushort)NG08Tables.NG08StandardBoardTable[Index].OriginalIndex);
                    int dotMergeAdress = NG08Tables.NG08StandardBoardTable[Index].DotMergeAddress;
                    int num6 = (int)tableIndex3.updates;
                    if (!this.PacketSplitterIsIndexValid(Index))
                        num6 = 0;
                    else if (NG08Tables.NG08StandardBoardTable[Index].OriginalTable == 0 && dotMergeAdress != (int)byte.MaxValue)
                    {
                        T_BOARD_ADDRESS tableIndex4 = this._board.Board_GetTableIndex(1U, (ushort)dotMergeAdress);
                        if ((long)num6 < (long)tableIndex4.updates)
                            num6 = (int)tableIndex4.updates;
                    }
                    if (num6 > num1)
                    {
                        num1 = num6;
                        index1 = Index;
                    }
                }
            }
        }
        if (num1 == 0)
        {
            this.pIndex[1] = 0;
            while (NG08Tables.NG08StandardBoardTable[this.pIndex[0]].OriginalIndex >= 12 && NG08Tables.NG08StandardBoardTable[this.pIndex[0]].OriginalIndex <= 15)
                this.pIndex[0] = (this.pIndex[0] + 1) % NG08Tables.NG08Entries;
            index1 = this.pIndex[0];
        }
        else
            this.pIndex[1] = index1 + 1;
        int num7 = 0;
        int destinationAdress = NG08Tables.NG08StandardBoardTable[index1].DestinationAddress;
        int destinationCommand = NG08Tables.NG08StandardBoardTable[index1].DestinationCommand;
        int index4 = index1;
        while (index4 < NG08Tables.NG08Entries && num7 < 5 && NG08Tables.NG08StandardBoardTable[index4].DestinationAddress == destinationAdress)
        {
            ++num7;
            ++index4;
            ++destinationCommand;
            if (index4 == NG08Tables.NG08Entries || (uint)NG08Tables.NG08StandardBoardTable[index4].OriginalTable > 0U || NG08Tables.NG08StandardBoardTable[index4].DestinationCommand != destinationCommand)
                break;
        }
        int Index1 = index1;
        if (this.pIndex[0] >= index1 && this.pIndex[0] < index1 + num7)
        {
            this.pIndex[0] = index1 + num7;
            if (this.pIndex[0] == NG08Tables.NG08Entries)
                this.pIndex[0] = 0;
        }
        this.PacketBuffer[(int)this.PacketSize++] = (uint)NG08Tables.NG08StandardBoardTable[Index1].DestinationAddress;
        this.PacketBuffer[(int)this.PacketSize++] = (uint)NG08Tables.NG08StandardBoardTable[Index1].DestinationCommand;
        for (; num7 > 0; --num7)
        {
            T_BOARD_ADDRESS tableIndex5 = this._board.Board_GetTableIndex((uint)NG08Tables.NG08StandardBoardTable[Index1].OriginalTable, (ushort)NG08Tables.NG08StandardBoardTable[Index1].OriginalIndex);
            num2 = (int)tableIndex5.updates;
            int dotMergeAdress = NG08Tables.NG08StandardBoardTable[Index1].DotMergeAddress;
            if (tableIndex5.updates > 0U)
            {
                if ((Index1 == 17 || Index1 == 144) && NG08Tables.NG08StandardBoardTable[Index1].OriginalTable == 0)
                {
                    if (this.PacketSplitterFlagAndCheckIfDone(Index1))
                        tableIndex5.updates = 0U;
                }
                else if (NG08Tables.NG08StandardBoardTable[Index1].OriginalTable == 0 && this.PacketSplitterFlagAndCheckIfDone(Index1))
                    tableIndex5.updates = 0U;
                else if (NG08Tables.NG08StandardBoardTable[Index1].OriginalTable == 1 && this.PacketSplitterDotMergeCheckIfDone(Index1))
                    tableIndex5.updates = 0U;
            }
            this.PacketBuffer[(int)this.PacketSize] = tableIndex5.datauint;
            if (NG08Tables.NG08StandardBoardTable[Index1].OriginalTable == 1)
            {
                switch (dotMergeAdress)
                {
                    case 0:
                        this.PacketBuffer[(int)this.PacketSize] >>= 4;
                        this.PacketBuffer[(int)this.PacketSize + 1] = tableIndex5.flashuint >> 4;
                        T_BOARD_ADDRESS tableIndex6 = this._board.Board_GetTableIndex(1U, (ushort)132);
                        if (tableIndex6.updates > 0U)
                            tableIndex6.updates = 0U;
                        if ((tableIndex6.datauint & 16U) > 0U)
                            this.PacketBuffer[(int)this.PacketSize] |= 16U;
                        if ((tableIndex6.datauint & 32U) > 0U)
                            this.PacketBuffer[(int)this.PacketSize] |= 8U;
                        ++this.PacketSize;
                        if ((tableIndex6.flashuint & 16U) > 0U)
                            this.PacketBuffer[(int)this.PacketSize] |= 16U;
                        if ((tableIndex6.flashuint & 32U) > 0U)
                            this.PacketBuffer[(int)this.PacketSize] |= 8U;
                        if (this._board.Board_GetTableIndex(2U, (ushort)4).datauint > 0U)
                            this.PacketBuffer[(int)this.PacketSize - 1] |= 32U;
                        if (this._board.Board_GetTableIndex(2U, (ushort)5).datauint > 0U)
                            this.PacketBuffer[(int)this.PacketSize - 1] |= 64U;
                        ++this.PacketSize;
                        this.PacketBuffer[(int)this.PacketSize] = 0U;
                        break;
                    case 1:
                        this.PacketBuffer[(int)this.PacketSize] = 0U;
                        if ((tableIndex5.datauint & 8U) > 0U)
                            this.PacketBuffer[(int)this.PacketSize] = 64U;
                        ++this.PacketSize;
                        this.PacketBuffer[(int)this.PacketSize++] = 0U;
                        this.PacketBuffer[(int)this.PacketSize] = 0U;
                        if ((tableIndex5.datauint & 1U) > 0U)
                        {
                            this.PacketBuffer[(int)this.PacketSize] = 32U;
                            break;
                        }
                        break;
                    default:
                        this.PacketBuffer[(int)this.PacketSize++] = (uint)(PacketBuilderNG08.communication_NG08DotSwitch_MSB_LSB((int)tableIndex5.datauint) & (int)sbyte.MaxValue);
                        this.PacketBuffer[(int)this.PacketSize++] = (uint)(PacketBuilderNG08.communication_NG08DotSwitch_MSB_LSB((int)tableIndex5.flashuint) & (int)sbyte.MaxValue);
                        this.PacketBuffer[(int)this.PacketSize] = 0U;
                        break;
                }
            }
            else if (dotMergeAdress != (int)byte.MaxValue)
            {
                T_BOARD_ADDRESS tableIndex7 = this._board.Board_GetTableIndex(1U, (ushort)dotMergeAdress);
                if (tableIndex7.updates > 0U && this.PacketSplitterDotMergeCheckIfDone(Index1))
                    tableIndex7.updates = 0U;
                if (((long)tableIndex7.datauint & (long)(1 << NG08Tables.NG08StandardBoardTable[Index1].DotMergeBit)) > 0L)
                    this.PacketBuffer[(int)this.PacketSize] |= 32U;
            }
            ++Index1;
            ++this.PacketSize;
        }
        lock (this._instance.NB)
        {
            foreach (NauBee nauBee in this._instance.NB)
                nauBee.NauBeeSend(ref this.PacketBuffer, (byte)this.PacketSize);
        }
        return this.PacketBuilderRun_Step == 5 ? 25 : 15;
    }

    public void ClearFlags()
    {
        for (int index1 = 0; index1 < 3; ++index1)
        {
            for (int index2 = 0; index2 < 32; ++index2)
                this.FlagList[index1, index2] = 0U;
        }
    }

    public void NauBeeEvent(int[] data)
    {
        if (data.Length < 3)
            return;
        this._instance.SendNauBeeEvent(data);
        try
        {
            if (!this._instance.PassiveListenerMode)
                return;
            this.DecodeCommandsNG08(data);
        }
        catch
        {
        }
    }

    private bool PacketSplitterIsIndexValid(int Index)
    {
        if (NG08Tables.NG08StandardBoardTable[Index].OriginalTable == 0)
        {
            T_BOARD_ADDRESS tableIndex = this._board.Board_GetTableIndex((uint)NG08Tables.NG08StandardBoardTable[Index].OriginalTable, (ushort)NG08Tables.NG08StandardBoardTable[Index].OriginalIndex);
            int num = 1 + ((int)tableIndex.datauint << 8) + ((int)tableIndex.flashuint << 16);
            if (this.SendFlags[Index] > 0 && this.SendFlags[Index] == num)
                return false;
        }
        return NG08Tables.NG08StandardBoardTable[Index].OriginalTable != 1 || this.SendMergeFlags[Index] <= 0;
    }

    private void DecodeCommandsNG08(int[] OriginalData)
    {
        List<int> intList1 = new List<int>((IEnumerable<int>)OriginalData);
        int val = OriginalData[0];
        int num1 = OriginalData[1];
        if (!val.Between(10, 19))
            return;
        int num2 = OriginalData.Length - 2;
        List<int> intList2 = new List<int>();
        int num3 = num1;
        if (num3.Between(12, 14))
        {
            for (int index = 0; index < num2; ++index)
            {
                T_BOARD_ADDRESS tableIndex = this._board.Board_GetTableIndex(0U, (ushort)num3);
                intList2.Add(num3);
                tableIndex.datauint = (uint)OriginalData[2 + index];
                num3 = (num3 + 1) % 256;
            }
        }
        else if (num3.Between(16, 20))
        {
            for (int index1 = 0; index1 < num2; ++index1)
            {
                if (num3 == 17)
                {
                    T_BOARD_ADDRESS tableIndex = this._board.Board_GetTableIndex(0U, (ushort)78);
                    intList2.Add(78);
                    tableIndex.datauint = (uint)OriginalData[2 + index1];
                }
                if (num3.Between(18, 19))
                {
                    int index2 = num3 - 10;
                    T_BOARD_ADDRESS tableIndex = this._board.Board_GetTableIndex(0U, (ushort)index2);
                    intList2.Add(index2);
                    tableIndex.datauint = (uint)OriginalData[2 + index1];
                }
                num3 = (num3 + 1) % 256;
            }
        }
        else
        {
            for (int index = 0; index < num2; ++index)
            {
                T_BOARD_ADDRESS tableIndex = this._board.Board_GetTableIndex(0U, (ushort)num3);
                intList2.Add(num3);
                tableIndex.datauint = (uint)OriginalData[2 + index];
                num3 = (num3 + 1) % 256;
            }
        }
        if (intList2.Count > 0)
            this._instance.SendNauBeeDigitDotRelayUpdateEvent(EventType.Digit, intList2.ToArray());
    }

    private void CheckCommandBufferNG08()
    {
        if (this.FinishProgress > 0)
        {
            this.FinishProgressStep = this.TextNo;
            switch (this.FinishProgress)
            {
                case 1:
                case 2:
                    if (this.FinishProgress == 1)
                    {
                        this.FinishProgressStep = this.TextNo - 1;
                        ++this.FinishProgress;
                    }
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[0];
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[1];
                    if (this.TxtEffect == 0)
                        this.PacketBuffer[(int)this.PacketSize++] = 109U;
                    else if (this.TxtEffect == 1)
                        this.PacketBuffer[(int)this.PacketSize++] = 108U;
                    else if (this.TxtEffect == 2)
                        this.PacketBuffer[(int)this.PacketSize++] = 107U;
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)this.FinishProgressStep;
                    this.PacketBuffer[(int)this.PacketSize++] = 0U;
                    this.PacketBuffer[(int)this.PacketSize++] = 0U;
                    if (this.TxtCommand == 0)
                    {
                        this.FinishProgress = 254;
                        break;
                    }
                    this.FinishProgress = 0;
                    break;
                case 254:
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[0];
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[1];
                    this.PacketBuffer[(int)this.PacketSize++] = 100U;
                    this.PacketBuffer[(int)this.PacketSize++] = 0U;
                    this.FinishProgress = 0;
                    break;
            }
        }
        else if (this.commandSize > 0)
        {
            if (this.TxtCommand == 11)
            {
                this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[0];
                this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[1];
                this.PacketBuffer[(int)this.PacketSize++] = 103U;
                this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)(6 + this.TextSeqProgress * 6);
                for (int index = 0; index < 6; ++index)
                {
                    if (this.BlockNo <= this.SeqList[(int)this.commandBuffer[5 + this.TextSeqProgress]])
                    {
                        this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)this.BlockNo++;
                    }
                    else
                    {
                        ++this.TextSeqProgress;
                        if (this.TextSeqProgress >= this.commandSize)
                        {
                            this.PacketBuffer[(int)this.PacketSize++] = 83U;
                            this.commandSize = 0;
                            break;
                        }
                        this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)this.BlockNo++;
                    }
                }
            }
            else
            {
                this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[0];
                this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[1];
                this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)this.TextNo;
                this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)this.BlockNo;
                for (int index = 0; index < 7; ++index)
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[5 + index + this.BlockNo * 7 + (this.TextNo - this.SeqList[this.TxtSeq]) * 21];
                ++this.BlockNo;
                if (this.BlockNo * 7 + (this.TextNo - this.SeqList[this.TxtSeq]) * 21 >= this.commandSize)
                {
                    this.PacketBuffer[3] |= 64U;
                    this.commandSize = 0;
                    this.FinishProgress = 2;
                }
                else if (this.BlockNo == 3)
                {
                    this.PacketBuffer[3] |= 64U;
                    this.BlockNo = 0;
                    ++this.TextNo;
                    this.FinishProgress = 1;
                }
            }
        }
        else
        {
            sCommand result;
            if (!this._board.Que.TryDequeue(out result))
                return;
            this.BlockNo = 0;
            this.SequenceNr = (this.SequenceNr + 1) % 15;
            this.commandRetryCount = result.Retries;
            if ((result.ControlByte & 64) > 0)
            {
                if (result.Data[1] == (byte)3)
                {
                    this.PacketBuffer[(int)this.PacketSize++] = 128U;
                    this.PacketBuffer[(int)this.PacketSize++] = 99U;
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)((((int)result.Data[2] << 8) + (int)result.Data[3]) / 100);
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)((((int)result.Data[4] << 8) + (int)result.Data[5]) / 100);
                }
                else if (result.Data[1] == (byte)1)
                {
                    this.PacketBuffer[(int)this.PacketSize++] = 128U;
                    this.PacketBuffer[(int)this.PacketSize++] = 126U;
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)result.Data[5];
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)result.Data[6];
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)result.Data[7];
                }
                else
                {
                    if (result.Data[1] != (byte)192 || ((int)result.Data[2] & 3) != 3)
                        return;
                    this.TxtCommand = (int)result.Data[5];
                    this.TxtEffect = (int)result.Data[7];
                    if (result.Data[6] == (byte)0)
                    {
                        if (this.TxtCommand == 11)
                        {
                            this.SeqList[this.TxtSeq] = this.TextNo;
                        }
                        else
                        {
                            if (this.TxtCommand == 7 || this.TxtCommand == 0)
                                this.SeqList[0] = 0;
                            this.TxtSeq = (int)result.Data[6];
                        }
                        this.TextNo = 0;
                    }
                    else if (this.TxtCommand == 7)
                    {
                        this.SeqList[this.TxtSeq] = this.TextNo;
                        ++this.TextNo;
                        this.TxtSeq = (int)result.Data[6];
                        this.SeqList[this.TxtSeq] = this.TextNo;
                    }
                    this.WasMultiCast = 1;
                    byte num1 = result.Data[3];
                    if (num1 >= (byte)128)
                    {
                        this.PacketBuffer[(int)this.PacketSize++] = 80U;
                        num1 -= (byte)128;
                    }
                    else
                        this.PacketBuffer[(int)this.PacketSize++] = 70U;
                    byte num2 = (byte)((uint)num1 / 4U);
                    if (num2 > (byte)0)
                        --num2;
                    this.PacketBuffer[(int)this.PacketSize++] = (uint)num2;
                    this.commandBuffer[0] = (byte)this.PacketBuffer[0];
                    this.commandBuffer[1] = (byte)this.PacketBuffer[1];
                    if (this.TxtCommand == 0 || this.TxtCommand == 7)
                    {
                        for (int index = 9; index < result.Data.Count; ++index)
                            this.commandBuffer[5 + this.commandSize++] = result.Data[index];
                        this.commandBuffer[5 + this.commandSize] = (byte)32;
                        for (int index = 1; index < 7; ++index)
                            this.commandBuffer[5 + this.commandSize + index] = (byte)32;
                        this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)this.TextNo;
                        this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)this.BlockNo;
                        for (int index = 0; index < 7; ++index)
                            this.PacketBuffer[(int)this.PacketSize++] = (uint)this.commandBuffer[5 + index + this.BlockNo * 7 + (this.TextNo - this.SeqList[this.TxtSeq]) * 21];
                        ++this.BlockNo;
                        if (this.BlockNo * 7 + (this.TextNo - this.SeqList[this.TxtSeq]) * 21 >= this.commandSize)
                        {
                            this.PacketBuffer[3] |= 64U;
                            this.commandSize = 0;
                            this.FinishProgress = 2;
                        }
                    }
                    if (this.TxtCommand == 11)
                    {
                        for (int index = 7; index < result.Data.Count; ++index)
                            this.commandBuffer[5 + this.commandSize++] = result.Data[index];
                        --this.commandSize;
                        this.PacketBuffer[(int)this.PacketSize++] = 103U;
                        this.PacketBuffer[(int)this.PacketSize++] = 0U;
                        this.TextSeqProgress = 0;
                        for (int index = 0; index < 6; ++index)
                        {
                            if (this.BlockNo <= this.SeqList[(int)this.commandBuffer[5 + this.TextSeqProgress]])
                            {
                                this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)this.BlockNo++;
                            }
                            else
                            {
                                ++this.TextSeqProgress;
                                if (this.TextSeqProgress >= this.commandSize)
                                {
                                    this.PacketBuffer[(int)this.PacketSize++] = 83U;
                                    this.commandSize = 0;
                                    break;
                                }
                                this.PacketBuffer[(int)this.PacketSize++] = (uint)(byte)this.BlockNo++;
                            }
                        }
                    }
                    if (this.TxtCommand != 13)
                        return;
                    this.PacketBuffer[(int)this.PacketSize++] = 104U;
                    this.commandSize = 0;
                }
            }
            else if ((result.ControlByte & 128) <= 0 || result.Destination != 128 && result.Destination != 129)
                ;
        }
    }

    private bool PacketSplitterDotMergeCheckIfDone(int Index)
    {
        bool flag = false;
        int num = NG08Tables.NG08StandardBoardTable[Index].OriginalTable == 0 ? NG08Tables.NG08StandardBoardTable[Index].DotMergeAddress : NG08Tables.NG08StandardBoardTable[Index].OriginalIndex;
        if (NG08Tables.NG08StandardBoardTable[Index].OriginalTable == 0 && num == (int)byte.MaxValue)
            return true;
        this.SendMergeFlags[Index] = 1;
        for (int index = 0; index < NG08Tables.NG08Entries; ++index)
        {
            if (NG08Tables.NG08StandardBoardTable[index].OriginalTable == 0 && NG08Tables.NG08StandardBoardTable[index].DotMergeAddress == num)
            {
                if (this.SendMergeFlags[index] == 0)
                    return false;
                flag = true;
            }
            if (NG08Tables.NG08StandardBoardTable[index].OriginalTable == 1 && NG08Tables.NG08StandardBoardTable[index].OriginalIndex == num)
            {
                if (this.SendMergeFlags[index] == 0)
                    return false;
                flag = true;
            }
        }
        if (flag)
        {
            for (int index = 0; index < NG08Tables.NG08Entries; ++index)
            {
                if (NG08Tables.NG08StandardBoardTable[index].OriginalTable == 0 && NG08Tables.NG08StandardBoardTable[index].DotMergeAddress == num)
                    this.SendMergeFlags[index] = 0;
                if (NG08Tables.NG08StandardBoardTable[index].OriginalTable == 1 && NG08Tables.NG08StandardBoardTable[index].OriginalIndex == num)
                    this.SendMergeFlags[index] = 0;
            }
        }
        else
            this.SendMergeFlags[Index] = 0;
        return true;
    }

    private bool PacketSplitterFlagAndCheckIfDone(int Index)
    {
        bool flag = false;
        T_BOARD_ADDRESS tableIndex = this._board.Board_GetTableIndex((uint)NG08Tables.NG08StandardBoardTable[Index].OriginalTable, (ushort)NG08Tables.NG08StandardBoardTable[Index].OriginalIndex);
        this.SendFlags[Index] = 1 + ((int)tableIndex.datauint << 8) + ((int)tableIndex.flashuint << 16);
        if (NG08Tables.NG08StandardBoardTable[Index].OriginalTable == 0 && NG08Tables.NG08StandardBoardTable[Index].DotMergeAddress != (int)byte.MaxValue)
            this.SendMergeFlags[Index] = 1;
        for (int index = 0; index < NG08Tables.NG08Entries; ++index)
        {
            if (NG08Tables.NG08StandardBoardTable[index].OriginalTable == NG08Tables.NG08StandardBoardTable[Index].OriginalTable && NG08Tables.NG08StandardBoardTable[index].OriginalIndex == NG08Tables.NG08StandardBoardTable[Index].OriginalIndex)
            {
                if (this.SendFlags[index] != this.SendFlags[Index] || this.SendFlags[index] == 0)
                    return false;
                flag = true;
            }
        }
        if (flag)
        {
            for (int index = 0; index < NG08Tables.NG08Entries; ++index)
            {
                if (NG08Tables.NG08StandardBoardTable[index].OriginalTable == NG08Tables.NG08StandardBoardTable[Index].OriginalTable && NG08Tables.NG08StandardBoardTable[index].OriginalIndex == NG08Tables.NG08StandardBoardTable[Index].OriginalIndex)
                    this.SendFlags[index] = 0;
            }
        }
        else
            this.SendFlags[Index] = 0;
        return true;
    }

    private static int communication_NG08DotSwitch_MSB_LSB(int value)
    {
        int num = 0;
        if ((value & 1) > 0)
            num |= 16;
        if ((value & 2) > 0)
            num |= 8;
        if ((value & 4) > 0)
            num |= 4;
        if ((value & 8) > 0)
            num |= 2;
        if ((value & 16) > 0)
            num |= 1;
        return num;
    }
}