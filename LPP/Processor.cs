using LPP.Helper_Classes;
using LPP.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using static LPP.Functions;

namespace LPP
{
    public class Processor
    {
        Node root;

        public Node Root { get { return root; } }

        /// <summary>
        /// Generates Graphiz Image
        /// </summary>
        /// <param name="pictureBox">PictureBox image will be displayed on</param>
        /// <param name="node">Tree imagige will be based upon</param>
        public void GenerateGraphImage (ref System.Windows.Forms.PictureBox pictureBox, Node node) {
            Graphiz graphiz = new Graphiz ();
            graphiz.GetGraphImage (ref pictureBox, in node);
        }

        /// <summary>
        /// Parses the string input and kicks off generation of a binary tree
        /// </summary>
        /// <param name="input"></param>
        public void ProcessStringInput (string input) {
            input = ParseInputString (input); // delete all unneccesary stuff

            char first_character = input[0];

            switch (first_character) {
                case '~':
                    root = new NotNode (input, null);
                    break;

                case '>':
                    root = new ImplicationNode (input, null);
                    break;

                case '=':
                    root = new BiImplicationNode (input, null);
                    break;

                case '&':
                    root = new ConjunctionNode (input, null);
                    break;

                case '|':
                    root = new DisjunctionNode (input, null);
                    break;

                default:
                    throw new Exception ("String processing went wrong. Source: class Processor, method ProcessStringInput(string input)");
            }

            BuildTree (root.Value, root);
        }

        private void BuildTree (string input, Node root) {
            if (input == string.Empty) return;

            char first_character = input[0];

            if (first_character == '~') {

                NotNode node = new NotNode (input, root);
                root.Insert (node);
                BuildTree (node.Value, node);

            } else if (first_character == '>') {

                ImplicationNode node = new ImplicationNode (input, root);
                root.Insert (node);
                BuildTree (node.Value, node);


            } else if (first_character == '=') {

                BiImplicationNode node = new BiImplicationNode (input, root);
                root.Insert (node);
                BuildTree (node.Value, node);

            } else if (first_character == '&') {

                ConjunctionNode node = new ConjunctionNode (input, root);
                root.Insert (node);
                BuildTree (node.Value, node);

            } else if (first_character == '|') {

                DisjunctionNode node = new DisjunctionNode (input, root);
                root.Insert (node);
                BuildTree (node.Value, node);

            } else if (first_character == ',') {
                if (root.parent == null) throw new Exception ("Error in your input");

                root = root.parent;
                input = input.Substring (1);
                BuildTree (input, root);

            } else if (Char.IsLetter (first_character)) {

                PropositionNode node = new PropositionNode (first_character, input, root);
                root.Insert (node);
                BuildTree (node.Value, node);

            } else if (first_character == ')') {

                int numberOflevels = CalculateNumberOfLevelsToGoUp (input);

                for (int i = 0; i < numberOflevels; i++) {
                    if (root.parent != null) root = root.parent;
                    else throw new Exception ("Error in string input. Source: class Processor, method BuildTree, else if (')')");
                }

                input = input.Substring (numberOflevels);
                BuildTree (input, root);
            }
        }

        // TODO: Create a method that prints out tree in infix notation (so the normal one)

        /// <summary>
        /// Prints out tree in infix notation as opposed to prefix
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private string PrintInfixNotation(Node root) {
            return string.Empty;
        }
        
        /// <summary>
        /// Creates a truth table
        /// </summary>
        /// <param name="root"></param>
        /// <returns>Returns a dictionary of RowCombination(A:0,B:1,C:0 etc.) : Int (result when those vars are plugged into formula)</returns>
        public Dictionary<RowCombination, int> DetermineTruthTable(Node root) {
            var nodes = GetPropositions (root).ToCharArray();
            var combinations = GetAllCombinations (nodes);
            
            Dictionary<RowCombination, int> result = new Dictionary<RowCombination, int> ();

            foreach (var item in combinations) {
                string truth_values = item.ToString ();
                result.Add (item, Convert.ToInt32 (root.GetValue (truth_values)));
            }

            return result;
        }

        /// <summary>
        /// Returns a string containing all propositions(variables A,B,C, etc) in a specified binary tree
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public string GetPropositions (Nodes.Node root) {
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
    }
}
