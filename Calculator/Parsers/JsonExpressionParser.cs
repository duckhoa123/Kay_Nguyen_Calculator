using Calculator.Expressions;
using Calculator.Interfaces;
using Newtonsoft.Json.Linq;

namespace Calculator.Parsers
{
    public class JsonExpressionParser:ExpressionParser
    {
        protected override IExpression ParseInput(string input)
        {
            try
            {
                var jsonObject = JObject.Parse(input);
                return ParseExpression(jsonObject["Operation"]);
            }
            catch
            {
                throw new InvalidOperationException("Invalid JSON format for the expression.");
            }
        }

        private IExpression ParseExpression(JToken token)
        {
            if (token.Type == JTokenType.Integer || token.Type == JTokenType.Float)
            {
                return new NumberExpression(token.Value<double>());
            }
            else if (token["Operation"] != null && token["@ID"] != null)
            {
                var operationId = token["@ID"].Value<string>();
                var values = token["Value"].Children().Select(ParseExpression).ToList();
                IExpression left = values.Aggregate((left, right) => new BinaryExpression(operationId, left, right));

                IExpression right = ParseExpression(token["Operation"]);
                return new BinaryExpression(operationId, left, right);
            }
            else if (token["@ID"] != null)
            {

                var operationId = token["@ID"].Value<string>();
                // Check if valid operationID
                if (operationId != "Plus" && operationId != "Minus" && operationId != "Multiplication" && operationId != "Divide")
                {
                    throw new InvalidOperationException("Invalid operation ID.");
                }
                var values = token["Value"].Children().Select(ParseExpression).ToList();
                return values.Aggregate((left, right) => new BinaryExpression(operationId, left, right));
            }
            else
            {
                throw new InvalidOperationException("Invalid JSON structure for the expression.");
            }
        }
    }
}
