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
        string Name { get; set; }
        string Path { get; set; }
    }

    public interface IConfigurationComponent : IProjectComponent
    {
        
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
