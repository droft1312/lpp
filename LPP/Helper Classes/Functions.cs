using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LPP.TruthTable;

namespace LPP
{
    /// <summary>
    /// Class containing helper-functions
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Returns a printed truth-table
        /// </summary>
        /// <param name="truthTable"></param>
        /// <returns></returns>
        public static string PrintOutTruthTable(Dictionary<RowCombination, int> truthTable) {
            string output = string.Empty;
            
            foreach (var pair in truthTable) {
                var rowCombination = pair.Key;
                var result = pair.Value;

                string temp = rowCombination.ToString () + " -- " + result;

                output += temp;
                output += Environment.NewLine;
            }

            return output;
        }

        /// <summary>
        /// Deletes ' ', '(' from a string
        /// </summary>
        /// <param name="input">String to parse</param>
        /// <returns>String without specific characters</returns>
        public static string ParseInputString(string input) {
            return input.Replace (" ", "").Replace ("(", "");
        }

        /// <summary>
        /// Calculates the number of levels your current node will have to go up (how many times root = root.Parent)
        /// </summary>
        /// <param name="input">Input string to parse</param>
        /// <returns></returns>
        public static int CalculateNumberOfLevelsToGoUp(string input) {
            int i = 0;

            while (input[i] == ')' && (input[i] != ',' || input[i] != ' ')) {
                i++;
                if (i == input.Length) break;
            }

            return i;
        }

        /// <summary>
        /// Tries to convert string s to a format T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Convert<T> (string s) {
            var typeConverter = TypeDescriptor.GetConverter (typeof (T));
            if (typeConverter != null && typeConverter.CanConvertFrom (typeof (string))) {
                return (T)typeConverter.ConvertFrom (s);
            }

            return default (T);
        }
    }
}
