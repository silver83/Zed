
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Scanner;
using System.IO;

namespace Z.Projects
{
    public interface IScanClueProvider
    {
        IEnumerable<IScanClue<T>> GetScanClues<T>(Type t);
    }

    /// <summary>
    /// Takes a directory and finds projects
    /// </summary>
    public class ProjectScanner
    {
        public IEnumerable<IProject> Scan(string directory, IScanClueProvider scanClueProvider)
        {
            IEnumerable<IScanClue<string>> clues = scanClueProvider.GetScanClues<string>(typeof(FileNameScanner));
            IEnumerable<string> files = ScanDirectory(directory);

            FileNameScanner scanner = new FileNameScanner(clues);
            var matchedClues = scanner.Scan(files); 
            foreach (var matchedClue in matchedClues)
            {
                Console.WriteLine(matchedClue);
            }

            yield break;
        }

        private IEnumerable<string> ScanDirectory(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            foreach (var file in files)
                yield return file;

            string[] subdirs = Directory.GetDirectories(directory);
            foreach (var dir in subdirs)
                foreach (var file in ScanDirectory(dir))
                    yield return file;
        }
    }
}
