using System;
using System.IO;
using System.Windows.Forms;
using LPP.Nodes;
using LPP.TruthTable;

namespace LPP
{
    public class Graphiz
    {
        string transitional_output = string.Empty;
        string output = string.Empty;
        int counterForInorderTraversal = 0;
        private string nodeConnections = "";

        /// <summary>
        /// Returns a complete image of graphviz
        /// </summary>
        /// <returns></returns>
        public void GetGraphImage (PictureBox pictureBox, Node baseNode) {
            WriteFileGRAPHVIZ (baseNode);
            System.Diagnostics.Process dot = new System.Diagnostics.Process ();
            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = "-Tpng -oabc.png abc.dot";
            dot.Start ();
            dot.WaitForExit ();
            pictureBox.ImageLocation = "abc.png";
        }

        public void GetGraphImage(PictureBox pictureBox, TableuxNode baseNode) {
            WriteFileGRAPHVIZ (baseNode);
            System.Diagnostics.Process dot = new System.Diagnostics.Process ();
            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = "-Tpng -oabc.png abc.dot";
            dot.Start ();
            dot.WaitForExit ();
            pictureBox.ImageLocation = "abc.png";
        }

        /// <summary>
        /// Writes output of <see cref="GenerateGraphVIZTEXT"/>() to a specific file
        /// </summary>
        private void WriteFileGRAPHVIZ (Node baseNode) {
            try {
                File.WriteAllText ("abc.dot", GenerateGraphVIZTEXT (baseNode));
            } catch (Exception e) {
                MessageBox.Show (e.Message);
            }
        }

        private void WriteFileGRAPHVIZ(TableuxNode baseNode) {
            try {
                var temp = GenerateGraphVIZTEXT(baseNode);
                File.WriteAllText ("abc.dot", GenerateGraphVIZTEXT (baseNode));
            } catch (Exception e) {
                MessageBox.Show (e.Message);
            }
        }

        /// <summary>
        /// Adds to the transitional_output relations between nodes. To be called ONLY AFTER <see cref="PreOrderTraverse"/>()
        /// </summary>
        /// <param name="root"></param>
        private void PrintNodeConnections (Node root) {
            if (root == null) {
                return;
            }

            /* first print data of node */

            nodeConnections += root.PrintConnections ();

            /* then recur on left subtree */
            PrintNodeConnections (root.left);

            /* now recur on right subtree */
            PrintNodeConnections (root.right);
        }

        private void PrintNodeConnections(TableuxNode root) {
            
            if (root == null) return;

            nodeConnections += root.PrintConnections();
            
            PrintNodeConnections(root.Left);
            PrintNodeConnections(root.Right);
        }

        /// <summary>
        /// Generates text that would be inputted to GraphVIZ
        /// </summary>
        /// <returns>Input string for graphviz</returns>
        private string GenerateGraphVIZTEXT (Node baseNode) {
            // -------------------------------------------------------------------
            // resetting all variables
            output = "graph logics {\nnode [ fontname = \"Arial\" ]\n";
            transitional_output = string.Empty;
            nodeConnections = string.Empty;
            counterForInorderTraversal = 0;
            PreOrderTraverse (baseNode);
            // -------------------------------------------------------------------

            PrintNodeConnections (baseNode);
            output += transitional_output;
            output += nodeConnections;

            output += "}";

            return output;
        }

        private string GenerateGraphVIZTEXT(TableuxNode baseNode) {
            // -------------------------------------------------------------------
            // resetting all variables
            output = "graph logics {\nnode [ fontname = \"Arial\" ]\n";
            transitional_output = string.Empty;
            nodeConnections = string.Empty;
            counterForInorderTraversal = 0;
            PreOrderTraverse (baseNode);
            // -------------------------------------------------------------------

            PrintNodeConnections (baseNode);
            output += transitional_output;
            output += nodeConnections;

            output += "}";

            return output;
        }

        /// <summary>
        /// Does the pre-order traversal of the tree and prints it to the transitional_output
        /// </summary>
        /// <param name="node">Root node</param>
        private void PreOrderTraverse (Node node) {
            if (node == null) {
                return;
            }

            counterForInorderTraversal++;
            /* first print data of node */

            transitional_output += "node" + node.NodeNumber + " [ label = \"" + node.ToString () + "\" ]\n";

            /* then recur on left sutree */
            PreOrderTraverse (node.left);

            /* now recur on right subtree */
            PreOrderTraverse (node.right);
        }

        private void PreOrderTraverse(TableuxNode node) {
            if (node == null) {
                return;
            }

            counterForInorderTraversal++;
            /* first print data of node */

            transitional_output += "node" + node.id + " [ label = \"" + node + "\" ]\n";

            /* then recur on left subtree */
            PreOrderTraverse (node.Left);

            /* now recur on right subtree */
            PreOrderTraverse (node.Right);
        }
    }
}
