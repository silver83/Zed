using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Components;
using System.IO;

namespace Z.Projects
{
    public class Project : IProject
    {
        private List<IProjectComponent> _components;
        private List<IProjectRelationship> _relationships;
        private string _name = "[UnknownProject]";
        private string _path;


        public IEnumerable<IProjectComponent> Components
        {
            get
            {
                if (_components == null)
                    _components = new List<IProjectComponent>();
                return _components;
            }
        }
        public IEnumerable<IProjectRelationship> Relationships
        {
            get
            {
                if (_relationships == null)
                    _relationships = new List<IProjectRelationship>();
                return _relationships;
            }
        }
        public string Name
        {
            get 
            {
                if (_name == null)
                    _name = Path.GetFileName(_path);
                return _name;
            }
        }

        public Project(string path)
        {
            _path = path;
        }
    }
}
