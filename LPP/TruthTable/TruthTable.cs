using System;
using System.Linq;
using System.Collections.Generic;

using LPP.Nodes;
using static LPP.Functions;

namespace LPP.TruthTable
{
    public class TruthTable
    {
        public Dictionary<RowCombination, int> RowResultPairs { get; private set; }
        private bool TruthTableSimplified = false;

        public TruthTable () {
        }

        public void Simplify () {
            if (RowResultPairs == null) throw new Exception ("There's no truth-table to simplify");
            if (TruthTableSimplified) return;

            bool DictionaryHasRowCombination (KeyValuePair<RowCombination, int> r, Dictionary<RowCombination, int> d) {
                foreach (KeyValuePair<RowCombination, int> item in d) {
                    if (r.Key.Matches (item.Key) && r.Value == item.Value) {
                        return true;
                    }
                }
                return false;
            }

            Dictionary<RowCombination, int> simplifiedTruth = new Dictionary<RowCombination, int> ();

            foreach (KeyValuePair<RowCombination, int> pair in RowResultPairs) {

                if (DictionaryHasRowCombination (pair, simplifiedTruth)) continue;

                if (pair.Key.SatisfiesConditionForSimplification ()) {
                    var distinctVariable = pair.Key.GetDistinctProposition ();

                    simplifiedTruth.Add (CreateRowCombinationWithStars (distinctVariable), pair.Value);
                }

            }

            foreach (KeyValuePair<RowCombination, int> pair in RowResultPairs) {

                if (!DictionaryHasRowCombination (pair, simplifiedTruth) && !pair.Key.SatisfiesConditionForSimplification ()) {
                    simplifiedTruth.Add (pair.Key, pair.Value);
                }

            }

            RowResultPairs = simplifiedTruth;
            TruthTableSimplified = true;
        }

        public string CreateDisjunctiveForm() {
            string result = string.Empty;

            foreach (KeyValuePair<RowCombination, int> item in RowResultPairs) {
                if (item.Value == 1) {
                    var row = item.Key;

                    result += row.GetDisjunctiveForm ();
                    result += " | ";
                    result += "\n";
                }
            }

            return result;
        }

        public void CreateTruthTable (Node root) {
            var props = ""; root.GetAllPropositions (ref props);
            var nodes = props.ToCharArray ();
            var combinations = GetAllCombinations (nodes);

            Dictionary<RowCombination, int> result = new Dictionary<RowCombination, int> ();

            foreach (var item in combinations) {
                string truth_values = item.ToString ();
                result.Add (item, System.Convert.ToInt32 (root.GetValue (truth_values)));
            }

            RowResultPairs = result;
            TruthTableSimplified = false;
        }

        /// <summary>
        /// Creates all combinations based on the given nodes
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public  RowCombination[] GetAllCombinations (char[] nodes) {
            var allCombinations = BitFluctuation (nodes.Length);

            List<RowCombination> rows = new List<RowCombination> ();

            foreach (var combination in allCombinations) {
                string temp = string.Empty;
                foreach (var character in combination) temp += character;

                rows.Add (new RowCombination (nodes, temp));
            }

            return rows.ToArray ();
        }

        /// <summary>
        /// Creates all possible combinations for truth-table
        /// </summary>
        /// <param name="numberOfVariables"></param>
        /// <returns>List of lists of combinations</returns>
        private dynamic BitFluctuation (int numberOfVariables) {
            const string set = "01";
            List<char[]> setOfSets = new List<char[]> ();
            for (int i = 0; i < numberOfVariables; i++) setOfSets.Add (set.ToArray ());
            var result = setOfSets.CartesianProduct ();
            return result;
        }

        public string GetHexaDecimal () {
            string results = string.Empty;
            for (int i = RowResultPairs.Values.Count () - 1; i >= 0; i--) results += RowResultPairs.Values.ElementAt (i);
            return System.Convert.ToInt32 (results, 2).ToString ("X");
        }

        private RowCombination CreateRowCombinationWithStars(KeyValuePair<char, int> pair) {

            var random_row = RowResultPairs.First ().Key;
            string names = random_row.GetNames ();
            string values = string.Empty;

            for (int i = 0; i < names.Length; i++) {
                if (names[i] != pair.Key) {
                    values += "*";
                } else {
                    values += pair.Value;
                }
            }

            return new RowCombination (names.ToCharArray (), values);
        }
    }
}
