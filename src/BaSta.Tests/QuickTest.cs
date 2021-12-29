using System;
using System.Threading;
using BaSta.TimeSync;
using BaSta.TimeSync.Input.Stramatel;
using BaSta.TimeSync.Output;
using Xunit;

namespace BaSta.Tests
{
    public class QuickTest
    {
        [Fact]
        public void ScoutSendTest()
        {
            var a = new ScoutTimeOutput();
            MemoryTimeSyncSettingsGroup settings = new MemoryTimeSyncSettingsGroup();

            a.LoadSettings("", settings);
            a.IsEnabled = true;
            a.Initialize();

            for (int i = 0; i < 60; i++)
            {
                a.Push(new TimeSpan(0, 7, i));
                Thread.Sleep(1000);
            }
        }

        [Fact]
        public void StramatelSerialInput_Test()
        {
            StramatelSerialInput input = new StramatelSerialInput();
            MemoryTimeSyncSettingsGroup settings = new();
            settings.AddSetting("PortName", "COM10");
            input.LoadSettings("", settings);

            input.Initialize();

            while (true) { }
        }
    }
}