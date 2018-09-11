using System;

namespace Navigation.App.Projections
{
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