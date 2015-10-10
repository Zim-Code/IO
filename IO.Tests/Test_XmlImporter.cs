using NUnit.Framework;
using System;
using ZimCode.IO;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;


namespace IO.Tests
{
    [TestFixture]
    public class Test_XmlImporter
    {
        const string SimpleXml = 
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + 
            "<note>" +
                "<to>Tove</to>" + 
                "<from>Jani</from>" +
                "<heading>Reminder</heading>" +
                "<body>Don't forget me this weekend!</body>" + 
            "</note>";

        [Test]
        public async Task XmlImporterTest()
        {
            XmlImporter importer = new XmlImporter();

            ProgressReporter reporter = new ProgressReporter(100);
            int reportCount = 0;
            reporter.ProgressChanged += (s, e) => reportCount++;

            XDocument result = (XDocument)(await importer.ImportAsync(new MemoryStream(Encoding.ASCII.GetBytes(SimpleXml)), reporter));

            if (importer.ErrorMessage != null)
                Assert.Fail("There was an error message:\n{0}", importer.ErrorMessage);

            Assert.IsTrue(importer.CompletedWithoutError, "Completed with errors");

            Assert.NotNull(result, "The result was null");

            XElement element = result.Descendants("heading").FirstOrDefault();
            Assert.AreEqual("Reminder", element.Value, "Could not decode value");

            Assert.AreEqual(1, reportCount, "XmlImporter did not report progress.");
            Assert.AreEqual(100, reporter.Progress);
        }
    }
}

