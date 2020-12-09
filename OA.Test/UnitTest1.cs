using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace OA.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] sections = { "Audience", "Issuer" };

            string section = string.Join(":", sections);

            Console.WriteLine(Guid.NewGuid().ToString());
        }
    }
}
