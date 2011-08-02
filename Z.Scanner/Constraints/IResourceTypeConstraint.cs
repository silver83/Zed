using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner.Constraints
{
    public interface IResourceTypeConstraint
    {
        bool IsMetBy(ResourceId componentId);
        bool IsRelevantToScheme(string uriScheme);
    }
}
