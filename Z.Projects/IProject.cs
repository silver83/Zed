using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Components;

namespace Z.Projects
{
    public interface IProjectRelationship
    {        
        IProject RelatedProject { get; set; }
        IProjectComponent RelationSource { get; set; }
    }

    public interface IProject : IProjectComponent
    {
        ProjectBuilder Builder { get; set; }
        IEnumerable<IProjectComponent> Components { get; }
        IEnumerable<IProjectRelationship> Relationships { get; }

        void AddComponents(IEnumerable<IProjectComponent> components);
        void AddComponent(IProjectComponent component);
    }
}
