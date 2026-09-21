using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class TestSetDataClassification
    {
        [TestMethod]
        public async Task TestSetDataClassificationGeneratedUrlIsCorrect()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/set-data-classification/all-response-body-properties", requestId.ToString());

            SheetDataClassification dataClassification = new SheetDataClassification();
            dataClassification.DataClassification = "CONFIDENTIAL";

            smartsheet.SheetResources.SetDataClassification(SheetCommonTestConstants.TEST_SHEET_ID, dataClassification);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());
            var uri = new Uri(foundRequest.AbsoluteUrl!);
            string path = uri.AbsolutePath;

            Assert.AreEqual($"/2.0/sheets/{SheetCommonTestConstants.TEST_SHEET_ID}/dataclassification", path);
            Assert.AreEqual("PUT", foundRequest.Method);
            Assert.AreEqual(string.Empty, uri.Query);
        }

        [TestMethod]
        public async Task TestSetDataClassificationAllResponseBodyProperties()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/set-data-classification/all-response-body-properties", requestId.ToString());

            SheetDataClassification dataClassification = new SheetDataClassification();
            dataClassification.DataClassification = "CONFIDENTIAL";

            smartsheet.SheetResources.SetDataClassification(SheetCommonTestConstants.TEST_SHEET_ID, dataClassification);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual("{\"dataClassification\":\"CONFIDENTIAL\"}", foundRequest.Body);
        }

        [TestMethod]
        public async Task TestSetDataClassificationCustomLabelIsNotRestrictedToEnum()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/sheets/set-data-classification/all-response-body-properties", requestId.ToString());

            SheetDataClassification dataClassification = new SheetDataClassification();
            dataClassification.DataClassification = "Top Secret";

            smartsheet.SheetResources.SetDataClassification(SheetCommonTestConstants.TEST_SHEET_ID, dataClassification);
            WiremockHelper wiremockHelper = new WiremockHelper();
            LogModel foundRequest = await wiremockHelper.FindWiremockRequestAsync(requestId.ToString());

            Assert.AreEqual("{\"dataClassification\":\"Top Secret\"}", foundRequest.Body);
        }

        [TestMethod]
        public void TestSetDataClassificationError500Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/500-response", requestId.ToString());

            SheetDataClassification dataClassification = new SheetDataClassification();
            dataClassification.DataClassification = "CONFIDENTIAL";

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.SheetResources.SetDataClassification(SheetCommonTestConstants.TEST_SHEET_ID, dataClassification));

            Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
        }

        [TestMethod]
        public void TestSetDataClassificationError400Response()
        {
            Guid requestId = Guid.NewGuid();
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("/errors/400-response", requestId.ToString());

            SheetDataClassification dataClassification = new SheetDataClassification();
            dataClassification.DataClassification = "CONFIDENTIAL";

            SmartsheetException exception = Assert.ThrowsException<SmartsheetException>(() =>
                smartsheet.SheetResources.SetDataClassification(SheetCommonTestConstants.TEST_SHEET_ID, dataClassification));

            Assert.IsTrue(exception.Message.Contains("Malformed Request"));
        }
    }
}
