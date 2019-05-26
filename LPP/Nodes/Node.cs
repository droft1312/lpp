using System;

namespace LPP.Nodes
{
    public class Node : INode
    {
        public Node left;
        public Node right;
        public Node parent;

        protected readonly string value;
        protected readonly int number;

        public int NodeNumber { get { return number; } }
        public string Value { get { return value; } }

        public Node () {
            number = ++GlobalCounter.nodes_count;
        }

        public Node(string input) : this() { // instantiates parent,left,right with null; this constructor is primarily used when one wants to copy that node
            value = input.Substring(1);
            parent = right = left = null;
        }
        
        public Node (string input, Node parent) : this () {
            value = input.Substring (1);
            this.parent = parent;
        }

        /// <summary>
        /// Inserts Node
        /// </summary>
        /// <param name="node">node to insert</param>
        public void Insert (Node node) {
            if (left == null) left = node;
            else if (right == null) right = node;
            else throw new Exception ("Insertion failed. Source: class Node, method Insert(Node node)");
        }

        /// <summary>
        /// Outputs all propositions that this tree has to a string 'output'
        /// </summary>
        /// <param name="output">literally the output boy</param>
        public void GetAllPropositions(ref string output) {
            if (this.left == null && this.right == null) {
                var c = (this as PropositionNode).Name.ToString();
                if (!output.Contains (c)) output += c;
            } else {
                left.GetAllPropositions (ref output);
                right?.GetAllPropositions (ref output);
            }
        }

        /// <summary>
        /// Returns node connections used for making a Graphiz graph
        /// </summary>
        /// <returns></returns>
        public virtual string PrintConnections () {
            return string.Format ("node{0} -- node{1}\nnode{0} -- node{2}\n", number, left.number, right.number);
        }

        /// <summary>
        /// Returns a whole proposition in infix notation
        /// </summary>
        /// <returns></returns>
        public virtual string GetInfix () {
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Method that has to be overwritten in every class
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual bool GetValue (string input) {
            // is used when a value of the whole proposition has to be calculated
            throw new NotImplementedException ();
        }
    }
}
