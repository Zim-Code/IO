using NUnit.Framework;
using System;
using System.Windows.Forms;
using ZimCode.IO;
using ZimCode.IO.Platforms.Windows;

namespace IO.Platforms.Windows.Tests
{
    [TestFixture]
    public class Test_OpenFileDialogImporters
    {
        [Test]
        public void OpenFileDialogTest()
        {
            XmlImporter importer = new XmlImporter();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.AddImporterFileTypes(importer);
        }
    }
}

