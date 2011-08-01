using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner
{
    public class ResourceIdFactory : IResourceIdFactory
    {
        public ResourceId ResourceFromFilePath(string filePath)
        {
            ResourceId id = new ResourceId();
            id.Uri = new Uri(filePath);
            return id;
        }
    }
}
