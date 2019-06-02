using System.Collections.Generic;
using LPP.Nodes;

namespace LPP.TruthTable
{
    public class TableuxNode
    {
        private List<Node> listOfNodes;
        public Tableux Left { get; set; } = null;
        public Tableux Right { get; set; } = null;
        // maybe will have to add a parent reference

        public TableuxNode(List<Node> nodes) {
            listOfNodes = nodes;
        }

        public TableuxNode Generate() {
            if (TableuxIsSimplifiable()) {
                
            }

            return null;
        }

        /// <summary>
        /// Checks if the current TableuxNode can be broken down in pieces mor
        /// </summary>
        /// <returns></returns>
        public bool TableuxIsSimplifiable() {
            
            bool TreeIsSimplifiable(Node tree) {
                bool simplifiable = false;
            
                /* MIGHT BE A PROBLEM HERE */
                void Traverse(Node root) {
                    if (root == null) return;

                    if (root is NotNode) {
                        var child = root.left;
                        if (!(child is PropositionNode)) {
                            simplifiable = true;
                            return;
                        }
                    }

                    if (!(root is PropositionNode)) {
                        simplifiable = true;
                        return;
                    }
                
                    Traverse(root.left);
                    Traverse(root.right);
                }
            
                Traverse(tree);

                return simplifiable;
            }
            
            foreach (var node in listOfNodes) {
                var result = TreeIsSimplifiable(node);
                if (result) return true;
            }

            return false;
        }
    }
}