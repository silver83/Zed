using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Scanner.Constraints;

namespace Z.Scanner
{
    public static class ResourceTypeExtensions
    {
        public static bool IsOfType(this ResourceId id, ResourceType type)
        {
            return type.IsOfType(id);
        }
    }

    /// <summary>
    /// Binds between an environmental resource type and a component type
    /// </summary>
    public class ResourceType
    {
        private string _uriScheme;
        private Type _componentType;
        private IEnumerable<IResourceTypeConstraint> _constraints;

        /// <summary>
        /// The URI schema for the configuration resource 
        /// </summary>
        public string UriSchema { get { return _uriScheme; } }

        /// <summary>
        /// The assembly-qualified component type name for this config type
        /// </summary>
        public Type ComponentType { get { return _componentType; } }

        public ResourceType(Type componentType, IEnumerable<IResourceTypeConstraint> constraints, string uriScheme = "file")
        {
            if (constraints == null)
                throw new ArgumentNullException("constraints");

            if (constraints.Count() == 0)
                throw new InvalidOperationException("cannot define a Config type with zero constraints.");

            foreach (var constraint in constraints)
            {
                if (!constraint.IsRelevantToScheme(uriScheme))
                    throw new InvalidOperationException(string.Format("the constraint '{0}' is invalid for schemes of type '{1}'", constraint, uriScheme));
            }

            _constraints = constraints;
            _uriScheme = uriScheme;
            _componentType = componentType;
        }

        public ResourceType(string qualifiedName, IEnumerable<IResourceTypeConstraint> constraints, string uriScheme = "file")
            : this(Type.GetType(qualifiedName), constraints, uriScheme) //Type.GetType will, and should, throw an exception if the qualified name is declared incorrectly
        { }

        public bool IsOfType(ResourceId id)
        {
            if (id.Uri.Scheme != this._uriScheme)
                return false;

            foreach (var constraint in _constraints)
                if (!constraint.IsMetBy(id))
                    return false;

            return true;
        }
    }
}
