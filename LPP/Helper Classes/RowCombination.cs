using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Helper_Classes
{
    public class RowCombination
    {
        /// <summary>
        /// Represents the structure containing name of an abstract proposition and its value
        /// </summary>
        struct NodeValue
        {
            public char Name { get; set; }
            public int Value { get; set; }

            public NodeValue (char name, int value) {
                Name = name;
                Value = value;
            }

            public void SetValue(int value) {
                Value = value;
            }
        }

        NodeValue[] nodeValues; // values in this row

        public RowCombination(char[] names, string input) {
            nodeValues = new NodeValue[names.Length];
            for (int i = 0; i < nodeValues.Length; i++) nodeValues[i] = new NodeValue (names[i], -1);

            AssignValues (input);
        }

        public bool GetValueOfElement(char c) {
            foreach (var node in nodeValues) if (node.Name == c) return (node.Value == 1);
            throw new Exception ("Such element wasn't found");
        }

        /// <summary>
        /// Parses the input to assign values to the nodes
        /// </summary>
        /// <param name="input"></param>
        private void AssignValues (string input) {
            if (input.Length != nodeValues.Length) throw new Exception ("Input length different");

            for (int i = 0; i < input.Length; i++) {
                int val = int.Parse (input[i].ToString());
                nodeValues[i].SetValue (val);
            }
        }


        public override string ToString () {
            string s = "";

            foreach (var node in nodeValues) {
                s += string.Format ("{0}: {1}, ", node.Name, node.Value);
            }
            
            return s;
        }
    }
}
