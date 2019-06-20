using System;

namespace LPP.Nodes
{
    public class PropositionNode : Node
    {
        private readonly string name;

        public string Name { get { return name; } }

        private PropositionNode () {
            left = right = null;
        }

        public PropositionNode(string name) : this() {
            this.name = name;
        }
        
        public PropositionNode (string name, string input, Node parent) : base (input, parent) {
            this.name = name;
        }

        public override string GetInfix () {
            return name.ToString ();
        }

        public override bool GetValue (string input) {
            int i = input.IndexOf (name);
            return Convert.ToBoolean (int.Parse (input[i + 2].ToString ()));
        }

        public override string PrintConnections () {
            return "";
        }

        public override string ToString () {
            return name.ToString ();
        }
    }
}
