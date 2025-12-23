using System.Text;
using Smartsheet.Api.Internal.Json;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    /// <summary>
    /// Tests for the JsonNetSerializer PreserveDecimalValues configuration.
    /// Ensures that the new decimal preservation feature works correctly and maintains backward compatibility.
    /// </summary>
    [TestClass]
    public class JsonSerializerDecimalTest
    {
        #region Backward Compatibility Tests - NumberObjectValue

        [TestMethod]
        public void DefaultBehavior_ShouldDeserializeFloatToNumberObjectValue()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            // Reset to default mode since EnableDecimalObjectValue modifies the shared static serializer
            // and previous tests may have set it to true, causing this test to fail with DecimalObjectValue
            serializer.EnableDecimalObjectValue = false;
            var json = "123.456";

            // Act
            var result = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NumberObjectValue));
            var numberValue = (NumberObjectValue)result;
            Assert.AreEqual(123.456, numberValue.Value, 0.00001);
        }

        [TestMethod]
        public void DefaultBehavior_ShouldDeserializeIntegerToNumberObjectValue()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            // Reset to default mode since EnableDecimalObjectValue modifies the shared static serializer
            // and previous tests may have set it to true, causing this test to fail with DecimalObjectValue
            serializer.EnableDecimalObjectValue = false;
            var json = "42";

            // Act
            var result = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NumberObjectValue));
            var numberValue = (NumberObjectValue)result;
            Assert.AreEqual(42.0, numberValue.Value);
        }

        [TestMethod]
        public void DefaultBehavior_ShouldDeserializeNegativeNumberToNumberObjectValue()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            // Reset to default mode since EnableDecimalObjectValue modifies the shared static serializer
            // and previous tests may have set it to true, causing this test to fail with DecimalObjectValue
            serializer.EnableDecimalObjectValue = false;
            var json = "-456.789";

            // Act
            var result = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NumberObjectValue));
            var numberValue = (NumberObjectValue)result;
            Assert.AreEqual(-456.789, numberValue.Value, 0.00001);
        }

        [TestMethod]
        public void DefaultBehavior_ShouldDeserializeZeroToNumberObjectValue()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            // Reset to default mode since EnableDecimalObjectValue modifies the shared static serializer
            // and previous tests may have set it to true, causing this test to fail with DecimalObjectValue
            serializer.EnableDecimalObjectValue = false;
            var json = "0";

            // Act
            var result = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NumberObjectValue));
            var numberValue = (NumberObjectValue)result;
            Assert.AreEqual(0.0, numberValue.Value);
        }

        [TestMethod]
        public void DefaultBehavior_ShouldTruncateHighPrecisionNumber()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            // Reset to default mode since EnableDecimalObjectValue modifies the shared static serializer
            // and previous tests may have set it to true, causing this test to fail with DecimalObjectValue
            serializer.EnableDecimalObjectValue = false;
            var json = "123.456789012345678901234567890";

            // Act
            var result = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NumberObjectValue));
            var numberValue = (NumberObjectValue)result;
            // Decimal has 28-29 significant digits
            Assert.AreEqual(123.45678901234568, numberValue.Value);
        }

        #endregion

        #region Decimal Preservation Tests - DecimalObjectValue

        [TestMethod]
        public void PreserveDecimalValues_ShouldDeserializeFloatToDecimalObjectValue()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = true;
            var json = "123.456";

            // Act
            var result = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DecimalObjectValue));
            var decimalValue = (DecimalObjectValue)result;
            Assert.AreEqual(123.456m, decimalValue.Value);
        }

        [TestMethod]
        public void PreserveDecimalValues_ShouldDeserializeIntegerToDecimalObjectValue()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = true;
            var json = "42";

            // Act
            var result = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DecimalObjectValue));
            var decimalValue = (DecimalObjectValue)result;
            Assert.AreEqual(42m, decimalValue.Value);
        }

        [TestMethod]
        public void PreserveDecimalValues_ShouldDeserializeHighPrecisionNumber()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = true;
            var json = "123.456789012345678901234567890";

            // Act
            var result = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DecimalObjectValue));
            var decimalValue = (DecimalObjectValue)result;
            // Decimal has 28-29 significant digits
            Assert.AreEqual(123.456789012345678901234567890m, decimalValue.Value);
        }

        [TestMethod]
        public void PreserveDecimalValues_ShouldDeserializeNegativeDecimal()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = true;
            var json = "-987.654321";

            // Act
            var result = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DecimalObjectValue));
            var decimalValue = (DecimalObjectValue)result;
            Assert.AreEqual(-987.654321m, decimalValue.Value);
        }

        #endregion

        #region Non-Numeric Value Tests

        [TestMethod]
        public void BothModes_ShouldDeserializeBooleanToBooleanObjectValue()
        {
            // Test with default mode
            var serializer1 = new JsonNetSerializer();
            var result1 = DeserializeObjectValue(serializer1, "true");
            Assert.IsInstanceOfType(result1, typeof(BooleanObjectValue));
            Assert.IsTrue(((BooleanObjectValue)result1).Value);

            // Test with preserve decimal mode
            var serializer2 = new JsonNetSerializer();
            serializer2.EnableDecimalObjectValue = true;
            var result2 = DeserializeObjectValue(serializer2, "false");
            Assert.IsInstanceOfType(result2, typeof(BooleanObjectValue));
            Assert.IsFalse(((BooleanObjectValue)result2).Value);
        }

        [TestMethod]
        public void BothModes_ShouldDeserializeStringToStringObjectValue()
        {
            // Test with default mode
            var serializer1 = new JsonNetSerializer();
            var result1 = DeserializeObjectValue(serializer1, "\"test string\"");
            Assert.IsInstanceOfType(result1, typeof(StringObjectValue));
            Assert.AreEqual("test string", ((StringObjectValue)result1).Value);

            // Test with preserve decimal mode
            var serializer2 = new JsonNetSerializer();
            serializer2.EnableDecimalObjectValue = true;
            var result2 = DeserializeObjectValue(serializer2, "\"another test\"");
            Assert.IsInstanceOfType(result2, typeof(StringObjectValue));
            Assert.AreEqual("another test", ((StringObjectValue)result2).Value);
        }

        #endregion

        #region Serialization Round-Trip Tests

        [TestMethod]
        public void RoundTrip_NumberObjectValue_ShouldPreserveValue()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            // Reset to default mode since EnableDecimalObjectValue modifies the shared static serializer
            // and previous tests may have set it to true, causing this test to fail with DecimalObjectValue
            serializer.EnableDecimalObjectValue = false;
            var originalValue = new NumberObjectValue(456.789);

            // Act
            string json = SerializeObjectValue(serializer, originalValue);
            var deserialized = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsInstanceOfType(deserialized, typeof(NumberObjectValue));
            var numberValue = (NumberObjectValue)deserialized;
            Assert.AreEqual(456.789, numberValue.Value, 0.00001);
        }

        [TestMethod]
        public void RoundTrip_DecimalObjectValue_ShouldPreserveValue()
        {
            // Arrange
            var serializer = new JsonNetSerializer();
            serializer.EnableDecimalObjectValue = true;
            var originalValue = new DecimalObjectValue(456.789m);

            // Act
            string json = SerializeObjectValue(serializer, originalValue);
            var deserialized = DeserializeObjectValue(serializer, json);

            // Assert
            Assert.IsInstanceOfType(deserialized, typeof(DecimalObjectValue));
            var decimalValue = (DecimalObjectValue)deserialized;
            Assert.AreEqual(456.789m, decimalValue.Value);
        }

        #endregion

        #region Helper Methods

        private ObjectValue DeserializeObjectValue(JsonNetSerializer serializer, string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            using (var reader = new StreamReader(stream))
            {
                return serializer.deserialize<ObjectValue>(reader);
            }
        }

        private string SerializeObjectValue(JsonNetSerializer serializer, ObjectValue value)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                serializer.serialize(value, writer);
                writer.Flush();
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}
