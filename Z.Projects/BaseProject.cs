using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Components;

namespace Z.Projects
{
    public class BaseProject : IProject
    {
        private List<IProjectComponent> _components;
        private List<IProjectRelationship> _relationships;
        private string _name = "[Project]";

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
            get { return _name; }
        }

        public BaseProject()
        {
        }
    }
}
