using System;
using System.Xml.Linq;
using System.IO;

namespace ZimCode.IO
{
    public class XmlImporter : BaseImporter<XDocument>
    {
        public XmlImporter()
            : base("Xml Importer", "XML")
        {
        }

        protected override IGetOperations GetLoader(Stream stream)
        {
            return new XmlLoader(stream);
        }
    }
}

