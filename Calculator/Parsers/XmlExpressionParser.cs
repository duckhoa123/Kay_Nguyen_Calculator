using Calculator.Expressions;
using Calculator.Interfaces;
using System.Xml;

namespace Calculator.Parsers
{
    public class XmlExpressionParser : ExpressionParser
    {
        protected override IExpression ParseInput(string input)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(input);

            var mathsNode = doc.DocumentElement;
            if (mathsNode.Name != "Maths" || mathsNode.ChildNodes.Count == 0)
            {
                throw new InvalidOperationException("Invalid XML format for Maths operation.");
            }

            return ParseOperation(mathsNode.FirstChild);
        }

        private IExpression ParseOperation(XmlNode operationNode)
        {
            if (operationNode == null || operationNode.Name != "Operation")
            {
                throw new InvalidOperationException("Invalid operation node.");
            }

            string operationId = operationNode.Attributes["ID"].Value;
            // Check for valid operation ID
            if (operationId != "Plus" && operationId != "Minus" && operationId != "Multiplication" && operationId != "Divide")
            {
                throw new InvalidOperationException("Invalid operation ID.");
            }
            List<IExpression> expressions = new List<IExpression>();

            foreach (XmlNode childNode in operationNode.ChildNodes)
            {
                if (childNode.Name == "Value")
                {
                    double value = double.Parse(childNode.InnerText);
                    expressions.Add(new NumberExpression(value));
                }
                else if (childNode.Name == "Operation")
                {
                    expressions.Add(ParseOperation(childNode));
                }
            }

            if (expressions.Count == 1)
            {
                return expressions[0];
            }

            return expressions.Skip(1).Aggregate(expressions.First(), (left, right) => new BinaryExpression(operationId, left, right));
        }
    }
}
