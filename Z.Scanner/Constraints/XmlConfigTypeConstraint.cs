using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Z.Scanner.Constraints
{
    /// <summary>
    /// Verifies an xml file contains at least one element with the specified name
    /// </summary>
    public class XmlConfigTypeConstraint : IResourceTypeConstraint
    {
        private string _elementName;
        private IEnumerable<KeyValuePair<string, string>> _attributeValues;

        public XmlConfigTypeConstraint(string elementName, IEnumerable<KeyValuePair<string, string>> attributeValues = null)
        {
            _elementName = elementName;
            _attributeValues = attributeValues;
        }

        public bool IsMetBy(ResourceId componentId)
        {
            try
            {
                var doc = XElement.Load(componentId.Uri.LocalPath);
                var elems = from e in doc.DescendantsAndSelf()
                            where e.Name == _elementName
                            select e;

                if (_attributeValues == null)
                    return (elems.FirstOrDefault() != null);
                else if (_attributeValues.Count() > 0)
                    //add logic that compares attributeValues to elem.attributes - if an elem is found with all attributes - return true;
                    throw new NotImplementedException();
                else
                    return false;
            }
            catch
            {
                return false;
            }
           
        }
        public bool IsRelevantToScheme(string scheme)
        {
            return scheme == "file";
        }
    }
}
