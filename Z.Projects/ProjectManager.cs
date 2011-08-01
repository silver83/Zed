
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
        private static List<string> PrefixExclusions = new List<string>() { "." };
        
        /// <summary>
        /// Each subdir of <paramref name="directory"/> is considered a project.
        /// </summary>
        /// <param name="directory">the root path to start scanning from</param>
        /// <param name="scanner">the scanner to use</param>
        /// <returns></returns>
        public IEnumerable<ProjectBuilder> CreateProjectBuilders(string directory, IResourceIdFactory resourceIdFactory, IComponentFactory componentFactory)
        {
            foreach (var dir in Directory.GetDirectories(directory))
            {
                if (IsExcluded(dir))
                    continue;

                var filter = new MultiFilter<string>(Configuration.FileFilters);
                ProjectBuilder builder = new ProjectBuilder(dir, filter, resourceIdFactory, componentFactory);
               
                yield return builder;
            }
        }
                
        private bool IsExcluded(string dir)
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
