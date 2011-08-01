using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Z.Scanner.Filters;
using Z.Common.Extensions;
using Z.Scanner;
using Microsoft.Practices.ServiceLocation;

namespace Z.Projects
{
    public class ProjectBuilder
    {
        private IResourceIdFactory _resourceIdFactory;
        private IComponentFactory _componentFactory;

        private string _path;
        private IProject _project;
        private ResourceSet _files;
        private IFilter<string> _fileFilter;        

        public IProject Project
        {
            get
            {
                if (_project == null)
                    CreateProject(_resourceIdFactory);

                return _project;
            }
        }
        public string Path { get { return _path; } }

        private void CreateProject(IResourceIdFactory resourceIdFactory)
        {
            _project = new Project(resourceIdFactory.ResourceFromFilePath(_path));
            _project.Builder = this;

            var files = GetRecursiveFileEnumeration(_path);
            files = _fileFilter.Filter(files);

            var resources = files.Select(x => resourceIdFactory.ResourceFromFilePath(x));
            resources = resources.MemoizeAll();

            _files = new ResourceSet(resources, "file");
        }

        public ProjectBuilder(string projectPath, IFilter<string> fileFilter, IResourceIdFactory resourceIdFactory, IComponentFactory componentFactory)
        {
            if (string.IsNullOrEmpty(projectPath) || !Directory.Exists(projectPath))
                throw new ArgumentException(string.Format("the projectPath supplied was null, empty, or a non existant path : '{0}'", projectPath ?? "[null]"));

            if (fileFilter == null)
                throw new ArgumentNullException("fileFilter");

            _path = projectPath;
            _fileFilter = fileFilter;

            _resourceIdFactory = resourceIdFactory;
            _componentFactory = componentFactory;
        }

        //from resource to resourceType, from resourceType to component
        public void PopulateProjectComponents()
        {
            var project = this.Project;

            var typedResources = TypedResources(_files);
            foreach (var typedResource in typedResources)
            {
                var component = _componentFactory.CreateComponent(_path, typedResource);
                project.AddComponent(component);
            }
        }

        private IEnumerable<Tuple<ResourceId, ResourceType>> TypedResources(ResourceSet resourceSet)
        {
            var matchedResources = resourceSet.Resources.Select(x => new { matched = false, resource = x });
            foreach (var type in Configuration.ResourceTypesBySchema[resourceSet.Scheme])
            {
                foreach (var rsrc in matchedResources)
                {
                    if (!rsrc.matched && rsrc.resource.IsOfType(type))
                        yield return new Tuple<ResourceId, ResourceType>(rsrc.resource, type);
                }
            }
        }

        private IEnumerable<string> GetRecursiveFileEnumeration(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            foreach (var file in files)
                yield return file;

            string[] subdirs = Directory.GetDirectories(directory);
            foreach (var dir in subdirs)
                foreach (var file in GetRecursiveFileEnumeration(dir))
                    yield return file;
        }
    }
}
