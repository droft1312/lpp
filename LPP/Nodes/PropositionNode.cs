﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP.Nodes
{
    public class PropositionNode : Node
    {
        private readonly char name;

        public PropositionNode () {
            left = right = null;
        }

        public PropositionNode (char name, string input, Node parent) : base(input, parent) {
            this.name = name;
        }

        public override string Print () {
            throw new NotImplementedException ();
        }
    }
}
