using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LPP;

using static LPP.Functions;

namespace Unit_Testing
{
    [TestClass]
    public class FunctionsTests
    {
        [TestMethod]
        public void TestCountPropositions() {
            Processor p = new Processor ();
            p.ProcessStringInput (">(A,B)");

            int total;
            string nodes;

            // TODO: figure out why this unit test doesnt wanna run
            (total, nodes) = GetPropositions (p.Root);

            Assert.AreEqual (2, total);
            Assert.AreEqual ("AB", nodes);
        }
    }
}
