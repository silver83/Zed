
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Z.Common.Logging;
using Z.Projects.Components;
using Microsoft.Practices.ServiceLocation;
using Z.Projects.Components.Configuration;
using System.Xml.Linq;
using Z.Scanner.Filters;
using Z.Scanner;

namespace Z.Projects
{
    public interface IComponentFactory
    {
        IProjectComponent CreateComponent(string path, Tuple<ResourceId, ResourceType> typedResource);
    }

    /// <summary>
    /// Takes a directory and finds projects
    /// </summary>
    public class ProjectManager
    {
        private static List<string> PrefixExclusions = new List<string>() { ".", "Backup" };

        /// <summary>
        /// Each subdir of <paramref name="rootDirectory"/> is considered a project.
        /// </summary>
        /// <param name="rootDirectory">the root path to start scanning from</param>
        /// <param name="scanner">the scanner to use</param>
        /// <returns></returns>
        public IEnumerable<ProjectBuilder> CreateProjectBuilders(string rootDirectory, IResourceIdFactory resourceIdFactory, IComponentFactory componentFactory)
        {
            foreach (var dir in GetProjectDirectories(rootDirectory))
            {
                if (IsExcluded(dir))
                    continue;

                var filter = new MultiFilter<string>(Configuration.FileFilters);
                ProjectBuilder builder = new ProjectBuilder(dir, filter, resourceIdFactory, componentFactory);

                yield return builder;
            }
        }

        private IEnumerable<string> GetProjectDirectories(string directory)
        {
            foreach (var subdir in Directory.GetDirectories(directory))
            {
                if (IsExcluded(subdir))
                    continue;
                if (IsProject(subdir))
                    yield return subdir;
                else
                    foreach (var subDirProject in GetProjectDirectories(subdir))
                        yield return subDirProject;
            }
        }

        private static bool IsProject(string subdir)
        {
            string dirName = Path.GetFileName(subdir);
            string[] endings = new string[] { ".exe", ".exe.config", ".dll", ".dll.config" };
            endings = endings.Select(x => string.Format("{0}{1}", dirName, x)).ToArray();

            var hasExeAndConfig = (from f in Directory.GetFiles(subdir)
                                   from e in endings
                                   where f.EndsWith(e)
                                   select f).Count();

            var isProject = hasExeAndConfig >= 2;
            return isProject;
        }

        private static bool IsExcluded(string dir)
        {
            string fileName = Path.GetFileName(dir);
            foreach (var pattern in PrefixExclusions)
            {
                if (fileName.StartsWith(pattern))
                    return true;
            }
            return false;
        }
    }
}
