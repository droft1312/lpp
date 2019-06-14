using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LPP.Nodes;

namespace LPP.TruthTable
{
    // TODO: Add a function that doesn't allow multiple '~(R)' be added
    // TODO: Add a function that will check if there's a tautology in one of the tableux nodes
    
    
    public class TableuxNode : INode
    {
        public readonly int id;
        
        private List<Node> listOfNodes;
        public TableuxNode Left { get; set; }
        public TableuxNode Right { get; set; }

        private TableuxNode() {
            id = ++GlobalCounter.tableux_count;
        }
        
        public TableuxNode(List<Node> nodes) : this() {
            listOfNodes = nodes;
        }

        public void Generate(ref bool result, ref bool resultGiven) {

            if (!TableuxIsSimplifiable()) {
                result = IsTautology(listOfNodes);
                return;
            }
            
            /* get the most important tree to work on (NotNode based trees come first always) */
            var (priorityTree, position) = GetPriorityTree();
            
            /* get id of all biimplications or nands if there are any */
            Functions.GetRidOfBiImplicationAndNand(ref priorityTree);
            
            /* create a list that contains all previous elements but the one that you're gonna apply alpha/beta rules to */
            var newList = GetListWithoutPriorityTree(position); // everything except for a tree that we're going to apply rules onto

            bool betaRuleWorked = false;

            if (IsBetaRule(priorityTree)) {
                var (leftBranchList, rightBranchList) = ApplyBetaRules(priorityTree);
                 
                leftBranchList.AddRange(newList);
                rightBranchList.AddRange(newList);
                
                TableuxNode newLeftNodeToInsert = new TableuxNode(leftBranchList);
                TableuxNode newRightNodeToInsert = new TableuxNode(rightBranchList);
                
                // insert two new nodes
                if (!(Insert(newLeftNodeToInsert) && Insert(newRightNodeToInsert)))
                    MessageBox.Show("Problems detected");
                else {
                    betaRuleWorked = true;
                }
            }
            else {
                newList.AddRange(ApplyAlphaRules(priorityTree));
            }
            
            // check if beta rule has been applied, if so, then do nothing and proceed to work on left and right branches
            // if beta rule wasn't applied, then insert alpha-rule
            if (!betaRuleWorked) {
                TableuxNode newNodeToInsert = new TableuxNode(newList);
                var insertionResult = Insert(newNodeToInsert);
                 
                 
                if (!insertionResult)
                    throw new Exception("Problems detected");
            }
             
            
            /* recursively do it all over again until there's no more work that needs to be done */
            Left?.Generate(ref result, ref resultGiven);
            Right?.Generate(ref result, ref resultGiven);
        }
        public override string ToString() {

            var result = "";

            for (var i = 0; i < listOfNodes.Count; i++) {
                result += listOfNodes[i].GetInfix() + (i != listOfNodes.Count - 1 ? ", " : "");
            }

            return result;
        }

        public string PrintConnections() {
            string result = "";
            
            if (Left != null) result += $"node{id} -- node{Left.id}\n";
            if (Right != null) result += $"node{id} -- node{Right.id}\n";

            return result;
        }

        /// <summary>
        /// Checks if the current TableuxNode can be broken down in pieces mor
        /// </summary>
        /// <returns></returns>
        public bool TableuxIsSimplifiable() {
            
            bool TreeIsSimplifiable(Node tree) {
                switch (tree) {
                    case PropositionNode _:
                    case NotNode _ when tree.left is PropositionNode:
                        return false;
                    case NotNode _:
                        return true;
                    default:
                        return true;
                }
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
                        default:
                            throw new Exception("Error. There is a Beta-rule");
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
            
            /*
             * Algorithm:
             * 1) Go over the list of trees and find the one matching Alpha-rules. If found, return it.
             * 2) If Alpha-rule is not found, look for beta rule. Return the first found one.
             * 
             */
            
            Node tree = null;
            int position = -1;
            
            // looking for alpha-rules
            for (int i = 0; i < listOfNodes.Count; i++) {

                if (listOfNodes[i] is NotNode notNode) {
                    if (notNode.left is PropositionNode) {
                        continue;
                    }
                }
                
                if (!IsBetaRule(listOfNodes[i]) && !(listOfNodes[i] is PropositionNode)) {
                    tree = listOfNodes[i];
                    position = i;
                    return (tree, position);
                }
            }
            
            
            // looking for anything
            for (int i = 0; i < listOfNodes.Count; i++) {
                if (listOfNodes[i] is PropositionNode) 
                    continue;
                if (listOfNodes[i] is NotNode) 
                    if (listOfNodes[i].left is PropositionNode) 
                        continue;
                
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
            switch (tree) {
                case DisjunctionNode _:
                case ImplicationNode _:
                case NotNode _ when tree.left is ConjunctionNode:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if the given list of nodes contains two contradictions
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public static bool IsTautology(List<Node> inputList) {

            /* returns true if two given string only differ by ~ , so for instance R and ~(R)*/
            bool OnlyDifferByNot(string s1, string s2) {
                int length1 = s1.Length;
                int length2 = s2.Length;

                if (s1[0] != '~' && s2[0] != '~') return false;
                if (length1 != 1 && length2 != 1) return false;

                var length1Is1 = length1 == 1;

                if (length1Is1) {
                    // s1 is the one that is of length 1

                    string cutString = s2.Substring(2, 1);
                    // returns true if stuff is the same
                    return s1 == cutString;
                }
                else {
                    // s2 is the one that is of length 1

                    string cutString = s1.Substring(2, 1);
                    return s2 == cutString;
                }

            }

            foreach (var node in inputList) {
                foreach (var comparingNode in inputList) {
                    if (node == comparingNode) continue;
                    
                    string s1 = node.GetInfix();
                    string s2 = comparingNode.GetInfix();

                    if (OnlyDifferByNot(s1, s2)) return true;
                }
            }
            
            return false;
        }

        #endregion
        
    }
}