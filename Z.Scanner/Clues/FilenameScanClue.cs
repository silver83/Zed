using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Z.Common;
using System.Text.RegularExpressions;

namespace Z.Scanner.Clues
{
    public class FileNameScanClue : IScanClue<string>
    {
        private Regex _pattern;

        public FileNameScanClue(string filenamePattern)
        {
            _pattern = filenamePattern.AsLikeRegex();
        }

        public bool Match(string fileName)
        {
            return _pattern.IsMatch(fileName);
        }
    }
}
