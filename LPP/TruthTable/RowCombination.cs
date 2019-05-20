using System;
using System.Collections.Generic;
using System.Linq;

namespace LPP.TruthTable
{
#pragma warning disable 0660
#pragma warning disable 0661
    public class RowCombination
    {
        /// <summary>
        /// Represents the structure containing name of an abstract proposition and its value
        /// </summary>
        private struct NodeValue
        {
            public char Name { get; set; }
            public object Value { get; set; }

            public NodeValue (char name, int value) {
                Name = name;
                Value = value;
            }

            public void SetValue(int value) {
                Value = value;
            }

            public void SetValue (string value) {
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
        private readonly string input;

        public RowCombination(char[] names, string input) {
            nodeValues = new NodeValue[names.Length];
            for (int i = 0; i < nodeValues.Length; i++) nodeValues[i] = new NodeValue (names[i], -1);

            this.input = input; // is done for disjunctive to reverse

            AssignValues (input);
        }

        public string GetPrefixDisjunctiveForm() {
            string result = string.Empty;

            for (int i = 0; i < nodeValues.Length; i++) {
            }

            return result;
        }

        public string GetDisjunctiveForm() {
            string result = string.Empty;

            for (int i = 0; i < nodeValues.Length; i++) {
                if (nodeValues[i].Value is Int32) { 
                    result += (((int)nodeValues[i].Value == 0) ? "~" : string.Empty) + nodeValues[i].Name;
                    result += (i != nodeValues.Length - 1) ? " & " : string.Empty;
                } else {
                    throw new Exception ("There was a star in a row apparently");
                }
            }

            return result;
        }

        /// <summary>
        /// This method shall be used in simplification process. ONLY TO BE USED ON ROWS THAT DO NOT CONTAIN STRING VALUES (*)
        /// </summary>
        /// <returns></returns>
        public bool SatisfiesConditionForSimplification() {

            List<int> GetValues () {
                List<int> result = new List<int> (nodeValues.Length);
                foreach (var item in nodeValues) {
                    if (item.Value is String) throw new Exception ("You used SatisfiesConditionForSimplification() on a row that has * in it!");
                    result.Add ((int)item.Value);
                }
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

        /// <summary>
        /// Checks whether 'r' matches this instance of RowCombination
        /// </summary>
        /// <param name="r">Row combination to compare with</param>
        /// <returns></returns>
        public bool Matches(RowCombination r) {

            // 0001     =>   1
            // 1111     =>   1
            // 0101     =>   1

            if (this.nodeValues.Length != r.nodeValues.Length) return false;

            var length = nodeValues.Length;

            for (int i = 0; i < length; i++) {
                var ourValue = this.nodeValues[i];
                var valueToCompare = r.nodeValues[i];

                if (!(ourValue.Value is String || valueToCompare.Value is String)) {
                    if ((int)ourValue.Value != (int)valueToCompare.Value) {
                        return false;
                    }
                }
            }


            return true;
        }

        /// <summary>
        /// Parses the input to assign values to the nodes
        /// </summary>
        /// <param name="input"></param>
        private void AssignValues (string input) {
            if (input.Length != nodeValues.Length) throw new Exception ("Input length different");

            for (int i = 0; i < input.Length; i++) {
                char character = input[i];

                dynamic toAdd = "";

                try {
                    toAdd = Functions.Convert<int> (character.ToString ());
                } catch (Exception) {
                    toAdd = character.ToString ();
                } finally {
                    nodeValues[i].SetValue (toAdd);
                }
            }
        }

        /// <summary>
        /// Returns a KeyValuePair of a variable in a rowcombination that is different.
        /// For instance: A:0, B:0, C:1. This function will return C:1, because it's a different one.
        /// Before calling this function, <b>make sure</b> that this RowCombination satisifies condition for Simplification
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<char, int> GetDistinctProposition() {
            if (!SatisfiesConditionForSimplification ()) throw new Exception ("This is not a Simplifiable RowCombination");

            var truths = this.nodeValues.Where (x => (int)x.Value == 1).Count ();
            var falses = this.nodeValues.Where (x => (int)x.Value == 0).Count ();

            var result = this.nodeValues.First (node => (int)node.Value == ((truths < falses) ? 1 : 0));

            return new KeyValuePair<char, int> (result.Name, (int)result.Value);
        }

        public string GetNames() {
            string res = string.Empty;

            for (int i = 0; i < nodeValues.Length; i++) {
                res += nodeValues[i].Name;
            }

            return res;
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

            return Enumerable.SequenceEqual (r1.nodeValues, r2.nodeValues);
        }

        public static bool operator != (RowCombination r1, RowCombination r2) {
            return !(r1 == r2);
        }

        public static RowCombination InstantiateRowCombinationOnlyWithStars(char[] names) {
            return new RowCombination (names, new string ('*', names.Length));
        }
    }

#pragma warning restore 0660
#pragma warning restore 0661
}
