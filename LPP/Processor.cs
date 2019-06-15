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
        public Node root;
        public Tableux Tableux => _tableux;

        private TruthTable.TruthTable truthTable;
        private Tableux _tableux;
        
        public TruthTable.TruthTable Truth => truthTable;
        

        public Node Root { get { return root; }
            set { root = value; }
        }

        /// <summary>
        /// Generates Graphiz Image
        /// </summary>
        /// <param name="pictureBox">PictureBox image will be displayed on</param>
        /// <param name="node">Tree imagige will be based upon</param>
        public void GenerateGraphImage (PictureBox pictureBox, Node node) {
            Graphiz graphiz = new Graphiz ();
            graphiz.GetGraphImage (pictureBox, node);
        }

        public void GenerateGraphImage(PictureBox pictureBox, TableuxNode node) {
            Graphiz graphiz = new Graphiz();
            graphiz.GetGraphImage(pictureBox,node);
        }

        /// <summary>
        /// Parses the string input and kicks off generation of a binary tree
        /// </summary>
        /// <param name="input"></param>
        public void ProcessStringInput (string input) {
            input = ParseInputString (input); // delete all unneccesary stuff

            char first_character = input[0];
            
            bool containsQuantifier = false;

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
                
                case '%':
                    root = new NandNode(input, null);
                    break;
                
                case '@':
                    case '!':
                    containsQuantifier = true;
                    break;

                default:
                    throw new Exception ("String processing went wrong. Source: class Processor, method ProcessStringInput(string input)");
            }
            
            if (containsQuantifier) HandleQuantifierInput(input);
            else BuildTree (root.Value, root);
        }
        
        /// <summary>
        /// A method that is called to actually build a tree from a given input. Is called from ProcessStringInput()
        /// </summary>
        /// <param name="input">input string</param>
        /// <param name="root">Node used in recursion for creating a binary tree</param>
        /// <exception cref="Exception"></exception>
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

            } else if (first_character == '%') {

                NandNode node = new NandNode(input, root);
                root.Insert(node);
                BuildTree(node.Value, node);

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

        private void HandleQuantifierInput(string input) {

            char c = input[0];

            switch (c) {
                case '@':
                    
                    break;

                case '!':

                    break;
                
                default:
                    throw new Exception("error in input");
            }

        }

        public Node Nandify(Node tree) {
            /*
             * possible cases:
             * 1) implication
             * 2) bi-implication
             * 3) disjunction
             * 4) conjunction
             * 5) not
             * 
             */

            if (tree == null) return null;

            
            var leftOfTree = tree.left;
            var rightOfTree = tree.right;
            
            var nandifiedLeftSubtree = Nandify(leftOfTree);
            var nandifiedRightSubtree = Nandify(rightOfTree);

            Node newTree;
            
            if (tree is ImplicationNode) { 
                newTree = new NandNode(nandifiedLeftSubtree, new NandNode(nandifiedRightSubtree, DeepCopyTree(nandifiedRightSubtree)));
            }
            else if (tree is BiImplicationNode) { 
                newTree = new NandNode(
                    new NandNode(
                        new NandNode(nandifiedLeftSubtree, DeepCopyTree(nandifiedLeftSubtree)),
                        new NandNode(nandifiedRightSubtree, DeepCopyTree(nandifiedRightSubtree))
                        ),
                    new NandNode(DeepCopyTree(nandifiedLeftSubtree), DeepCopyTree(nandifiedRightSubtree))
                );
                
            }
            else if (tree is DisjunctionNode) {
                newTree = new NandNode(
                    new NandNode(nandifiedLeftSubtree, DeepCopyTree(nandifiedLeftSubtree)),
                    new NandNode(nandifiedRightSubtree, DeepCopyTree(nandifiedRightSubtree))
                    );
            }
            else if (tree is ConjunctionNode) {
                newTree = new NandNode(
                    new NandNode(nandifiedLeftSubtree, DeepCopyTree(nandifiedRightSubtree)),
                    new NandNode(DeepCopyTree(nandifiedLeftSubtree), nandifiedRightSubtree)
                    );
            }
            else if (tree is NotNode) {
                newTree = new NandNode(nandifiedLeftSubtree, DeepCopyTree(nandifiedLeftSubtree));
            }
            else {
                newTree = tree;
            }

            return newTree;
        }

        public void GenerateTableux() {
            if (root == null) return;
            
            /*
             * There is probably two cases:
             * a) when the root is NOTNODE
             * b) when the root IS NOT NOTNODE
             * 
             */
            
            _tableux = new Tableux(root);
            
        }

        // TODO: Finish GenerateSixTruths()
        public void GenerateSixTruths(string input, RichTextBox outputTextBox) {
            ProcessStringInput(input);

            string hexadecimals = "";
            outputTextBox.Text = "";
            
            // normal inputted tree
            var truthTable = DetermineTruthTable(root);
            hexadecimals += "Normal tree: " + GenerateHexaDecimal(truthTable);

            
            hexadecimals += "\n" + new string('-', 15) + "\n";
            
            
            // dnf tree
            string disjunctivePrefixForm = truthTable.DisjunctiveForm();
            ProcessStringInput(disjunctivePrefixForm);
            truthTable = DetermineTruthTable(root);
            hexadecimals += "Disjunctive tree: " + GenerateHexaDecimal(truthTable);
            
            
            hexadecimals += "\n" + new string('-', 15) + "\n";
            
            // nandified
            root = Nandify(root);
            truthTable = DetermineTruthTable(root);
            hexadecimals += "Nandified: " + GenerateHexaDecimal(truthTable);
            
            
            outputTextBox.Text = hexadecimals;
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
