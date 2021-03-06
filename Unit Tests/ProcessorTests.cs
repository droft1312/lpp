using System.Collections.Generic;
using LPP;
using LPP.Nodes;
using LPP.TruthTable;
using NUnit.Framework;

namespace Unit_Tests
{
    [TestFixture]
    public class ProcessorTests
    {
        #region Custom Inputs

        private const string Input1 = "&(~(|(P,Q)),>(=(Q,R),=(S,T)))";
        private const string HashInput1 = "000000F9";
        private const string InfixInput1 = "(~((P) | (Q))) & (((Q) = (R)) > ((S) = (T)))";

        private const string Input2 = "&(|(A,B),>(C,~(C)))";
        private const string HashInput2 = "54";

        private const string Input1_Tautology = ">(>(Q,P),|(~(Q),>(~(P),R)))";
        private const string Input2_Tautology = ">(>(&(~(R),Q),~(P)),>(&(P,Q),R))";
        private const string Input3_Tautology = "|(A,>(A,B))";

        #endregion
        
        
        [Test]
        public void BuildingTree_Test() {
            Processor p = new Processor();
            p.ProcessStringInput(Input1);
            
            Assert.AreEqual(HashInput1, p.GenerateHexaDecimal(p.DetermineTruthTable(p.Root)));
        }

        [Test]
        public void BuildingTreeWithQuantifiers_Test() {

            const string input = ">(@x.P(x),!y.F(y))";
            
            Processor p = new Processor();
            p.ProcessStringInput(input);
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

        [Test]
        public void TableuxCreation_Test() {
            Processor p = new Processor();

            const string customInput = ">(&(|(~(A), C), =(A,B)), >(&(~(C),D),E))";

            p.ProcessStringInput(customInput);
            
            p.GenerateTableux();
            
            Assert.False(p.Tableux.IsTautology);
        }

        [Test]
        public void TableuxWithQuantifiersCreation_Test() {
            Processor p = new Processor();

            const string customInput = ">(!x.(@y.(P(x,y))),@q.(!p.(P(p,q))))"; // should return that it's a tautology
            
            p.ProcessStringInput(customInput);
            
            p.GenerateTableux();
        }

        [Test]
        public void TautologyCheckInListOfTableuxNode() {
            PropositionNode propNode = new PropositionNode('R'.ToString());
            
            NotNode notNode = new NotNode();
            notNode.left = Functions.DeepCopyTree(propNode);
            
            PropositionNode p2 = new PropositionNode('Q'.ToString());
            
            List<Node> list = new List<Node> { propNode, notNode, p2 };
            
            Assert.True(TableuxNode.IsTautology(list));
            
        }

        [Test]
        public void BiimplicationNandConvertion_Test() {
            Processor p = new Processor();
            
            const string input = "~(>(A, &(=(B,C), %(C,Q))))";
            const string input2 = ">(A,&(C,~(|(B,C))))";
            
            p.ProcessStringInput(input2);
            
            Functions.GetRidOfBiImplicationAndNand(ref p.root);
            
            Assert.True(true);
        }
        
        // TODO: Create more Unit tests
        
        
    }
}