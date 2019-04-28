using LPP.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using static LPP.Functions;

namespace LPP.TruthTable
{
    public class TruthTable
    {
        public Dictionary<RowCombination, int> RowResultPairs { get; private set; }

        public TruthTable () {

        }

        public void Simplify () {
            if (RowResultPairs == null) throw new Exception ("There's no truth-table to simplify");

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
        }

        public void CreateTruthTable (Node root) {
            var nodes = GetPropositions (root).ToCharArray ();
            var combinations = GetAllCombinations (nodes);

            Dictionary<RowCombination, int> result = new Dictionary<RowCombination, int> ();

            foreach (var item in combinations) {
                string truth_values = item.ToString ();
                result.Add (item, System.Convert.ToInt32 (root.GetValue (truth_values)));
            }

            RowResultPairs = result;
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
