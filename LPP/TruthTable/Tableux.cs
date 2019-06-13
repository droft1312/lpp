using System.Collections.Generic;
using System.Windows.Forms;
using LPP.Nodes;

namespace LPP.TruthTable
{
    public class Tableux
    {
        private readonly Node _root;
        
        private TableuxNode tree;

        private bool isTautology;
        private bool resultGiven;
        
        public Tableux(Node root) {
            _root = Functions.NegateTree(root);

            List<Node> passIN = new List<Node>(1);
            passIN.Add(_root);
            tree = new TableuxNode(passIN);
            
            BuildTableux(tree);
        }

        private void BuildTableux(TableuxNode root) {

            void ResetVariable() {
                isTautology = false;
                resultGiven = false;
            }
            
            if (root.TableuxIsSimplifiable()) {
                ResetVariable();
                root.Generate(ref isTautology, ref resultGiven);
            }
        }
    }
}