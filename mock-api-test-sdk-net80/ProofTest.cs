using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api.Internal.Json;
using Smartsheet.Api.Models;
using System.IO;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class ProofTest
    {
        [TestMethod]
        public void TestProofDeserialization()
        {
            // Mirrors the proof object the API returns on a row when include=proofs is set.
            string json =
                "{" +
                "\"id\":456," +
                "\"originalId\":123," +
                "\"name\":\"My proof\"," +
                "\"type\":\"IMAGE\"," +
                "\"documentType\":\"PNG\"," +
                "\"proofRequestUrl\":\"https://app.smartsheet.com/proofs/123\"," +
                "\"version\":2," +
                "\"isCompleted\":false" +
                "}";

            var serializer = new JsonNetSerializer();

            Proof proof;
            using (var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                using (var streamReader = new StreamReader(memoryStream))
                {
                    proof = serializer.deserialize<Proof>(streamReader);
                }
            }

            Assert.AreEqual(456L, proof.Id);
            Assert.AreEqual("My proof", proof.Name);
            Assert.AreEqual(123L, proof.OriginalId);
            Assert.AreEqual(ProofType.IMAGE, proof.Type);
            Assert.AreEqual("PNG", proof.DocumentType);
            Assert.AreEqual("https://app.smartsheet.com/proofs/123", proof.ProofRequestUrl);
            Assert.AreEqual(2, proof.Version);
            Assert.AreEqual(false, proof.IsCompleted);
        }

        [TestMethod]
        public void TestProofSerializationWireNames()
        {
            var serializer = new JsonNetSerializer();
            var proof = new Proof
            {
                Type = ProofType.IMAGE,
                IsCompleted = false
            };

            string json;
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    serializer.serialize(proof, streamWriter);
                    streamWriter.Flush();
                    memoryStream.Position = 0;

                    using (var streamReader = new StreamReader(memoryStream))
                    {
                        json = streamReader.ReadToEnd();
                    }
                }
            }

            // Property Type -> wire "type"; IsCompleted -> wire "isCompleted".
            Assert.IsTrue(json.Contains("\"type\":\"IMAGE\""));
            Assert.IsTrue(json.Contains("\"isCompleted\":false"));
        }
    }
}
