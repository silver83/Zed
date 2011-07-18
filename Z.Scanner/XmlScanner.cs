using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Z.Scanner
{
    public class XmlScanner : BaseStreamScanner<XElement>
    {
        public XmlScanner(IEnumerable<IScanClue<XElement>> cluesToScan) : base(cluesToScan) { }

        protected override IEnumerable<XElement> LoadStream(Stream inputStream)
        {
            XmlReader reader = XmlReader.Create(inputStream);
            return XElement.Load(reader).Descendants();
        }
    }
}
