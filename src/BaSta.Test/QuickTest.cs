using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BaSta.TimeSync.Output;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaSta.Test
{
    [TestClass]
    public class QuickTest
    {
        [TestMethod]
        public void CharToByte()
        {
            char[] numberChars = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            foreach (char numberChar in numberChars)
            {
                byte numberByte = Convert.ToByte(numberChar);
            }
        }

        [TestMethod]
        public void StramatelClassic_ParseTest()
        {
            string fileLocation = @"C:\Users\m1cdu\Desktop\output_2020-02-16_17-05-31.log";
            string fileText = File.ReadAllText(fileLocation);

            StramatelClassicTimeParser parser = new StramatelClassicTimeParser();

            for (int i = 0; i < fileText.Length; i = i + 2)
            {
                string hex = fileText.Substring(i, 2);

                if (hex == Environment.NewLine)
                    continue;

                int value = Convert.ToInt32(hex, 16);
                parser.Push((byte) value);
            }
        }

        [TestMethod]
        public void StramatelClassic_ParseTest2()
        {
            string fileLocation = @"C:\Users\m1cdu\Desktop\14400_Q1 - Kopie";
            string fileText = File.ReadAllText(fileLocation);

            StramatelClassicTimeParser parser = new StramatelClassicTimeParser();

            for (int i = 0; i < fileText.Length; i = i + 2)
            {
                string hex = fileText.Substring(i, 2);

                if (hex == Environment.NewLine)
                    continue;

                int value = Convert.ToInt32(hex, 16);
                parser.Push((byte)value);
            }
        }

        public class StramatelClassicTimeParser
        {
            private const byte StartByte1 = 0x66;
            private static readonly byte[] StartByte2 = { 0x3C, 0xBC, 0x3E, 0x9E, 0x9C };
            private const byte CommandByte1 = 0x66;
            private const byte CommandByte2 = 0x0F;
            private const byte StopByte1 = 0x66;
            private const byte StopByte2 = 0x33;
            private const int MaxDataCount = 8;

            private readonly byte[] _data = new byte[MaxDataCount];
            private int _dataPointer;
            private ParseState _state = ParseState.AwaitCommandByte1;

            private IDictionary<byte[], int> _assigned;

            public StramatelClassicTimeParser()
            {
                _assigned = new Dictionary<byte[], int>();
                Assign(0, 0x86, 0x9E);
                Assign(0, 0x1E, 0x98);
                Assign(0, 0x78, 0xCC);
                Assign(0, 0x0F, 0x98);
                Assign(0, 0x0F, 0x18);
                Assign(0, 0x78, 0x66); // 0/5
                Assign(0, 0x78, 0x0C);
                Assign(0, 0x8F, 0x98);
                Assign(0, 0x1E, 0x0F);
                Assign(0, 0x1E, 0x8F);
                Assign(0, 0x1E, 0x18);
                Assign(0, 0x78, 0x6C);
                Assign(0, 0x3C, 0x33);
                Assign(0, 0xCC, 0x66);
                Assign(0, 0x98, 0x66);
                Assign(0, 0xC3, 0xCF);
                Assign(0, 0x1E, 0x87);
                Assign(0, 0x63, 0xCF);
                Assign(0, 0x61, 0xCF);
                Assign(0, 0x66, 0x33);
                Assign(0, 0x0F, 0x86);
                Assign(0, 0xE6, 0x66);
                Assign(0, 0x78, 0x86);
                Assign(0, 0xF3, 0x66);
                Assign(0, 0x43, 0xCF);
                Assign(0, 0xF3, 0x33);
                Assign(0, 0x78, 0xE6);
                Assign(0, 0xC3, 0x9E);
                Assign(0, 0x87, 0x98);
                Assign(0, 0x78, 0x8C);
                Assign(0, 0x78, 0x64);
                Assign(0, 0x78, 0x06);
                Assign(0, 0x8F, 0x18);
                Assign(0, 0x0E, 0x98);
                Assign(0, 0xE4, 0x66);
                Assign(0, 0xC8, 0x66);
                Assign(0, 0x78, 0x6E);
                Assign(0, 0x87, 0x18);
                Assign(0, 0xCF, 0x33);
                Assign(1, 0xE6, 0x98);
                Assign(1, 0x66, 0x98);
                Assign(1, 0x33, 0x18);
                Assign(1, 0x33, 0x98);
                Assign(1, 0x99, 0x18);
                Assign(1, 0x66, 0x0F);
                Assign(1, 0xF3, 0xCC);
                Assign(1, 0xE6, 0x18); // 1/4
                Assign(1, 0x66, 0x8F);
                Assign(1, 0x66, 0x87);
                Assign(1, 0x66, 0x18);
                Assign(1, 0x99, 0x98);
                Assign(1, 0x93, 0x18);
                Assign(1, 0xB3, 0x98);
                Assign(2, 0x78, 0x98);
                Assign(2, 0x9E, 0x18);
                Assign(2, 0xCF, 0x98);
                Assign(2, 0x3C, 0x18); 
                Assign(2, 0x3C, 0x98);
                Assign(2, 0x9C, 0x18);
                Assign(2, 0x9E, 0x98);
                Assign(2, 0x1E, 0x33);
                Assign(2, 0x78, 0x0F);
                Assign(2, 0xCF, 0x18);
                Assign(2, 0xEF, 0x18);
                Assign(2, 0xEF, 0x98);
                Assign(2, 0x1E, 0x03);
                Assign(2, 0xE7, 0x98);
                Assign(2, 0x1E, 0xF3);
                Assign(2, 0x78, 0x8F);
                Assign(2, 0x78, 0x87);
                Assign(2, 0x78, 0x18);
                Assign(2, 0xDE, 0x98);
                Assign(2, 0x8F, 0x86);
                Assign(2, 0x1E, 0xC3);
                Assign(2, 0x1E, 0x13);
                Assign(2, 0x1E, 0x83);
                Assign(2, 0x9C, 0x98);
                Assign(2, 0xE7, 0x18);
                Assign(2, 0xBC, 0x98);
                Assign(2, 0x1E, 0x9B);
                Assign(2, 0x1E, 0xB3);
                Assign(2, 0x1E, 0x99);
                Assign(2, 0x78, 0x07);
                Assign(2, 0x1E, 0x1B);
                Assign(2, 0x1E, 0x86); // 2/8
                Assign(3, 0x86, 0x98);
                Assign(3, 0x86, 0x18);
                Assign(3, 0xF8, 0x98);
                Assign(3, 0xC3, 0x18);
                Assign(3, 0xC3, 0x98);
                Assign(3, 0xE3, 0x98);
                Assign(3, 0xBC, 0x66);
                Assign(3, 0xFC, 0xCC);
                Assign(3, 0xF8, 0x18);
                Assign(3, 0xFC, 0x98);
                Assign(3, 0xE3, 0x18);
                Assign(3, 0xFC, 0x18);
                Assign(3, 0xE1, 0x98);
                Assign(4, 0x98, 0x98);
                Assign(4, 0xF3, 0x98);
                Assign(4, 0xCC, 0x18);
                Assign(4, 0xCC, 0x98);
                Assign(4, 0xFB, 0x98);
                Assign(4, 0xF9, 0x98);
                Assign(4, 0x98, 0x18);
                Assign(4, 0x7E, 0xCC);
                Assign(4, 0xF3, 0x18);
                Assign(4, 0x7E, 0x0C);
                Assign(4, 0xFB, 0x18);
                Assign(4, 0x7E, 0x64);
                Assign(4, 0x3F, 0x66);
                Assign(4, 0x7E, 0x66);
                Assign(4, 0xEC, 0x18);
                Assign(4, 0x7E, 0x86);
                Assign(5, 0x78, 0xCF);
                Assign(5, 0xF0, 0xCF);
                Assign(5, 0xE0, 0x9E);
                Assign(5, 0x3C, 0x66);
                Assign(5, 0x70, 0xCF);
                Assign(5, 0x9C, 0x66);
                Assign(5, 0x9E, 0x66);
                Assign(5, 0x3E, 0x66);
                Assign(5, 0x9E, 0x06);
                Assign(5, 0xBC, 0x86);
                Assign(5, 0x3C, 0x86);
                Assign(5, 0x3C, 0x06);
                Assign(5, 0xCF, 0x66);
                Assign(5, 0x3C, 0xE6);
                Assign(5, 0x9E, 0x86);
                Assign(5, 0x9E, 0xE6);
                Assign(5, 0x3E, 0x06);
                Assign(5, 0xBC, 0x06);
                Assign(5, 0x9C, 0x86);
                Assign(6, 0x78, 0x78);
                Assign(6, 0xCF, 0x3C);
                Assign(6, 0x86, 0x66);
                Assign(6, 0x6F, 0x3C);
                Assign(6, 0x4F, 0x3C);
                Assign(6, 0x78, 0xC3);
                Assign(6, 0x78, 0xE1);
                Assign(6, 0x3C, 0x78);
                Assign(6, 0x9E, 0x78);
                Assign(6, 0x86, 0x86);
                Assign(6, 0xCF, 0x78);
                Assign(6, 0x86, 0x1E);
                Assign(6, 0x78, 0xE3);
                Assign(6, 0xBC, 0x78);
                Assign(6, 0x67, 0x3C);
                Assign(6, 0x9C, 0x78);
                Assign(6, 0x78, 0xC1);
                Assign(7, 0x86, 0x78);
                Assign(7, 0x78, 0x9E);
                Assign(7, 0x78, 0x3C);
                Assign(7, 0x78, 0x9C);
                Assign(7, 0x3C, 0xCF);
                Assign(7, 0x3C, 0x9E);
                Assign(7, 0xC3, 0x78);
                Assign(7, 0x78, 0x1E);
                Assign(7, 0x78, 0x1C);
                Assign(7, 0xE3, 0x78);
                Assign(7, 0x78, 0xBC);
                Assign(7, 0x7B, 0x3C);
                Assign(7, 0x1C, 0xCF);
                Assign(7, 0x3C, 0x9F);
                Assign(7, 0x3C, 0xCE);
                Assign(8, 0x98, 0x78);
                Assign(8, 0xF3, 0x3C);
                Assign(8, 0xCC, 0x78);
                Assign(8, 0x1E, 0xCF);
                Assign(8, 0x1E, 0x6F);
                Assign(8, 0x73, 0x3C);
                Assign(8, 0xE6, 0x78);
                Assign(8, 0x79, 0x3C);
                Assign(8, 0xF3, 0x78);
                Assign(8, 0x1E, 0x66);
                Assign(9, 0xE0, 0x78);
                Assign(9, 0x7C, 0x3C);
                Assign(9, 0xFC, 0x3C);
                Assign(9, 0x7E, 0x3C);
                Assign(9, 0x3F, 0xCF);
                Assign(9, 0x3F, 0x9E);
                Assign(9, 0x78, 0x33);
                Assign(9, 0xF0, 0x78);
                Assign(9, 0xF8, 0x78);
                Assign(9, 0xFC, 0x78);
            }

            private void Assign(int value, byte lowByte, byte highByte)
            {
                if (_assigned.Keys.Any(x => x[0] == lowByte && x[1] == highByte))
                    throw new Exception();

                _assigned.Add(new[] { lowByte, highByte }, value);
            }

            public void Push(byte value)
            {
                switch (_state)
                {
                    case ParseState.AwaitStartByte1:
                        if (value == StartByte1)
                            _state = ParseState.AwaitStartByte2;
                        break;
                    case ParseState.AwaitStartByte2:
                        if (StartByte2.Contains(value))
                            _state = ParseState.AwaitCommandByte1;
                        else
                            _state = ParseState.AwaitStartByte1;
                        break;
                    case ParseState.AwaitCommandByte1:
                        if (value == CommandByte1)
                            _state = ParseState.AwaitCommandByte2;
                        else
                            _state = ParseState.AwaitStartByte1;
                        break;
                    case ParseState.AwaitCommandByte2:
                        if (value == CommandByte2)
                            _state = ParseState.PushData;
                        else
                            _state = ParseState.AwaitStartByte1;
                        break;
                    case ParseState.PushData:
                        //if (value == StopByte1 || value == StopByte2)
                        //    FinishData();

                        _data[_dataPointer++] = value;

                        if (_dataPointer >= MaxDataCount)
                            FinishData();

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private void FinishData()
            {
                byte[] data = new byte[_dataPointer];
                Array.Copy(_data, 0, data, 0, _dataPointer);

                _state = ParseState.AwaitStartByte1;
                _dataPointer = 0;

                Debug.Write(string.Join(" ", data.Select(x => Convert.ToString(x, 2).PadLeft(8, '0'))) + " > ");

                Debug.Write(string.Join("", data.Select(x => $"{x:X2}")) + " > ");
                
                for (int i = 0; i < data.Length; i = i + 2)
                {
                    bool found = false;
                    foreach (var assigned in _assigned)
                    {
                        if (assigned.Key[0] == data[i] && assigned.Key[1] == data[i + 1])
                        {
                            Debug.Write(assigned.Value + "");
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        Debug.Write($"[{data[i]:X2} {data[i+1]:X2}]");
                    }

                    if (i == 2)
                        Debug.Write(":");
                }

                Debug.Write($" > {BitConverter.ToUInt32(data, 0)} | {BitConverter.ToUInt32(data, 4)}");

                Debug.Write($" > {BitConverter.ToUInt64(data, 0)}");

                Debug.Write(Environment.NewLine);
            }

            private enum ParseState
            {
                AwaitStartByte1,
                AwaitStartByte2,
                AwaitCommandByte1,
                AwaitCommandByte2,
                PushData
            }
        }

        [TestMethod]
        public void ScoutSendTest()
        {
            var a = new ScoutTimeOutput();
            a.Push(new TimeSpan(00, 7, 26));
        }
    }
}