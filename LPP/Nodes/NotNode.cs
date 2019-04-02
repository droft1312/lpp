using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    public class NotNode : Node, INode
    {
#pragma warning disable 0414
        private new readonly Node right;
#pragma warning restore 0414

        public NotNode (string input, Node parent) : base(input, parent) {
            right = null; // want to make sure that this.right will never be changed or anything
        }

        public override void Insert (Node node) {
            if (left == null) left = node;
            else throw new Exception ("Insertion failed. Source: class NotNode, method Insert(Node node)");
        }

        public string Print () {
            throw new NotImplementedException ();
        }
    }
}
