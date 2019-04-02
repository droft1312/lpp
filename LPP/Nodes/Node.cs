using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    public class Node : INode
    {
        public Node left;
        public Node right;
        public Node parent;

        protected readonly string value;
        protected readonly int number;

        public string Value { get { return value; } }
        public int NodeNumber { get { return number; } }

        public Node () {
            number = ++GlobalCounter.nodes_count;
        }

        public Node(string input, Node parent) : this() {
            value = input.Substring (1);
            this.parent = parent;
        }

        /// <summary>
        /// Inserts Node
        /// </summary>
        /// <param name="node">node to insert</param>
        public void Insert(Node node) {
            if (left == null) left = node;
            else if (right == null) right = node;
            else throw new Exception ("Insertion failed. Source: class Node, method Insert(Node node)");
        }

        /// <summary>
        /// Returns node connections used for making a Graphiz graph
        /// </summary>
        /// <returns></returns>
        public virtual string Print () {
            return string.Format ("node{0} -- node{1}\nnode{0} -- node{2}\n", number, left.number, right.number);
        }
    }
}
