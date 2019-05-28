namespace LPP.Nodes
{
    public class NandNode : Node
    {
        public NandNode() : base() {
            left = null;
            right = null;
            parent = null;
        }
        
        public NandNode(string input) : base(input) {
            
        }

        public NandNode(Node left, Node right, Node parent = null) {
            this.left = left;
            this.right = right;
            this.parent = parent;
        }
        
        public NandNode (string input, Node parent) : base (input, parent) {

        }
        
        public override string GetInfix () {
            string res = "(" + left.GetInfix () + ")" + " % " + "(" + right.GetInfix () + ")";
            return res;
        }
        
        public override bool GetValue (string input) {
            var leftVal = left.GetValue (input);
            var rightVal = right.GetValue (input);

            return !(leftVal && rightVal);
        }
        
        public override string ToString () {
            return "%";
        }
    }
}