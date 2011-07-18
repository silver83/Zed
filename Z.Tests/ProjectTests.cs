using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using Rhino.Mocks;
using Z.Projects;
using Z.Scanner;
using Z.Scanner.Clues;

namespace Z.Tests
{
    [TestFixture]
    public class ProjectTests
    {
        [Test]
        public void start()
        {
            string dir = Directory.GetCurrentDirectory();
            dir = Directory.GetParent(dir).Parent.FullName;

            List<IScanClue<string>> clues = new List<IScanClue<string>>() { new FileNameScanClue("*.csproj") };
            FileNameScanner fileNameScanner = new FileNameScanner(clues);

            ProjectManager scanner = new ProjectManager();
            IEnumerable<IProject> projects = scanner.FindProjects(dir, fileNameScanner);
            projects.ToList();

            //pipeline :
            //directory -> enumerate all files and subdirs -> matches filenames -> configuration keys -> Projects 
            //IEnumerable<string> directories;

            //IEnumerable<KeyValuePair<IProject, IProjectFile>> matchedFiles = 
            //                                                        from d in directories
            //                                                        group d by 
            //                                                        group 

            //IEnumerable<KeyValuePair<IProject, IProjectComponent> a = from f in matchedFiles
            //                                                      select f.Parse();


            //IEnumerable<IProject> project = 
        }
    }
}
