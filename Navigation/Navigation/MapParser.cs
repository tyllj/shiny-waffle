using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Navigation
{
    public interface IMapProvider
    {
        IList<Node> GetAllNodes();

        Node GetNode(int id);
    }
    
    public class XmlMapParser : IMapProvider
    {
        private IList<Node> _map;

        public XmlMapParser()
        {
            _map = ReadMap();
        }

        public IList<Node> GetAllNodes()
        {
            return _map;
        }
        
        public Node GetNode(int id)
        {
            return _map.Single(node => node.Id == id);
        }

        private static IList<Node> ReadMap()
        {
            var serializer = new XmlSerializer(typeof(List<Node>), new XmlRootAttribute("nodes"));
            using (var xmlStream =
                File.OpenText(@"/media/tyll/personal/DEV/Navigation/Navigation/daten_hl_altstadt_routenplaner_koordinaten.xml"))
            {
                return (IList<Node>) serializer.Deserialize(xmlStream);
            }
        }


    }
}