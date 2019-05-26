using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


using LPP.Nodes;
using LPP.TruthTable;
using static LPP.Functions;

namespace LPP
{
    public class Processor
    {
        private Node root;
        private TruthTable.TruthTable truthTable;

        public Node Root { get { return root; } }
        public TruthTable.TruthTable Truth { get { return truthTable; } } 

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

        public void CreateNANDTree(Node inputNode) {

        }
        
        /// <summary>
        /// Prints out tree in infix notation as opposed to prefix
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public string GetInfixNotation(Node root) {
            return root.GetInfix ();
        }

        /// <summary>
        /// Prints out a tree in infix notation to a given textbox
        /// </summary>
        /// <param name="root"></param>
        /// <param name="textBox"></param>
        public void PrintOutInfixNotation(Node root, TextBox textBox) {
            var result = GetInfixNotation (root);
            textBox.Text = result;
        }
        
        /// <summary>
        /// Returns a truth table based off the root
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public TruthTable.TruthTable DetermineTruthTable(Node root) {
            TruthTable.TruthTable truth = new TruthTable.TruthTable ();
            truth.CreateTruthTable (root);
            truthTable = truth; 
            return truth;
        }

        public TruthTable.TruthTable SimplifyTruthTable (TruthTable.TruthTable table) {
            table.Simplify ();
            return table;
        }

        public string GenerateHexaDecimal(TruthTable.TruthTable truth) {
            return truth.GetHexaDecimal ();
        }
    }
}
