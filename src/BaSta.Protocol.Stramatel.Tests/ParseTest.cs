using BaSta.Protocol.Stramatel.Tests.Resources;
using BaSta.Tests.Resources;
using Xunit;

namespace BaSta.Protocol.Stramatel.Tests;

public class StramatelProtocolMessageParserTests
{
    [Fact]
    public void StramatelProtocolMessageParser_FileTest()
    {
        byte[] traceDump = ResourceHelper.ExtractFile(EmbeddedResources.TraceDump);

        StramatelProtocolMessageParser parser = new();
        parser.Parse(traceDump);

        Assert.Equal(134, parser.MessagesAvailable);
    }
}