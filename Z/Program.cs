using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Practices.ServiceLocation;
using Z.Common.Logging;
using Z.Common;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using Z.Projects;
using Z.Scanner;
using Z.Projects.Components;

namespace Z
{
    class Program
    {
        private static void Usage()
        {
            Console.WriteLine("Z [directory path]");
        }

        /// <summary>
        /// Zed is an Environment Validation tool, allowing deployment validation - alerts and fast editing for unknown host/connectionstring/endpoint etc
        /// * scan a deployment root, detecting components (projects)
        /// * each component should contain all environment dependant records
        /// * these records should be saved and compared against on each deployment
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string rootDir = @"E:\products\Services";

            Infrastructure.Load();
            IResourceIdFactory resourceIdFactory = new ResourceIdFactory();
            IComponentFactory componentFactory = new ComponentFactory();
            var projectManager = new ProjectManager();
            var builders = projectManager.CreateProjectBuilders(rootDir, resourceIdFactory, componentFactory);

            List<IProject> projects = new List<IProject>();
            foreach (var builder in builders)
            {
                builder.PopulateProjectComponents();
                projects.Add(builder.Project);
            }

            foreach (var proj in projects)
            {
                Console.WriteLine(proj.ResourceId.Uri.LocalPath.Replace(rootDir, ""));
                foreach (var comp in proj.Components)
                {
                    Console.WriteLine("\t" + comp.ResourceId.Uri.AbsoluteUri.Replace(proj.ResourceId.Uri.AbsoluteUri, ""));
                }
            }
            Console.ReadKey();

            //var depsManager = new DependencyManager();
            //var deps = depsManager.Extract(projects);
            //depsManager.Verify(deps);
            //depsManager.Save(deps);

            //find configuration files and detect their type
            //according to type extract dependencies
            //verify dependencies and persist


            //var componentFactory = ServiceLocator.Current.GetInstance<IComponentFactory>();

            //var fileUris = files.Select(x => new ComponentURI(x));

            //Configuration.ConfigTypes

            ////identify and add configuration components
            //var components = componentFactory.CreateComponents(files, x => filtered.Add(x));

            //project.AddComponents(components);
        }

       
    }
}
