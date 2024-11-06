using Calculator.Interfaces;

namespace Calculator.Expressions
{
    public class NumberExpression:IExpression
    {
        public double Value { get; }

        public NumberExpression(double value)
        {
            Value = value;
        }

        public double Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
