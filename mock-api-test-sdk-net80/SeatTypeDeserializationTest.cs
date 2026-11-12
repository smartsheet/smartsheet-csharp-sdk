using System.Text;
using Smartsheet.Api.Internal.Json;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class SeatTypeDeserializationTest
    {
        [TestMethod]
        public void UserPlan_UnrecognisedSeatType_DeserializesToContributor()
        {
            var serializer = new JsonNetSerializer();

            var plan = Deserialize<UserPlan>(serializer, @"{""planId"":1,""seatType"":""VIEWER""}");

            Assert.AreEqual(SeatType.CONTRIBUTOR, plan.SeatType);
        }

        [TestMethod]
        public void User_UnrecognisedSeatType_DeserializesToNull()
        {
            var serializer = new JsonNetSerializer();

            var user = Deserialize<User>(serializer, @"{""id"":1,""seatType"":""VIEWER""}");

            Assert.IsNull(user.SeatType);
        }

        [TestMethod]
        public void SeatType_ContributorIsFirst()
        {
            // CONTRIBUTOR must stay first: UserPlan.SeatType is non-nullable, so an unrecognised seat type falls back to ordinal 0.
            var expected = new[] { SeatType.CONTRIBUTOR, SeatType.GUEST, SeatType.MEMBER, SeatType.PROVISIONAL_MEMBER };

            CollectionAssert.AreEqual(expected, Enum.GetValues<SeatType>());
        }

        [TestMethod]
        public void DowngradeSeatType_ContributorIsFirst()
        {
            var expected = new[] { DowngradeSeatType.CONTRIBUTOR, DowngradeSeatType.GUEST };

            CollectionAssert.AreEqual(expected, Enum.GetValues<DowngradeSeatType>());
        }

        private T Deserialize<T>(JsonNetSerializer serializer, string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            using (var reader = new StreamReader(stream))
            {
                return serializer.deserialize<T>(reader);
            }
        }
    }
}
