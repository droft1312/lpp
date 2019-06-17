using System;
using System.Collections.Generic;
using LPP.Custom_Exception;
using LPP.Nodes;

// Typical input: @x.P(x)
// Typical input: !y.@x.P(x,y)
// Typical input: !y.@z.!x.F(x,y,z)

namespace LPP
{
    public class QuantifierInputHandler
    {
        private string input;

        private List<char> listOfAcceptableVars = new List<char>();
        
        /// <summary>
        /// Given input must have a quantifier in it, otherwise code will break
        /// </summary>
        /// <param name="input"></param>
        public QuantifierInputHandler(string input) {
            this.input = input;
        }

        /// <summary>
        /// Creates and returns a quantifier tree
        /// </summary>
        /// <returns>Tree with quantifiers</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="InputException"></exception>
        public Node Create() {

            if (IsPredicate(input[0])) {
                
                /* case with predicates */
                
                PredicateNode predicate = new PredicateNode(input[0]);

                input = StringBetweenParenthesis(input);

                var varsOfPredicate = GetVariables(input);
                
                List<PropositionNode> propositions = new List<PropositionNode>(varsOfPredicate.Length);

                foreach (var s in varsOfPredicate) {
                    if (!IsVariable(s[0])) throw new Exception("problems");
                    
                    if (!listOfAcceptableVars.Contains(s[0])) throw new InputException();
                    
                    propositions.Add(new PropositionNode(s[0]));
                }

                predicate.Formulas = propositions;

                return predicate;

            }
            
            /* case with quantifiers */
                
            var quantifier = GenerateQuantifier(input[0]);
            
            quantifier.Variable = new PropositionNode(input[1]);
            
            AddAllowableVar(input[1]);

            input = input.Substring(3); // get the string starting (excluding) from '.'
            
            quantifier.Insert(Create());

            return quantifier;
        }

        private Quantifier GenerateQuantifier(char c) {
            Quantifier q = null;

            switch (c) {
                case '@':
                    q = new ForAllQuantifier();
                    break;
                case '!':
                    q = new ExistentialQuantifier();
                    break;
                default:
                    throw new Exception("problems over here boy");
            }

            return q;
        }

        private bool IsPredicate(char c) {
            return char.IsUpper(c);
        }

        private bool IsVariable(char c) {
            return char.IsLower(c);
        }

        private string[] GetVariables(string s) {
            return s.Replace(" ", "").Split(',');
        }
        
        private string StringBetweenParenthesis(string s) {
            /*int index1 = s.IndexOf('(');
            string temp = s.Substring(++index1);
            int index2 = temp.IndexOf(')');
            string result = temp.Substring(0, index2);
            return result;*/

            string temp = s.Substring(1);

            return temp.GetUntilOrEmpty(")");
        }

        private void AddAllowableVar(char c) {
            listOfAcceptableVars.Add(c);
        }
    }
}