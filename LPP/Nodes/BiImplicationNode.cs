﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    public class BiImplicationNode : Node
    {
        public BiImplicationNode (string input, Node parent) : base (input, parent) {

        }

        public override string ToString () {
            return "=";
        }
    }
}
