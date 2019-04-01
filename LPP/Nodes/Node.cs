using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    class Node
    {
        public Node left;
        public Node right;
        public Node parent;

        public string value;
        public int number;

        public Node () {
            number = ++GlobalCounter.nodes_count;
        }
    }
}
