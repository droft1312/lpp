using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPP.Nodes;
using LPP.TruthTable;

namespace LPP
{
    /// <summary>
    /// Class containing helper-functions
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Creates a prefix string
        /// </summary>
        /// <param name="initial">Current string</param>
        /// <param name="toAdd">Thing you wanna add</param>
        /// <param name="symbol">Symbol that you wrap it all around with</param>
        /// <returns>Wrapped new string</returns>
        public static string Wrap(string initial, string toAdd, char symbol) {

            const char openingParentheses = '(';
            const char closingParentheses = ')';
            const char comma = ',';

            if (initial == string.Empty) return toAdd;
            return symbol.ToString() + openingParentheses + toAdd + comma + initial + closingParentheses;
        }
        
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
        
        /// <summary>
        /// Copies given 'root' node but doesn't deep copy the whole tree
        /// </summary>
        /// <param name="root">Node to copy</param>
        /// <returns>Copy of a given node</returns>
        /// <exception cref="Exception">Throws an exception if you input a not-specified-before Node</exception>
        public static Node DeepCopyTree(Node root) {
            
            Node node = null;

            if (root == null) return node; // return null basically
            

            if (root is BiImplicationNode) {
                node = new BiImplicationNode(root.Value);
            }
            else if (root is ConjunctionNode) {
                node = new ConjunctionNode(root.Value);
            }
            else if (root is DisjunctionNode) {
                node = new DisjunctionNode(root.Value);
            }
            else if (root is ImplicationNode) {
                node = new ImplicationNode(root.Value);
            }
            else if (root is NandNode) {
                node = new NandNode(root.Value);
            }
            else if (root is NotNode) {
                node = new NotNode(root.Value);
            }
            else if (root is PropositionNode) {
                node = new PropositionNode(((PropositionNode)root).Name, root.Value, null);
            }
            else {
                throw new Exception("Something went wrong!");
            }

            node.left = DeepCopyTree(root.left);
            node.right = DeepCopyTree(root.right);

            return node;
        }
    }
}
