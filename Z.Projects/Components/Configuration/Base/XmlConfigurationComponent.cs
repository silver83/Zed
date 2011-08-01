using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.Scanner;

namespace Z.Projects.Components.Configuration
{
    public class XmlConfigurationComponent : BaseConfigrationComponent
    {
        private ResourceId _resourceId;

        public override ResourceId ResourceId { get { return _resourceId; } }

        public XmlConfigurationComponent(ResourceId resourceId)
        {
            _resourceId = resourceId;
        }
    }
}
