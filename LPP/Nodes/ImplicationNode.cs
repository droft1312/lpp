namespace LPP.Nodes
{
    public class ImplicationNode : Node
    {
        public ImplicationNode (string input, Node parent) : base (input, parent) {

        }

        public override string ToString () {
            return ">";
        }
    }
}
