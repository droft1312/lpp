using System.Collections.Generic;
using System.Linq;

namespace LPP.Nodes
{
    public class PredicateNode : Node
    {
        private List<PropositionNode> formulas;

        private new readonly Node left, right;

        private char title;

        public char Title => title;

        public List<PropositionNode> Formulas
        {
            set => formulas = value;
            get => formulas;
        }

        private PredicateNode() {
            left = right = null;
            parent = null;
        }

        public PredicateNode(char title) : this() {
            this.title = title;
        }

        public void ChangeVariable(char _oldVariable, char _newVariable) {
            for (int i = 0; i < formulas.Count; i++) {
                if (formulas[i].Name == _oldVariable) {
                    formulas[i] = new PropositionNode(_newVariable);
                }
            }
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