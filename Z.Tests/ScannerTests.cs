using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Z.Projects.Scanner;

namespace Z.Tests
{
    [TestFixture]
    class ScannerTests
    {
        [Test]
        public void Extension_Csproj_Matched()
        {
            var filenameClues = new List<IScanClue<string>>();
            filenameClues.Add(new FileNameScanClue("*.csproj"));
            FileNameScanner scanner = new FileNameScanner(filenameClues);


            List<string> fileNames = new List<string>();
            fileNames.Add("");
            scanner.Scan(fileNames);
        }
    }
}
