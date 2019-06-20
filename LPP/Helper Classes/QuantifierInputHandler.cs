using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using LPP.Custom_Exception;
using LPP.Nodes;

// Typical input: @x.P(x)
// Typical input: !y.@x.P(x,y)
// Typical input: !y.@z.!x.F(x,y,z)
// Typical input: >(~(!x.P(x)),@y.F(y))

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

                    if (!listOfAcceptableVars.Contains(s[0])) {
                        MessageBox.Show(
                            "You cannot create a predicate with variables that have not been introduced by a quantifier!");
                        throw new InputException();
                    }
                    
                    propositions.Add(new PropositionNode(s[0].ToString()));
                }

                predicate.Formulas = propositions;

                return predicate;

            } 
            
            if (IsBasicExpression(input[0])) {
                /* for instance: |(P(x),Q(x))), need to get: |(P(x),Q(x))*/

                int indexOfClosingBracket = Functions.GetIndexOfClosingBracket(input, 1);

                string expression = StringStartEndIndex(input, 0, indexOfClosingBracket);
                
                Processor temp = new Processor();
                temp.ProcessStringInput(expression);

                return temp.Root;
            }

            /* case with quantifiers */
                
            var quantifier = GenerateQuantifier(input[0]);
            
            quantifier.Variable = new PropositionNode(input[1].ToString());
            
            AddAllowableVar(input[1]);
            
            input = input.Substring(3); // get the string starting (excluding) from '.'
            
            quantifier.Insert(Create());

            return quantifier;
        }

        public static string ParseOutInputForQuantifiers(string expression) {

            int indexOfOpeningBracket = expression.IndexOf('(');

            int indexOfClosingBracket = Functions.GetIndexOfClosingBracket(expression, indexOfOpeningBracket);


            return StringStartEndIndex(expression, 0, indexOfClosingBracket);
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

        private bool IsBasicExpression(char c) {
            return c == '>' || c == '|' || c == '=' || c == '&' || c == '%' || c == '~';
        }

        private bool IsVariable(char c) {
            return char.IsLower(c);
        }

        private string[] GetVariables(string s) {
            return s.Replace(" ", "").Replace("(", "").Split(',');
        }

        private string StringBetweenParenthesis(string s) {
            string temp = s.Substring(1);
            return temp.GetUntilOrEmpty(")");
        }

        public static string StringStartEndIndex(string s, int start, int end) {
            string result = "";

            for (int i = start; i <= end; i++)
                result += s[i];

            return result;
        }

        private void AddAllowableVar(char c) {
            listOfAcceptableVars.Add(c);
        }
    }
}