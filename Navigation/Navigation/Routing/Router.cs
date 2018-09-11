using System.Collections.Generic;
using System.Linq;
using Navigation.Entities;

namespace Navigation.Routing
{
    public class Router : IRouter
    {
        private IList<Node> _map;
        
        public Router(IMapProvider mapProvider)
        {
            _map = mapProvider.GetAllNodes();
        }
        
        public IReadOnlyList<Node> FindRoute(int startNodeId, int endNodeId)
        {
            var previousNodes = FindAllPaths(startNodeId);

            // Walk the shortest path backwards
            var path = new List<Node>();
            var reachedNode = GetNode(endNodeId);
            do
            {
                path.Add(reachedNode);
                reachedNode = previousNodes[reachedNode];
            } while (reachedNode.Id != startNodeId);

            path.Reverse();
            return path;

        }

        private Dictionary<Node, Node> FindAllPaths(int startNodeId)
        {
            Node startNode = GetNode(startNodeId);
            
            // Initialize sets ready for walking
            var queue = new List<Node>() {startNode};
            var distances = new Dictionary<Node, int>() {{startNode, 0}};
            var predecessors = new Dictionary<Node, Node>();
            var unvisited = _map.Except(new [] {startNode}).ToList();
            
            IEnumerable<Edge> GetUnvisitedEdges(Node node) =>
                node.Edges
                    .OrderBy(edge => edge.Distance)
                    .Where(edge => unvisited.Any(visitedNode => edge.EndNode == visitedNode.Id));

            while (queue.Any())
            {
                var currentNode = queue.First();

                var currentDistance = distances[currentNode];
                var unvisitedEdges = GetUnvisitedEdges(currentNode);
                foreach (var edge in unvisitedEdges)
                {
                    var unvisitedEndPoint = GetNode(edge.EndNode);
                    
                    queue.Add(unvisitedEndPoint);
                    distances.Add(unvisitedEndPoint, currentDistance + edge.Distance);
                    predecessors.Add(unvisitedEndPoint, currentNode);
                    
                    unvisited.Remove(unvisitedEndPoint);
                }

                queue.Remove(currentNode);
            }

            return predecessors;
        }

        private Node GetNode(int id) => _map.Single(node => node.Id == id);
    }
}