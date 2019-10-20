using System.IO;
using BaSta.IO.Dbase;
using BaSta.Test.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaSta.Test.IO.Dbase
{
    [TestClass]
    public class DbaseParserTest
    {
        [TestMethod]
        public void DbaseParser_ConstructorTest()
        {
            byte[] actionFileData = ResourceHelper.TryExtractResource(EmbeddedResources.Example2ActionsDatabase);
            DbaseParser parser = new DbaseParser(new MemoryStream(actionFileData));
        }
    }
}