using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Z.Algorithms;
using Z.Common;
using System.Reflection;
using System.IO;

namespace Z.Tests
{
    [TestFixture]
    public class ReferenceVerificationTest
    {
        [Test]
        [Ignore]
        public void Test()
        {
            Infrastructure.Load();
            ReferenceManager.VerifyReferences(Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), "test\\Z.Tests.dll")));
        }
    }
}
