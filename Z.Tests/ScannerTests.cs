using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Z.Scanner;
using Z.Scanner.Clues;

namespace Z.Tests
{
    [TestFixture]
    class ScannerTests
    {
        [Test]
        public void Extension_Csproj_Matched()
        {
            var filenameClues = new List<IScanClue<string>>() { new FileNameScanClue("*.csproj") };
            FileNameScanner scanner = new FileNameScanner(filenameClues);

            List<string> fileNames = new List<string>() { "moshe.csproj" };
            IEnumerable<KeyValuePair<IScanClue<string>, string>> found = scanner.Scan(fileNames);
            foreach (var item in found)
            {
                Console.WriteLine(item);
            }
        }
    }
}
