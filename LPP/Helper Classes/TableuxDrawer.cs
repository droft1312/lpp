using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace LPP
{
    public class TableuxDrawer
    {
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
    }
}