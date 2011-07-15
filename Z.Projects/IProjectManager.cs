using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z
{
    public interface IProjectRelationship
    {        
        IProject RelatedProject { get; set; }
        IProjectComponent RelationSource { get; set; }
    }

    public interface IProjectComponent
    {
        string ComponentType { get; }
        string Name { get; }
    }

    public interface IDependency
    {
    }

    public interface IDependencyExtractor
    {
        IEnumerable<IDependency> GetDepedencies(IProjectComponent component);
    }

    public interface IDependencyExtractorProvider
    {
        IDependencyExtractor GetDependencyExtractor(string componentType);
    }

    public interface IConfigurationComponent : IProjectComponent
    {
        bool DefinesProject { get; }
        IEnumerable<IDependency> FindDependencies(IDependencyExtractorProvider extractorProvider);
    }

    public abstract class BaseConfigrationComponent : IConfigurationComponent
    {
        public abstract bool DefinesProject { get; }
        public abstract string ComponentType { get; }
        public abstract string Name { get; }

        public IEnumerable<IDependency> FindDependencies(IDependencyExtractorProvider extractorProvider)
        {
            IDependencyExtractor extractor = extractorProvider.GetDependencyExtractor(this.ComponentType);
            return extractor.GetDepedencies(this);
        }        
    }

    public abstract class XmlConfigurationComponent : BaseConfigrationComponent
    {
        private bool _definesProject;
        private string _componentType;
        private string _name;
        private string _path;

        public override bool DefinesProject { get { return _definesProject;  } }
        public override string ComponentType { get { return _componentType; } }
        public override string Name { get { return _name; } }
        public string Path { get { return _path; } }

        public XmlConfigurationComponent(string name, string path, string componentType, bool definesProject)
        {
            _name = name;
            _path = path;
            _componentType = componentType;
            _definesProject = definesProject;
        }
    }

    public interface IProject : IProjectComponent
    {
        IEnumerable<IProjectComponent> Components { get; set; }
        IEnumerable<IProjectRelationship> Relationships { get; }
    }

    public interface IProjectManager
    {
        void Add(IProject project);
    }
}
