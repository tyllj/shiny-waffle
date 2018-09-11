using System;
using Navigation.Routing;
using NUnit.Framework;

namespace Navigation.Test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void BasicRouterTest()
        {
            var router = new Router(new XmlMapParser(@"/media/tyll/personal/DEV/shiny-waffle/Navigation/Navigation/daten_hl_altstadt_routenplaner_koordinaten.xml"));
            var route = router.FindRoute(1, 15);
            foreach (var node in route)
            {
                TestContext.WriteLine(node.Name);
            }
        }
    }
}