using Calculator.Interfaces;

namespace Calculator.Parsers
{
    public abstract class ExpressionParser
    {
        public IExpression Parse(string input)
        {
            return ParseInput(input);
        }

        protected abstract IExpression ParseInput(string input);
    }
}
