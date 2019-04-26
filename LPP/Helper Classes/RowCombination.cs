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
        private struct NodeValue
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

            public static bool operator ==(NodeValue n1, NodeValue n2) {
                return (n1.Name == n2.Name) && (n1.Value == n2.Value);
            }
            public static bool operator != (NodeValue n1, NodeValue n2) {
                return !(n1 == n2);
            }
        }

        NodeValue[] nodeValues; // values in this row

        public RowCombination(char[] names, string input) {
            nodeValues = new NodeValue[names.Length];
            for (int i = 0; i < nodeValues.Length; i++) nodeValues[i] = new NodeValue (names[i], -1);

            AssignValues (input);
        }

        // indexer for rowcombination
        public bool this[char c] {
            get {
                foreach (var node in nodeValues) if (node.Name == c) return (node.Value == 1);
                throw new Exception ("Such element wasn't found");
            }
        }

        
        private NodeValue GetDistinctVariable() {
            if (!SatisfiesConditionForSimplification ()) throw new Exception ("Not Good");


        }

        /// <summary>
        /// This method shall be used in simplification process
        /// </summary>
        /// <returns></returns>
        public bool SatisfiesConditionForSimplification() {

            List<int> GetValues () {
                List<int> result = new List<int> (nodeValues.Length);
                foreach (var item in nodeValues) result.Add (item.Value);
                return result;
            }

            var vals = GetValues ().Distinct().ToList();

            if (vals.Count != 2) return false;

            // count 0s and 1s
            Dictionary<int, int> counter = new Dictionary<int, int> ();
            counter.Add (0, 0);
            counter.Add (1, 0);

            for (int i = 0; i < vals.Count(); i++) {
                counter[vals[i]]++;
            }

            if (counter[0] == vals.Count - 1 || counter[1] == vals.Count - 1) return true;
            else return false;
        }

        public bool Matches(RowCombination r) {

            // 0001     =>   1
            // 1111     =>   1
            // 0101     =>   1

            /*    
             *  Find which variable is different, for example 0001 means that variable D = 1 and thus is a distinct one
             *  Check if the other rowcombination has it as well, if so, return true, if not, return false
             */

            return false;
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
                s += string.Format ("{0}:{1} ", node.Name, node.Value);
            }
            
            return s;
        }

        public static bool operator ==(RowCombination r1, RowCombination r2) {
            if (object.ReferenceEquals(r1, null)) {
                if (object.ReferenceEquals(r2, null)) {
                    return true;
                }
                return false;
            }
            if (object.ReferenceEquals (r2, null)) {
                if (object.ReferenceEquals (r1, null)) {
                    return true;
                }
                return false;
            }

            return r1.nodeValues == r2.nodeValues;
        }

        public static bool operator != (RowCombination r1, RowCombination r2) {
            return !(r1 == r2);
        }
    }
}
