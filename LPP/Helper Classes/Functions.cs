using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        /// Clears redundant 0s from given hexadecimal
        /// </summary>
        /// <param name="hexa"></param>
        /// <returns></returns>
        public static string ClearOutHexadecimal(string hexa) {

            var inputs = hexa.ToCharArray();

            string s = "";
            bool done = false;

            foreach (var character in inputs) {
                if (!done) {
                    if (character != '0') {
                        s += character;
                        done = true;
                    }
                }
                else {
                    s += character;
                }
            }
            
            
            return s;
        }
        
        
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
        /// Substrings a string from start index till end index (inclusive of the end index)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static string SubstringStartEndIndexBased(this string s, int startIndex, int endIndex) {
            
            if (startIndex < endIndex || startIndex < 0 || endIndex > s.Length) throw new IndexOutOfRangeException();

            string result = string.Empty;
            
            for (int i = startIndex; i <= endIndex; i++) {
                result += s[i];
            }
            
            return result;
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
            
            Node node;

            switch (root) {
                case null:
                    return null; 
                case BiImplicationNode _:
                    node = new BiImplicationNode();
                    break;
                case ConjunctionNode _:
                    node = new ConjunctionNode();
                    break;
                case DisjunctionNode _:
                    node = new DisjunctionNode();
                    break;
                case ImplicationNode _:
                    node = new ImplicationNode();
                    break;
                case NandNode _:
                    node = new NandNode();
                    break;
                case NotNode _:
                    node = new NotNode();
                    break;
                case PropositionNode propositionNode:
                    node = new PropositionNode(propositionNode.Name);
                    break;
                default:
                    throw new Exception("Something went wrong!");
            }

            node.left = DeepCopyTree(root.left);
            node.right = DeepCopyTree(root.right);

            return node;
        }
        
        /// <summary>
        /// Negates the tree if necessary. To be used in Tableux generation
        /// </summary>
        /// <param name="root"></param>
        /// <returns>Negated tree</returns>
        public static Node NegateTree(Node root) {
            if (root is NotNode) return DeepCopyTree(root.left); // so if input was ~(>(A,B)) it becomes >(A,B)
            NotNode notNode = new NotNode {left = DeepCopyTree(root)};
            return notNode;

        }

        #region Convertion of BiImplication and NAND

        /// <summary>
        /// Replaces all occurrences of Biimplication and Nand in a tree
        /// </summary>
        /// <param name="root"></param>
        public static void GetRidOfBiImplicationAndNand(ref Node root) {

            void Traverse(ref Node tree) {
                if (tree == null) return;

                if (tree is BiImplicationNode temp) {
                    tree = ConvertBiImplication(temp);
                }
                else if (tree is NandNode tempNand) {
                    tree = ConvertNand(tempNand);
                }
                
                Traverse(ref tree.left);
                Traverse(ref tree.right);
            }
            
            Traverse(ref root);
        }

        /// <summary>
        /// Converts biimplication
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private static Node ConvertBiImplication(BiImplicationNode root) {
            // =(A,B)    ==     &(>(A,B),>(B,A))
            
            ConjunctionNode conj = new ConjunctionNode();
            ImplicationNode imp1 = new ImplicationNode();
            ImplicationNode imp2 = new ImplicationNode();

            imp1.left = root.left;
            imp1.right = root.right;

            imp2.left = DeepCopyTree(root.right);
            imp2.right = DeepCopyTree(root.left);

            conj.left = imp1;
            conj.right = imp2;

            return conj;
        }

        /// <summary>
        /// Converts NandNode
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private static Node ConvertNand(NandNode root) {
            
            // %(A,B) == ~(&(A,B))

            NotNode notNode = new NotNode();
            
            ConjunctionNode conjunctionNode = new ConjunctionNode();
            conjunctionNode.left = root.left;
            conjunctionNode.right = root.right;

            notNode.left = conjunctionNode;
            
            return notNode;
        }

        #endregion
        
        
        /// <summary>
        /// Clones a generic list
        /// </summary>
        /// <param name="listToClone"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T: ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
