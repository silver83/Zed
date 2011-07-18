using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Components;

namespace Z.Projects.Dependencies
{
    /// <summary>
    /// provides a dependency extractor for the supplied component. 
    /// for example - takes a configuration component and finds an extractor that can parse that type of config file and find all dependencies
    /// </summary>
    public interface IDependencyExtractorProvider
    {
        IDependencyExtractor GetDependencyExtractor(IProjectComponent component);
    }
}
