using System;
using System.Collections.Generic;
using System.Linq;
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

        
        public static bool treeHasQuantifiers = false;
        public static int truthsCounterForQuantifiers = 0;
        
        #region Properties

        
        public TableuxNode Tree => tree;

        public bool IsTautology => isTautology;

        
        #endregion

        public Tableux(Node root, bool containsQuantifiers = false) {
            GlobalCounter.nrOfBetaRules = 1;
            GlobalCounter.nrOfGammaRules = 0;
            this.containsQuantifiers = containsQuantifiers;
            _root = Functions.NegateTree(root);

            List<Node> passIN = new List<Node>(1);
            passIN.Add(_root);
            tree = new TableuxNode(passIN);

            treeHasQuantifiers = HasQuantifiers(root);
            
            BuildTableux(tree);
        }

        private void BuildTableux(TableuxNode root) {

            void ResetVariable() { isTautology = false;
                treeHasQuantifiers = HasQuantifiers(_root);
                truthsCounterForQuantifiers = 0;
                GlobalCounter.nrOfBetaRules = 1;
                GlobalCounter.nrOfGammaRules = 0;
            }
            
            if (!root.TableuxIsSimplifiable()) return;

            ResetVariable();
            
            root.Generate();
        }

        public static bool ValidateTautology(TableuxNode tree) {
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

        public static int CountLeaves(TableuxNode tree) {
            int leafsCounter = 0;
            
            void IterateOverLeafs(TableuxNode node) {
                if (node == null) return;

                if (node.Left == null && node.Right == null) {
                    // we are in a leaf
                    leafsCounter++;
                }
                else {
                    IterateOverLeafs(node.Left);
                    IterateOverLeafs(node.Right);
                }
            }
            
            IterateOverLeafs(tree);

            return leafsCounter;
        }
        

        private bool HasQuantifiers(Node node) {
            return node.GetInfix().Contains("@") || node.GetInfix().Contains("!");
        }
    }
}