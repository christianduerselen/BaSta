﻿using System.IO;
using BaSta.IO.Dbase;
using BaSta.Tests.Resources;
using Xunit;

namespace BaSta.Tests.IO.Dbase;

public class DbaseParserTest
{
    [Fact]
    public void DbaseParser_ConstructorTest()
    {
        byte[] actionFileData = ResourceHelper.ExtractFile(EmbeddedResources.Example2ActionsDatabase);
        DbaseParser parser = new DbaseParser(new MemoryStream(actionFileData));
    }
}