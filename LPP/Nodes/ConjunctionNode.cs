namespace LPP.Nodes
{
    public class ConjunctionNode : Node
    {
        public ConjunctionNode (string input, Node parent) : base (input, parent) {

        }

        public override string ToString () {
            return "&";
        }
    }
}
