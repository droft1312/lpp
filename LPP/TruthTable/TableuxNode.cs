using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LPP.Nodes;

namespace LPP.TruthTable
{
    public class TableuxNode : INode
    {
        public readonly int id;
        
        private List<Node> listOfNodes;
        public TableuxNode Left { get; set; }
        public TableuxNode Right { get; set; }

        public List<Node> ListOfNodes => listOfNodes;
        
        /* variables that Functions.GetNewVariable() returns */
        public static List<string> ReturnedVariables { get; private set; } = new List<string>();

        private TableuxNode() {
            id = ++GlobalCounter.tableux_count;
            
        }
        
        public TableuxNode(List<Node> nodes) : this() {
            listOfNodes = nodes;
        }

        public void Generate() {

            if (GlobalCounter.tautologyBeenIdentified || GlobalCounter.nrOfGammaRules > 250) return;
            if (!TableuxIsSimplifiable()) return;

            /* get the most important tree to work on (NotNode based trees come first always) */
            var (priorityTree, position) = GetPriorityTree();
            
            /* get id of all biimplications or nands if there are any */
            Functions.GetRidOfBiImplicationAndNand(ref priorityTree);
            
            /* create a list that contains all previous elements but the one that you're gonna apply alpha/beta rules to */
            var newList = GetListWithoutPriorityTree(position); // everything except for a tree that we're going to apply rules onto

            bool betaRuleWorked = false;

            if (IsBetaRule(priorityTree)) {
                GlobalCounter.nrOfBetaRules++;
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
            } else if (IsDeltaRule(priorityTree)) {
                newList.AddRange(ApplyDeltaRules(priorityTree));
            } else if (IsGammaRule(priorityTree)) {
                newList.AddRange(ApplyGammaRules(priorityTree));
                GlobalCounter.nrOfGammaRules++;

                if (GlobalCounter.nrOfGammaRules > 250) {
                    MessageBox.Show(
                        "Have done 250 iterations... didn't find anything. So it's probably not a tautology");
                    return;
                }
            } else {
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
            
            
            if (Tableux.treeHasQuantifiers) {
                var isItTautologyNow = IsTautologyQuantifiers(listOfNodes);
                if (isItTautologyNow) GlobalCounter.nrOfTruthsReturnedByQuantifiers++;

                if (GlobalCounter.nrOfTruthsReturnedByQuantifiers == GlobalCounter.nrOfBetaRules) {
                    MessageBox.Show("It's a tautology!");
                    GlobalCounter.tautologyBeenIdentified = true;
                    return;
                }
            }
            else {
                var isTautologyNow = IsTautology(listOfNodes);

                if (isTautologyNow) GlobalCounter.nrOfTruthsReturnedNotByQuantifiers++;

                if (GlobalCounter.nrOfTruthsReturnedNotByQuantifiers == GlobalCounter.nrOfBetaRules) {
                    GlobalCounter.tautologyBeenIdentified = true;
                    return;
                }
            }
            
            /* recursively do it all over again until there's no more work that needs to be done */
            Left?.Generate();
            Right?.Generate();
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
        /// Applies gamma rules
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private List<Node> ApplyGammaRules(Node tree) {
            
            /* Gamma rules: @x.P   =>   @x.P,P[x:=t]
                            ~(!x.P) =>  ~(!x.P), ~(P[x:=t])    , where 't' is a new variable       */

            switch (tree) {
                case ForAllQuantifier forAllQuantifier:
                {
                    var leftOfQuantifier = forAllQuantifier.left;
                    
                    // list of the new Predicates(or whatever) that will be added
                    /* So if we have three previously introduced variables: v1, v2 v3
                     @y.P(v1,y) => @y.P(v1,y), P(v1,v1), P(v1,v2), P(v1, v3)
                     */
                    var list = InstantiateNClones(leftOfQuantifier, ReturnedVariables.Count);
                    
                    int counterForVariables = 0;
                    list.ForEach(item => {
                        item.ChangeVariable(forAllQuantifier.Variable.Name, ReturnedVariables[counterForVariables]);
                        counterForVariables++;
                    });
                    
                    list.Add(tree);
                    
                    return list;
                }

                case NotNode notNode when notNode.left is ExistentialQuantifier existentialQuantifier:
                {
                    var leftOfQuantifier = existentialQuantifier.left;
    
                    var list = InstantiateNClones(leftOfQuantifier, ReturnedVariables.Count);
                    
                    int counterForVariables = 0;
                    
                    list.ForEach(item => {
                        item.ChangeVariable(existentialQuantifier.Variable.Name, ReturnedVariables[counterForVariables]);
                        counterForVariables++;
                    });

                    List<Node> negatedList = new List<Node>();
                    
                    foreach (var node in list) {
                        var t = Functions.NegateTree(node);
                        negatedList.Add(t);
                    }
                    
                    negatedList.Add(tree);                    

                    return negatedList;
                }

                default:
                    return null;
            }
        }

        /// <summary>
        /// Applies delta rules to a given tree (quantifier related)
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private List<Node> ApplyDeltaRules(Node tree) {

            List<Node> result = new List<Node>();

            var newVariable = Functions.GetNewVariable();
            
            if (tree is NotNode && tree.left is ForAllQuantifier) {
                Quantifier quantifier = (Quantifier) tree.left;
                var leftOfQuantifier = quantifier.left;

                if (!ReturnedVariables.Contains(newVariable)) ReturnedVariables.Add(newVariable);
                
                leftOfQuantifier.ChangeVariable(quantifier.Variable.Name, newVariable);
                
                result.Add(Functions.NegateTree(leftOfQuantifier));

                return result;
            }

            if (tree is ExistentialQuantifier ) {
                Quantifier quantifier = (Quantifier) tree;
                var leftOfQuantifier = quantifier.left;
                
                if (!ReturnedVariables.Contains(newVariable)) ReturnedVariables.Add(newVariable);

                leftOfQuantifier.ChangeVariable(quantifier.Variable.Name, newVariable);

                result.Add(leftOfQuantifier);

                return result;
            }
            
            return null;
        }
        
        /// <summary>
        /// Gets a priority tree out of the List of Trees. ASSUMPTION: function TableuxIsSimplifiable() has been called upfront and returned true
        /// </summary>
        /// <returns></returns>
        private (Node priorityTree, int position) GetPriorityTree() {
            /*
             * Priority: alpha, delta (looks like an egg), beta, gamma (looks like Y)
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

                if (listOfNodes[i] is ConjunctionNode) {
                    tree = listOfNodes[i];
                    position = i;
                    return (tree, position);
                }

                if (listOfNodes[i] is NotNode && listOfNodes[i].left is DisjunctionNode) {
                    tree = listOfNodes[i];
                    position = i;
                    return (tree, position);
                }

                if (listOfNodes[i] is NotNode && listOfNodes[i].left is ImplicationNode) {
                    tree = listOfNodes[i];
                    position = i;
                    return (tree, position);
                }
            }
            
            // looking for delta rules
            for (int i = 0; i < listOfNodes.Count; i++) {
                switch (listOfNodes[i]) {
                    case ExistentialQuantifier _:
                        tree = listOfNodes[i];
                        position = i;
                        return (tree, position);
                    case NotNode notNode when notNode.left is ForAllQuantifier:
                        tree = listOfNodes[i];
                        position = i;
                        return (tree, position);
                }
            }
            
            
            /*looking for beta rules*/
            
            for (int i = 0; i < listOfNodes.Count; i++) {
                switch (listOfNodes[i]) {
                    case NotNode not when not.left is ConjunctionNode:
                        tree = listOfNodes[i];
                        position = i;
                        return (tree, position);
                    case DisjunctionNode _:
                    case ImplicationNode _:
                        tree = listOfNodes[i];
                        position = i;
                        return (tree, position);
                }
            }
            
            /*looking for gamma rules (or basically anything)*/

            for (int i = 0; i < listOfNodes.Count; i++) {
                
                if (listOfNodes[i] is PropositionNode) 
                    continue;
                if (listOfNodes[i] is NotNode)
                    if (listOfNodes[i].left is PropositionNode)
                        continue;

                if (listOfNodes[i] is ForAllQuantifier) {
                    tree = listOfNodes[i];
                    position = i;
                    return (tree, position);
                }

                if (listOfNodes[i] is NotNode && listOfNodes[i].left is ExistentialQuantifier) {
                    tree = listOfNodes[i];
                    position = i;
                    return (tree, position);
                }

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
        /// Returns true if given tree is a delta-rule
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private bool IsDeltaRule(Node tree) {
            switch (tree) {
                case ExistentialQuantifier _:
                case NotNode _ when tree.left is ForAllQuantifier:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsGammaRule(Node tree) {
            switch (tree) {
                case ForAllQuantifier _:
                case NotNode _ when tree.left is ExistentialQuantifier:
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
            
            foreach (var node in inputList) {
                foreach (var comparingNode in inputList) {
                    if (node == comparingNode) continue;
                    
                    string s1 = node.GetInfix();
                    string s2 = comparingNode.GetInfix();

                    if (s1.DifferByNCharacters(s2,1)) return true;
                }
            }
            
            return false;
        }

        public static bool IsTautologyQuantifiers(List<Node> inputList) {
//            foreach (var node in inputList) {
//                foreach (var comparingNode in inputList) {
//                    if (node == comparingNode) continue;
//                    
//                    string s1 = node.GetInfix();
//                    string s2 = comparingNode.GetInfix();
//
//                    if (s1.DifferByNCharacters(s2,3)) return true;
//                }
//            }
//            
//            return false;

            foreach (var node in inputList) {
                foreach (var comparingNode in inputList) {
                    if (node == comparingNode) continue;

                    string s1 = node.GetInfix();
                    string s2 = comparingNode.GetInfix();

                    s1 = s1.Substring(1);// delete first character

                    Functions.DeleteBrackets(ref s1, s1.IndexOf('('));

                    if (s1 == s2) return true;

                }
            }

            return false;
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Creates N clones of a specified node
        /// </summary>
        /// <param name="node">Node to clone</param>
        /// <param name="N">Number of times to clone</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<T> InstantiateNClones<T>(T node, int N) where T : Node {
            List<T> result = new List<T>();
            
            for (int i = 0; i < N; i++)
                result.Add(Functions.DeepCopyTree(node) as T);

            return result;
        }

        #endregion
        
    }
}