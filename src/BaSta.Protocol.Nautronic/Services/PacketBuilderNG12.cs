using System;
using System.Collections.Generic;
using System.Linq;
using BaSta.Protocol.Nautronic.Driver;
using BaSta.Protocol.Nautronic.Extra;

namespace BaSta.Protocol.Nautronic.Services;

internal class PacketBuilderNG12 : iPacketBuilder
{
    private NauCom Instance;
    private cBoard Board;
    private ushort[] PacketBuilderRun_pIndex = new ushort[4];
    private uint PacketBuilderRun_Step = 0;
    private uint PacketBuilderRun_tPacketSize = 0;
    internal uint PacketSize = 0;
    internal uint SendItems = 0;
    internal uint[] PacketBuffer = new uint[128];
    internal uint[,] FlagList = new uint[3, 32];
    internal uint SequenceNr = 0;
    internal uint waitingForAck = 0;
    internal uint RetryCount = 0;
    internal uint commandRetryCount = 3;
    internal T_COMMUNICATION_TEST communicationTestVars = new T_COMMUNICATION_TEST();
    internal uint commandSize = 0;
    internal uint[] commandBuffer = new uint[128];
    private uint CheckCommandBuffer_WasMultiCast = 0;
    internal uint[] MessageBuffer = new uint[256];
    internal uint MessageBufferLen;
    private int[] OldEventData = new int[1];
    private DateTime LastLogEvent = DateTime.MinValue;
    private Dictionary<int, DateTime> LastKeyPressed = new Dictionary<int, DateTime>();
    private string[] TextBuilder = new string[(int)byte.MaxValue];
    private int[] TextBuilderIndex = new int[(int)byte.MaxValue];

    public PacketBuilderNG12(NauCom instance, cBoard instance2)
    {
        this.Instance = instance;
        this.Board = instance2;
    }

    public int Run()
    {
        this.PacketSize = 0U;
        if (this.PacketBuilderRun_Step == 2U)
            this.CheckCommandBuffer();
        if (this.PacketSize == 0U && !this.Instance.IsPassiveListenerMode() && this.PacketBuilderRun_tPacketSize > 0U)
            this.communication_AddFlaggedPacketsToTXBuffer();
        if (this.PacketSize > 0U)
        {
            ++this.SendItems;
            lock (this.Instance.NB)
            {
                foreach (NauBee nauBee in this.Instance.NB)
                    nauBee.NauBeeSend(ref this.PacketBuffer, (byte)this.PacketSize);
            }
        }
        this.PacketBuilderRun_Step = (this.PacketBuilderRun_Step + 1U) % 3U;
        if (!this.Instance.IsPassiveListenerMode())
        {
            this.ClearFlags();
            this.PacketBuilderRun_tPacketSize = 0U;
            if (this.Instance.PauseSending == 0)
            {
                this.PacketBuilderRun_tPacketSize = this.communication_FlagPackets(this.PacketBuilderRun_tPacketSize, 3U, ref this.PacketBuilderRun_pIndex[0], 1U);
                if (this.PacketBuilderRun_Step == 0U)
                    this.PacketBuilderRun_tPacketSize = this.communication_FlagPackets(this.PacketBuilderRun_tPacketSize, 2U, ref this.PacketBuilderRun_pIndex[1], 1U);
                this.PacketBuilderRun_tPacketSize = this.communication_FlagPackets(this.PacketBuilderRun_tPacketSize, 1U, ref this.PacketBuilderRun_pIndex[2], 1U);
                this.PacketBuilderRun_tPacketSize = this.communication_FlagPackets(this.PacketBuilderRun_tPacketSize, 1U, ref this.PacketBuilderRun_pIndex[3], 0U);
            }
        }
        return this.PacketBuilderRun_Step == 0U ? 40 : 30;
    }

    internal void communication_AddFlaggedPacketsToTXBuffer()
    {
        uint index1 = 0;
        this.PacketSize = 0U;
        this.PacketBuffer[(int)this.PacketSize++] = (uint)this.Instance.Group;
        this.PacketBuffer[(int)this.PacketSize++] = 128U;
        this.PacketBuffer[(int)this.PacketSize++] = 64U + this.SequenceNr;
        this.SequenceNr = (this.SequenceNr + 1U) % 15U;
        for (uint index2 = 0; index2 < 3U; ++index2)
        {
            uint num = 0;
            for (ushort index3 = 0; (int)index3 < (int)this.Board.BoardSize[(int)index2]; ++index3)
            {
                if (this.communication_CheckFlag(index2, index3) > 0U)
                {
                    T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(index2, index3);
                    if (tableIndex.updates > 0U)
                        --tableIndex.updates;
                    if (num == 0U)
                    {
                        num = 1U;
                        index1 = this.PacketSize++;
                        switch (index2)
                        {
                            case 0:
                                this.PacketBuffer[(int)this.PacketSize++] = 64U;
                                break;
                            case 1:
                                this.PacketBuffer[(int)this.PacketSize++] = 128U;
                                break;
                            case 2:
                                this.PacketBuffer[(int)this.PacketSize++] = 0U;
                                break;
                        }
                        this.PacketBuffer[(int)this.PacketSize++] = (uint)index3;
                        this.PacketBuffer[(int)index1] = 2U;
                    }
                    this.PacketBuffer[(int)this.PacketSize++] = tableIndex.datauint;
                    ++this.PacketBuffer[(int)index1];
                    if (index2 == 1U)
                    {
                        this.PacketBuffer[(int)this.PacketSize++] = tableIndex.flashuint;
                        ++this.PacketBuffer[(int)index1];
                    }
                }
                else
                    num = 0U;
            }
        }
    }

    internal void CheckCommandBuffer()
    {
        if (this.waitingForAck > 0U)
        {
            ++this.RetryCount;
            ++this.communicationTestVars.TotalRetryCounter;
            if (this.RetryCount <= this.commandRetryCount)
            {
                this.PacketSize = this.commandSize;
                for (uint index = 0; index < this.PacketSize; ++index)
                    this.PacketBuffer[(int)index] = this.commandBuffer[(int)index];
                ++this.communicationTestVars.SendMessages;
                return;
            }
        }
        sCommand result;
        if (!this.Board.Que.TryDequeue(out result))
            return;
        this.SequenceNr = (this.SequenceNr + 1U) % 15U;
        if ((uint)(result.ControlByte & 64) > 0U)
        {
            this.PacketBuffer[(int)this.PacketSize++] = (uint)this.Instance.Group;
            this.CheckCommandBuffer_WasMultiCast = 1U;
        }
        else
        {
            this.CheckCommandBuffer_WasMultiCast = 0U;
            if ((uint)(result.ControlByte & 128) > 0U && result.Data[1] == (byte)8)
                ++this.communicationTestVars.SendMessages;
            this.PacketBuffer[(int)this.PacketSize++] = (uint)result.Destination;
        }
        this.PacketBuffer[(int)this.PacketSize++] = result.Destination == 128 ? 129U : 128U;
        this.PacketBuffer[(int)this.PacketSize++] = (uint)result.ControlByte + this.SequenceNr;
        for (uint index = 0; (long)index < (long)result.Data.Count; ++index)
            this.PacketBuffer[(int)this.PacketSize++] = (uint)result.Data[(int)index];
        for (uint index = 0; index < this.PacketSize; ++index)
            this.commandBuffer[(int)index] = this.PacketBuffer[(int)index];
        this.commandSize = this.PacketSize;
        this.RetryCount = 0U;
    }

    public void ClearFlags()
    {
        for (int index1 = 0; index1 < 3; ++index1)
        {
            for (int index2 = 0; index2 < 32; ++index2)
                this.FlagList[index1, index2] = 0U;
        }
    }

    internal void communication_SetFlag(uint Table, ushort index) => this.FlagList[(int)Table, (int)index / 8] |= (uint)(1 << (int)index % 8);

    internal uint communication_CheckFlag(uint Table, ushort index) => this.FlagList[(int)Table, (int)index / 8] & (uint)(1 << (int)index % 8);

    internal uint communication_FlagPackets(
        uint tPacketSize,
        uint Priority,
        ref ushort Index,
        uint Update)
    {
        int index1 = (int)Index % 1000;
        int index2 = (int)Index / 1000 % 3;
        if (tPacketSize < 80U)
        {
            for (uint index3 = 0; index3 < 3U; ++index3)
            {
                uint num = 0;
                for (; index1 < (int)this.Board.BoardSize[index2] && tPacketSize < 80U; ++index1)
                {
                    T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex((uint)index2, (ushort)index1);
                    if ((int)tableIndex.priority == (int)Priority && Update > 0U && tableIndex.updates > 0U || Update == 0U && (tableIndex.priority > 0U || tableIndex.updates > 0U))
                    {
                        this.communication_SetFlag((uint)index2, (ushort)index1);
                        if (num > 0U)
                            tPacketSize += 2U;
                        else
                            tPacketSize += 4U;
                        num = 1U;
                    }
                    else
                        num = 0U;
                }
                if (index1 >= (int)this.Board.BoardSize[index2])
                {
                    index2 = (index2 + 1) % 3;
                    index1 = 0;
                }
            }
        }
        Index = (ushort)(index1 + index2 * 1000);
        return tPacketSize;
    }

    public void NauBeeEvent(int[] data)
    {
        if (data.Length < 3)
            return;
        if ((data[3] & 64) == 0 && data[1] == 254)
        {
            if (((IEnumerable<int>)this.OldEventData).SequenceEqual<int>((IEnumerable<int>)this.OldEventData) && DateTime.Now < this.LastLogEvent.AddSeconds(1.0))
                return;
            this.LastLogEvent = DateTime.Now;
            if (data[4] > 0)
                this.Instance.SendGameLogUpdateEvent(((IEnumerable<int>)data).Skip<int>(6).Take<int>(data[4] - 1).ToArray<int>());
        }
        else
        {
            if ((data[1] & this.Instance.Group) == 0)
                return;
            if ((data[3] & 32) > 0)
            {
                if (this.waitingForAck > 0U)
                    this.waitingForAck = 0U;
            }
            else if (data.Length > 3)
            {
                this.Instance.SendNauBeeEvent(data);
                if (data.Length == 9 && data[1] == (int)byte.MaxValue && data[4] == 3 && data[5] == 31 && (NauBeeChannels)(data[5] >> 6 & 3) == this.Instance.WirelessChannel)
                {
                    bool flag = true;
                    DateTime dateTime;
                    if (this.LastKeyPressed.TryGetValue(data[6], out dateTime))
                    {
                        this.LastKeyPressed[data[6]] = DateTime.Now;
                        if ((DateTime.Now - dateTime).TotalMilliseconds < 100.0)
                            flag = false;
                    }
                    else
                        this.LastKeyPressed.Add(data[6], DateTime.Now);
                    if (flag)
                        this.Instance.SendNauBeeRemoteEvent(data);
                }
            }
        }
        try
        {
            if (!this.Instance.PassiveListenerMode)
                return;
            this.DecodeCommandsNG12(data);
        }
        catch
        {
        }
    }

    private void DecodeCommandsNG12(int[] OriginalData)
    {
        List<int> intList1 = new List<int>((IEnumerable<int>)OriginalData);
        int num1 = OriginalData[0];
        int num2 = OriginalData[1];
        int num3 = OriginalData[2];
        int num4 = OriginalData[3];
        intList1.RemoveRange(0, 4);
        while (intList1.Count > 1)
        {
            List<int> range = intList1.GetRange(1, intList1[0]);
            intList1.RemoveRange(0, intList1[0] + 1);
            if (range[0] == 64)
            {
                int index1 = range[1];
                List<int> intList2 = new List<int>();
                intList2.Add(index1);
                for (int index2 = 2; index2 < range.Count; ++index2)
                {
                    T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(0U, (ushort)index1);
                    index1 = (index1 + 1) % 256;
                    intList2.Add(index1);
                    tableIndex.datauint = (uint)range[index2];
                }
                this.Instance.SendNauBeeDigitDotRelayUpdateEvent(EventType.Digit, intList2.ToArray());
            }
            else if (range[0] == 128)
            {
                int index3 = range[1];
                List<int> intList3 = new List<int>();
                intList3.Add(index3);
                for (int index4 = 2; index4 < range.Count; index4 += 2)
                {
                    T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(1U, (ushort)index3);
                    index3 = (index3 + 1) % 256;
                    intList3.Add(index3);
                    tableIndex.datauint = (uint)range[index4];
                    tableIndex.flashuint = (uint)range[index4 + 1];
                }
                this.Instance.SendNauBeeDigitDotRelayUpdateEvent(EventType.Dot, intList3.ToArray());
            }
            else if (range[0] == 0)
            {
                int index5 = range[1];
                List<int> intList4 = new List<int>();
                intList4.Add(index5);
                for (int index6 = 2; index6 < range.Count; ++index6)
                {
                    T_BOARD_ADDRESS tableIndex = this.Board.Board_GetTableIndex(2U, (ushort)index5);
                    index5 = (index5 + 1) % 256;
                    intList4.Add(index5);
                    tableIndex.datauint = (uint)range[index6];
                }
                this.Instance.SendNauBeeDigitDotRelayUpdateEvent(EventType.Relay, intList4.ToArray());
            }
            else if ((range[0] & 192) == 192)
            {
                int adress = range[2];
                List<char> source = new List<char>();
                string.Join<int>(",", (IEnumerable<int>)range);
                Dictionary<int, char> conversionDictionary = NauASCIIDictionaries.WesternConversionDictionary;
                if (this.Instance.ConsoleLocalization == eConsoleLocalization.ARABIC)
                    conversionDictionary = NauASCIIDictionaries.ArabicConversionDictionary;
                for (int index = 8; index < range.Count; ++index)
                {
                    if (conversionDictionary.ContainsKey(range[index]))
                    {
                        if (conversionDictionary[range[index]] != char.MaxValue)
                            source.Add(conversionDictionary[range[index]]);
                    }
                    else
                        source.Add((char)range[index]);
                }
                string str1 = "\u200E";
                string str2 = str1 ?? "";
                foreach (char ch in source)
                    str2 = str2 + ch.ToString() + str1;
                string text = str2;
                if (this.Instance.ConsoleLocalization == eConsoleLocalization.ARABIC)
                    text = this.Instance.HandleRightToLeft((IEnumerable<char>)source);
                if ((uint)range[4] > 0U)
                {
                    if (range[4] == 7)
                    {
                        if (range[5] == 0)
                        {
                            this.TextBuilderIndex[adress] = 0;
                            this.TextBuilder[adress] = ((char)(range[6] + 1)).ToString() + text;
                            break;
                        }
                        if (range[5] <= this.TextBuilderIndex[adress])
                            break;
                        this.TextBuilderIndex[adress] = range[5];
                        if (range[6] == 0)
                        {
                            // ISSUE: explicit reference operation
                            TextBuilder[adress] += "\u0001";
                        }
                        // ISSUE: explicit reference operation
                        TextBuilder[adress] += text;
                        break;
                    }
                    if (range[4] != 13)
                        break;
                    if (this.TextBuilder[adress].Length > 0)
                    {
                        text = "\u0001" + this.TextBuilder[adress];
                        this.TextBuilder[adress] = "";
                    }
                }
                this.Instance.SendNauBeeTextUpdateEvent(adress, text);
            }
            else if (range[0] == 13)
            {
                this.Instance.ClearAll(254);
            }
            else
            {
                int num5 = range[0];
            }
        }
    }
}