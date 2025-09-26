using Smartsheet.Api.Models;

namespace integration_test_sdk_net80
{
    [TestClass]
    public class UserResourcesTest : TestResources
    {
        private static string email = "test+email@invalidsmartsheet.com";
        User? user;

        [TestInitialize]
        public void SetUp()
        {
            smartsheet = CreateClient();

            // Remove user if it exists from a previous run.
            PaginatedResult<User> users = smartsheet.UserResources.ListUsers(null, null, paging: new PaginationParameters(true, null, null));
            foreach (User tmpUser in users.Data)
            {
                if (tmpUser.Email == email)
                {
                    Assert.IsNotNull(tmpUser.Id);
                    smartsheet.UserResources.RemoveUser((long)tmpUser.Id, removeFromSharing: true);
                    break;
                }
            }
        }

        [TestMethod]
        public void GetMe()
        {
            Assert.IsNotNull(smartsheet);
            UserProfile userMe = smartsheet.UserResources.GetCurrentUser();
            Assert.IsTrue(userMe.Email != null);
        }

        // fails on add user with a 500 error
        [Ignore] 
        [TestMethod]
        public void TestUserResources()
        {
            AddUser();
            GetUser();
            UpdateUser();
            ListOneUser();
            ListAllUsers();
            RemoveUser();
        }

        private void AddUser()
        {
            User _user = new User.AddUserBuilder(email, true, true).Build();
            _user.FirstName = "Mister";
            _user.LastName = "CSharp";
            Assert.IsNotNull(smartsheet);
            user = smartsheet.UserResources.AddUser(_user, true, true);
            Assert.IsNotNull(user.Admin);
            Assert.IsTrue(user.Admin.Value);
            Assert.IsNotNull(user.LicensedSheetCreator);
            Assert.IsTrue(user.LicensedSheetCreator.Value);
        }

        private void GetUser()
        {
            Assert.IsNotNull(user?.Id);
            Assert.IsNotNull(smartsheet);
            smartsheet.UserResources.GetUser(user.Id.Value);
        }

        private void UpdateUser()
        {
            Assert.IsNotNull(user?.Id);
            User _user = new User.UpdateUserBuilder(user.Id.Value, false, false).Build();
            Assert.IsNotNull(smartsheet);
            user = smartsheet.UserResources.UpdateUser(_user);
            Assert.IsNotNull(user.Admin);
            Assert.IsFalse(user.Admin.Value);
            Assert.IsNotNull(user.LicensedSheetCreator);
            Assert.IsFalse(user.LicensedSheetCreator.Value);
        }

        private void ListOneUser()
        {
            Assert.IsNotNull(smartsheet);
            PaginatedResult<User> users = smartsheet.UserResources.ListUsers(new String[] { "test+email@invalidsmartsheet.com" }, null);
            Assert.AreEqual(1, users.TotalCount);
            Assert.AreEqual(users.Data[0].Email, "test+email@invalidsmartsheet.com");
        }

        private void ListAllUsers()
        {
            Assert.IsNotNull(smartsheet);
            PaginatedResult<User> users = smartsheet.UserResources.ListUsers(new String[] { "test+email@invalidsmartsheet.com" }, null);
            // current user + added user
            Assert.IsTrue(users.Data.Count >= 2);
        }

        private void RemoveUser()
        {
            Assert.IsNotNull(user?.Id);
            Assert.IsNotNull(smartsheet);
            smartsheet.UserResources.RemoveUser(user.Id.Value);
        }

        [TestMethod]
        public void TestAlternateEmail()
        {
            Assert.IsNotNull(smartsheet);
            UserProfile me = smartsheet.UserResources.GetCurrentUser();

            AlternateEmail altEmail1 = new AlternateEmail.AlternateEmailBuilder("test+altemail2@invalidsmartsheet.com").Build();
            AlternateEmail altEmail2 = new AlternateEmail.AlternateEmailBuilder("test+altemail3@invalidsmartsheet.com").Build();
            Assert.IsNotNull(me.Id);
            smartsheet.UserResources.AddAlternateEmail(me.Id.Value, new AlternateEmail[] { altEmail1, altEmail2 });

            PaginatedResult<AlternateEmail> altEmails = smartsheet.UserResources.ListAlternateEmails(me.Id.Value);
            Assert.IsTrue(altEmails.Data.Count >= 2);

            var altEmailsData = altEmails.Data[0].Id;
            Assert.IsNotNull(altEmailsData);
            AlternateEmail altEmail = smartsheet.UserResources.GetAlternateEmail(me.Id.Value, altEmailsData.Value);
            Assert.AreEqual(altEmail.Email, "test+altemail2@invalidsmartsheet.com");

            smartsheet.UserResources.DeleteAlternateEmail(me.Id.Value, altEmailsData.Value);
            smartsheet.UserResources.DeleteAlternateEmail(me.Id.Value, altEmailsData.Value);
        }

        [TestMethod]
        public void AddProfileImage()
        {
            Assert.IsNotNull(smartsheet);
            UserProfile me = smartsheet.UserResources.GetCurrentUser();
            Assert.IsNotNull(me.Id);
            smartsheet.UserResources.AddProfileImage(me.Id.Value, "../../../../integration-test-sdk-net80/profileimage.png", "image/png");
            me = smartsheet.UserResources.GetCurrentUser();
            Assert.IsNotNull(me.ProfileImage.ImageId);
            const int squareProfileImageSize = 1050;
            Assert.AreEqual(squareProfileImageSize, me.ProfileImage.Width);
            Assert.AreEqual(squareProfileImageSize, me.ProfileImage.Height);
        }
    }
}