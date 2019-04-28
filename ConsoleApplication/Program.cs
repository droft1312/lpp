using System;
using System.Collections.Generic;
using System.Linq;


using static LPP.Functions;
using LPP;
using LPP.TruthTable;
using System.Collections;

namespace ConsoleApplication
{
    class Program
    {
        static string[] inputs = { ">(A,~(B))", ">(A,B)", "=( >(A,B), |( ~(A) ,B) )", "|(|(A,B),C)" };

        static void Main (string[] args) {

            Processor p = new Processor ();

            p.ProcessStringInput (inputs[3]);

            var truthTable = p.DetermineTruthTable (p.Root);

            truthTable.Simplify ();


            Console.ReadKey ();
        }
    }
}
