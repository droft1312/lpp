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
        
        public static (int total, string names) CountPropositions(Nodes.Node root) {
            int total = 0;
            string visitedNodes = "";

            // Iterates through the binary tree and counts the number of propositions + adds to a string
            // TOTEST: Test IterateThroughNodes(). Check whether it works properly
            void IterateThroughNodes(Nodes.Node node) {

                if (node.left != null) {
                    IterateThroughNodes (node.left);
                }

                if (node.right != null) {
                    IterateThroughNodes (node.right);
                }

                if (node.left == null && node.right == null) {
                    if (!visitedNodes.Contains ((node as Nodes.PropositionNode).Name)) {
                        total++;
                        visitedNodes += node;
                    }
                }
            }

            IterateThroughNodes (root);

            return (total, visitedNodes);
        }
    }
}
