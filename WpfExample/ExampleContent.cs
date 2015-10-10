using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WpfExample
{
    class ExampleContent
    {
        private ExampleContent() { }

        public IEnumerable<string> Items { get; private set; }

        internal static ExampleContent Parse(XDocument d)
        {
            return new ExampleContent
            {
                Items = d.Descendants("Item").Select(e => e.Attribute("Value").Value)
            };
        }

        public override string ToString()
        {
            string result = "";
            foreach (var item in Items)
                result += item + "\n";
            return result;
        }
    }
}
