using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Z.Scanner.Filters
{
    public class FileExtensionFilter : IFilter<string>
    {
        private IEnumerable<string> _extensions;

        public FileExtensionFilter(IEnumerable<string> extensions)
        {
            _extensions = extensions;
        }
        public FileExtensionFilter(string extensionsSeperatedByComma)
            : this(extensionsSeperatedByComma.Split(','))
        {
        }
        public IEnumerable<string> Filter(IEnumerable<string> elems)
        {
            return from e in elems
                   where _extensions.Contains(Path.GetExtension(e))
                   select e;
        }
    }
}
