using System;

namespace LPP.Nodes
{
    public class NotNode : Node
    {
#pragma warning disable 0414
        private new readonly Node right;
#pragma warning restore 0414
        
        public NotNode() : base() {
            left = null;
            right = null;
            parent = null;
        }
        
        public NotNode(string input) : base(input) {
            
        }
        
        public NotNode (string input, Node parent) : base (input, parent) {
            right = null; // want to make sure that this.right will never be changed or anything
        }

        public new void Insert (Node node) {
            if (left == null) left = node;
            else throw new Exception ("Insertion failed. Source: class NotNode, method Insert(Node node)");
        }

        public override string GetInfix () {
            string res = "~(" + left.GetInfix() + ")";
            return res;
        }

        public override bool GetValue (string input) {
            var leftVal = left.GetValue(input);
            return !leftVal;
        }

        public override string ToString () {
            return "~";
        }
    }
}
