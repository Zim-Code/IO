using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZimCode.IO;

namespace WpfExample
{
    internal class ExampleLoader : IGetOperations
    {
        private Stream stream;

        public ExampleLoader(Stream stream)
        {
            this.stream = stream;
        }

        public IEnumerable<Operation> GetOperations()
        {
            yield return Operation.Generate("Opening File", () =>
            {
                Task.Delay(1000).Wait();
                return XDocument.Load(stream);
            });
            yield return Operation.ConsumeGenerate<XDocument, ExampleContent>("Reading File", (d) =>
            {
                Task.Delay(1000).Wait();
                return ExampleContent.Parse(d);
            });
        }
    }
}