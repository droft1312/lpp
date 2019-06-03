using System;
using System.Collections.Generic;
using LPP.Nodes;

namespace LPP.TruthTable
{
    public class TableuxNode
    {
        private List<Node> listOfNodes;
        public TableuxNode Left { get; set; } = null;
        public TableuxNode Right { get; set; } = null;
        // maybe will have to add a parent reference

        public TableuxNode(List<Node> nodes) {
            listOfNodes = nodes;
        }

        public void Generate() {
            if (TableuxIsSimplifiable()) {
                // TODO: Change this test code
                ApplyRules(listOfNodes[0]);
            }
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
        /// Gets a priority tree out of the List of Trees
        /// </summary>
        /// <returns></returns>
        private Node GetPriorityTree() {
            
            // TODO: Write a function that prioritizes trees that have to be dealt with first

            return null;
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