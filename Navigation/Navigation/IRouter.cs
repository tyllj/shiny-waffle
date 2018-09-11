using System.Collections.Generic;

namespace Navigation
{
    public interface IRouter
    {
        IReadOnlyList<Node> FindRoute(int startNodeId, int endNodeId);
    }
}