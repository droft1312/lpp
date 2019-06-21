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

            List<RowCombination> allPatterns = new List<RowCombination>();
            
            List<RowCombination> patterns = new List<RowCombination>(); // pattern
            List<List<RowCombination>> matchingRowsForPatterns = new List<List<RowCombination>>(); // rows from a truthtable that match it
            List<int> matchingRowsResults = new List<int>(); // result it returns (either 1 or 0)
            
            #region Initialize patterns
            
            var propositions = RowResultPairs.First().Key.GetNames();

            var allStarCombinations = GenerateAllPossibilities(propositions.Length);

            foreach (var combination in allStarCombinations) {
                string temp = ""; foreach (var c in combination) temp += c;
                
                allPatterns.Add(new RowCombination(propositions.ToCharArray(), temp));
            }

            #endregion

            #region Get the list of patterns and their matches

            for (int i = 0; i < allPatterns.Count; i++) {
                List<RowCombination> matches = new List<RowCombination>();
                List<int> matchesResults = new List<int>();
                
                foreach (var rowResult in RowResultPairs) {
                    if (allPatterns[i].Matches(rowResult)) {
                        matches.Add(rowResult.Key);
                        matchesResults.Add(rowResult.Value);
                    }
                }

                if (AreAllElementsSame(matchesResults) && matches.Count > 1) {
                    patterns.Add(allPatterns[i]);
                    matchingRowsForPatterns.Add(matches);
                    matchingRowsResults.Add(matchesResults[0]);
                }
            }

            #endregion

            for (int i = 0; i < patterns.Count; i++) {
                bool removeAtExecuted = false;
                
                for (int j = 0; j < patterns.Count; j++) {
                    
                    if (i == j) continue;

                    var _thisRowMatches = matchingRowsForPatterns[i];
                    var _toCompareRowMatches = matchingRowsForPatterns[j];

                    if (matchingRowsResults[i] == matchingRowsResults[j]) {
                        var commonElementsCounter = _thisRowMatches.Intersect(_toCompareRowMatches).Count();
                        if (commonElementsCounter == _thisRowMatches.Count) {
                            // means i is inside of j
                            patterns.RemoveAt(i);
                            matchingRowsForPatterns.RemoveAt(i);
                            matchingRowsResults.RemoveAt(i);

                            removeAtExecuted = true;

                            j--;
                        }
                    }
                }

                if (removeAtExecuted) i--;
            }
            
            /* assumption: unnecessary patterns deleting is done */

            for (int i = 0; i < RowResultPairs.Count; i++) {
                bool rowResultPairMatchedSomething = false;

                for (int j = 0; j < patterns.Count; j++) {
                    if (patterns[j].Matches(RowResultPairs.ElementAt(i)) &&
                        matchingRowsResults[j] == RowResultPairs.ElementAt(i).Value) {
                        rowResultPairMatchedSomething = true;
                        break;
                    }
                }

                if (!rowResultPairMatchedSomething) {
                    patterns.Add(RowResultPairs.ElementAt(i).Key);
                    matchingRowsResults.Add(RowResultPairs.ElementAt(i).Value);
                }
            }
            
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

        private dynamic GenerateAllPossibilities(int numberOfVariables) {
            const string set = "012";
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

        private bool AreAllElementsSame(List<int> list) {
            return list.Any(o => o != list[0]);
        }
    }
}