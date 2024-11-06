using Calculator.Interfaces;

namespace Calculator.Expressions
{
    public class BinaryExpression:IExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }
        public string Operator { get; }


        public BinaryExpression(string op, IExpression left, IExpression right)
        {
            Operator = op;
            Left = left;
            Right = right;
        }

        public double Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
