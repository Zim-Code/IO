using System;
using System.Xml.Linq;
using System.IO;

namespace ZimCode.IO
{
    /// <summary>
    /// Importer to load an XDocument object.
    /// </summary>
    public class XmlImporter : BaseImporter<XDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZimCode.IO.XmlImporter"/> class.
        /// </summary>
        public XmlImporter(params string[] fileExtensions)
            : base("Xml Importer", fileExtensions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZimCode.IO.XmlImporter"/> class.
        /// </summary>
        public XmlImporter()
            : base("Xml Importer", "XML")
        {
        }

        /// <summary>
        /// Gets a new loader to execute the operations from.
        /// </summary>
        /// <returns>The loader.</returns>
        /// <param name="stream">Stream.</param>
        protected override IGetOperations GetLoader(Stream stream)
        {
            return new XmlLoader(stream);
        }
    }
}

