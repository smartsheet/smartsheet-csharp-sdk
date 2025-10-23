using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api.Internal.Json;
using Smartsheet.Api.Models;
using System;
using System.IO;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class UserProvisionalExpirationDateTest
    {
        [TestMethod]
        public void TestProvisionalExpirationDateSerialization()
        {
            var serializer = new JsonNetSerializer();
            var originalUser = new User
            {
                ProvisionalExpirationDate = new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                SeatTypeLastChangedAt = new DateTime(2024, 12, 13, 23, 0, 0, DateTimeKind.Utc)
            };

            string json1;
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    serializer.serialize(originalUser, streamWriter);
                    streamWriter.Flush();
                    memoryStream.Position = 0;

                    using (var streamReader = new StreamReader(memoryStream))
                    {
                        json1 = streamReader.ReadToEnd();
                    }
                }
            }

            User deserializedUser;
            using (var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json1)))
            {
                using (var streamReader = new StreamReader(memoryStream))
                {
                    deserializedUser = serializer.deserialize<User>(streamReader);
                }
            }

            string json2;
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    serializer.serialize(deserializedUser, streamWriter);
                    streamWriter.Flush();
                    memoryStream.Position = 0;

                    using (var streamReader = new StreamReader(memoryStream))
                    {
                        json2 = streamReader.ReadToEnd();
                    }
                }
            }

            Assert.AreEqual(json1, json2);
            Assert.AreEqual(originalUser.ProvisionalExpirationDate, deserializedUser.ProvisionalExpirationDate);
            Assert.AreEqual(originalUser.SeatTypeLastChangedAt, deserializedUser.SeatTypeLastChangedAt);
        }
    }
}