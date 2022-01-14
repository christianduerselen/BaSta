using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace BaSta.Protocol.Nautronic
{
    public class cBoard
    {
        private byte[] boardCmdData = new byte[256];
        private T_BOARD_ADDRESS[] boardDigitTable;
        private T_BOARD_ADDRESS[] boardDotTable;
        private T_BOARD_ADDRESS[] boardRelayTable;
        public ushort[] BoardSize = { 256, 256, 32 };
        public ConcurrentQueue<sCommand> Que = new();
        internal Timer BoardTimer;
        private NauCom Instance;

        public cBoard(NauCom Owner)
        {
            Instance = Owner;
            BoardTimer = new Timer(100.0);
            BoardTimer.Elapsed += BoardTimer_Elapsed;
            BoardTimer.Start();
            boardDigitTable = new T_BOARD_ADDRESS[256];
            boardDotTable = new T_BOARD_ADDRESS[256];
            boardRelayTable = new T_BOARD_ADDRESS[256];
            for (int index = 0; index < 256; ++index)
            {
                boardDigitTable[index] = new T_BOARD_ADDRESS();
                boardDigitTable[index].datauint = 15U;
                boardDotTable[index] = new T_BOARD_ADDRESS();
                boardRelayTable[index] = new T_BOARD_ADDRESS();
            }
        }

        private void BoardTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (int relayNo = 0; relayNo < this.boardRelayTable.Length; ++relayNo)
            {
                if (this.boardRelayTable[relayNo].datauint > 0U && this.boardRelayTable[relayNo].datauint < (uint)byte.MaxValue && --this.boardRelayTable[relayNo].datauint == 0U)
                    this.Board_RelayUpdate(relayNo, 0);
            }
        }

        public void Board_RelayUpdate(int relayNo, int msTime)
        {
            uint num = (uint)msTime / 100U;
            if (msTime == -1)
                num = (uint)byte.MaxValue;
            if ((int)num != (int)this.boardRelayTable[relayNo].datauint)
                this.boardRelayTable[relayNo].updates = 10U;
            this.boardRelayTable[relayNo].datauint = num;
            this.boardRelayTable[relayNo].flashuint = 0U;
            this.boardRelayTable[relayNo].priority = 3U;
            switch (relayNo)
            {
                case 0:
                    relayNo = 3;
                    if ((int)num != (int)this.boardRelayTable[relayNo].datauint)
                        this.boardRelayTable[relayNo].updates = 10U;
                    this.boardRelayTable[relayNo].datauint = num;
                    this.boardRelayTable[relayNo].flashuint = 0U;
                    this.boardRelayTable[relayNo].priority = 3U;
                    break;
                case 16:
                    relayNo = 18;
                    if ((int)num != (int)this.boardRelayTable[relayNo].datauint)
                        this.boardRelayTable[relayNo].updates = 10U;
                    this.boardRelayTable[relayNo].datauint = num;
                    this.boardRelayTable[relayNo].flashuint = 0U;
                    this.boardRelayTable[relayNo].priority = 3U;
                    break;
            }
        }

        public T_BOARD_ADDRESS Board_GetTableIndex(uint TableID, ushort index)
        {
            switch (TableID)
            {
                case 0:
                    return this.boardDigitTable[(int)index];
                case 1:
                    return this.boardDotTable[(int)index];
                case 2:
                    return this.boardRelayTable[(int)index];
                default:
                    return this.boardDotTable[0];
            }
        }

        public void Board_SetTableIndex(uint TableID, ushort index, T_BOARD_ADDRESS Data)
        {
            switch (TableID)
            {
                case 0:
                    this.boardDigitTable[(int)index] = Data;
                    break;
                case 1:
                    this.boardDotTable[(int)index] = Data;
                    break;
                case 2:
                    this.boardRelayTable[(int)index] = Data;
                    break;
            }
        }

        public void Board_NauBee_SetChannel(uint channel)
        {
            sCommand sCommand = new sCommand()
            {
                Destination = 0,
                ControlByte = 0,
                Retries = 0,
                Data = { 0, 0, (byte) channel }
            };
            sCommand.Data[0] = (byte) (sCommand.Data.Count - 1);
            Que.Enqueue(sCommand);
        }

        public void Board_NauBee_SetDBM(uint value)
        {
            sCommand sCommand = new sCommand
            {
                Destination = 0,
                ControlByte = 0,
                Retries = 0,
                Data = { 0, 4, (byte) value }
            };
            sCommand.Data[0] = (byte) (sCommand.Data.Count - 1);
            Que.Enqueue(sCommand);
        }

        private static int Board_GetTextLenVal(int value)
        {
            value = value != 0 ? (value + 3) / 4 : 4;
            return (value >= 4 ? (value >= 5 ? (value >= 6 ? (value >= 7 ? (value >= 9 ? (value >= 11 ? (value >= 13 ? (value >= 15 ? (value >= 17 ? 0 : 7) : 6) : 5) : 4) : 0) : 3) : 2) : 1) : 0) << 3;
        }

        public void Board_TextLineUpdate(byte Adr, string Text)
        {
            byte[] bytes = new ASCIIEncoding().GetBytes(Text);
            sCommand sCommand = new sCommand();
            sCommand.Destination = (int)byte.MaxValue;
            sCommand.ControlByte = 64;
            sCommand.Retries = 0;
            sCommand.Data.Add((byte)0);
            sCommand.Data.Add((byte)192);
            byte num1 = 195;
            byte num2 = Adr != (byte)0 && Adr != (byte)128 ? (byte)((uint)num1 | (uint)(byte)cBoard.Board_GetTextLenVal(0)) : (byte)((uint)num1 | (uint)(byte)cBoard.Board_GetTextLenVal(this.Instance._teamDispLength));
            sCommand.Data.Add(num2);
            sCommand.Data.Add(Adr);
            sCommand.Data.Add((byte)0);
            sCommand.Data.Add((byte)0);
            sCommand.Data.Add((byte)0);
            sCommand.Data.Add((byte)0);
            sCommand.Data.Add((byte)0);
            sCommand.Data.AddRange((IEnumerable<byte>)bytes);
            sCommand.Data[4] = (byte)(sCommand.Data.Count - 5);
            sCommand.Data[0] = (byte)(sCommand.Data.Count - 1);
            this.Que.Enqueue(sCommand);
        }

        public void SetScore(int Score)
        {
            sCommand sCommand = new sCommand()
            {
                Destination = 128,
                ControlByte = 0,
                Retries = 20,
                Data = {
          (byte) 0,
          (byte) 5,
          (byte) 0,
          (byte) 0,
          (byte) 1,
          (byte) Score,
          (byte) 0,
          (byte) 0,
          (byte) Score,
          (byte) 0
        }
            };
            sCommand.Data[0] = (byte)(sCommand.Data.Count - 1);
            this.Que.Enqueue(sCommand);
        }
    }
}