namespace LPP.Nodes
{
    public class DisjunctionNode : Node
    {
        public DisjunctionNode (string input, Node parent) : base (input, parent) {

        }

        public override bool GetValue (bool input) {
            return (this.left.GetValue (input) || this.right.GetValue (input));
        }

        public override string ToString () {
            return "|";
        }
    }
}
