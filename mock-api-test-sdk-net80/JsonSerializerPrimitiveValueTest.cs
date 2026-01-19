using System.Text;
using Smartsheet.Api.Internal.Json;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    /// <summary>
    /// Tests for verifying that primitive numeric values in Cell.Value are handled correctly
    /// based on the EnableDecimalObjectValue setting. This addresses the issue reported in
    /// https://github.com/smartsheet/smartsheet-csharp-sdk/discussions/151#discussioncomment-15516369
    /// </summary>
    [TestClass]
    public class JsonSerializerPrimitiveValueTest
    {
        [TestMethod]
        public void DefaultBehavior_CellValueAsDouble_ToStringReturnsWithoutDecimal()
        {
            // Arrange - simulate the JSON response from Smartsheet API
            var serializer = new JsonNetSerializer();
            // Ensure we're in default mode (should be unnecessary but makes it explicit)
            serializer.EnableDecimalObjectValue = false;

            var json = @"{
                ""id"": 123,
                ""columnId"": 456,
                ""value"": 65.0
            }";

            // Act
            var cell = DeserializeCell(serializer, json);
            var valueAsString = Convert.ToString(cell.Value);

            // Assert
            Assert.IsNotNull(cell);
            Assert.IsNotNull(cell.Value);
            // The key assertion: Convert.ToString should return "65" not "65.0"
            Assert.AreEqual("65", valueAsString, "Cell value 65.0 should convert to string as '65' when EnableDecimalObjectValue is false");
        }

        [TestMethod]
        public void DefaultBehavior_CellValueAsInteger_ToStringReturnsCorrectly()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = false;

            var json = @"{
                ""id"": 123,
                ""columnId"": 456,
                ""value"": 65
            }";

            // Act
            var cell = DeserializeCell(serializer, json);
            var valueAsString = Convert.ToString(cell.Value);

            // Assert
            Assert.IsNotNull(cell);
            Assert.IsNotNull(cell.Value);
            Assert.AreEqual("65", valueAsString, "Cell value 65 should convert to string as '65'");
        }

        [TestMethod]
        public void DecimalMode_CellValueAsDecimal_ToStringIncludesDecimal()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = true;

            var json = @"{
                ""id"": 123,
                ""columnId"": 456,
                ""value"": 65.0
            }";

            // Act
            var cell = DeserializeCell(serializer, json);
            var valueAsString = Convert.ToString(cell.Value);

            // Assert
            Assert.IsNotNull(cell);
            Assert.IsNotNull(cell.Value);
            // In decimal mode, we expect "65.0"
            Assert.AreEqual("65.0", valueAsString, "Cell value 65.0 should convert to string as '65.0' when EnableDecimalObjectValue is true");
        }

        [TestMethod]
        public void DefaultBehavior_ComplexFilterScenario_StringComparisonWorks()
        {
            // Arrange - simulates the user's actual scenario from the discussion
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = false;

            var json = @"{
                ""id"": 123,
                ""columnId"": 456,
                ""value"": 65.0
            }";

            var cell = DeserializeCell(serializer, json);
            string filterValue = "65";

            // Act - This is similar to what the user's code does
            var cellValueAsString = Convert.ToString(cell.Value);
            bool matches = cellValueAsString.Equals(filterValue);

            // Assert
            Assert.IsTrue(matches, "String comparison should work: Convert.ToString(65.0) should equal '65' in default mode");
        }

        [TestMethod]
        public void DefaultBehavior_FloatValueWithDecimals_PreservesDecimals()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = false;

            var json = @"{
                ""id"": 123,
                ""columnId"": 456,
                ""value"": 65.5
            }";

            // Act
            var cell = DeserializeCell(serializer, json);
            var valueAsString = Convert.ToString(cell.Value);

            // Assert
            Assert.IsNotNull(cell);
            Assert.IsNotNull(cell.Value);
            Assert.AreEqual("65.5", valueAsString, "Cell value 65.5 should convert to string as '65.5'");
        }

        [TestMethod]
        public void DefaultBehavior_NegativeValue_ToStringCorrect()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = false;

            var json = @"{
                ""id"": 123,
                ""columnId"": 456,
                ""value"": -42.0
            }";

            // Act
            var cell = DeserializeCell(serializer, json);
            var valueAsString = Convert.ToString(cell.Value);

            // Assert
            Assert.IsNotNull(cell);
            Assert.IsNotNull(cell.Value);
            Assert.AreEqual("-42", valueAsString, "Cell value -42.0 should convert to string as '-42' when EnableDecimalObjectValue is false");
        }

        [TestMethod]
        public void SwitchBetweenModes_BehaviorChangesCorrectly()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            var json = @"{
                ""id"": 123,
                ""columnId"": 456,
                ""value"": 65.0
            }";

            // Act & Assert - Test default mode
            serializer.EnableDecimalObjectValue = false;
            var cell1 = DeserializeCell(serializer, json);
            Assert.AreEqual("65", Convert.ToString(cell1.Value), "Should be '65' in default mode");

            // Act & Assert - Switch to decimal mode
            serializer.EnableDecimalObjectValue = true;
            var cell2 = DeserializeCell(serializer, json);
            Assert.AreEqual("65.0", Convert.ToString(cell2.Value), "Should be '65.0' in decimal mode");

            // Act & Assert - Switch back to default mode
            serializer.EnableDecimalObjectValue = false;
            var cell3 = DeserializeCell(serializer, json);
            Assert.AreEqual("65", Convert.ToString(cell3.Value), "Should be '65' again in default mode");
        }

        #region Helper Methods

        private Cell DeserializeCell(JsonNetSerializer serializer, string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            using (var reader = new StreamReader(stream))
            {
                return serializer.deserialize<Cell>(reader);
            }
        }

        #endregion
    }
}
