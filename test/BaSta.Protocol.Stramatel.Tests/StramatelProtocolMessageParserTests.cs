using BaSta.Model;
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

        Assert.Equal(21267, parser.MessagesAvailable);
    }

    [Fact]
    public void StramatelProtocolMessageParser_FileApplyTest()
    {
        byte[] traceDump = ResourceHelper.ExtractFile(EmbeddedResources.TraceDump);

        StramatelProtocolMessageParser parser = new();
        parser.Parse(traceDump);

        Game game = new();

        while (parser.TryDequeue(out IStramatelMessage message))
        {
            message.ApplyTo(game);
        }
    }
}