using Calculator.Parsers;
using Calculator.Services;
using Calculator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTest
{
    public class CalculatorServiceTest
    {
        private readonly JsonExpressionParser jsonExpressionParser;
        private readonly XmlExpressionParser xmlExpressionParser;
        private readonly ExpressionVisitor expressionVisitor;
        public CalculatorServiceTest()
        {

            jsonExpressionParser = new JsonExpressionParser();
            xmlExpressionParser = new XmlExpressionParser();
            expressionVisitor = new ExpressionVisitor();
        }
        [Fact]
        public void CalculateFromJson_ReturnOk()
        {
            // Arrange
            //JsonExpressionParser jsonParser = new JsonExpressionParser();
            var calculatorService = new CalculatorService
                (
                    jsonExpressionParser,
                    xmlExpressionParser,
                    expressionVisitor
                );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [2, 3]
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(5, result);

        }

        [Fact]
        public void CalculateFromXml_ReturnOk()
        {
            // Arrange
            var calculatorService = new CalculatorService
                 (
                     jsonExpressionParser,
                     xmlExpressionParser,
                     expressionVisitor
                 );
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
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(-18, result);
        }

        [Fact]
        public void CalculateFromXml_InvalidOperation_ThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService
                 (
                     jsonExpressionParser,
                     xmlExpressionParser,
                     expressionVisitor
                 );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""InvalidOperation"">
                    <Value>2</Value>
                    <Value>3</Value>
                  </Operation>
             </Maths>";
            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => calculatorService.CalculateFromXml(xmlInput));
        }

        [Fact]
        public void CalculateFromJson_WhenInvalidJson_ThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService
                (
                    jsonExpressionParser,
                    xmlExpressionParser,
                    expressionVisitor
                );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [2, 3]
              }
            ";
            // Act
            Action act = () => calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void CalculateFromJson_ReturnOk_WithMultipleOperations()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [
                  3,
                  2
                ],
                ""Operation"": {
                  ""@ID"": ""Multiplication"",
                  ""Value"": [5, 3]
                }
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(20, result);
        }

        [Fact]
        public void CalculateFromXml_ReturnOk_WithMultipleOperations()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Plus"">
                    <Value>3</Value>
                    <Value>2</Value>
                    <Operation ID=""Multiplication"">
                      <Value>5</Value>
                      <Value>3</Value>
                    </Operation>
                  </Operation>
             </Maths>";
            // Act
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(20, result);
        }

        [Fact]
        // Test for Multiplication with zero
        public void CalculateFromXml_ReturnOk_WithZero()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Multiplication"">
                    <Value>3</Value>
                    <Value>0</Value>
                  </Operation>
             </Maths>";
            // Act
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        // Test for Multiplication with zero Json
        public void CalculateFromJson_ReturnOk_WithZero()
        {
            // Arrange
            var calculatorService = new CalculatorService
             (
                 jsonExpressionParser,
                 xmlExpressionParser,
                 expressionVisitor
             );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Multiplication"",
                ""Value"": [3, 0]
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        // Test for Division by zero
        public void CalculateFromXml_DivideByZero_ThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Divide"">
                    <Value>10</Value>
                    <Value>0</Value>
                  </Operation>
             </Maths>";
            // Act
            Action act = () => calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Throws<DivideByZeroException>(act);
        }

        [Fact]
        // Test for Division by zero Json
        public void CalculateFromJson_DivideByZero_ThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
            jsonExpressionParser,
            xmlExpressionParser,
            expressionVisitor
            );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Divide"",
                ""Value"": [10, 0]
              }
            }";
            // Act
            Action act = () => calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Throws<DivideByZeroException>(act);
        }

        [Fact]
        //Test for decimal number
        public void CalculateFromJson_ReturnOk_WithDecimalNumber()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [2.5, 3.5]
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(6, result);
        }

        [Fact]
        //Test for decimal number Xml
        public void CalculateFromXml_ReturnOk_WithDecimalNumber()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Plus"">
                    <Value>2.5</Value>
                    <Value>3.5</Value>
                  </Operation>
             </Maths>";
            // Act
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(6, result);
        }

        [Fact]
        // Mutiplication with decimal number
        public void CalculateFromJson_ReturnOk_WithDecimalNumberMultiplication()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Multiplication"",
                ""Value"": [2.5, 3.5]
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(8.75, result);
        }

        [Fact]
        // Mutiplication with decimal number Xml
        public void CalculateFromXml_ReturnOk_WithDecimalNumberMultiplication()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Multiplication"">
                    <Value>2.5</Value>
                    <Value>3.5</Value>
                  </Operation>
             </Maths>";
            // Act
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(8.75, result);
        }

        [Fact]
        // Division with decimal number
        public void CalculateFromJson_ReturnOk_WithDecimalNumberDivision()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Divide"",
                ""Value"": [10, 2.5]
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(4, result);
        }

        [Fact]
        // Division with decimal number Xml
        public void CalculateFromXml_ReturnOk_WithDecimalNumberDivision()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Divide"">
                    <Value>10</Value>
                    <Value>2.5</Value>
                  </Operation>
             </Maths>";
            // Act
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(4, result);
        }

        [Fact]
        // Multiplication with negative number
        public void CalculateFromJson_ReturnOk_WithNegativeNumberMultiplication()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Multiplication"",
                ""Value"": [-2, 3]
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(-6, result);
        }

        [Fact]
        // Multiplication with negative number Xml
        public void CalculateFromXml_ReturnOk_WithNegativeNumberMultiplication()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Multiplication"">
                    <Value>-2</Value>
                    <Value>3</Value>
                  </Operation>
             </Maths>";
            // Act
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(-6, result);
        }

        [Fact]
        // Division with negative number
        public void CalculateFromJson_ReturnOk_WithNegativeNumberDivision()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Divide"",
                ""Value"": [-10, -2]
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        // Division with negative number Xml
        public void CalculateFromXml_ReturnOk_WithNegativeNumberDivision()
        {
            // Arrange
            var calculatorService = new CalculatorService
             (
                 jsonExpressionParser,
                 xmlExpressionParser,
                 expressionVisitor
             );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Divide"">
                    <Value>-20</Value>
                    <Value>-2</Value>
                  </Operation>
             </Maths>";
            // Act
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        // Nested Operation with negative number
        public void CalculateFromJson_ReturnOk_WithNegativeDecimalNumberNestedOperation()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Multiplication"",
                ""Value"": [
                  -3.5,
                  -10
                ],
                ""Operation"": {
                  ""@ID"": ""Divide"",
                  ""Value"": [6.5, 3]
                }
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(75.8333, result, 0.0001);
        }

        [Fact]
        // Nested Operation with negative number Xml
        public void CalculateFromXml_ReturnOk_WithNegativeDecimalNumberNestedOperation()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Multiplication"">
                    <Value>-3.5</Value>
                    <Value>-10</Value>
                    <Operation ID=""Divide"">
                      <Value>6.5</Value>
                      <Value>3</Value>
                    </Operation>
                  </Operation>
             </Maths>";
            // Act
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(75.8333, result, 0.0001);
        }

        [Fact]
        // Invalid Number Input
        public void CalculateFromJson_InvalidNumberInput_ThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService
           (
               jsonExpressionParser,
               xmlExpressionParser,
               expressionVisitor
           );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Plus"",
                ""Value"": [2, ""3""]
              }
            }";
            // Act
            Action act = () => calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        // Invalid Number Input Xml
        public void CalculateFromXml_InvalidNumberInput_ThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService
           (
               jsonExpressionParser,
               xmlExpressionParser,
               expressionVisitor
           );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Plus"">
                    <Value>2</Value>
                    <Value>3</Value>
                    <Value>Invalid</Value>
                  </Operation>
             </Maths>";
            // Act
            Action act = () => calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Throws<FormatException>(act);
        }

        [Fact]
        // Divide Zero by Number Return OK
        public void CalculateFromJson_ReturnOk_WithZeroDivideByNumber()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var jsonInput = @"{
              ""Operation"": {
                ""@ID"": ""Divide"",
                ""Value"": [0, 3]
              }
            }";
            // Act
            var result = calculatorService.CalculateFromJson(jsonInput);
            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        // Divide Zero by Number Return OK Xml
        public void CalculateFromXml_ReturnOk_WithZeroDivideByNumber()
        {
            // Arrange
            var calculatorService = new CalculatorService
            (
                jsonExpressionParser,
                xmlExpressionParser,
                expressionVisitor
            );
            var xmlInput = @"
             <Maths xmlns="""">
                  <Operation ID=""Divide"">
                    <Value>0</Value>
                    <Value>3</Value>
                  </Operation>
             </Maths>";
            // Act
            var result = calculatorService.CalculateFromXml(xmlInput);
            // Assert
            Assert.Equal(0, result);
        }
    }
}
