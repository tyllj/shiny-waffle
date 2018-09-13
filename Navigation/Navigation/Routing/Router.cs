using System.Collections.Generic;
using System.Linq;
using Navigation.Entities;

namespace Navigation.Routing
{
    public class Router : IRouter
    {
        private IMapProvider _mapProvider;

        public Router(IMapProvider mapProvider)
        {
            _mapProvider = mapProvider;
        }
        
        public IReadOnlyList<Node> FindRoute(int startNodeId, int endNodeId)
        {
            var previousNodes = FindAllPaths(startNodeId);

            // Walk the shortest path backwards
            var path = new List<Node>();
            var reachedNode = _mapProvider.GetNode(endNodeId);
            do
            {
                path.Add(reachedNode);
                reachedNode = previousNodes[reachedNode];
            } while (reachedNode.Id != startNodeId);

            path.Reverse();
            return path;

        }

        /// <summary>
        /// Returns a dictionary where each Key is a Node on the map with its closest predecessor as value.
        /// </summary>
        private Dictionary<Node, Node> FindAllPaths(int startNodeId)
        {
            Node startNode = _mapProvider.GetNode(startNodeId);
            
            // Initialize sets ready for walking
            var queue = new List<Node>() {startNode};
            var distances = new Dictionary<Node, int>() {{startNode, 0}};
            var predecessors = new Dictionary<Node, Node>();
            
            var unvisited = _mapProvider.GetAllNodes()
                                        .Except(new [] {startNode}).ToList();
            
            IEnumerable<Edge> GetUnvisitedEdges(Node node) =>
                node.Edges
                    .Where(edge => unvisited.Any(visitedNode => edge.EndNode == visitedNode.Id));

            Node GetNearestNodeInQueue() => queue.OrderBy(node => distances[node])
                                                 .First();

            while (queue.Any())
            {
                var currentNode = GetNearestNodeInQueue();
                queue.Remove(currentNode);
                
                var currentDistance = distances[currentNode];
                var unvisitedEdges = GetUnvisitedEdges(currentNode);
                foreach (var edge in unvisitedEdges)
                {
                    var unvisitedEndPoint = _mapProvider.GetNode(edge.EndNode);

                    queue.Add(unvisitedEndPoint);
                    distances.Add(unvisitedEndPoint, currentDistance + edge.Distance);
                    predecessors.Add(unvisitedEndPoint, currentNode);

                    unvisited.Remove(unvisitedEndPoint);
                }
            }

            return predecessors;
        }
    }
}