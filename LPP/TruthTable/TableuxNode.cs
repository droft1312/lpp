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

        public List<Node> ListOfNodes => listOfNodes;

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
    }
}