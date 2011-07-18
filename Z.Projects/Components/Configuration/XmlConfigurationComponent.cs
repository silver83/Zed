using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z.Projects.Components.Configuration
{
    public class XmlConfigurationComponent : BaseConfigrationComponent
    {
        private bool _definesProject;
        private string _name;
        private string _path;

        public override bool DefinesProject { get { return _definesProject; } }
        public override string Name { get { return _name; } }
        public string Path { get { return _path; } }

        public XmlConfigurationComponent(string name, string path, bool definesProject)
        {
            _name = name;
            _path = path;
            _definesProject = definesProject;
        }
    }
}
