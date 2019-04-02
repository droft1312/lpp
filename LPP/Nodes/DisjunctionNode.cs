namespace LPP.Nodes
{
    public class DisjunctionNode : Node
    {
        public DisjunctionNode (string input, Node parent) : base (input, parent) {

        }

        public override string ToString () {
            return "|";
        }
    }
}
