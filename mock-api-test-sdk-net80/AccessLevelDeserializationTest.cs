using System.Text;
using Smartsheet.Api.Internal.Json;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    /// <summary>
    /// Tests that a COMMENTER access level returned by the API deserializes to
    /// <see cref="AccessLevel.COMMENTER"/> rather than silently becoming null.
    ///
    /// JsonEnumTypeConverter maps an access level missing from the enum to null for a nullable
    /// property, so the value was dropped without an error and callers could not tell
    /// "this is a Commenter share" apart from "this share has no access level".
    /// See https://github.com/smartsheet/smartsheet-csharp-sdk/issues/218
    /// </summary>
    [TestClass]
    public class AccessLevelDeserializationTest
    {
        [TestMethod]
        public void AssetShare_CommenterAccessLevel_DeserializesToCommenter()
        {
            var serializer = new JsonNetSerializer();
            var json = @"{""accessLevel"":""COMMENTER"",""email"":""user@example.com""}";

            var share = Deserialize<AssetShare>(serializer, json);

            Assert.IsNotNull(share);
            Assert.AreEqual(AccessLevel.COMMENTER, share.AccessLevel);
            Assert.AreEqual("user@example.com", share.Email);
        }

        [TestMethod]
        public void Sheet_CommenterAccessLevel_DeserializesToCommenter()
        {
            var serializer = new JsonNetSerializer();

            var sheet = Deserialize<Sheet>(serializer, @"{""accessLevel"":""COMMENTER""}");

            Assert.IsNotNull(sheet);
            Assert.AreEqual(AccessLevel.COMMENTER, sheet.AccessLevel);
        }

        [TestMethod]
        public void Report_CommenterAccessLevel_DeserializesToCommenter()
        {
            var serializer = new JsonNetSerializer();

            var report = Deserialize<Report>(serializer, @"{""accessLevel"":""COMMENTER""}");

            Assert.IsNotNull(report);
            Assert.AreEqual(AccessLevel.COMMENTER, report.AccessLevel);
        }

        [TestMethod]
        public void Workspace_CommenterAccessLevel_DeserializesToCommenter()
        {
            var serializer = new JsonNetSerializer();

            var workspace = Deserialize<Workspace>(serializer, @"{""accessLevel"":""COMMENTER""}");

            Assert.IsNotNull(workspace);
            Assert.AreEqual(AccessLevel.COMMENTER, workspace.AccessLevel);
        }

        [TestMethod]
        public void CreateShareRequest_CommenterAccessLevel_SerializesToCommenter()
        {
            var serializer = new JsonNetSerializer();
            var request = new CreateShareRequest
            {
                Email = "user@example.com",
                AccessLevel = AccessLevel.COMMENTER
            };

            string json = Serialize(serializer, request);

            StringAssert.Contains(json, @"""accessLevel"":""COMMENTER""");
        }

        [TestMethod]
        public void UpdateShareRequest_CommenterAccessLevel_SerializesToCommenter()
        {
            var serializer = new JsonNetSerializer();
            var request = new UpdateShareRequest { AccessLevel = AccessLevel.COMMENTER };

            string json = Serialize(serializer, request);

            StringAssert.Contains(json, @"""accessLevel"":""COMMENTER""");
        }

        [TestMethod]
        public void Sheet_UnmappedAccessLevel_StillDeserializesToNull()
        {
            // Forward-compatibility is intentional: an access level this SDK version does not
            // know must not fail the whole response.
            var serializer = new JsonNetSerializer();

            var sheet = Deserialize<Sheet>(serializer, @"{""accessLevel"":""NOT_A_REAL_ACCESS_LEVEL""}");

            Assert.IsNotNull(sheet);
            Assert.IsNull(sheet.AccessLevel);
        }

        [TestMethod]
        public void AccessLevel_ContainsEveryDocumentedValue()
        {
            // Mirrors the AccessLevel enum in the public API specification.
            var expected = new[]
            {
                AccessLevel.VIEWER,
                AccessLevel.COMMENTER,
                AccessLevel.EDITOR,
                AccessLevel.EDITOR_SHARE,
                AccessLevel.ADMIN,
                AccessLevel.OWNER
            };

            CollectionAssert.AreEquivalent(expected, Enum.GetValues<AccessLevel>());
        }

        #region Helper Methods

        private T Deserialize<T>(JsonNetSerializer serializer, string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            using (var reader = new StreamReader(stream))
            {
                return serializer.deserialize<T>(reader);
            }
        }

        private string Serialize<T>(JsonNetSerializer serializer, T value)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                serializer.serialize(value, writer);
                writer.Flush();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        #endregion
    }
}
