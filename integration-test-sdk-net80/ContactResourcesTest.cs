using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net60
{
    [TestClass]
    public class ContactResourcesTest
    {
        [TestMethod]
        public void TestContactResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();
            
            PaginatedResult<Contact> contactResults = smartsheet.ContactResources.ListContacts(null);
            Assert.IsTrue(contactResults.TotalCount >= 0);
        }
    }
}