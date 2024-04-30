using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net80
{
    [TestClass]
    public class ContactResourcesTest
    {
        [TestMethod]
        public void TestContactResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();

            //Test without paginated results to make sure it is optional. 
            PaginatedResult<Contact> contactResults = smartsheet.ContactResources.ListContacts();
            Assert.IsTrue(contactResults.TotalCount >= 0);

            //Test with paginated results set to null. 
            PaginatedResult<Contact> contactResults = smartsheet.ContactResources.ListContacts(null);
            Assert.IsTrue(contactResults.TotalCount >= 0);

            //Test with paginated results set to an object. 
            PaginationParameters paginationParameters = new PaginationParameters(true, 100, 1);
            PaginatedResult<Contact> contactResults = smartsheet.ContactResources.ListContacts(paginationParameters);
            Assert.IsTrue(contactResults.TotalCount >= 0);
        }
    }
}