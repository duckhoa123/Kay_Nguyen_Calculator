namespace Calculator.Interfaces
{
    public interface IExpression
    {
        double Accept(IVisitor visitor);
    }
}
