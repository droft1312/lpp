using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    public class DisjunctionNode : Node
    {
        public DisjunctionNode (string input, Node parent) : base (input, parent) {

        }

        public override string ToString () {
            return "|";
        }
    }
}
