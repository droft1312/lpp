namespace LPP.Nodes
{
    public abstract class Quantifier : Node
    {
        private PropositionNode variable;
        private new readonly Node right;

        public PropositionNode Variable
        {
            set => variable = value;
            get => variable;
        }
        
        public Quantifier() {
            right = null;
        }

        public Quantifier(PropositionNode propositionNode) {
            variable = propositionNode;
        }

        public override string GetInfix() {
            return ToString() + "(" + variable + ", " + left.GetInfix() + ")";
        }

        public void ChangeVariable(char _oldVariable, char _newVariable) {

            if (variable.Name == _oldVariable) {
                variable = new PropositionNode(_newVariable);
            }

            if (left is Quantifier q) {
                q?.ChangeVariable(_oldVariable, _newVariable);
            } else if (left is PredicateNode p) {
                p?.ChangeVariable(_oldVariable, _newVariable);
            }
        }
    }
}