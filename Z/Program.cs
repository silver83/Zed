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
using Z.Projects.Scanner;

namespace Z
{


    class Program
    {
        /// <summary>
        /// Zed is an Environment Validation tool, allowing deployment validation - alerts and fast editing for unknown host/connectionstring/endpoint etc
        /// * scan a deployment root, detecting components (projects)
        /// * each component should contain all environment dependant records
        /// * these records should be saved and compared against on each deployment
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Infrastructure.Load();
            Log.Application.Info("Message");

            //IClueRepository repo = ServiceLocator.Current.GetInstance<IClueRepository>();
            //IEnumerable<IScanClue<string>> clues = repo.CluesForScanner(typeof(FileNameScanner));
            //FileNameScanner scanner = new FileNameScanner(clues);


            //scanner.Scan();
            
            //IScanner Scaner;
            //Scanr.Clues = new List<IScanClue>();

            //FileScanner scanner = new FileScanner(path, clues);
            //scanner.scan();
            //IProjectManager manager = ServiceLocator.Current.GetInstance<IProjectManager>();            
            //IProjectManager manager = ServiceLocation.
            //IEnumerable<Project> = ScanDirectory(rootDir);


            //scan for projects, so that we know for each project its environemnt-dependant configuration (endpoint hosts and methods, connectionStrings, etc)
            //file scanning should be a one-off, throwing events for expected tags/lines

            //web csproj with webservice endpoints Scan flow

            //CSharpProj:FileExtention("*.csproj")
            //CSharpConfig:File("*.config", CSharpProj)
            //ConnectionString:XmlTag("<connectionString>{0}</connectionString>", CSharpConfig)
        
        
            //first - find problems with refs/endpoints/queues
        }
    }
}
