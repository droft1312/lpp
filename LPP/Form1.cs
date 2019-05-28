using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            _mainUnit.GenerateGraphImage (ref graphPicture, _mainUnit.Root);
            _mainUnit.PrintOutInfixNotation (_mainUnit.Root, infixTextBox);
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

            var truth_table = _mainUnit.DetermineTruthTable (_mainUnit.Root);

            string output = PrintOutTruthTable (truth_table.RowResultPairs);

            output += "Hexadecimal: " + _mainUnit.GenerateHexaDecimal (truth_table);

            OutputInformationToTextBox (output, ref outputTextbox);
        }

        private void simplifyTruthTableButton_Click (object sender, EventArgs e) {
            if (_mainUnit.Truth == null) return;

            var simplified = _mainUnit.SimplifyTruthTable (_mainUnit.Truth);

            string output = PrintOutTruthTable (simplified.RowResultPairs);

            output += "Hexadecimal: " + _mainUnit.GenerateHexaDecimal (simplified);

            OutputInformationToTextBox (output, ref outputTextbox);
        }

        private void disjunctiveFormButton_Click (object sender, EventArgs e) {
            if (_mainUnit.Truth == null) {
                MessageBox.Show("Please, generate a truth-table first!");
                return;
            }

            string disjunctivePrefixForm = _mainUnit.Truth.DisjunctiveForm();
            
            // create a tree and display it
            _mainUnit.ProcessStringInput(disjunctivePrefixForm);
            _mainUnit.GenerateGraphImage (ref graphPicture, _mainUnit.Root);
            _mainUnit.PrintOutInfixNotation (_mainUnit.Root, infixTextBox);
            
            // show a truth-table
            truthtableButton_Click(this, EventArgs.Empty);
        }

        private void nandifyButton_Click(object sender, EventArgs e) {
            _mainUnit.Root = _mainUnit.Nandify(_mainUnit.Root);
            _mainUnit.GenerateGraphImage(ref graphPicture, _mainUnit.Root);
        }
    }
}
