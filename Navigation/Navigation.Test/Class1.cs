using System;
using NUnit.Framework;

namespace Navigation.Test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void RouterTest()
        {
            var router = new Router(new XmlMapParser());
            var route = router.FindRoute(1, 127);
            foreach (var node in route)
            {
                TestContext.WriteLine(node.Name);
            }
        }
    }
}