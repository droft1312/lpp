namespace LPP.Nodes
{
    public class PropositionNode : Node
    {
        private readonly char name;

        public char Name { get { return name; } }

        public PropositionNode () {
            left = right = null;
        }

        public PropositionNode (char name, string input, Node parent) : base (input, parent) {
            this.name = name;
        }

        public override string Print () {
            return "";
        }

        public override string ToString () {
            return name.ToString ();
        }
    }
}
