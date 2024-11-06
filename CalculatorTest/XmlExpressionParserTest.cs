using Calculator.Expressions;
using Calculator.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTest
{
    public class XmlExpressionParserTest
    {
        [Fact]
        public void ParseExpression_ReturnOk()
        {
            // Arrange
            var parser = new XmlExpressionParser();
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Minus"">
                    <Value>2</Value>
                    <Value>3</Value>
                    <Value>5</Value>
                    <Operation ID=""Multiplication"">
                      <Value>3</Value>
                      <Value>4</Value>
                    </Operation>
                  </Operation>
             </Maths>";

            // Act
            var expression = parser.Parse(xmlInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Minus", binaryExpression.Operator);
            var leftExpressionWith5 = (BinaryExpression)binaryExpression.Left;
            Assert.Equal("Minus", leftExpressionWith5.Operator);
            var leftExpression2And3 = (BinaryExpression)leftExpressionWith5.Left;
            Assert.Equal("Minus", leftExpression2And3.Operator);
            Assert.Equal(2, ((NumberExpression)leftExpression2And3.Left).Value);
            Assert.Equal(3, ((NumberExpression)leftExpression2And3.Right).Value);
            Assert.Equal(5, ((NumberExpression)leftExpressionWith5.Right).Value);
            var rightExpression = (BinaryExpression)binaryExpression.Right;
            Assert.Equal("Multiplication", rightExpression.Operator);
            Assert.Equal(3, ((NumberExpression)rightExpression.Left).Value);
            Assert.Equal(4, ((NumberExpression)rightExpression.Right).Value);

        }

        [Fact]
        // Test invalid Operation
        public void ParseExpression_InvalidOperation_ThrowException()
        {
            // Arrange
            var parser = new XmlExpressionParser();
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""InvalidOperation"">
                    <Value>2</Value>
                    <Value>3</Value>
                  </Operation>
             </Maths>";

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => parser.Parse(xmlInput));
        }

        [Fact]
        // Test invalid XML
        public void ParseExpression_InvalidXml_ThrowException()
        {
            // Arrange
            var parser = new XmlExpressionParser();
            var xmlInput = @"
             <Math xmlns="""">
                  <Operation ID=""Minus"">
                    <Value>2</Value>
                    <Value>3</Value>
                  </Operation>
             </Math>";

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => parser.Parse(xmlInput));
        }

        [Fact]
        // Test for Division
        public void ParseExpression_ReturnOk_WithDivision()
        {
            // Arrange
            var parser = new XmlExpressionParser();
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Divide"">
                    <Value>10</Value>
                    <Value>2</Value>
                  </Operation>
             </Maths>";

            // Act
            var expression = parser.Parse(xmlInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Divide", binaryExpression.Operator);
            Assert.Equal(10, ((NumberExpression)binaryExpression.Left).Value);
            Assert.Equal(2, ((NumberExpression)binaryExpression.Right).Value);
        }

        [Fact]
        // Test for Division by zero
        public void ParseExpression_ReturnOk_WhenParseByZero()
        {
            // Arrange
            var parser = new XmlExpressionParser();
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Divide"">
                    <Value>10</Value>
                    <Value>0</Value>
                  </Operation>
             </Maths>";

            // Act
            var expression = parser.Parse(xmlInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Divide", binaryExpression.Operator);
            Assert.Equal(10, ((NumberExpression)binaryExpression.Left).Value);
            Assert.Equal(0, ((NumberExpression)binaryExpression.Right).Value);
        }

        [Fact]
        // Test for decimal number input
        public void ParseExpression_ReturnOk_WithDecimalNumber()
        {
            // Arrange
            var parser = new XmlExpressionParser();
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Plus"">
                    <Value>10.5</Value>
                    <Value>2.5</Value>
                  </Operation>
             </Maths>";

            // Act
            var expression = parser.Parse(xmlInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Plus", binaryExpression.Operator);
            Assert.Equal(10.5, ((NumberExpression)binaryExpression.Left).Value);
            Assert.Equal(2.5, ((NumberExpression)binaryExpression.Right).Value);
        }

        [Fact]
        // Test for empty input number
        public void ParseExpression_WithEmptyInputNumber_ThrowException()
        {
            // Arrange
            var parser = new XmlExpressionParser();
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Plus"">
                    <Value></Value>
                    <Value>2.5</Value>
                  </Operation>
             </Maths>";

            // Act and Assert
            Assert.Throws<FormatException>(() => parser.Parse(xmlInput));
        }
    }
}
