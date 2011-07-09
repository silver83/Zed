using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Z.Common.Runtime;

namespace Z.Tests
{
    public interface IWillBeExposed
    {
        string Prop { get; set; }
        string Method1(string arg1, bool arg2, params object[] arg3);
    }

    public class IAmThirdParty
    {
        public string Prop { get; set; }
        public string Method1(string arg1, bool arg2, params object[] arg3)
        {
            return string.Format("{0}{1}{2}", arg1, arg2, arg3);
        }
    }

    [TestFixture]
    public class DynamicDuckTests
    {
        [Test]
        public void CreateDynamicDuck()
        {
            var obj = new IAmThirdParty();
            obj.Prop = Guid.NewGuid().ToString();
            IWillBeExposed exposeMe = DynamicDuckFactory.CreateDynamicDuck<IAmThirdParty, IWillBeExposed>(obj);

            Assert.AreEqual(exposeMe.Prop, obj.Prop);
            Console.WriteLine(exposeMe.Prop);
            string output = exposeMe.Method1("a", false, "Moshe");
            Assert.AreEqual(output, string.Format("{0}{1}{2}", "a", false, new object[]{ "Moshe" }));
        }
    }
}
