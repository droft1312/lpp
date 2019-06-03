using System.Collections.Generic;
using System.Windows.Forms;
using LPP.Nodes;

namespace LPP.TruthTable
{
    public class Tableux
    {
        private readonly Node _root;
        
        private TableuxNode tree;
        
        public Tableux(Node root) {
            _root = Functions.NegateTree(root);

            List<Node> passIN = new List<Node>(1);
            passIN.Add(_root);
            tree = new TableuxNode(passIN);
            
            BuildTableux(tree);
        }

        private void BuildTableux(TableuxNode root) {
            if (root.TableuxIsSimplifiable()) {
                root.Generate();
            }
        }
    }
}