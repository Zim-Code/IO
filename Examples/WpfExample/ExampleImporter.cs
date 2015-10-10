using System;
using System.IO;
using System.Threading.Tasks;
using ZimCode.IO;

namespace WpfExample
{
    class ExampleImporter : BaseImporter<ExampleContent>
    {
        public ExampleImporter()
            : base("Example Files", "exampl")
        {
        }

        protected override IGetOperations GetLoader(Stream stream)
        {
            return new ExampleLoader(stream);
        }
    }
}