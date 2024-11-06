using Calculator.Parsers;
using Calculator.Visitors;

namespace Calculator.Services
{
    public class CalculatorService
    {
        private readonly JsonExpressionParser _jsonParser;
        private readonly XmlExpressionParser _xmlParser;
        private readonly ExpressionVisitor _visitor;


        public CalculatorService(
           JsonExpressionParser jsonParser,
        XmlExpressionParser xmlParser,
        ExpressionVisitor visitor)
        {
            _jsonParser = jsonParser;
            _xmlParser = xmlParser;
            _visitor = visitor;
        }

        public double CalculateFromJson(string jsonInput)
        {
            var expression = _jsonParser.Parse(jsonInput);
            return expression.Accept(_visitor);
        }

        public double CalculateFromXml(string xmlString)
        {
            var expression = _xmlParser.Parse(xmlString);
            return expression.Accept(_visitor);
        }
    }
}
