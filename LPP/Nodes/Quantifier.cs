namespace LPP.Nodes
{
    public abstract class Quantifier : Node
    {
        private PropositionNode variable;
        private new readonly Node right;

        public Quantifier() {
            right = null;
        }
    }
}