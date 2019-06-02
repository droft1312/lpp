using LPP;
using NUnit.Framework;

namespace Unit_Tests
{
    [TestFixture]
    public class ProcessorTests
    {
        private const string Input1 = "&(~(|(P,Q)),>(=(Q,R),=(S,T)))";
        private const string HashInput1 = "000000F9";
        private const string InfixInput1 = "(~((P) | (Q))) & (((Q) = (R)) > ((S) = (T)))";

        private const string Input2 = "&(|(A,B),>(C,~(C)))";
        private const string HashInput2 = "54";
        
        
        [Test]
        public void BuildingTree_Test() {
            Processor p = new Processor();
            p.ProcessStringInput(Input1);
            
            Assert.AreEqual(HashInput1, p.GenerateHexaDecimal(p.DetermineTruthTable(p.Root)));
        }

        [Test]
        public void DNFTree_Test() {
            Processor p = new Processor();
            p.ProcessStringInput(Input1);

            var truthTable = p.DetermineTruthTable(p.Root);
            string disjunctive = truthTable.DisjunctiveForm();
            
            p.ProcessStringInput(Input1);

            Assert.AreEqual(HashInput1, p.GenerateHexaDecimal(p.DetermineTruthTable(p.Root)));
        }

        [Test]
        public void InfixGeneration_Test() {
            Processor p = new Processor();
            p.ProcessStringInput(Input1);
            
            Assert.AreEqual(InfixInput1, p.GetInfixNotation(p.Root));
        }

        [Test]
        public void NANDGeneration_Test() {
            Processor p = new Processor();
            p.ProcessStringInput(Input1);

            var root = p.Nandify(p.Root);
            
            Assert.AreEqual(HashInput1, p.GenerateHexaDecimal(p.DetermineTruthTable(root)));
        }

        [Test]
        public void Simplification_Test() {
            Processor p = new Processor();
            p.ProcessStringInput(Input2);

            
        }

        [Test]
        public void GetNames_Test() {
            Processor p = new Processor();
            p.ProcessStringInput(Input1);

            const string expected = "PQRST";
            string result = "";
            
            p.Root.GetAllPropositions(ref result);

            Assert.AreEqual(expected, result);
        }
        
        // TODO: Create more Unit tests
        
        
    }
}