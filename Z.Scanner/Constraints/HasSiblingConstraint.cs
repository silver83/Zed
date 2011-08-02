using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Z.Scanner.Constraints
{
    public class HasSiblingConstraint : IResourceTypeConstraint
    {
        //can contain place holders such as [filename] [fileext]
        private string _constraintConfig;

        public HasSiblingConstraint(string constraintConfig)
        {
            _constraintConfig = constraintConfig;
        }

        public bool IsMetBy(ResourceId componentId)
        {
            string path = componentId.Uri.LocalPath;
            string dir = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string fileExt = Path.GetExtension(path);

            string fileNameToSearch = _constraintConfig;
            fileNameToSearch = fileNameToSearch.Replace("[filename]", fileName);
            fileNameToSearch = fileNameToSearch.Replace("[fileext]", fileExt);

            string fullPathToSearch = Path.Combine(dir, fileNameToSearch);
            return File.Exists(fullPathToSearch);
        }

        public bool IsRelevantToScheme(string uriScheme)
        {
            return uriScheme == "file";
        }
    }
}
