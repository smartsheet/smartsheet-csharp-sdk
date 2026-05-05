using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestDeleteDataClassification
    {
        [TestMethod]
        public async Task TestDeleteDataClassificationGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/delete-data-classification/all-response-body-properties", requestId.ToString());

            smartsheet.SheetResources.DeleteDataClassification(SheetCommonTestConstants.TEST_SHEET_ID);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl!);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/sheets/{SheetCommonTestConstants.TEST_SHEET_ID}/dataclassification", path);
        }

        [TestMethod]
        public void TestDeleteDataClassificationAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/delete-data-classification/all-response-body-properties", requestId.ToString());

            smartsheet.SheetResources.DeleteDataClassification(SheetCommonTestConstants.TEST_SHEET_ID);
        }

        [TestMethod]
        public void TestDeleteDataClassificationError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.SheetResources.DeleteDataClassification(SheetCommonTestConstants.TEST_SHEET_ID));

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestDeleteDataClassificationError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.SheetResources.DeleteDataClassification(SheetCommonTestConstants.TEST_SHEET_ID));

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}
