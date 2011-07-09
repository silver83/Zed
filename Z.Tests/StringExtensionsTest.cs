using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Z.Common;

namespace Z.Tests
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void Extension_Csproj_Matched()
        {
            Assert.IsTrue("moshe.csproj".Like("*.csproj"));
            Assert.IsTrue("DirPath\\moshe.csproj".Like("*.csproj"));
            Assert.IsTrue("D:\\DirPath1\\DirPath2\\moshe.csproj".Like("*.csproj"));
            Assert.IsTrue("  D:\\DirPath1\\DirPath2\\moshe.csproj".Like("*.csproj"));
            Assert.IsTrue("D:\\D     h1\\ ir th2\\moshe.csproj".Like("*.csproj"));

            Assert.IsFalse("D:\\D irPath1\\DirPath2\\mohe.cspr".Like("*.csproj"));
            Assert.IsFalse("D:\\D irPath1\\DirPath2\\csproj".Like("*.csproj"));
        }
    }
}
