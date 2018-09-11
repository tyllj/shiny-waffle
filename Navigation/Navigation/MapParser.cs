using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Navigation.Entities;

namespace Navigation
{
    public interface IMapProvider
    {
        IList<Node> GetAllNodes();

        Node GetNode(int id);
    }
    
    public class XmlMapParser : IMapProvider
    {
        private string _filepath;
        private IList<Node> _cachedNodes;

        public XmlMapParser(string filePath)
        {
            _filepath = filePath;
            _cachedNodes = ReadMap();
        }

        public IList<Node> GetAllNodes()
        {
            return _cachedNodes;
        }
        
        public Node GetNode(int id)
        {
            return _cachedNodes.Single(node => node.Id == id);
        }

        private IList<Node> ReadMap()
        {
            var serializer = new XmlSerializer(typeof(List<Node>), new XmlRootAttribute("nodes"));
            using (var xmlStream = File.OpenText(_filepath))
            {
                return (IList<Node>) serializer.Deserialize(xmlStream);
            }
        }


    }
}