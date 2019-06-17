using System.Collections.Generic;
using System.Linq;

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

        public override string ToString() {
            string result = "";

            result += title;
            result += "(";

            foreach (var formula in formulas) {
                result += formula;
                if (formula == formulas.Last()) continue;
                result += ",";
            }

            result += ")";
            
            return result;
        }

        public override string GetInfix() {
            return ToString();
        }
    }
}