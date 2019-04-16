namespace LPP.Nodes
{
    public class ImplicationNode : Node
    {
        public ImplicationNode (string input, Node parent) : base (input, parent) {

        }

        public override string GetInfix () {
            string res = "(" + left.GetInfix () + ")" + " > " + "(" + right.GetInfix() + ")";
            return res;
        }

        public override bool GetValue (string input) {
            return (!(this.left.GetValue (input)) || this.right.GetValue (input));
        }

        public override string ToString () {
            return ">";
        }
    }
}
