using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    public class ConjunctionNode : Node, INode
    {
        public ConjunctionNode (string input, Node parent) : base(input, parent) {

        }

        public string Print () {
            throw new NotImplementedException ();
        }
    }
}
