using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LPP.Nodes;

namespace LPP.TruthTable
{
    public class TableuxNode
    {
        private List<Node> listOfNodes;
        public TableuxNode Left { get; set; }
        public TableuxNode Right { get; set; } 

        public TableuxNode(List<Node> nodes) {
            listOfNodes = nodes;
        }

        public void Generate() {
            
            /*
             * Algorithm:
             * 1) Get the priority node out of the list of nodes 
             * 2) Apply rules to it
             * 3) Create a new TableuxNode and insert it into the current one
             * 4) Move on
             */

            if (!TableuxIsSimplifiable()) return;
            
            // TODO: Finish this stuff off
            var (priorityTree, position) = GetPriorityTree();
            var newList = GetListWithoutPriorityTree(position); // everything except for a tree that we're going to apply rules onto
            newList.AddRange(ApplyRules(priorityTree));
            
            TableuxNode newNodeToInsert = new TableuxNode(newList);
            var insertionResult = Insert(newNodeToInsert);

            if (!insertionResult)
                MessageBox.Show("Problems detected");

            Left?.Generate();
            Right?.Generate();
        }

        /// <summary>
        /// Checks if the current TableuxNode can be broken down in pieces mor
        /// </summary>
        /// <returns></returns>
        public bool TableuxIsSimplifiable() {
            
            bool TreeIsSimplifiable(Node tree) {
                if (tree is PropositionNode) return false;
                if (!(tree is NotNode)) return true;
                return !(tree.left is PropositionNode);
            }
            
            foreach (var node in listOfNodes) {
                var result = TreeIsSimplifiable(node);
                if (result) return true;
            }

            return false;
        }


        #region Hidden functions

        
        /// <summary>
        /// Applies alpha/beta rules to a given tree
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private List<Node> ApplyRules(Node tree) {
            
            List<Node> result = new List<Node>();

            if (tree is ConjunctionNode) {
                result.Add(tree.left);
                result.Add(tree.right);
            }
            else if (tree is DisjunctionNode) {
                
            }
            else if (tree is ImplicationNode) {
                
            }
            else if (tree is BiImplicationNode) {
                
            }
            else if (tree is NandNode) {


            }
            else if (tree is NotNode) {
                
                var child = tree.left;

                // TODO: Implement
                if (child is ConjunctionNode) {
                }
                else if (child is DisjunctionNode) {
                    result.Add(Functions.NegateTree(child.left));
                    result.Add(Functions.NegateTree(child.right));
                }
                else if (child is ImplicationNode) {
                    
                    
                    result.Add(child.left);
                    var negatedRightSubtree = Functions.NegateTree(child.right);
                    result.Add(negatedRightSubtree);
                    
                }
                else if (child is BiImplicationNode) {
                }
                else if (child is NandNode) {
                }
            }
            else {
                throw new Exception("Something went wrong");
            }

            return result.Count == 0 ? null : result;
        }
        
        /// <summary>
        /// Gets a priority tree out of the List of Trees. ASSUMPTION: function TableuxIsSimplifiable() has been called upfront and returned true
        /// </summary>
        /// <returns></returns>
        private (Node priorityTree, int position) GetPriorityTree() {
            /*
             * Priority is given to the trees whose root node is NotNode
             * If those are already out of the equation(not literally), then it is at the god's will
             * which tree shall get rules applied to
             *
             * Algorithm:
             * 1) Look for trees whose root is NotNode
             * 2) If those are not found, pick the first one that is just not a PropositionNode or NotNode whose child is a PropositionNode
             */
            
            Node tree = null;
            int position = -1;

            // looking for NotNode
            for (int i = 0; i < listOfNodes.Count; i++) {
                if (listOfNodes[i] is NotNode) {
                    
                    if (listOfNodes[i].left is PropositionNode) continue;

                    tree = listOfNodes[i];
                    position = i;
                    return (tree, position);
                }
            }
            
            // looking for anything
            for (int i = 0; i < listOfNodes.Count; i++) {
                if (listOfNodes[i] is PropositionNode) continue;
                tree = listOfNodes[i];
                position = i;
                return (tree, position);
            }



            return (tree, position);
        }

        private List<Node> GetListWithoutPriorityTree(int positionToSkip) {
            List<Node> _newList = new List<Node>(listOfNodes.Count - 1);
            _newList.AddRange(listOfNodes.Where((t, i) => i != positionToSkip));
            return _newList;
        }
        
        /// <summary>
        /// Inserts a new TableuxNode into the current one
        /// </summary>
        /// <param name="toInsert"></param>
        /// <returns></returns>
        private bool Insert(TableuxNode toInsert) {
            if (Left == null) { Left = toInsert;
                return true;
            }
            
            if (Right == null) { Right = toInsert;
                return true;
            }

            return false;
        }

        #endregion
        
    }
}