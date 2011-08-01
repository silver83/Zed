using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Scanner;
using Z.Projects.Components.Configuration;

namespace Z.Projects.Components
{
    public class ComponentFactory : IComponentFactory
    {
        public IProjectComponent CreateComponent(string path, Tuple<ResourceId, ResourceType> typedResource)
        {
            return new StructureMapConfigurationComponent(typedResource.Item1);
        }
    }
}
