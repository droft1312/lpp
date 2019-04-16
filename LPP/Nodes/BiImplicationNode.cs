namespace LPP.Nodes
{
    public class BiImplicationNode : Node
    {
        public BiImplicationNode (string input, Node parent) : base (input, parent) {

        }

        public override string GetInfix () {
            string res = "(" + left.GetInfix () + ")" + " = " + "(" + right.GetInfix () + ")";
            return res;
        }

        public override bool GetValue (string input) {
            bool leftVal = this.left.GetValue (input);
            bool rightVal = this.right.GetValue (input);
            return ((!leftVal || rightVal) && (!rightVal || leftVal));
        }

        public override string ToString () {
            return "=";
        }
    }
}
