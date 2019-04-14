using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP
{
    public static class Functions
    {
        /// <summary>
        /// Deletes ' ', '(' from a string
        /// </summary>
        /// <param name="input">String to parse</param>
        /// <returns>String without specific characters</returns>
        public static string ParseInputString(string input) {
            // CHECK: Figure out why if not using this method, tree will not be built properly
            return input.Replace (" ", "").Replace ("(", "");
        }

        /// <summary>
        /// Calculates the number of levels your current node will have to go up (how many times root = root.Parent)
        /// </summary>
        /// <param name="input">Input string to parse</param>
        /// <returns></returns>
        public static int CalculateNumberOfLevelsToGoUp(string input) {
            int i = 0;

            while (input[i] == ')' && (input[i] != ',' || input[i] != ' ')) {
                i++;
                if (i == input.Length) break;
            }

            return i;
        }
        
        /// <summary>
        /// Returns a string containing all propositions(variables A,B,C, etc) in a specified binary tree
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static string GetPropositions(Nodes.Node root) {
            string visitedNodes = "";

            // Iterates through the binary tree and counts the number of propositions + adds to a string
            void IterateThroughNodes(Nodes.Node node) {

                if (node.left != null) {
                    IterateThroughNodes (node.left);
                }

                if (node.right != null) {
                    IterateThroughNodes (node.right);
                }

                if (node.left == null && node.right == null) {
                    if (!visitedNodes.Contains ((node as Nodes.PropositionNode).Name)) { // if we haven't visited that node we add it to the list
                        visitedNodes += node;
                    }
                }
            }

            IterateThroughNodes (root);

            return visitedNodes;
        }
    }
}
