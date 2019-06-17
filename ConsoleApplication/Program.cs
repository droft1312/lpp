using System;
using System.Collections.Generic;
using System.Linq;


using static LPP.Functions;
using LPP;
using LPP.TruthTable;
using System.Collections;
using System.Diagnostics;

namespace ConsoleApplication
{
    class Program
    {
        static string[] inputs = { ">(A,~(B))", ">(A,B)", "=( >(A,B), |( ~(A) ,B) )", "|(|(A,B),C)", ">(~(A),&(B,|(C,D)))" };

        static void Main (string[] args) {

            string input = "P(x,y,z)";

            Console.WriteLine(StringBetweenParanthesis(input));
            
            Console.ReadKey ();
            
        }
        
        private static string StringBetweenParanthesis(string s) {
            int index1 = s.IndexOf('(');
            string temp = s.Substring(++index1);
            int index2 = temp.IndexOf(')');
            string result = temp.Substring(0, index2);
            return result;
        }

        
        private static string ParseOutStringForParenthesis(string s) {
            int openingParenthesisCounter = 0;
            int closingParenthesisCounter = 0;

            int i = 0;

            while (s[i] == '(' && i < s.Length) {
                openingParenthesisCounter++;
                i++;
            }

            for (; i < s.Length; i++) {
                if (s[i] == ')') {
                    closingParenthesisCounter++;

                    if (openingParenthesisCounter == closingParenthesisCounter) {
                        break;
                    }
                }
            }

            return null;
        }


        
        /* Typical input: >(~(!x.P(x)),@y.F(y)) */
        /* @z.!y.@x.P(x,y,z),>(???,??) */
        private static string ParseOutQuantifierString(string s) {
            
            /* assumption: s[0] is either @ or ! */

            const char openingBracket = '(';
            const char closingBracket = ')';
            
            string res = "";
            int counter = 0;
            
            while (counter < s.Length && s[counter] != '(') {
                res += s[counter];
                counter++;
            }

            int openingBracketsCounter = 0;
            int closingBracketsCounter = 0;


            for (int i = counter; i < s.Length; i++) {
                if (s[i] == openingBracket) openingBracketsCounter++;
                else if (s[i] == closingBracket) closingBracketsCounter++;

                if (openingBracketsCounter == closingBracketsCounter) break;
            }
            
            return res;
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}
