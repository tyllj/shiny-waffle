using System;
using System.Xml.Serialization;

namespace Navigation.Entities
{
    [Serializable]
    [XmlType("edge")]
    public class Edge
    {
        [XmlAttribute("to")]
        public int EndNode { get; set; }
        
        [XmlAttribute("distance")]
        public int Distance { get; set; }
    }
}