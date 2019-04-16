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
using LPP.Helper_Classes;

namespace LPP
{
    public partial class Form1 : Form
    {
        Processor mainUnit;

        public Form1 () {
            InitializeComponent ();

            mainUnit = new Processor ();
        }

        private void OutputInformationToTextBox(string input, ref RichTextBox textBox) {
            textBox.Text = input;
        }

        private void processInput_Click (object sender, EventArgs e) {
            string input = inputTextBox.Text;
            mainUnit.ProcessStringInput (input);
            mainUnit.GenerateGraphImage (ref graphPicture, mainUnit.Root);
        }

        private void inputTextBox_KeyDown (object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                this.processInput_Click (sender, EventArgs.Empty);
            }
        }

        private void propositionsNamesButton_Click (object sender, EventArgs e) {
            if (mainUnit.Root == null) { MessageBox.Show ("Enter your proposition first!"); return; }

            var props = mainUnit.GetPropositions (mainUnit.Root);

            string temp = "Propositions:\n";
            foreach (var c in props) {
                temp += c;
                temp += Environment.NewLine;
            }

            props = temp;

            OutputInformationToTextBox (props, ref outputTextbox);
        }

        private void truthtableButton_Click (object sender, EventArgs e) {
            if (mainUnit.Root == null) { MessageBox.Show ("Enter your proposition first!"); return; }

            var truth_table = mainUnit.DetermineTruthTable (mainUnit.Root);

            string output = string.Empty;

            foreach (var pair in truth_table) {
                var rowCombination = pair.Key;
                var result = pair.Value;

                string temp = rowCombination.ToString () + " -- " + result;

                output += temp;
                output += Environment.NewLine;
            }

            OutputInformationToTextBox (output, ref outputTextbox);
        }
    }
}
