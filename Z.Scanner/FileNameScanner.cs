using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner
{
    public class FileNameScanner : BaseScanner<string>
    {
        public FileNameScanner(IEnumerable<IScanClue<string>> cluesToSearch) : base(cluesToSearch) { }
    }
}
