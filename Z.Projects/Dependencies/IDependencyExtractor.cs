using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Components;

namespace Z.Projects.Dependencies
{
    public interface IDependencyExtractor
    {
        IEnumerable<IDependency> GetDepedencies(IProjectComponent component);
    }
}
