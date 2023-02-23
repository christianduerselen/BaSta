using BaSta.Model;
using Xunit;

namespace BaSta.Protocol.Stramatel.Tests;

public class StramatelMessagePartParserTests
{
    [Fact]
    public void StramatelMessagePartParser_ParseStramatelPossessionTest()
    {
        Assert.Null(StramatelMessagePartParser.ParseStramatelPossession(0x5));
        Assert.Equal(PossessionState.None, StramatelMessagePartParser.ParseStramatelPossession(0x20));
        Assert.Equal(PossessionState.Home, StramatelMessagePartParser.ParseStramatelPossession(0x31));
        Assert.Equal(PossessionState.Guest, StramatelMessagePartParser.ParseStramatelPossession(0x32));
    }

    [Fact]
    public void StramatelMessagePartParser_ParseStramatelGameClockTest()
    {

    }
}