using System.Collections.Generic;
using Navigation.Entities;

namespace Navigation.Routing
{
    public interface IRouter
    {
        IReadOnlyList<Node> FindRoute(int startNodeId, int endNodeId);
    }
}