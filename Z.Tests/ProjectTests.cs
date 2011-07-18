using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using Rhino.Mocks;
using Z.Projects;
using Z.Common;

namespace Z.Tests
{
    [TestFixture]
    public class ProjectTests
    {
        [SetUp]
        public void Setup()
        {
            Infrastructure.Load();
        }
    }
}
