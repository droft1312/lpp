using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LPP.Nodes;

namespace LPP
{
    public class Processor
    {
        Node root;

        public void ProcessStringInput(string input) {
            char first_character = input[0];

            switch (first_character) {
                case '~':
                    root = new NotNode (input, null);
                    break;

                case '>':
                    root = new ImplicationNode (input, null);
                    break;

                case '=':
                    root = new BiImplicationNode (input, null);
                    break;

                case '&':
                    root = new ConjunctionNode (input, null);
                    break;

                case '|':
                    root = new DisjunctionNode (input, null);
                    break;

                default:
                    throw new Exception ("String processing went wrong. Source: class Processor, method ProcessStringInput(string input)");
            }
        }
    }
}
