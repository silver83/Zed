using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Scanner.Constraints
{
    public class OrConstraint : IResourceTypeConstraint
    {
        private IResourceTypeConstraint _constraint1, _constraint2;

        public OrConstraint(IResourceTypeConstraint constraint1, IResourceTypeConstraint constraint2)
        {
            _constraint1 = constraint1;
            _constraint2 = constraint2;
        }

        public bool IsMetBy(ResourceId componentId)
        {
            return (_constraint1.IsMetBy(componentId) || _constraint2.IsMetBy(componentId));
        }

        public bool IsRelevantToScheme(string uriScheme)
        {
            return _constraint1.IsRelevantToScheme(uriScheme) && _constraint2.IsRelevantToScheme(uriScheme);
        }
    }
}
