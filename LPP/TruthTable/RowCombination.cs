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

        public bool Matches(KeyValuePair<RowCombination, int> rowResult) {
            var row = rowResult.Key;

            if (row.GetNames().Length != GetNames().Length) return false;

            for (int i = 0; i < _nodeValues.Length; i++) {
                if (this._nodeValues[i].Value == 2) continue;
                if (this._nodeValues[i].Value != row._nodeValues[i].Value) return false;
            }

            return true;
        }
    }

#pragma warning restore 0660
#pragma warning restore 0661
}
