using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization.Configuration;
using LPP.TruthTable;

namespace LPP
{
    public class TableuxDrawer
    {
        string transitional_output = string.Empty;
        string output = string.Empty;
        int counterForInorderTraversal = 0;
        private string nodeConnections = "";
        
        public void GenerateImage(PictureBox pictureBox, string inputToDraw) {
            var result = WriteToFile(inputToDraw);

            if (!result) {
                throw new Exception("There has been a problem with your input!");
            }
            
            Process dot = new Process ();
            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = "-Tpng -oabc.png abc.dot";
            dot.Start ();
            dot.WaitForExit ();
            pictureBox.ImageLocation = "abc.png";
        }
        
        private bool WriteToFile(string input) {
            try {
                File.WriteAllText ("abc.dot", input);
                return true;
            } catch (Exception e) {
                MessageBox.Show (e.Message);
                return false;
            }
        }

        // TODO: Finish it up here
        
        public string GenerateGraphvizInput<T>(T node) where T : TableuxNode {

            void ResetVariables() {
                output = "graph logics {\nnode [ fontname = \"Arial\" ]\n";
                transitional_output = string.Empty;
                nodeConnections = string.Empty;
                counterForInorderTraversal = 0;
                
                PreOrderTraverse(node);
            }
            
            ResetVariables();

            PrintNodeConnections(node);
            
            return "";
        }

        private void PrintNodeConnections<T>(T node) where T : TableuxNode {

            if (node == null) return;

            nodeConnections += node.GetConnections();
            
            PrintNodeConnections(node.Left);
            PrintNodeConnections(node.Right);
        }


        /// <summary>
        /// Does the pre-order traversal of the tree and prints it to the transitional_output
        /// </summary>
        /// <param name="node">Root node</param>
        private void PreOrderTraverse<T> (T node) where T : TableuxNode {
            if (node == null) {
                return;
            }

            counterForInorderTraversal++;
            /* first print data of node */

            transitional_output += "node" + node.id + " [ label = \"" + node + "\" ]\n";

            /* then recur on left sutree */
            PreOrderTraverse (node.Left);

            /* now recur on right subtree */
            PreOrderTraverse (node.Right);
        }
    }
}