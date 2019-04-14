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

            //int a = 5; // number of variables
            //int max = Convert.ToInt32(new string ('1', a), 2);

            //for (int i = max; i > 0; i--) {
            //    Console.WriteLine ("i: {0} ||||||| {1}", i, Convert.ToString(i, 2));
            //}

            int a = 5;
            var b = BitFluctuation (a);
            
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
