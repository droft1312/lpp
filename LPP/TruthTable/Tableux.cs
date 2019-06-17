using System;
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

        private readonly bool containsQuantifiers;

        #region Properties

        
        public TableuxNode Tree => tree;

        public bool IsTautology => isTautology;

        
        #endregion

        public Tableux(Node root, bool containsQuantifiers = false) {
            this.containsQuantifiers = containsQuantifiers;
            _root = Functions.NegateTree(root);

            List<Node> passIN = new List<Node>(1);
            passIN.Add(_root);
            tree = new TableuxNode(passIN);
            
            BuildTableux(tree);
        }

        private void BuildTableux(TableuxNode root) {

            void ResetVariable() { isTautology = false; }
            
            if (!root.TableuxIsSimplifiable()) return;

            ResetVariable();
            
            root.Generate();
        }

        public bool ValidateTautology() {
            if (tree == null) throw new ArgumentNullException();

            int leafsCounter = 0;
            int truthsCounter = 0;
            
            void IterateOverLeafs(TableuxNode node) {
                if (node == null) return;

                if (node.Left == null && node.Right == null) {
                    // we are in a leaf
                    leafsCounter++;
                    truthsCounter += TableuxNode.IsTautology(node.ListOfNodes) ? 1 : 0;
                }
                else {
                    IterateOverLeafs(node.Left);
                    IterateOverLeafs(node.Right);
                }
            }
            
            IterateOverLeafs(tree);

            return leafsCounter == truthsCounter;
        }
    }
}