using System;
using ZimCode.IO;
using NUnit.Framework;

namespace IO.Tests
{
    [TestFixture]
    public class Test_ImporterManager
    {
        [Test]
        public void ImporterManagerTest()
        {
            ImporterManager manager = new ImporterManager();
            manager.AddImporter(new XmlImporter());

            BaseImporter importer = manager.GetImporterForFileExtension("xml");

            Assert.IsTrue(importer.GetType().Equals(typeof(XmlImporter)));
        }
    }
}

