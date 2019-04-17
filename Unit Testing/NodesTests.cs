using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using LPP;
using LPP.Nodes;
using LPP.Helper_Classes;

namespace Unit_Testing
{
    [TestClass]
    public class NodesTests
    {
        [TestMethod]
        public void BinaryTreeCreation_Test() {
            Processor p = new Processor ();
            p.ProcessStringInput (">(A, ~(=(B,C)))");
            string infix = "(A) > (~((B) = (C)))";
            string res = p.GetInfixNotation (p.Root);
            Assert.AreEqual (infix, res);
        }

        [TestMethod]
        public void BinaryTreeCreation_Test2() {
            Processor p = new Processor ();
            p.ProcessStringInput ("&(|(A,B),>(C,D))");
            string infix = "((A) | (B)) & ((C) > (D))";
            string res = p.GetInfixNotation (p.Root);
            Assert.AreEqual (infix, res);
        }

        [TestMethod]
        public void BinaryTreeCalculation_Test() {
            Processor p = new Processor ();
            p.ProcessStringInput ("&(|(A,B),>(C,=(D,A)))");
            var truthTable = p.DetermineTruthTable (p.Root);
            var results = p.GenerateHexaDecimal (truthTable);
            var expected = "BB70";
            Assert.AreEqual (expected, results);
        }

        [TestMethod]
        public void BinaryTreeListOfPropositions_Test() {
            Processor p = new Processor ();
            p.ProcessStringInput ("&(|(A,B),>(C,=(D,A)))");
            var expected = "ABCD";
            var result = p.GetPropositions (p.Root);
            Assert.AreEqual (expected, result);
        }
    }
}
