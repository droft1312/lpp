using LPP.Nodes;
using System;
using System.Collections.Generic;
using static LPP.Functions;

namespace LPP
{
    public class Processor
    {
        Node root;

        public Node Root { get { return root; } }

        public void GenerateGraphImage (ref System.Windows.Forms.PictureBox pictureBox, Node node) {
            Graphiz graphiz = new Graphiz ();
            graphiz.GetGraphImage (ref pictureBox, in node);
        }

        public void ProcessStringInput (string input) {
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

        private string PrintInfixNotation(Node root) {
            return string.Empty;
        }

        // TODO: Calculate the truth-table

        private void DetermineTruthTable(Node root) {
            var nodes = GetPropositions (root).ToCharArray();
            int number_of_props = nodes.Length;

            var allCombinations = BitFluctuation (number_of_props);
        }
    }
}
