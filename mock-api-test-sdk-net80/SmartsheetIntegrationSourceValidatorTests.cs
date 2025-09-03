using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api.Internal.Util;
using Smartsheet.Api;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class SmartsheetIntegrationSourceValidatorTests
    {
        [TestMethod]
        public void IsValidFormat_Throws_WhenInputIsNull()
        {
            HelperFunctions.AssertRaisesException<SmartsheetException>(
                () => SmartsheetIntegrationSourceValidator.IsValidFormat(null!),
                "Smartsheet integration source cannot be null");
        }

        [TestMethod]
        public void IsValidFormat_Throws_WhenNotThreeParts()
        {
            HelperFunctions.AssertRaisesException<SmartsheetException>(
                () => SmartsheetIntegrationSourceValidator.IsValidFormat("AI,OnlyTwo"),
                "Invalid smartsheet integration source format");
        }

        [TestMethod]
        public void IsValidFormat_Throws_WhenInvalidType()
        {
            HelperFunctions.AssertRaisesException<SmartsheetException>(
                () => SmartsheetIntegrationSourceValidator.IsValidFormat("BAD,Org,Integrator"),
                "Invalid smartsheet integration source format. The integration type has to be one of the following: AI, SCRIPT, APPLICATION");
        }

        [TestMethod]
        public void IsValidFormat_Throws_WhenIntegratorEmpty()
        {
            HelperFunctions.AssertRaisesException<SmartsheetException>(
                () => SmartsheetIntegrationSourceValidator.IsValidFormat("AI,Org,"),
                "Invalid smartsheet integration source format. The integrator name cannot be empty.");
        }

        [TestMethod]
        public void IsValidFormat_ReturnsTrue_ForValidInput()
        {
            bool result = SmartsheetIntegrationSourceValidator.IsValidFormat("AI,Some Org,Some Integrator");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidFormat_HandlesWhitespaceAndCase()
        {
            bool result = SmartsheetIntegrationSourceValidator.IsValidFormat("  script  ,  Org Name  ,  Integrator  ");
            Assert.IsTrue(result);
        }
    }
}
