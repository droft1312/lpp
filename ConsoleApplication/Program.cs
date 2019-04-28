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

            Processor p = new Processor ();

            p.ProcessStringInput (inputs[4]);

            Console.ReadKey ();
        }
    }
}
