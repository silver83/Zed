using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Dependencies;
using Z.Scanner;

namespace Z.Projects.Components.Configuration
{
    public interface IConfigurationComponent : IProjectComponent
    {
        IEnumerable<IDependency> FindDependencies(IDependencyExtractorProvider extractorProvider);
    }
}
