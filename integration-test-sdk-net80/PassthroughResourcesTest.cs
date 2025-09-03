using Smartsheet.Api;
using Newtonsoft.Json;

namespace integration_test_sdk_net80
{
    [TestClass]
    public class PassthroughResourcesTest
    {
        [TestMethod]
        public void TestPassthroughMethods()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).SetSmartsheetIntegrationSource("AI,MyOrg,MyGPT").Build();

            string payload =
                "{\"name\": \"my new sheet\"," +
                    "\"columns\": [" +
                        "{\"title\": \"Favorite\", \"type\": \"CHECKBOX\", \"symbol\": \"STAR\"}," +
                        "{\"title\": \"Primary Column\", \"primary\": true, \"type\": \"TEXT_NUMBER\"}" +
                    "]" +
                "}";

            string jsonResponse = smartsheet.PassthroughResources.PostRequest("sheets", payload);

            long id = 0;
            JsonReader reader = new JsonTextReader(new StringReader(jsonResponse));

            while(id == 0 && reader.Read()) {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        break;
                    case JsonToken.PropertyName:
                        var value = reader.Value?.ToString();
                        Assert.IsNotNull(value);
                        if(value.Contains("message")) 
                        {
                            string message = reader.ReadAsString() ?? string.Empty;
                            Assert.AreEqual(message, "SUCCESS");
                        }
                        else if(value.Contains("id"))
                        {
                            reader.Read();
                            Assert.IsNotNull(reader.Value);
                            id = (long)reader.Value;
                        }
                        else
                        {
                            reader.Read();
                        }
                        break;
                    default:
                        reader.Read();
                        break;
                }
            }
            Assert.AreNotEqual(id, 0);

            IDictionary<string, string> parameters = new Dictionary<string,string>();
            parameters.Add("include", "objectValue");
            jsonResponse = smartsheet.PassthroughResources.GetRequest("sheets/" + id, parameters);
            reader = new JsonTextReader(new StringReader(jsonResponse));
            Assert.IsNotNull(reader.Value);

            string? name = null;
            while (name == null && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        break;
                    case JsonToken.PropertyName:

                        var value = reader.Value.ToString();
                        Assert.IsNotNull(value);
                        if (value.Contains("name"))
                        {
                            var nameValue = reader.ReadAsString();
                            Assert.IsNotNull(nameValue);
                            name = nameValue;
                        }
                        else
                        {
                            reader.Read();
                        }
                        break;
                    default:
                        reader.Read();
                        break;
                }
            }
            Assert.AreEqual(name, "my new sheet");

            payload = "{\"name\": \"my new new sheet\"}";
            jsonResponse = smartsheet.PassthroughResources.PutRequest("sheets/" + id, payload);

            name = null;
            reader = new JsonTextReader(new StringReader(jsonResponse));
            while (name == null && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        break;
                    case JsonToken.PropertyName:
                        if ((reader.Value?.ToString() ?? string.Empty) .Contains("name"))
                        {
                            name = reader.ReadAsString();
                        }
                        else
                        {
                            reader.Read();
                        }
                        break;
                    default:
                        reader.Read();
                        break;
                }
            }
            Assert.AreEqual(name, "my new new sheet");

            jsonResponse = smartsheet.PassthroughResources.DeleteRequest("sheets/" + id);

            bool success = false;
            reader = new JsonTextReader(new StringReader(jsonResponse));
            while (!success && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        break;
                    case JsonToken.PropertyName:
                        if ((reader.Value?.ToString() ?? string.Empty).Contains("message"))
                        {
                            string? message = reader.ReadAsString();
                            Assert.AreEqual(message, "SUCCESS");
                            success = true;
                        }
                        else
                        {
                            reader.Read();
                        }
                        break;
                    default:
                        reader.Read();
                        break;
                }
            }
            Assert.IsTrue(success);
        }
    }
}
