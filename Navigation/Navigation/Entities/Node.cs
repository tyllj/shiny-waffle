using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Navigation.Entities
{
    [Serializable]
    [XmlType("node")]
    public class Node
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlElement("coordinates")]
        public Coordinates Coordinates { get; set; }
        
        [XmlArray("edges")]
        public List<Edge> Edges { get; set; }
    }
}