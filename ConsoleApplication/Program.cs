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
        static void Main (string[] args) {
            //Processor p = new Processor ();
            //p.ProcessStringInput (ParseInputString("=( >(A,B), |( ~(C) ,D) "));

            char[] a = { 'P', 'Q', 'R' };

            var res = GetAllCombinations (a);

            foreach (var combination in res) {
                Console.WriteLine (combination.ToString());
            }

            Console.ReadKey ();
        }

        
        static void Cartesian() {

            char[] set1 = "01".ToCharArray ();
            char[] set2 = "01".ToCharArray ();
            char[] set3 = "01".ToCharArray ();

            var cartesian = from l in set1
                            from k in set2
                            from z in set3
                            select new { l, k, z };

            Console.WriteLine (new string ('-', 25));

            foreach (var ProductList in cartesian) {
                Console.WriteLine (ProductList);
            }

        }
    }
}
