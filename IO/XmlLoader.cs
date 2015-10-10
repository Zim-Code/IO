using System;
using System.Xml.Linq;
using System.IO;

namespace ZimCode.IO
{
    class XmlLoader : IGetOperations
    {
        Stream stream;

        public XmlLoader(Stream stream)
        {
            this.stream = stream;
        }

        public System.Collections.Generic.IEnumerable<Operation> GetOperations()
        {
            yield return Operation.Generate<XDocument>(OpenDocument);
        }

        private XDocument OpenDocument()
        {
            return XDocument.Load(stream);
        }
	}

}

