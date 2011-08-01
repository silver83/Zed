using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner
{
    /// <summary>
    /// A set of resources of the same scheme
    /// </summary>
    public class ResourceSet
    {
        public IEnumerable<ResourceId> Resources { get; set; }
        public string Scheme{ get; set; }

        public ResourceSet(IEnumerable<ResourceId> resources, string scheme)
        {
            Resources = resources;
            Scheme = scheme;
        }
    }
}
