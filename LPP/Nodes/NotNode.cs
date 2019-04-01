using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    public class NotNode : Node, INode
    {
        public NotNode (string input, Node parent) : base(input, parent) {

        }

        public string Print () {
            throw new NotImplementedException ();
        }
    }
}
