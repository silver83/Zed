using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Projects.Components;
using System.IO;
using Z.Scanner;

namespace Z.Projects
{
    public class Project : IProject
    {
        private List<IProjectComponent> _components;
        private List<IProjectRelationship> _relationships;
        private ResourceId _resourceId;

        public ProjectBuilder Builder { get; set; }
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
        public ResourceId ResourceId { get; set; }

        public Project(ResourceId resourceId)
        {
            _resourceId = resourceId;
        }

        public void AddComponents(IEnumerable<IProjectComponent> components)
        {
            (Components as List<IProjectComponent>).AddRange(components);
        }

        public void AddComponent(IProjectComponent component)
        {
            (Components as List<IProjectComponent>).Add(component);
        }        
    }
}
