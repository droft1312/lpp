using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPP.Nodes;
using static LPP.Functions;

namespace LPP
{
    public partial class Form1 : Form
    {
        private readonly Processor _mainUnit;

        public Form1 () {
            InitializeComponent ();

            _mainUnit = new Processor ();
        }

        private void OutputInformationToTextBox(string input, ref RichTextBox textBox) {
            textBox.Text = input;
        }

        private void processInput_Click (object sender, EventArgs e) {
            string input = inputTextBox.Text;
            _mainUnit.ProcessStringInput (input);
            _mainUnit.GenerateGraphImage (graphPicture, _mainUnit.Root);
            _mainUnit.PrintOutInfixNotation (_mainUnit.Root, infixTextBox);
            PrintOutDepthOfTree();
        }

        private void inputTextBox_KeyDown (object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                processInput_Click (sender, EventArgs.Empty);
            }
        }

        private void propositionsNamesButton_Click (object sender, EventArgs e) {
            if (_mainUnit.Root == null) { MessageBox.Show ("Enter your proposition first!"); return; }

            string props = "";
            _mainUnit.Root.GetAllPropositions (ref props);


            string temp = "Propositions:\n";
            foreach (var c in props) {
                temp += c;
                temp += Environment.NewLine;
            }

            props = temp;

            OutputInformationToTextBox (props, ref outputTextbox);
        }

        private void truthtableButton_Click (object sender, EventArgs e) {
            if (_mainUnit.Root == null) { MessageBox.Show ("Enter your proposition first!"); return; }

            if (_mainUnit.Root is Quantifier) { MessageBox.Show("Cannot do that stuff on a quantifier!"); return; }

            var truth_table = _mainUnit.DetermineTruthTable (_mainUnit.Root);

            string output = PrintOutTruthTable (truth_table.RowResultPairs);

            output += "Hexadecimal: " + ClearOutHexadecimal(_mainUnit.GenerateHexaDecimal (truth_table));

            OutputInformationToTextBox (output, ref outputTextbox);
        }

        private void simplifyTruthTableButton_Click (object sender, EventArgs e) {
            if (_mainUnit.Truth == null) return;
            
            if (_mainUnit.Root is Quantifier) { MessageBox.Show("Cannot do that stuff on a quantifier!"); return; }
            
            var simplified = _mainUnit.SimplifyTruthTable (_mainUnit.Truth);

            string output = PrintOutTruthTable (simplified.RowResultPairs);

            output += "Hexadecimal: " + ClearOutHexadecimal(_mainUnit.GenerateHexaDecimal (simplified));

            OutputInformationToTextBox (output, ref outputTextbox);
        }

        private void disjunctiveFormButton_Click (object sender, EventArgs e) {
            if (_mainUnit.Truth == null) {
                MessageBox.Show("Please, generate a truth-table first!");
                return;
            }
            
            if (_mainUnit.Root is Quantifier) { MessageBox.Show("Cannot do that stuff on a quantifier!"); return; }

            string disjunctivePrefixForm = _mainUnit.Truth.DisjunctiveForm();
            
            // create a tree and display it
            _mainUnit.ProcessStringInput(disjunctivePrefixForm);
            _mainUnit.GenerateGraphImage (graphPicture, _mainUnit.Root);
            _mainUnit.PrintOutInfixNotation (_mainUnit.Root, infixTextBox);
            PrintOutDepthOfTree();
            
            // show a truth-table
            truthtableButton_Click(this, EventArgs.Empty);
        }

        private void nandifyButton_Click(object sender, EventArgs e) {
            if (_mainUnit.Root is Quantifier) { MessageBox.Show("Cannot do that stuff on a quantifier!"); return; }

            _mainUnit.Root = _mainUnit.Nandify(_mainUnit.Root);
            
//            new Thread(delegate() { _mainUnit.GenerateGraphImage(graphPicture, _mainUnit.root); }).Start();

            PrintOutDepthOfTree();

//            _mainUnit.GenerateGraphImage(graphPicture, _mainUnit.Root);
        }

        private void sixTruthsButton_Click(object sender, EventArgs e) {
            if (_mainUnit.Root is Quantifier) { MessageBox.Show("Cannot do that stuff on a quantifier!"); return; }
            
            string input = inputTextBox.Text;

            if (input == string.Empty) {
                MessageBox.Show("Please enter input textbox!");
                return;
            }
            
            _mainUnit.GenerateSixTruths(input, outputTextbox);
        }

        private void tableuxButton_Click(object sender, EventArgs e) {
            if (_mainUnit.Root == null) return;
            
            _mainUnit.GenerateTableux();
//            _mainUnit.GenerateGraphImage(graphPicture, _mainUnit.Tableux.Tree);

            var result = _mainUnit.Tableux.ValidateTautology();

            outputTextbox.Text = result ? "Given tree is a tautology!" : "Given tree IS NOT a tautology!";
            PrintOutDepthOfTree();
        }


        private void PrintOutDepthOfTree() {
            if (_mainUnit.Root == null) return;
            outputTextbox.Text += $"\n{new string('-', 15)}\nDepth: " + MaxDepthOfTree(_mainUnit.Root);
        }
    }
}
