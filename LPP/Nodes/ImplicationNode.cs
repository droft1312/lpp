namespace LPP.Nodes
{
    public class ImplicationNode : Node
    {
        public ImplicationNode(string input) : base(input) {
            
        }
        
        public ImplicationNode(string input, Node parent) : base(input, parent) { }

        public override string GetInfix() {
            string res = "(" + left.GetInfix() + ")" + " > " + "(" + right.GetInfix() + ")";
            return res;
        }

        public override bool GetValue(string input) {
            var leftVal = left.GetValue(input);
            
            // if the expression on the left is false, then there's no need to calculate 
            // what's on the right cuz the expression is always gonna return true
            if (!leftVal)
                return true;
            
            
            var rightVal = right.GetValue(input);
            
            return rightVal;
        }

        public override string ToString() {
            return ">";
        }
    }
}