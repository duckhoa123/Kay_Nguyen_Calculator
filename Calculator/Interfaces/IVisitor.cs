using Calculator.Expressions;

namespace Calculator.Interfaces
{
    public interface IVisitor
    {
        double Visit(NumberExpression number);
        double Visit(BinaryExpression operation);
    }
}
