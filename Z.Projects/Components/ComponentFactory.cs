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
            var resourceType = typedResource.Item2;
            return (IProjectComponent)Activator.CreateInstance(resourceType.ComponentType, typedResource.Item1);
        }
    }
}
