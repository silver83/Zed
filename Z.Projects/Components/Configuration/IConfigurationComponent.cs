using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Dependencies;

namespace Z.Projects.Components.Configuration
{
    public interface IConfigurationComponent : IProjectComponent
    {
        bool DefinesProject { get; }
        IEnumerable<IDependency> FindDependencies(IDependencyExtractorProvider extractorProvider);
    }
}
