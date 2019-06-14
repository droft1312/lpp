using System.Linq;
using LPP;
using NUnit.Framework;

namespace Unit_Tests
{
    [TestFixture]
    public class TruthTableTests
    {
        [Test]
        public void GetNames_Test() {
            Processor p = new Processor();
            p.ProcessStringInput("&(A,|(B,&(R,|(N,&(W,&(X,C))))))");

            var truthTable = p.DetermineTruthTable(p.root);

            var expected = "ABCNRWX";
            var actual = truthTable.RowResultPairs.First().Key.GetNames();
            
            Assert.AreEqual(expected, actual);
        }
    }
}