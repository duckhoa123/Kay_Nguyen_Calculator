using Calculator.Expressions;
using Calculator.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTest
{
    public class JsonExpressionParserTest
    {
        [Fact]
        public void ParseExpression_ReturnOk()
        {
            // Arrange
            var parser = new JsonExpressionParser();
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [2, 3]
              }
            }";

            // Act
            var expression = parser.Parse(jsonInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Plus", binaryExpression.Operator);
            Assert.Equal(2, ((NumberExpression)binaryExpression.Left).Value);
            Assert.Equal(3, ((NumberExpression)binaryExpression.Right).Value);
        }

        [Fact]
        public void ParseExpression_ReturnOk_WithNestedExpression()
        {
            // Arrange
            var parser = new JsonExpressionParser();
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [
                  3,
                  2
                ],
                ""Operation"": {
                  ""@ID"": ""Minus"",
                  ""Value"": [5, 3]
                }
              }
            }";

            // Act
            var expression = parser.Parse(jsonInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Plus", binaryExpression.Operator);
            Assert.IsType<BinaryExpression>(binaryExpression.Left);
            var leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
            Assert.Equal("Plus", leftBinaryExpression.Operator);
            Assert.Equal(3, ((NumberExpression)leftBinaryExpression.Left).Value);
            Assert.Equal(2, ((NumberExpression)leftBinaryExpression.Right).Value);
            var rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
            Assert.Equal("Minus", rightBinaryExpression.Operator);
            Assert.Equal(5, ((NumberExpression)rightBinaryExpression.Left).Value);
            Assert.Equal(3, ((NumberExpression)rightBinaryExpression.Right).Value);

        }

        [Fact]
        public void ParseExpression_ThrowException_WhenInvalidJson()
        {
            // Arrange
            var parser = new JsonExpressionParser();
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [2, 3]
              }
            ";

            // Act
            Action act = () => parser.Parse(jsonInput);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        // Test for Multiplication and Addition
        public void ParseExpression_ReturnOk_WithMultipleOperations()
        {
            // Arrange
            var parser = new JsonExpressionParser();
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [
                  {
                    ""@ID"": ""Multiplication"",
                    ""Value"": [5, 3]
                  },
                  2
                ]
              }
            }";

            // Act
            var expression = parser.Parse(jsonInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Plus", binaryExpression.Operator);
            Assert.IsType<BinaryExpression>(binaryExpression.Left);
            var leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
            Assert.Equal("Multiplication", leftBinaryExpression.Operator);
            Assert.Equal(5, ((NumberExpression)leftBinaryExpression.Left).Value);
            Assert.Equal(3, ((NumberExpression)leftBinaryExpression.Right).Value);
            Assert.Equal(2, ((NumberExpression)binaryExpression.Right).Value);
        }

        [Fact]
        // Test for Division and Subtraction
        public void ParseExpression_ReturnOk_WithMultipleOperations2()
        {
            // Arrange
            var parser = new JsonExpressionParser();
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Minus"",
                ""Value"": [
                  {
                    ""@ID"": ""Divide"",
                    ""Value"": [10, 2]
                  },
                  3
                ]
              }
            }";

            // Act
            var expression = parser.Parse(jsonInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Minus", binaryExpression.Operator);
            Assert.IsType<BinaryExpression>(binaryExpression.Left);
            var leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
            Assert.Equal("Divide", leftBinaryExpression.Operator);
            Assert.Equal(10, ((NumberExpression)leftBinaryExpression.Left).Value);
            Assert.Equal(2, ((NumberExpression)leftBinaryExpression.Right).Value);
            Assert.Equal(3, ((NumberExpression)binaryExpression.Right).Value);
        }

        [Fact]
        // Test for Division by zero
        public void ParseExpression_ReturnOk_WhenParseByZero()
        {
            // Arrange
            var parser = new JsonExpressionParser();
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Divide"",
                ""Value"": [10, 0]
              }
            }";

            // Act
            var expression = parser.Parse(jsonInput);

            // Assert
            Assert.True(expression is BinaryExpression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Divide", binaryExpression.Operator);
            Assert.Equal(10, ((NumberExpression)binaryExpression.Left).Value);
            Assert.Equal(0, ((NumberExpression)binaryExpression.Right).Value);
        }


        [Fact]
        // Test for Invalid operator
        public void ParseExpression_ThrowException_WhenInvalidOperator()
        {
            // Arrange
            var parser = new JsonExpressionParser();
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Invalid"",
                ""Value"": [2, 3]
              }
            }";

            // Act
            Action act = () => parser.Parse(jsonInput);

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        // Test for double input
        public void ParseExpression_ReturnOk_WhenDoubleInput()
        {
            // Arrange
            var parser = new JsonExpressionParser();
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [2.5, 3.5]
              }
            }";

            // Act
            var expression = parser.Parse(jsonInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Plus", binaryExpression.Operator);
            Assert.Equal(2.5, ((NumberExpression)binaryExpression.Left).Value);
            Assert.Equal(3.5, ((NumberExpression)binaryExpression.Right).Value);
        }

        [Fact]
        // Test for double input nested Operation
        public void ParseExpression_ReturnOk_WhenDoubleInputNestedOperation()
        {
            // Arrange
            var parser = new JsonExpressionParser();
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [
                  3.5,
                  2.5
                ],
                ""Operation"": {
                  ""@ID"": ""Minus"",
                  ""Value"": [5.5, 3.5]
                }
              }
            }";

            // Act
            var expression = parser.Parse(jsonInput);

            // Assert
            Assert.NotNull(expression);
            Assert.IsType<BinaryExpression>(expression);
            var binaryExpression = (BinaryExpression)expression;
            Assert.Equal("Plus", binaryExpression.Operator);
            Assert.IsType<BinaryExpression>(binaryExpression.Left);
            var leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
            Assert.Equal("Plus", leftBinaryExpression.Operator);
            Assert.Equal(3.5, ((NumberExpression)leftBinaryExpression.Left).Value);
            Assert.Equal(2.5, ((NumberExpression)leftBinaryExpression.Right).Value);
            var rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
            Assert.Equal("Minus", rightBinaryExpression.Operator);
            Assert.Equal(5.5, ((NumberExpression)rightBinaryExpression.Left).Value);
            Assert.Equal(3.5, ((NumberExpression)rightBinaryExpression.Right).Value);
        }
    }
}
