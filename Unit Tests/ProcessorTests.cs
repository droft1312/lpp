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
        
        // TODO: Create more Unit tests
    }
}