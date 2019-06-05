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
        
        // TODO: Do a thing with ~(~(SomeProposition)) = SomeProposition

        public void Generate() {
            
            if (!TableuxIsSimplifiable()) return;
            
            /* get the most important tree to work on (NotNode based trees come first always) */
            var (priorityTree, position) = GetPriorityTree();
            
            /* check if it's biimplication or nand */
            
            
            /* create a list that contains all previous elements but the one that you're gonna apply alpha/beta rules to */
            var newList = GetListWithoutPriorityTree(position); // everything except for a tree that we're going to apply rules onto

            if (IsBetaRule(priorityTree)) {
                var (leftBranchList, rightBranchList) = ApplyBetaRules(priorityTree);
                 
                leftBranchList.AddRange(newList);
                rightBranchList.AddRange(newList);
                
                TableuxNode newLeftNodeToInsert = new TableuxNode(leftBranchList);
                TableuxNode newRightNodeToInsert = new TableuxNode(rightBranchList);
                
                // insert two new nodes
                if (!(Insert(newLeftNodeToInsert) && Insert(newRightNodeToInsert)))
                    MessageBox.Show("Problems detected");
            }
            else {
                newList.AddRange(ApplyAlphaRules(priorityTree));
            }
            
            /* create a new TableuxNode */
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
                if (tree is PropositionNode) return false; // if given tree consists of just a PropositionNode
                if (!(tree is NotNode)) return true; // if given tree is not a NotNode
                return !(tree.left is PropositionNode); // returns: tree is simplifiable if left of NotNode is not PropositionNode
            }
            
            foreach (var node in listOfNodes) {
                var result = TreeIsSimplifiable(node);
                if (result) return true;
            }

            return false;
        }


        #region Hidden functions

        
        /// <summary>
        /// Applies alpha/beta rules to a given tree.
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private List<Node> ApplyAlphaRules(Node tree) {
            
            // we check if the given root node is Biimplication because we need to convert that tree then
            if (tree is BiImplicationNode node) tree = Functions.ConvertBiImplication(node);
            else if (tree is NandNode tempNode) tree = Functions.ConvertNand(tempNode);
            
            List<Node> result = new List<Node>();

            switch (tree) {
                case ConjunctionNode _:
                    result.Add(tree.left);
                    result.Add(tree.right);
                    break;
                case NotNode _:
                {
                    var child = tree.left;

                    switch (child) {
                        // TODO: Implement
                        case DisjunctionNode _:
                            result.Add(Functions.NegateTree(child.left));
                            result.Add(Functions.NegateTree(child.right));
                            break;
                        case ImplicationNode _:
                            result.Add(child.left);
                            result.Add(Functions.NegateTree(child.right));
                            break;
                        case BiImplicationNode _:
                            break;
                        case NandNode _:
                            break;
                    }

                    break;
                }

                default:
                    throw new Exception("Something went wrong");
            }

            return result.Count == 0 ? null : result;
        }

        /// <summary>
        /// Applies rules to a tree that will generate two branches
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="isBetaRule">You have to indicate this as true if you really want to use this function</param>
        /// <returns>Left and Right subtrees</returns>
        private (List<Node> left, List<Node> right) ApplyBetaRules(Node tree) {

            List<Node> leftSubtree = new List<Node>();
            List<Node> rightSubtree = new List<Node>();

            switch (tree) {
                case DisjunctionNode _:
                    leftSubtree.Add(tree.left);
                    rightSubtree.Add(tree.right);
                    break;
                case ImplicationNode _:
                    leftSubtree.Add(Functions.NegateTree(tree.left));
                    rightSubtree.Add(tree.right);
                    break;
                case NotNode _ when tree.left is ConjunctionNode:
                    leftSubtree.Add(Functions.NegateTree(tree.left.left));
                    rightSubtree.Add(Functions.NegateTree(tree.left.right));
                    break;
                default:
                    return (null, null);
            }


            return (leftSubtree, rightSubtree);
        } 
        
        /// <summary>
        /// Gets a priority tree out of the List of Trees. ASSUMPTION: function TableuxIsSimplifiable() has been called upfront and returned true
        /// </summary>
        /// <returns></returns>
        private (Node priorityTree, int position) GetPriorityTree() {
            
            // TODO: Change the algorithm for prioritization. Preference is given to Alpha-rules
            
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
        
        /// <summary>
        /// Returns true if given tree is beta-rule
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private bool IsBetaRule(Node tree) {
            // TODO: Add more cases!
            switch (tree) {
                case DisjunctionNode _:
                case ImplicationNode _:
                case NotNode _ when tree.left is ConjunctionNode:
                    return true;
                default:
                    return false;
            }
        }

        #endregion
        
    }
}