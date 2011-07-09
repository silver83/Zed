using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using StructureMap.ServiceLocatorAdapter;
using Microsoft.Practices.ServiceLocation;
using log4net.Config;
using System.IO;
using Z.Common.Logging;

namespace Z.Common
{
    public class Infrastructure
    {
        public static void Load()
        {
            LoadLog4Net();
            LoadStructureMap();
        }

        private static void LoadLog4Net()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo("Logging\\log4net.xml"));
            Log.Infrastructure.Info("log4net up");
        }

        private static void LoadStructureMap()
        {
            //structuremap
            Log.Infrastructure.Info("waking structuremap");

            ObjectFactory.Initialize(x =>
            {
                //x.ForRequestedType<IValidator>().TheDefaultIsConcreteType<Validator>();
            });

            ObjectFactory.AssertConfigurationIsValid();
            var locator = new StructureMapServiceLocator(ObjectFactory.Container);
            ServiceLocator.SetLocatorProvider(() => locator);

            Log.Infrastructure.Info("structuremap up");
        }
    }
}
