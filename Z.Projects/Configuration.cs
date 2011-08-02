using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Scanner.Filters;
using Z.Scanner.Constraints;
using Z.Scanner;
using Z.Projects.Components.Configuration;

namespace Z.Projects
{
    // Everything here is hardcoded right now to get things working... goal is to have an API to define this/ load it from xml file
    public class Configuration
    {
        private static IEnumerable<IFilter<string>> _fileFilters;
        private static IEnumerable<ResourceType> _resourceTypes;
        private static IDictionary<string, IEnumerable<ResourceType>> _resourceTypesBySchema;

        public static IEnumerable<IFilter<string>> FileFilters
        {
            get
            {
                if (_fileFilters == null)
                {
                    _fileFilters = new List<IFilter<string>>() 
                    { 
                        new FileExtensionFilter(".config") 
                    };
                }
                return _fileFilters;
            }
        }

        /// <summary>
        /// Maps between a uri scheme type a list of possible config types
        /// </summary>
        public static IDictionary<string, IEnumerable<ResourceType>> ResourceTypesBySchema
        {
            get
            {
                if (_resourceTypesBySchema == null)
                    GenerateConfigTypes();

                return _resourceTypesBySchema;
            }
        }
        public static IEnumerable<ResourceType> ResourceTypes
        {
            get
            {
                if (_resourceTypes == null)
                    GenerateConfigTypes();
                return _resourceTypes;
            }
        }

        private static void GenerateConfigTypes()
        {
            var types = new List<ResourceType>();
            var typesBySchema = new Dictionary<string, IEnumerable<ResourceType>>();

            //create structureMap type
            var structureMapTypeConstraints = new List<IResourceTypeConstraint>();
            structureMapTypeConstraints.Add(new XmlConfigTypeConstraint("StructureMap"));
            var structureMapConfigType = new ResourceType(typeof(StructureMapConfigurationComponent), structureMapTypeConstraints);

            //create dotnet type
            var netConfigTypeConstraints = new List<IResourceTypeConstraint>();
            netConfigTypeConstraints.Add(new XmlConfigTypeConstraint("configuration"));

            var constraint = ("[filename]".AsHasSiblingConstraint().Or(
                              "(.*)/web.config$".AsUriRegexConstraint())).And("^((?!vshost).)*$".AsUriRegexConstraint());

            netConfigTypeConstraints.Add(constraint);
            var netConfigConfigType = new ResourceType(typeof(DotNetConfigurationComponent), netConfigTypeConstraints);

            //add types
            AddTypeToLists(types, typesBySchema, structureMapConfigType);
            AddTypeToLists(types, typesBySchema, netConfigConfigType);

            _resourceTypes = types;
            _resourceTypesBySchema = typesBySchema;
        }

        private static void AddTypeToLists(List<ResourceType> types, Dictionary<string, IEnumerable<ResourceType>> typesBySchema, ResourceType structureMapConfigType)
        {
            types.Add(structureMapConfigType);

            IEnumerable<ResourceType> typesForSchema = null;
            if (!typesBySchema.TryGetValue(structureMapConfigType.UriSchema, out typesForSchema))
                typesBySchema[structureMapConfigType.UriSchema] = new List<ResourceType>();

            (typesBySchema[structureMapConfigType.UriSchema] as List<ResourceType>).Add(structureMapConfigType);
        }
    }
}
