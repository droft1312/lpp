using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LPP.TruthTable;

namespace LPP
{
    /// <summary>
    /// Class containing helper-functions
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Returns a printed truth-table
        /// </summary>
        /// <param name="truthTable"></param>
        /// <returns></returns>
        public static string PrintOutTruthTable(Dictionary<RowCombination, int> truthTable) {
            string output = string.Empty;
            
            foreach (var pair in truthTable) {
                var rowCombination = pair.Key;
                var result = pair.Value;

                string temp = rowCombination.ToString () + " -- " + result;

                output += temp;
                output += Environment.NewLine;
            }

            return output;
        }

        /// <summary>
        /// Deletes ' ', '(' from a string
        /// </summary>
        /// <param name="input">String to parse</param>
        /// <returns>String without specific characters</returns>
        public static string ParseInputString(string input) {
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
        public static string GetPropositions (Nodes.Node root) {
            string visitedNodes = "";

            // Iterates through the binary tree and counts the number of propositions + adds to a string
            void IterateThroughNodes (Nodes.Node node) {

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

        /// <summary>
        /// Creates all combinations based on the given nodes
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static RowCombination[] GetAllCombinations(char[] nodes) {
            var allCombinations = BitFluctuation (nodes.Length);
            
            List<RowCombination> rows = new List<RowCombination> ();

            foreach (var combination in allCombinations) {
                string temp = string.Empty;
                foreach (var character in combination) temp += character;

                rows.Add (new RowCombination (nodes, temp));
            }

            return rows.ToArray ();
        }
        
        /// <summary>
        /// Creates all possible combinations for truth-table
        /// </summary>
        /// <param name="numberOfVariables"></param>
        /// <returns>List of lists of combinations</returns>
        private static dynamic BitFluctuation(int numberOfVariables) {
            const string set = "01";
            List<char[]> setOfSets = new List<char[]> ();
            for (int i = 0; i < numberOfVariables; i++) setOfSets.Add (set.ToArray ());
            var result = setOfSets.CartesianProduct ();
            return result;
        }

        /// <summary>
        /// Tries to convert string s to a format T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Convert<T> (string s) {
            var typeConverter = TypeDescriptor.GetConverter (typeof (T));
            if (typeConverter != null && typeConverter.CanConvertFrom (typeof (string))) {
                return (T)typeConverter.ConvertFrom (s);
            }

            return default (T);
        }
    }
}
