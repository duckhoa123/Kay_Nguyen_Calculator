using Calculator.Expressions;
using Calculator.Interfaces;

namespace Calculator.Visitors
{
    public class ExpressionVisitor:IVisitor
    {
        public double Visit(NumberExpression number)
        {
            return number.Value;
        }
        public double Visit(BinaryExpression operation)
        {
            double leftVal = operation.Left.Accept(this);
            double rightVal = operation.Right.Accept(this);

            return operation.Operator switch
            {
                "Plus" => leftVal + rightVal,
                "Minus" => leftVal - rightVal,
                "Multiplication" => leftVal * rightVal,
                "Divide" => rightVal != 0 ? leftVal / rightVal : throw new DivideByZeroException("Cannot divide by zero."),
                _ => throw new InvalidOperationException("Invalid operator")
            };

        }
    }
}
