using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net80
{
    public class TestResources
    {
        protected SmartsheetClient? smartsheet;

        public SmartsheetClient CreateClient()
        {
            smartsheet = new SmartsheetBuilder()
                .SetBaseURI("https://api.test.smartsheet.com/2.0/")
                .SetMaxRetryTimeout(30000)
                .SetSmartsheetIntegrationSource("AI,MyOrg,MyGPT")
                .Build();
            return smartsheet;
        }

        public Sheet CreateSheetObject()
        {
            Column colA = new Column.AddColumnBuilder("Favorite", null, ColumnType.CHECKBOX).SetSymbol(Symbol.STAR).Build();
            Column colB = new Column.AddColumnBuilder("Primary Column", null, ColumnType.TEXT_NUMBER).Build();
            Column colC = new Column.AddColumnBuilder("Col 3", null, ColumnType.PICKLIST).SetOptions(new List<string> { "Not Started", "Started", "Completed" }).Build();
            Column colD = new Column.AddColumnBuilder("Date", null, ColumnType.DATE).Build();
            colB.Primary = true;
            Sheet sheet = new Sheet.CreateSheetBuilder("CSharp SDK Test", new List<Column> { colA, colB, colC, colD }).Build();
            return sheet;
        }

        public Sheet CreateSheet()
        {
            Assert.IsNotNull(smartsheet);
            Sheet sheet = smartsheet.SheetResources.CreateSheet(CreateSheetObject());
            Assert.IsNotNull(sheet.Id);
            var sheetColumns = sheet.Columns[1].Id;
            Assert.IsNotNull(sheetColumns);
            Cell cellA = new Cell.AddCellBuilder(sheetColumns.Value, null).SetValue("A").SetStrict(false).Build();
            Cell cellB = new Cell.AddCellBuilder(sheetColumns.Value, null).SetValue("B").SetStrict(false).Build();
            Cell cellC = new Cell.AddCellBuilder(sheetColumns.Value, null).SetValue("C").SetStrict(false).Build();
            Row rowA = new Row.AddRowBuilder(true, null, null, null, null).SetCells(new Cell[] { cellA }).Build();
            Row rowB = new Row.AddRowBuilder(true, null, null, null, null).SetCells(new Cell[] { cellB }).Build();
            Row rowC = new Row.AddRowBuilder(true, null, null, null, null).SetCells(new Cell[] { cellC }).Build();
            sheet.Rows = smartsheet.SheetResources.RowResources.AddRows(sheet.Id.Value, new Row[] { rowA, rowB, rowC });
            return sheet;
        }

        public Folder CreateFolderHome()
        {
            Folder folder = new Folder.CreateFolderBuilder("CSharp SDK Test").Build();
            Assert.IsNotNull(smartsheet);
            Folder newFolderHome = smartsheet.HomeResources.FolderResources.CreateFolder(folder);
            return newFolderHome;
        }

        public Workspace CreateWorkspace(string name)
        {
            Assert.IsNotNull(smartsheet);
            Workspace newWorkspace = smartsheet.WorkspaceResources.CreateWorkspace(new Workspace.CreateWorkspaceBuilder(name).Build());
            return newWorkspace;
        }

        public void DeleteFolder(long folderId)
        {
            Assert.IsNotNull(smartsheet);
            smartsheet.FolderResources.DeleteFolder(folderId);
        }

        public void DeleteSheet(long sheetId)
        {
            Assert.IsNotNull(smartsheet);
            smartsheet.SheetResources.DeleteSheet(sheetId);
        }

        public void DeleteWorkspace(long workspaceId)
        {
            Assert.IsNotNull(smartsheet);
            smartsheet.WorkspaceResources.DeleteWorkspace(workspaceId);
        }
    }
}