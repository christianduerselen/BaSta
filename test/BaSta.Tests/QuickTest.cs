using System;
using System.Threading;
using BaSta.Link;
using BaSta.Link.Bodet;
using BaSta.Link.Stramatel;
using BaSta.Link.SwissTiming;
using BaSta.Link.Wige;
using Xunit;

namespace BaSta.Tests;

public class QuickTest
{
    [Fact(Skip = "Requires additional running software")]
    public void ScoutSendTest()
    {
        ScoutTimeOutput output = new();
        MemoryTimeSyncSettingsGroup settings = new();

        output.LoadSettings("", settings);
        output.IsEnabled = true;
        output.Initialize();

        for (int i = 0; i < 60; i++)
        {
            output.Push(new TimeSpan(0, 7, i));
            Thread.Sleep(1000);
        }
    }

    [Fact(Skip = "Requires additional running software")]
    public void StramatelSerialInput_Test()
    {
        StramatelSerialInput input = new();
        MemoryTimeSyncSettingsGroup settings = new();
        settings.AddSetting("PortName", "COM7");
        input.LoadSettings("", settings);

        input.Initialize();

        while (true) { }
    }

    [Fact(Skip = "Requires additional running software")]
    public void WigeSerialInput_Test()
    {
        WigeSerialInput input = new();
        MemoryTimeSyncSettingsGroup settings = new();
        settings.AddSetting("PortName", "COM10");
        input.LoadSettings("", settings);

        input.Initialize();

        while (true) { }
    }

    [Fact(Skip = "Requires additional running software")]
    public void BodetSerialInput_Test()
    {
        BodetSerialInput input = new();
        MemoryTimeSyncSettingsGroup settings = new();
        settings.AddSetting("PortName", "COM5");
        input.LoadSettings("", settings);

        input.Initialize();

        while (true) { }
    }
}