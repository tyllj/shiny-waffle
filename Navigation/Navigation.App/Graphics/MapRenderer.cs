using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Navigation.App.Projections;
using Navigation.Entities;
using Drawing = System.Drawing;

namespace Navigation.App.Graphics
{
    public interface IMapRenderer
    {
        ICollection<NodeToNodeEdge> HighlightedEdges { get; }
        Stream Render();
    }
    
    public class MapRenderer : IMapRenderer
    {
        #region constants
        
        private const float SCALING_FACTOR = 45000;
        
        #endregion
        
        #region private fields
        
        private readonly IMapProvider _mapProvider;
        private float _xOffset;
        private float _yOffset;
        
        #endregion

        public MapRenderer(IMapProvider mapProvider)
        {
            _mapProvider = mapProvider;
            _xOffset = GetMinLatitude() - 10f/SCALING_FACTOR;
            _yOffset = GetMinLongitude() - 10f/SCALING_FACTOR;
            
            HighlightedEdges = new List<NodeToNodeEdge>();
        }
        
        #region public properties
        
        public ICollection<NodeToNodeEdge> HighlightedEdges { get; }

        #endregion
        
        #region public methods
        
        public Stream Render()
        {
            var outputStream = new MemoryStream();
            using (var bitmap = new Bitmap(1080, 720))
            {
                using (var canvas = Drawing.Graphics.FromImage(bitmap))
                {
                    var distinctEdges = GetDistinctEdges();
                    var regularPen = new Pen(Color.White, 4f);
                    var highlightingPen = new Pen(Color.RoyalBlue, 4f);
                    
                    canvas.FillRectangle(new SolidBrush(Color.Salmon), 0, 0, 1080, 720);

                    foreach (var edge in distinctEdges)
                    {
                        var node1 = _mapProvider.GetNode(edge.Node1);
                        var node2 = _mapProvider.GetNode(edge.Node2);

                        var end1 = GetPointOfNode(node1);
                        var end2 = GetPointOfNode(node2);
                        
                        if (IsHighlightedEdge(edge))
                            canvas.DrawLine(highlightingPen, end1, end2);
                        else
                            canvas.DrawLine(regularPen, end1, end2);
                    }

                    canvas.Save();
                }
                bitmap.Save(outputStream, ImageFormat.Png);
                outputStream.Flush();
                outputStream.Position = 0;
            }

            return outputStream;
        }
        
        #endregion

        #region private methods

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

        private bool IsHighlightedEdge(NodeToNodeEdge edge)
        {
            return HighlightedEdges.Any(he => he.Equals(edge));
        }
        
        private PointF GetPointOfNode(Node node)
        {
            var x = (node.Coordinates.Latitude - _xOffset) * SCALING_FACTOR;
            var y = (node.Coordinates.Longitude - _yOffset) * SCALING_FACTOR;
            return new PointF(x,y);
        }
    
        private float GetMinLongitude()
        {
            return _mapProvider.GetAllNodes()
                .Min(node => node.Coordinates.Longitude);
        }

        private float GetMinLatitude()
        {
            return _mapProvider.GetAllNodes()
                .Min(node => node.Coordinates.Latitude);
        }

        #endregion
        
    }
}