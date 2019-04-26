using System;
using System.Collections.Generic;
using System.Linq;


using static LPP.Functions;
using LPP;
using System.Collections;

namespace ConsoleApplication
{
    class Program
    {
        static string[] inputs = { ">(A,~(B))", ">(A,B)", "=( >(A,B), |( ~(A) ,B) )", "|(|(A,B),C)" };

        static void Main (string[] args) {
            Processor p = new Processor ();
            //p.ProcessStringInput (ParseInputString("=( >(A,B), |( ~(C) ,D) "));
            p.ProcessStringInput (ParseInputString (inputs[3]));
            var props = p.GetPropositions (p.Root);
            var ss = GetAllCombinations (props.ToCharArray());
            Console.WriteLine (ss[7].SatisfiesConditionForSimplification());
            Console.ReadKey ();
        }
    }
}
