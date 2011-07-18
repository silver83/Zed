
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Z.Scanner;

namespace Z.Projects
{
    /// <summary>
    /// Takes a directory and finds projects
    /// </summary>
    public class ProjectManager
    {
        public IEnumerable<IProject> FindProjects(string directory, BaseScanner<string> scanner)
        {
            var files = GetDirScanner(directory);
            var matchedClues = scanner.Scan(files);
            LinkedList<List<string>> paths = new LinkedList<List<string>>();
            
            foreach (var matchedClue in matchedClues)
            {
                
            }

            yield break;
        }

        private IEnumerable<string> GetDirScanner(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            foreach (var file in files)
                yield return file;

            string[] subdirs = Directory.GetDirectories(directory);
            foreach (var dir in subdirs)
                foreach (var file in GetDirScanner(dir))
                    yield return file;
        }
    }
}
