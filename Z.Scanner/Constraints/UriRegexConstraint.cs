using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Z.Scanner.Constraints
{
    public class UriRegexConstraint : IResourceTypeConstraint
    {
        private string _pattern;

        public UriRegexConstraint(string pattern)
        {
            _pattern = pattern;
        }

        public bool IsMetBy(ResourceId componentId)
        {
            return Regex.IsMatch(componentId.Uri.AbsoluteUri, _pattern);
        }

        public bool IsRelevantToScheme(string uriScheme)
        {
            return true;
        }
    }
}
