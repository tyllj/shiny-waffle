using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Navigation
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

    [Serializable]
    [XmlType("edge")]
    public class Edge
    {
        [XmlAttribute("to")]
        public int EndNode { get; set; }
        
        [XmlAttribute("distance")]
        public int Distance { get; set; }
    }

    [Serializable]
    [XmlType("coordinates")]
    public class Coordinates
    {
        [XmlElement("longitude")]
        public float Longitude { get; set; }
        
        [XmlElement("latitude")]
        public float Latitude { get; set; }
    }
}