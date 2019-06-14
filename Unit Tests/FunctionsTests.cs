using System.IO;
using LPP;
using NUnit.Framework;

namespace Unit_Tests
{
    [TestFixture]
    public class FunctionsTests
    {
        [Test]
        public void ClearOutHexaDecimal_Test() {
            const string input = "000000000F9";
            const string expectedOutput = "F9";
            
            Assert.AreEqual(expectedOutput, Functions.ClearOutHexadecimal(input));
        }

        [Test]
        public void PrintOutTruthTable_Test() {
            string expected= File.ReadAllText(@"C:\Users\Turar Jumaniyaz\source\repos\LPP\test.txt");
            
            Processor p = new Processor();
            p.ProcessStringInput("&(A,|(B,&(R,|(N,&(W,&(X,C))))))");

            var truthTable = p.DetermineTruthTable(p.root);

            string actual = Functions.PrintOutTruthTable(truthTable.RowResultPairs);
            
            Assert.AreEqual(expected, actual);
        }
    }
}