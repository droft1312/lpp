using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LPP.Nodes;
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

            /* 
             Algorithm:
             1) Evaluate if a row is simplifiable (all but one of the nodes in a row are the same)
             2) Go over the truth-table.rowCombination and find a matching row to our current one
             3) Add it to the new table
             
             */

            var simplifiedTruthTable = new Dictionary<RowCombination, int> ();


            foreach (KeyValuePair<RowCombination, int> pair in RowResultPairs) {
                var simplifiable = pair.Key.SatisfiesConditionForSimplification ();

                if (simplifiable) {
                    foreach (KeyValuePair<RowCombination, int> item in RowResultPairs) {
                        if (item.Key != pair.Key && item.Value == pair.Value) {

                        }
                    }
                }
            }
        }

        public void CreateTruthTable(Node root) {
            var nodes = GetPropositions (root).ToCharArray ();
            var combinations = GetAllCombinations (nodes);

            Dictionary<RowCombination, int> result = new Dictionary<RowCombination, int> ();

            foreach (var item in combinations) {
                string truth_values = item.ToString ();
                result.Add (item, System.Convert.ToInt32 (root.GetValue (truth_values)));
            }

            RowResultPairs = result;
        }

        public string GetHexaDecimal() {
            string results = string.Empty;
            for (int i = RowResultPairs.Values.Count () - 1; i >= 0; i--) results += RowResultPairs.Values.ElementAt (i);
            return System.Convert.ToInt32 (results, 2).ToString ("X");
        }
    }
}
