namespace LPP.Nodes
{
    public abstract class Quantifier : Node
    {
        private PropositionNode variable;
        private new readonly Node right;

        public PropositionNode Variable
        {
            set => variable = value;
            protected get => variable;
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
    }
}