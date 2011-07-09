using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using Rhino.Mocks;
using Z.Projects.Scanner;
using Z.Projects;

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

            IScanClueProvider provider = MockRepository.GenerateMock<IScanClueProvider>();
            provider.Expect(x=>provider.GetScanClues<string>(typeof(string)))
                .IgnoreArguments()
                .Return(clues);

            ProjectScanner scanner = new ProjectScanner();
            IEnumerable<IProject> projects = scanner.Scan(dir, provider);
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
