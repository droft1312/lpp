using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    static class GlobalCounter
    {
        public static int nodes_count = 0;
        public static int tableux_count = 0;
        public static int variable_count = 0; // is used in semantic tableux bullshit for quantifiers cuz they need a new variable
        
        
        /*--------------------------------------------------------------*/
        public static int nrOfTruthsReturnedByQuantifiers = 0;
        public static int nrOfBetaRules = 1;
        public static int nrOfGammaRules = 0;
        public static bool tautologyBeenIdentified = false;
    }
}
