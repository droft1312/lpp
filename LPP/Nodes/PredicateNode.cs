using System.Collections.Generic;

namespace LPP.Nodes
{
    public class PredicateNode : Node
    {
        private List<PropositionNode> formulas;

        private new readonly Node left, right;

        private char title;
        
        public List<PropositionNode> Formulas
        {
            set => formulas = value;
        }

        private PredicateNode() {
            left = right = null;
            parent = null;
        }

        public PredicateNode(char title) : this() {
            this.title = title;
        }
    }
}