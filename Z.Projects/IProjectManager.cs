using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Components;

namespace Z
{
    public interface IProjectRelationship
    {        
        IProject RelatedProject { get; set; }
        IProjectComponent RelationSource { get; set; }
    }

    public interface IProject : IProjectComponent
    {
        IEnumerable<IProjectComponent> Components { get; }
        IEnumerable<IProjectRelationship> Relationships { get; }
    }
}
