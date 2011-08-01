using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner
{
    public class ResourceId
    {
        public Uri Uri { get; set; }
    }

    public interface IResourceIdFactory
    {
        ResourceId ResourceFromFilePath(string filePath);
    }
}
