using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Scanner.Constraints;

namespace Z.Scanner
{
    public static class ConstraintExtensions
    {
        public static IResourceTypeConstraint AsHasSiblingConstraint(this string pattern)
        {
            return new HasSiblingConstraint(pattern);
        }

        public static IResourceTypeConstraint AsUriRegexConstraint(this string pattern)
        {
            return new UriRegexConstraint(pattern);
        }

        public static IResourceTypeConstraint And(this IResourceTypeConstraint constraint1, IResourceTypeConstraint constraint2)
        {
            return new AndConstraint(constraint1, constraint2);
        }

        public static IResourceTypeConstraint Or(this IResourceTypeConstraint constraint1, IResourceTypeConstraint constraint2)
        {
            return new OrConstraint(constraint1, constraint2);
        }
    }
}
