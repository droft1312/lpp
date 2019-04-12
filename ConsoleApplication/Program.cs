using System;
using LPP;

using static LPP.Functions;

namespace ConsoleApplication
{
    class Program
    {
        static void Main (string[] args) {
            Processor p = new Processor ();
            p.ProcessStringInput (ParseInputString(">(A,B)"));
            Console.WriteLine (p.Root);
            Console.ReadKey ();
        }
    }
}
