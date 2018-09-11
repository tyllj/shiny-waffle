using System;
using System.Collections;
using System.Collections.Generic;
using Navigation;
using System.Drawing;
using System.Linq;

namespace Navigation.App
{
    public class MapViewModel
    {
        private readonly IMapProvider _mapProvider;
        private float _xOffset;
        private float _yOffset;
        
        
        private const double SCALING_FACTOR = 1.0;
        
        
        public MapViewModel(IMapProvider mapProvider)
        {
            _mapProvider = mapProvider;
            _xOffset = GetXOffset();
            _yOffset = GetYOffset();
        }
        
        private void DrawMap()
        {
            var bitmap = new Bitmap(1080, 720);
            using (Graphics canvas = Graphics.FromImage(bitmap))
            {
                var distinctEdges = GetDistinctEdges();
                var regularPen = new Pen(Color.Gray, 4.0f);
                
                foreach (var edge in distinctEdges)
                {
                    var node1 = _mapProvider.GetNode(edge.Node1);
                    var node2 = _mapProvider.GetNode(edge.Node2);

                    var end1 = GetPointOfNode(node1);
                    var end2 = GetPointOfNode(node2);
                    
                    canvas.DrawLine(regularPen, end1,end2);
                }

                canvas.Save();
            }
        }

        private PointF GetPointOfNode(Node node)
        {
            var x = (node.Coordinates.Latitude - _xOffset) * 1000;
            var y = (node.Coordinates.Longitude - _yOffset) * 1000;
            return new PointF(x,y);
        }
        
        private float GetYOffset()
        {
            return _mapProvider.GetAllNodes()
                .Min(node => node.Coordinates.Longitude);
        }

        private float GetXOffset()
        {
            return _mapProvider.GetAllNodes()
                .Min(node => node.Coordinates.Latitude);
        }

        private IEnumerable<NodeToNodeEdge> GetDistinctEdges()
        {
            var distinctEdges = new List<NodeToNodeEdge>();
            var nodes = _mapProvider.GetAllNodes();
            foreach (var node in nodes)
            {
                var edges = node.Edges
                                .Select(e => new NodeToNodeEdge(node.Id, e.EndNode));
                foreach (var edge in edges)
                {
                    if (!distinctEdges.Any(e => e.Equals(edge)))
                        distinctEdges.Add(edge);
                }
            }

            return distinctEdges;
        }
    }

    public class NodeToNodeEdge : IEquatable<NodeToNodeEdge>
    {
        public NodeToNodeEdge(int node1, int node2)
        {
            Node1 = node1;
            Node2 = node2;
        }

        public int Node1 { get; }
        public int Node2 { get; }

        public bool Equals(NodeToNodeEdge other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (Node1 == other.Node1 && Node2 == other.Node2) ||
                   (Node1 == other.Node2 && Node2 == other.Node1);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NodeToNodeEdge) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Node1 * 397) ^ Node2;
            }
        }

        public static bool operator ==(NodeToNodeEdge left, NodeToNodeEdge right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NodeToNodeEdge left, NodeToNodeEdge right)
        {
            return !Equals(left, right);
        }
    }
}