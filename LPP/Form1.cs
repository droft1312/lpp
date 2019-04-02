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
        Processor mainUnit;

        public Form1 () {
            InitializeComponent ();

            mainUnit = new Processor ();
        }

        private void processInput_Click (object sender, EventArgs e) {
            string input = ParseInputString (inputTextBox.Text);
            mainUnit.ProcessStringInput (input);
            MessageBox.Show ("");
        }
    }
}
