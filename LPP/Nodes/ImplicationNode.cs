namespace LPP.Nodes
{
    public class ImplicationNode : Node
    {
        public ImplicationNode(string input, Node parent) : base(input, parent) { }

        public override string GetInfix() {
            string res = "(" + left.GetInfix() + ")" + " > " + "(" + right.GetInfix() + ")";
            return res;
        }

        public override bool GetValue(string input) {
            var leftVal = left.GetValue(input);
            var rightVal = right.GetValue(input);

            if (!leftVal)
                return true;

            return !leftVal || rightVal;
        }

        public override string ToString() {
            return ">";
        }
    }
}