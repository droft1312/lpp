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

            string s1 = "~(P(x))";
            string s2 = "P(x)";

            Console.WriteLine(Functions.DifferenceBetweenStrings(s1,s2));

            Console.ReadKey ();
            
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}
