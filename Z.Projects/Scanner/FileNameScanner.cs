using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Projects.Scanner
{
    public class FileNameScanner : BaseScanner<string>
    {
        public FileNameScanner(IEnumerable<IScanClue<string>> cluesToSearch) : base(cluesToSearch) { }
    }
}
