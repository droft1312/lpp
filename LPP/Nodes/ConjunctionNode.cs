﻿namespace LPP.Nodes
{
    public class ConjunctionNode : Node
    {
        public ConjunctionNode() : base() {
            left = null;
            right = null;
            parent = null;
        }
        
        public ConjunctionNode(string input) : base(input) {
            
        }
        
        public ConjunctionNode (string input, Node parent) : base (input, parent) {

        }

        public override string GetInfix () {
            string res = "(" + left.GetInfix () + ")" + " & " + "(" + right.GetInfix () + ")";
            return res;
        }

        public override bool GetValue (string input) {
            var leftVal = left.GetValue(input);
            var rightVal = right.GetValue(input);

            return leftVal && rightVal;
        }

        public override string ToString () {
            return "&";
        }
    }
}
