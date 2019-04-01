using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    public class Node
    {
        public Node left;
        public Node right;
        public Node parent;

        public string value;
        public int number;

        public Node () {
            number = ++GlobalCounter.nodes_count;
        }

        public Node(string input, Node parent) : this() {
            value = input.Substring (1);
            this.parent = parent;
        }

        public void Insert(Node node) {
            if (left == null) left = node;
            else if (right == null) right = node;
            else throw new Exception ("Insertion failed. Source: class Node, method Insert(Node node)");
        }
    }
}
