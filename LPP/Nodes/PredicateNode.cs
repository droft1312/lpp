using System.Collections.Generic;

namespace LPP.Nodes
{
    public class PredicateNode : Node
    {
        private List<Node> formulas;

        private new readonly Node left, right;

        public PredicateNode() {
            left = right = null;
            parent = null;
        }
    }
}