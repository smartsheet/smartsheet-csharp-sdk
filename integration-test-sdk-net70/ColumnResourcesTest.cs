﻿using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net70
{
    [TestClass]
    public class ColumnResourcesTest
    {
        [TestMethod]
        public void TestColumnResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();
            long sheetId = CreateSheet(smartsheet);
            AddColumns(smartsheet, sheetId);

            long columnId = ListColumns(smartsheet, sheetId);
            UpdateColumn(smartsheet, sheetId, columnId);
            DeleteAndGetColumn(smartsheet, sheetId, columnId);

            columnId = AddColumnFormula(smartsheet, sheetId);
            ClearColumnFormula(smartsheet, sheetId, columnId);

            smartsheet.SheetResources.DeleteSheet(sheetId);
        }

        private static void DeleteAndGetColumn(SmartsheetClient smartsheet, long sheetId, long columnId)
        {
            smartsheet.SheetResources.ColumnResources.DeleteColumn(sheetId, columnId);
            try
            {
                smartsheet.SheetResources.ColumnResources.GetColumn(sheetId, columnId, null);
                Assert.Fail("Cannot get a column that is deleted.");
            }
            catch
            {
                // Not found.
            }
        }

        private static long ListColumns(SmartsheetClient smartsheet, long sheetId)
        {
            PaginatedResult<Column> columnsResult = smartsheet.SheetResources.ColumnResources.ListColumns(sheetId, new ColumnInclusion[] { ColumnInclusion.FILTERS }, null);
            Assert.IsTrue(columnsResult.TotalCount == 4);
            Assert.IsTrue(columnsResult.Data.Count == 4);
            return columnsResult.Data[3].Id.Value;
        }

        private static void UpdateColumn(SmartsheetClient smartsheet, long sheetId, long columnId)
        {
            Column updatedColumn = smartsheet.SheetResources.ColumnResources.UpdateColumn(sheetId, new Column.UpdateColumnBuilder(columnId, "col 4 updated", 2).Build());
            Assert.IsTrue(updatedColumn.Title == "col 4 updated");
        }

        private static void AddColumns(SmartsheetClient smartsheet, long sheetId)
        {
            IList<Column> columnsAdded = smartsheet.SheetResources.ColumnResources.AddColumns(sheetId, new Column[] { new Column.AddColumnBuilder("col 4", 3, ColumnType.CONTACT_LIST).Build() });
            Assert.IsTrue(columnsAdded.Count == 1);
            Assert.IsTrue(columnsAdded[0].Title == "col 4");
        }

        private static long AddColumnFormula(SmartsheetClient smartsheet, long sheetId)
        {
            Column col = new Column.AddColumnBuilder("colFormula", 3, ColumnType.DATE).Build();
            col.Formula = " = TODAY()";
            IList<Column> columnsAdded = smartsheet.SheetResources.ColumnResources.AddColumns(sheetId, new Column[] { col });
            Assert.IsTrue(columnsAdded.Count == 1);
            Assert.IsNotNull(columnsAdded[0].Formula);
            return columnsAdded[0].Id.Value;
        }

        private static void ClearColumnFormula(SmartsheetClient smartsheet, long sheetId, long columnId)
        {
            Column col = new Column.UpdateColumnBuilder(columnId, "colFormula updated", 2).Build();
            col.Formula = "";
            Column updatedColumn = smartsheet.SheetResources.ColumnResources.UpdateColumn(sheetId, col);
            Assert.IsNull(updatedColumn.Formula);
        }

        private static long CreateSheet(SmartsheetClient smartsheet)
        {
            Column[] columnsToCreate = new Column[] {
            new Column.CreateSheetColumnBuilder("col 1", true, ColumnType.TEXT_NUMBER).Build(),
            new Column.CreateSheetColumnBuilder("col 2", false, ColumnType.DATE).Build(),
            new Column.CreateSheetColumnBuilder("col 3", false, ColumnType.TEXT_NUMBER).Build(),
            };
            Sheet createdSheet = smartsheet.SheetResources.CreateSheet(new Sheet.CreateSheetBuilder("new sheet", columnsToCreate).Build());
            Assert.IsTrue(createdSheet.Columns.Count == 3);
            Assert.IsTrue(createdSheet.Columns[1].Title == "col 2");
            return createdSheet.Id.Value;
        }
    }
}