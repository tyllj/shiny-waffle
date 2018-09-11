using System;
using System.Xml.Serialization;

namespace Navigation.Entities
{
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