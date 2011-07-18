using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Dependencies;

namespace Z.Projects.Components.Configuration
{
    public abstract class BaseConfigrationComponent : IConfigurationComponent
    {
        public abstract bool DefinesProject { get; }
        public abstract string Name { get; }

        public IEnumerable<IDependency> FindDependencies(IDependencyExtractorProvider extractorProvider)
        {
            IDependencyExtractor extractor = extractorProvider.GetDependencyExtractor(this);
            return extractor.GetDepedencies(this);
        }
    }

}
