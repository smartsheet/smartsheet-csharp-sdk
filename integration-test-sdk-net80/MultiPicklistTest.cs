using Smartsheet.Api.Models;

namespace integration_test_sdk_net80
{
    [TestClass]
    public class MultiPicklistTest : TestResources
    {
        private long sheetId;
        IList<Column> addCols;

        [TestInitialize]
        public void TestInitialize()
        {
            smartsheet = CreateClient();

            Sheet sheetHome = CreateSheetObject();
            Sheet sheet = smartsheet.SheetResources.CreateSheet(sheetHome);
            Assert.IsTrue(sheet.Id.HasValue);
            sheetId = sheet.Id.Value;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DeleteSheet(sheetId);
        }

        [TestMethod]
        public void TestMultiPicklist()
        {
            TestAddMultiPicklistColumn();
            TestListMultiPicklistColumn();
            TestAddMultiPicklistRow();
            TestGetMultiPicklistSheet();
        }

        private void TestAddMultiPicklistColumn()
        {
            Column mpl = new Column
            {
                Title = "This is a new multi-picklist column",
                Index = 0,
                Type = ColumnType.MULTI_PICKLIST,
                Options = new string[] { "Cat", "Rat", "Bat" }
            };
            addCols = smartsheet.SheetResources.ColumnResources.AddColumns(sheetId, new Column[] { mpl });
            Assert.AreEqual(addCols.Count, 1);
        }

        private void TestListMultiPicklistColumn()
        {
            PaginatedResult<Column> cols = smartsheet.SheetResources.ColumnResources.ListColumns(sheetId);
            // should be TEXT_NUMBER since level not specified
            Assert.AreEqual(cols.Data[0].Type, ColumnType.TEXT_NUMBER);

            cols = smartsheet.SheetResources.ColumnResources.ListColumns(sheetId, null, null, 2);
            // should be MULTI_PICKLIST since level 2 specified
            Assert.AreEqual(cols.Data[0].Type, ColumnType.MULTI_PICKLIST);
        }

        private void TestAddMultiPicklistRow()
        {
            Cell[] insert_cells = new Cell[]
            {
                new Cell
                {
                    ColumnId = addCols[0].Id,
                    ObjectValue = new MultiPicklistObjectValue(new string[] {"Bat", "Cat"})
                }
            };
            Row insert_row = new Row
            {
                ToTop = true,
                Cells = insert_cells
            };
            IList<Row> insert_rows = smartsheet.SheetResources.RowResources.AddRows(sheetId, new Row[] { insert_row });
            Assert.AreEqual(insert_rows.Count, 1);
        }

        private void TestGetMultiPicklistSheet()
        {
            Sheet mpl = smartsheet.SheetResources.GetSheet(sheetId,
             new List<SheetLevelInclusion> { SheetLevelInclusion.OBJECT_VALUE }, columnIds: new List<long> { addCols[0].Id.Value });
            // should be TEXT_NUMBER since level not specified
            Assert.AreEqual(mpl.Rows[0].Cells[0].ObjectValue.GetType(), typeof(StringObjectValue));

            mpl = smartsheet.SheetResources.GetSheet(sheetId, new List<SheetLevelInclusion> { SheetLevelInclusion.OBJECT_VALUE }, columnIds: new List<long> { addCols[0].Id.Value }, level: 2);
            // should be MULTI_PICKLIST since level 2 specified
            Assert.AreEqual(mpl.Rows[0].Cells[0].ObjectValue.GetType(), typeof(MultiPicklistObjectValue));
        }
    }
}