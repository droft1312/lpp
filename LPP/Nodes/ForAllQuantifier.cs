namespace LPP.Nodes
{
    public class ForAllQuantifier : Quantifier
    {
        public override string ToString() {
            return "@" + Variable;
        }
    }
}