using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class SheetTestsAsync
    {
        [TestMethod]
        public async Task ListSheetsAsync_NoParams()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("List Sheets - No Params");

            PaginatedResult<Sheet> sheets = await smartsheet.SheetResources.ListSheetsAsync(null, null);

            Assert.IsNotNull(sheets.Data.Where(s => s.Name.Equals("Copy of Sample Sheet")).FirstOrDefault());
        }

        [TestMethod]
        public async Task ListSheetsAsync_IncludeOwnerInfo()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("List Sheets - Include Owner Info");

            PaginatedResult<Sheet> sheets = await smartsheet.SheetResources.ListSheetsAsync(new List<SheetInclusion> { SheetInclusion.OWNER_INFO });

            Assert.IsNotNull(sheets.Data.Where(s => s.Owner.Equals("john.doe@smartsheet.com")).FirstOrDefault());
        }

        [TestMethod]
        public async Task CreateSheetFromTemplateAsync_NoColumns()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Create Sheet - Invalid - No Columns");

            Sheet sheetA = new Sheet
            {
                Name = "New Sheet",
                Columns = new List<Column>()
            };

            await HelperFunctions.AssertRaisesExceptionAsync<SmartsheetException>(
                () => smartsheet.SheetResources.CreateSheetFromTemplateAsync(sheetA),
                "The new sheet requires either a fromId or columns.");
        }
    }
}
