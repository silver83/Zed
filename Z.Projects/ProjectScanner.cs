
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Z.Common.Logging;

namespace Z.Projects
{
    /// <summary>
    /// Takes a directory and finds projects
    /// </summary>
    public class ProjectManager
    {
        /// <summary>
        /// Each subdir of <paramref name="directory"/> is considered a project.
        /// </summary>
        /// <param name="directory">the root path to start scanning from</param>
        /// <param name="scanner">the scanner to use</param>
        /// <returns></returns>
        //public IEnumerable<IProject> FindProjects(string directory)
        //{
        //    List<Project> projects = new List<Project>();
        //    foreach (var dir in Directory.GetDirectories(directory))
        //    {
        //        if (Path.GetFileName(dir).StartsWith("."))
        //            continue;

        //        Project project = new Project(dir);
        //        var files = GetDirScanner(dir);
        //        //var matchedClues = scanner.Scan(files);

        //        //foreach (var matchedClue in matchedClues)
        //        //{

        //        //}

        //        //yield return project;
        //    }
        //}

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
