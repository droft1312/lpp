using System;
using System.Linq;
using System.Collections.Generic;
using LPP.Nodes;
using System.Text;

namespace LPP.TruthTable
{
    public class TruthTable
    {
        public Dictionary<RowCombination, int> RowResultPairs { get; private set; }
        private bool _truthTableSimplified;

        // TODO: Simplify doesn't work properly always. FIX IT
        public void Simplify() {
            if (RowResultPairs == null) throw new Exception("There's no truth-table to simplify");
            if (_truthTableSimplified) return;

            #region Check for Contradiction / Tautology

            if (IsContradiction()) {
                // truth-table is contradiction
                SimplifyContradiction();
                _truthTableSimplified = true;
                return;
            }

            if (IsTautology()) {
                // if the truth-table is actually tautology
                SimplifyTautology();
                _truthTableSimplified = true;
                return;
            }

            #endregion


            bool DictionaryHasRowCombination(KeyValuePair<RowCombination, int> r, Dictionary<RowCombination, int> d) {
                foreach (KeyValuePair<RowCombination, int> item in d) {
                    if (r.Key.Matches(item.Key) && r.Value == item.Value) {
                        return true;
                    }
                }

                return false;
            }

            Dictionary<RowCombination, int> simplifiedTruth = new Dictionary<RowCombination, int>();

            foreach (KeyValuePair<RowCombination, int> pair in RowResultPairs) {
                if (DictionaryHasRowCombination(pair, simplifiedTruth)) continue;

                if (pair.Key.SatisfiesConditionForSimplification()) {
                    var distinctVariable = pair.Key.GetDistinctProposition();

                    simplifiedTruth.Add(CreateRowCombinationWithStars(distinctVariable), pair.Value);
                }
            }

            foreach (KeyValuePair<RowCombination, int> pair in RowResultPairs) {
                if (!DictionaryHasRowCombination(pair, simplifiedTruth) &&
                    !pair.Key.SatisfiesConditionForSimplification()) {
                    simplifiedTruth.Add(pair.Key, pair.Value);
                }
            }

            RowResultPairs = simplifiedTruth;
            _truthTableSimplified = true;
        }


        /// <summary>
        /// Creates a truth-table consisting only of *
        /// </summary>
        private void SimplifyTautology() {
            Dictionary<RowCombination, int> simplifiedTruth = new Dictionary<RowCombination, int>();
            simplifiedTruth.Add(
                RowCombination.InstantiateRowCombinationOnlyWithStars(RowResultPairs.First().Key.GetNames()
                    .ToCharArray()), 1);
            RowResultPairs = simplifiedTruth;
        }

        private void SimplifyContradiction() {
            Dictionary<RowCombination, int> simplifiedTruth = new Dictionary<RowCombination, int>();
            simplifiedTruth.Add(
                RowCombination.InstantiateRowCombinationOnlyWithStars(RowResultPairs.First().Key.GetNames()
                    .ToCharArray()), 0);
            RowResultPairs = simplifiedTruth;
        }

        // TODO: Finish disjunctive form
        public string DisjunctiveForm() {
            
            const int TRUTH = 1;

            string s = string.Empty;
            
            foreach (KeyValuePair<RowCombination,int> pair in RowResultPairs) {
                
                if (pair.Value == TRUTH) { // if the result of the row is true, we do a disjunctive form
                    var row = pair.Key;
                    string prefixDisjFormOfRow = row.GetPrefixDisjunctiveForm();
                    s = Functions.Wrap(s, prefixDisjFormOfRow, '|');
                }
            }
 
            return s;
        }

        public void CreateTruthTable(Node root) {
            var props = "";
            root.GetAllPropositions(ref props);
            props = String.Concat(props.OrderBy(c => c));
            var nodes = props.ToCharArray();
            var combinations = GetAllCombinations(nodes);

            Dictionary<RowCombination, int> result = new Dictionary<RowCombination, int>();

            int tempCounter = 0;

            foreach (var item in combinations) {
                tempCounter++;
                string truth_values = item.ToString();
                result.Add(item, System.Convert.ToInt32(root.GetValue(truth_values)));
            }

            RowResultPairs = result;
            _truthTableSimplified = false;
        }

        /// <summary>
        /// Creates all combinations based on the given nodes
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public RowCombination[] GetAllCombinations(char[] nodes) {
            var allCombinations = BitFluctuation(nodes.Length);

            List<RowCombination> rows = new List<RowCombination>();

            foreach (var combination in allCombinations) {
                string temp = string.Empty;
                foreach (var character in combination) temp += character;

                rows.Add(new RowCombination(nodes, temp));
            }

            return rows.ToArray();
        }

        /// <summary>
        /// Creates all possible combinations for truth-table
        /// </summary>
        /// <param name="numberOfVariables"></param>
        /// <returns>List of lists of combinations</returns>
        private dynamic BitFluctuation(int numberOfVariables) {
            const string set = "01";
            List<char[]> setOfSets = new List<char[]>();
            for (int i = 0; i < numberOfVariables; i++) setOfSets.Add(set.ToArray());
            var result = setOfSets.CartesianProduct();
            return result;
        }

        public string GetHexaDecimal() {
            // code taken from here: https://stackoverflow.com/questions/5612306/converting-long-string-of-binary-to-hex-c-sharp
            string binary = string.Empty;
            for (int i = RowResultPairs.Values.Count() - 1; i >= 0; i--) binary += RowResultPairs.Values.ElementAt(i);

            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0) {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8) {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", System.Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        private RowCombination CreateRowCombinationWithStars(KeyValuePair<char, int> pair) {
            var random_row = RowResultPairs.First().Key;
            string names = random_row.GetNames();
            string values = string.Empty;

            for (int i = 0; i < names.Length; i++) {
                if (names[i] != pair.Key) {
                    values += "*";
                }
                else {
                    values += pair.Value;
                }
            }

            return new RowCombination(names.ToCharArray(), values);
        }

        /// <summary>
        /// checks if a truth-table is tautology
        /// </summary>
        /// <returns></returns>
        private bool IsTautology() {
            return RowResultPairs.Values.All(x => x == 1);
        }

        /// <summary>
        /// checks if a truth-table is contradiction
        /// </summary>
        /// <returns></returns>
        private bool IsContradiction() {
            return RowResultPairs.Values.All(x => x == 0);
        }
    }
}