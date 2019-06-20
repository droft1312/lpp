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

        private readonly NodeValue[] _nodeValues; // values in this row

        public RowCombination(char[] names, string input) {
            _nodeValues = new NodeValue[names.Length];
            for (int i = 0; i < _nodeValues.Length; i++) _nodeValues[i] = new NodeValue (names[i], -1);


            AssignValues (input);
        }

        public string GetPrefixDisjunctiveForm() {

            string s = string.Empty;

            for (int i = 0; i < _nodeValues.Length; i++) {
                var nodeValue = _nodeValues[i];

                if (nodeValue.Value is Int32) {
                    s = Functions.Wrap(s, ((int) nodeValue.Value) == 1 ? nodeValue.Name.ToString() : "~(" + nodeValue.Name + ")", '&');
                }
            }


            return s;
        }

        /// <summary>
        /// This method shall be used in simplification process. ONLY TO BE USED ON ROWS THAT DO NOT CONTAIN STRING VALUES (*)
        /// </summary>
        /// <returns></returns>
        public bool SatisfiesConditionForSimplification() {
            List<int> GetValues () {
                List<int> result = new List<int> (_nodeValues.Length);
                foreach (var item in _nodeValues) {
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

            var listOfVars = GetValues();
            for (int i = 0; i < listOfVars.Count; i++) {
                counter[listOfVars[i]]++;
            }

            if (counter[0] == vals.Count - 1 || counter[1] == vals.Count - 1) return true;
            return false;
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

            if (_nodeValues.Length != r._nodeValues.Length) return false;

            var length = _nodeValues.Length;

            for (int i = 0; i < length; i++) {
                var ourValue = _nodeValues[i];
                var valueToCompare = r._nodeValues[i];

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
            if (input.Length != _nodeValues.Length) throw new Exception ("Input length different");

            for (int i = 0; i < input.Length; i++) {
                char character = input[i];

                dynamic toAdd = "";

                try {
                    toAdd = Functions.Convert<int> (character.ToString ());
                } catch (Exception) {
                    toAdd = character.ToString ();
                } finally {
                    _nodeValues[i].SetValue (toAdd);
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

            var truths = _nodeValues.Where (x => (int)x.Value == 1).Count ();
            var falses = _nodeValues.Where (x => (int)x.Value == 0).Count ();

            var result = _nodeValues.First (node => (int)node.Value == ((truths < falses) ? 1 : 0));

            return new KeyValuePair<char, int> (result.Name, (int)result.Value);
        }

        public string GetNames() {
            string res = string.Empty;

            for (int i = 0; i < _nodeValues.Length; i++) {
                res += _nodeValues[i].Name;
            }

            return res;
        }

        public override string ToString () {
            string s = "";
            foreach (var node in _nodeValues) {
                s += string.Format ("{0}:{1} ", node.Name, node.Value);
            }
            
            return s;
        }
        
        
        public static bool operator ==(RowCombination r1, RowCombination r2) {
            if (ReferenceEquals(r1, null)) {
                if (ReferenceEquals(r2, null)) {
                    return true;
                }
                return false;
            }
            if (ReferenceEquals (r2, null)) {
                if (ReferenceEquals (r1, null)) {
                    return true;
                }
                return false;
            }

            return r1._nodeValues.SequenceEqual (r2._nodeValues);
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
