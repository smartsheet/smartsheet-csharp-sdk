using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace mock_api_test_sdk_net80
{
    [TestClass]
    public class RowTests
    {
        [TestMethod]
        public void GetRow_Serialization_Proof()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Serialization - Proof");

            Row row = smartsheet.SheetResources.RowResources.GetRow(
                1, 2, new List<RowInclusion> { RowInclusion.PROOFS }, null);

            Proof proof = row.Proof;
            Assert.IsNotNull(proof);
            Assert.AreEqual(100L, proof.Id);
            Assert.AreEqual(100L, proof.OriginalId);
            Assert.AreEqual("Sample Proof Document", proof.Name);
            Assert.AreEqual(ProofType.IMAGE, proof.Type);
            Assert.AreEqual("NONE", proof.DocumentType);
            Assert.AreEqual("https://app.smartsheet.com/b/proofs/sheets/test123/proofs/proof456", proof.ProofRequestUrl);
            Assert.AreEqual(1, proof.Version);
            Assert.AreEqual("john.doe@smartsheet.com", proof.LastUpdatedBy.Email);
            Assert.AreEqual(false, proof.IsCompleted);
        }

        [TestMethod]
        public void AddRows_AssignValues_String()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Assign Values - String");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Apple"
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Red Fruit"
                    }
                }
            };

            Row rowB = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Banana"
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Yellow Fruit"
                    }
                }
            };

            // Update rows in sheet
            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA, rowB });

            Row? row = addedRows.FirstOrDefault(r => r.Id == 10);
            Cell? cell = row?.Cells.FirstOrDefault(c => c.Value.Equals("Apple"));

            Assert.IsNotNull(cell);
            Assert.AreEqual(101, cell.ColumnId);
        }

        [TestMethod]
        public void AddRows_AssignValues_Int()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Assign Values - Int");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = 100
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "One Hundred"
                    }
                }
            };

            Row rowB = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = 2.1
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Two Point One"
                    }
                }
            };

            // Update rows in sheet
            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA, rowB });

            Row? row = addedRows.FirstOrDefault(r => r.Id == 10);
            Cell? cell = row?.Cells.FirstOrDefault(c => Convert.ToInt32(c.Value) == 100);
            Assert.IsNotNull(cell);
            Assert.AreEqual(101, cell.ColumnId);
        }

        [TestMethod]
        public void AddRows_AssignValues_Bool()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Assign Values - Bool");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = true
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "This is True"
                    }
                }
            };

            Row rowB = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = false
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "This is False"
                    }
                }
            };

            // Update rows in sheet
            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA, rowB });

            Row? row = addedRows.FirstOrDefault(r => r.Id == 10);
            Cell? cell = row?.Cells.FirstOrDefault(c => c.Value.Equals(true));

            Assert.IsNotNull(cell);
            Assert.AreEqual(101, cell.ColumnId);
        }


        [TestMethod]
        public void AddRows_AssignFormulae()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Assign Formulae");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Formula = "=SUM([Column2]3, [Column2]4)*2"
                    },
                    new Cell{
                        ColumnId = 102,
                        Formula = "=SUM([Column2]3, [Column2]3, [Column2]4)"
                    }
                }
            };

            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA });

            Cell? cell = addedRows[0].Cells.FirstOrDefault(c => c.Formula.Equals("=SUM([Column2]3, [Column2]3, [Column2]4)"));
            Assert.IsNotNull(cell);
            Assert.AreEqual(102, cell.ColumnId);
        }

        [TestMethod]
        public void AddRows_AssignValues_Hyperlink()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Assign Values - Hyperlink");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Google",
                        Hyperlink = new Hyperlink{
                            Url = "http://google.com"
                        }
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Bing",
                        Hyperlink = new Hyperlink{
                            Url = "http://bing.com"
                        }
                    }
                }
            };

            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA });

            Cell? cell = addedRows[0].Cells.FirstOrDefault(c => c.Value.Equals("Google"));
            Assert.IsNotNull(cell);
            Hyperlink link = cell.Hyperlink;

            Assert.AreEqual(101, cell.ColumnId);
            Assert.AreEqual("http://google.com", link.Url);
        }

        [TestMethod]
        public void AddRows_AssignValues_HyperlinkSheetID()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Assign Values - Hyperlink SheetID");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Sheet2",
                        Hyperlink = new Hyperlink{
                            SheetId = 2
                        }
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Sheet3",
                        Hyperlink = new Hyperlink{
                            SheetId = 3
                        }
                    }
                }
            };

            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA });
            var cellsValue = addedRows[0].Cells;
            Cell? cell = cellsValue.FirstOrDefault(c => c.Value.Equals("Sheet3"));
            Assert.IsNotNull(cell);
            Hyperlink link = cell.Hyperlink;

            Assert.AreEqual(102, cell.ColumnId);
            Assert.AreEqual(3, link.SheetId);
        }

        [TestMethod]
        public void AddRows_AssignValues_HyperlinkReportID()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Assign Values - Hyperlink ReportID");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Report9",
                        Hyperlink = new Hyperlink{
                            ReportId = 9
                        }
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Report8",
                        Hyperlink = new Hyperlink{
                            ReportId = 8
                        }
                    }
                }
            };

            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA });

            Cell? cell = addedRows[0].Cells.FirstOrDefault(c => c.Value.Equals("Report8"));
            Assert.IsNotNull(cell);
            Hyperlink link = cell.Hyperlink;

            Assert.AreEqual(102, cell.ColumnId);
            Assert.AreEqual(8, link.ReportId);
        }

        [TestMethod]
        public void AddRows_Invalid_AssignValueAndFormulae()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Invalid - Assign Value and Formulae");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Formula = "=SUM([Column2]3, [Column2]4)*2",
                        Value = "20"

                    },
                    new Cell{
                        ColumnId = 102,
                        Formula = "=SUM([Column2]3, [Column2]3, [Column2]4)"
                    }
                }
            };

            HelperFunctions.AssertRaisesException<SmartsheetException>(() =>
                smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA }),
                "If cell.formula is specified, then value, objectValue, image, hyperlink, and linkInFromCell must not be specified.");
        }

        [TestMethod]
        public void AddRows_Invalid_AssignHyperlinkUrlandSheetId()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Invalid - Assign Hyperlink URL and SheetId");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Google",
                        Hyperlink = new Hyperlink{
                            Url = "http://google.com",
                            SheetId = 2
                        }
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Bing",
                        Hyperlink = new Hyperlink{
                            Url = "http://bing.com"
                        }
                    }
                }
            };


            HelperFunctions.AssertRaisesException<SmartsheetException>(() =>
                smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA }),
                "hyperlink.url must be null for sheet, report, or Sight hyperlinks.");
        }

        [TestMethod]
        public void AddRows_AssignObjectValue_PredecessorList()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Assign Object Value - Predecessor List (using floats)");

            Row rowA = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell
                    {
                        ColumnId = 101,
                        ObjectValue = new PredecessorList
                        {
                            Predecessors = new List<Predecessor>
                            {
                                new Predecessor
                                {
                                    RowId = 10,
                                    Type = "FS",
                                    Lag = new Duration
                                    {
                                        Days = 2.5
                                    }
                                }
                            }
                        }
                    }
                }
            };

            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA });

            Cell predecessorCell = addedRows[0].Cells.Single(c => c.ColumnId == 101);
            Assert.AreEqual("2FS +2.5d", predecessorCell.Value);
        }

        [TestMethod]
        public void AddRows_Location_Top()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Location - Top");

            Row rowA = new Row
            {
                ToTop = true,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Apple"
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Red Fruit"
                    }
                }
            };

            // Update rows in sheet
            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA });

            Row? row = addedRows.FirstOrDefault(r => r.Id == 10);
            Cell? cell = row?.Cells.FirstOrDefault(c => c.Value.Equals("Apple"));

            Assert.AreEqual(1, row?.RowNumber);
            Assert.IsNotNull(cell);
            Assert.AreEqual(101, cell.ColumnId);
        }

        [TestMethod]
        public void AddRows_Location_Bottom()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Add Rows - Location - Bottom");

            Row rowA = new Row
            {
                ToBottom = true,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Apple"
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Red Fruit"
                    }
                }
            };

            // Update rows in sheet
            IList<Row> addedRows = smartsheet.SheetResources.RowResources.AddRows(1, new Row[] { rowA });

            Row? row = addedRows.FirstOrDefault(r => r.Id == 10);
            Cell? cell = row?.Cells.FirstOrDefault(c => c.Value.Equals("Apple"));

            Assert.AreEqual(100, row?.RowNumber);
            Assert.IsNotNull(cell);
            Assert.AreEqual(101, cell.ColumnId);
        }

        [TestMethod]
        public void UpdateRows_AssignValues_String()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Assign Values - String");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Apple"
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Red Fruit"
                    }
                }
            };

            Row rowB = new Row
            {
                Id = 11,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Banana"
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Yellow Fruit"
                    }
                }
            };

            // Update rows in sheet
            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA, rowB });

            Row? row = updatedRows.Where(r => r.Id == 10).FirstOrDefault();
            Cell? cell = row?.Cells.Where(c => c.Value.Equals("Apple")).FirstOrDefault();
            Assert.IsNotNull(cell);
            Assert.AreEqual(101, cell.ColumnId);
        }

        [TestMethod]
        public void UpdateRows_AssignValues_Int()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Assign Values - Int");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = 100
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "One Hundred"
                    }
                }
            };

            Row rowB = new Row
            {
                Id = 11,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = 2.1
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Two Point One"
                    }
                }
            };

            // Update rows in sheet
            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, [rowA, rowB]);
            Row? row = updatedRows.FirstOrDefault(r => r.Id == 10);
            Cell? cell = row?.Cells.FirstOrDefault(c => Convert.ToInt32(c.Value) == 100);
            Assert.IsNotNull(cell);
            Assert.AreEqual(101, cell.ColumnId);
        }

        [TestMethod]
        public void UpdateRows_AssignValues_Bool()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Assign Values - Bool");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = true
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "This is True"
                    }
                }
            };

            Row rowB = new Row
            {
                Id = 11,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = false
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "This is False"
                    }
                }
            };

            // Update rows in sheet
            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA, rowB });

            Row? row = updatedRows.FirstOrDefault(r => r.Id == 10);
            Cell? cell = row?.Cells.FirstOrDefault(c => c.Value.Equals(true));
            Assert.IsNotNull(cell);
            Assert.AreEqual(101, cell.ColumnId);
        }

        [TestMethod]
        public void UpdateRows_AssignFormulae()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Assign Formulae");

            Row rowA = new Row
            {
                Id = 11,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Formula = "=SUM([Column2]3, [Column2]4)*2"
                    },
                    new Cell{
                        ColumnId = 102,
                        Formula = "=SUM([Column2]3, [Column2]3, [Column2]4)"
                    }
                }
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });

            Cell? cell = updatedRows[0].Cells.Where(c => c.Formula.Equals("=SUM([Column2]3, [Column2]3, [Column2]4)")).FirstOrDefault();
            Assert.IsNotNull(cell);
            Assert.AreEqual(102, cell.ColumnId);
        }

        [TestMethod]
        public void UpdateRows_AssignValues_Hyperlink()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Assign Values - Hyperlink");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Google",
                        Hyperlink = new Hyperlink{
                            Url = "http://google.com"
                        }
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Bing",
                        Hyperlink = new Hyperlink{
                            Url = "http://bing.com"
                        }
                    }
                }
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });

            Cell? cell = updatedRows[0].Cells.Where(c => c.Value.Equals("Google")).FirstOrDefault();
            Assert.IsNotNull(cell);
            Hyperlink link = cell.Hyperlink;

            Assert.AreEqual(101, cell.ColumnId);
            Assert.AreEqual("http://google.com", link.Url);
        }

        [TestMethod]
        public void UpdateRows_AssignValues_HyperlinkSheetID()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Assign Values - Hyperlink SheetID");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Sheet2",
                        Hyperlink = new Hyperlink{
                            SheetId = 2
                        }
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Sheet3",
                        Hyperlink = new Hyperlink{
                            SheetId = 3
                        }
                    }
                }
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });
            var cells = updatedRows[0].Cells;
            Assert.IsNotNull(cells);
            Cell? cell = cells.FirstOrDefault(c => c.Value.Equals("Sheet3"));
            Assert.IsNotNull(cell);
            Hyperlink link = cell.Hyperlink;

            Assert.AreEqual(102, cell.ColumnId);
            Assert.AreEqual(3, link.SheetId);
        }

        [TestMethod]
        public void UpdateRows_AssignValues_HyperlinkReportID()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Assign Values - Hyperlink ReportID");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Report9",
                        Hyperlink = new Hyperlink{
                            ReportId = 9
                        }
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Report8",
                        Hyperlink = new Hyperlink{
                            ReportId = 8
                        }
                    }
                }
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });

            Cell? cell = updatedRows[0].Cells.FirstOrDefault(c => c.Value.Equals("Report8"));
            Assert.IsNotNull(cell);
            Hyperlink link = cell.Hyperlink;

            Assert.AreEqual(102, cell.ColumnId);
            Assert.AreEqual(8, link.ReportId);
        }

        [TestMethod]
        public void UpdateRows_Invalid_AssignValueAndFormulae()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Invalid - Assign Value and Formulae");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Formula = "=SUM([Column2]3, [Column2]4)*2",
                        Value = "20"

                    },
                    new Cell{
                        ColumnId = 102,
                        Formula = "=SUM([Column2]3, [Column2]3, [Column2]4)"
                    }
                }
            };

            HelperFunctions.AssertRaisesException<SmartsheetException>(() =>
                smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA }),
                "If cell.formula is specified, then value, objectValue, image, hyperlink, and linkInFromCell must not be specified.");
        }

        [TestMethod]
        public void UpdateRows_Invalid_AssignHyperlinkUrlandSheetId()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Invalid - Assign Hyperlink URL and SheetId");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell{
                        ColumnId = 101,
                        Value = "Google",
                        Hyperlink = new Hyperlink{
                            Url = "http://google.com",
                            SheetId = 2
                        }
                    },
                    new Cell{
                        ColumnId = 102,
                        Value = "Bing",
                        Hyperlink = new Hyperlink{
                            Url = "http://bing.com"
                        }
                    }
                }
            };

            HelperFunctions.AssertRaisesException<SmartsheetException>(() =>
                smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA }),
                "hyperlink.url must be null for sheet, report, or Sight hyperlinks.");
        }

        [TestMethod]
        public void UpdateRows_ClearValue_TextNumber()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Clear Value - Text Number");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell
                    {
                        ColumnId = 101,
                        Value = ""
                    }
                }
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });

            Cell updatedCell = updatedRows[0].Cells.Single(c => c.ColumnId == 101);
            Assert.AreEqual(null, updatedCell.Value);
        }

        [TestMethod]
        public void UpdateRows_ClearValue_Checkbox()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Clear Value - Checkbox");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell
                    {
                        ColumnId = 101,
                        Value = ""
                    }
                }
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });

            Cell updatedCell = updatedRows[0].Cells.Single(c => c.ColumnId == 101);
            Assert.AreEqual(false, updatedCell.Value);
        }

        [TestMethod]
        public void UpdateRows_ClearValue_Hyperlink()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Clear Value - Hyperlink");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell
                    {
                        ColumnId = 101,
                        Value = "",
                        Hyperlink = new Hyperlink()
                    }
                }
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });

            Cell updatedCell = updatedRows[0].Cells.Single(c => c.ColumnId == 101);
            Assert.AreEqual(null, updatedCell.Hyperlink);
            Assert.AreEqual(null, updatedCell.Value);
        }

        [TestMethod]
        public void UpdateRows_ClearValue_CellLink()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Clear Value - Cell Link");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell
                    {
                        ColumnId = 101,
                        Value = "",
                        LinkInFromCell = new CellLink()
                    }
                }
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });

            Cell updatedCell = updatedRows[0].Cells.Single(c => c.ColumnId == 101);
            Assert.AreEqual(null, updatedCell.LinkInFromCell);
            Assert.AreEqual(null, updatedCell.Value);
        }

        [TestMethod]
        public void UpdateRows_ClearValue_PredecessorList()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Clear Value - Predecessor List");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell
                    {
                        ColumnId = 123,
                        Value = new ExplicitNull()
                    }
                }
            };
            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });
            Cell updatedCell = updatedRows[0].Cells.Single(c => c.ColumnId == 123);
            Assert.AreEqual(updatedRows[0].Id, 10);
            Assert.AreEqual(null, updatedCell.Value);
        }


        [TestMethod]
        public void UpdateRows_Invalid_AssignHyperlinkAndCellLink()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Invalid - Assign Hyperlink and Cell Link");

            Row rowA = new Row
            {
                Id = 10,
                Cells = new List<Cell>
                {
                    new Cell
                    {
                        ColumnId = 101,
                        Value = "",
                        LinkInFromCell = new CellLink
                        {
                            ColumnId = 201,
                            RowId = 20,
                            SheetId = 2
                        },
                        Hyperlink = new Hyperlink
                        {
                            Url = "www.google.com"
                        }
                    }
                }
            };

            HelperFunctions.AssertRaisesException<SmartsheetException>(() =>
                    smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA }),
                "Only one of cell.hyperlink or cell.linkInFromCell may be non-null.");
        }

        [TestMethod]
        public void UpdateRows_Location_Top()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Location - Top");

            Row rowA = new Row
            {
                Id = 10,
                ToTop = true
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });

            Row updateRow = updatedRows.Single(r => r.Id == 10);
            Assert.AreEqual(1, updateRow.RowNumber);
        }

        [TestMethod]
        public void UpdateRows_Location_Bottom()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Update Rows - Location - Bottom");

            Row rowA = new Row
            {
                Id = 10,
                ToBottom = true
            };

            IList<Row> updatedRows = smartsheet.SheetResources.RowResources.UpdateRows(1, new Row[] { rowA });

            Row updateRow = updatedRows.Single(r => r.Id == 10);
            Assert.AreEqual(100, updateRow.RowNumber);
        }

        [TestMethod]
        public void MoveRow_AnotherSheet()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Move row to another sheet");

            CopyOrMoveRowResult result = smartsheet.SheetResources.RowResources.MoveRowsToAnotherSheet(
                1228520367122308,
                new CopyOrMoveRowDirective
                {
                    RowIds = new List<long>
                    {
                        1765250516182916
                    },
                    To = new CopyOrMoveRowDestination
                    {
                        SheetId = 799249123305348
                    }
                });
            Assert.AreEqual(result.DestinationSheetId, 799249123305348);
        }

        [TestMethod]
        public void CopyRow_AnotherSheet()
        {
            SmartsheetClient smartsheet = HelperFunctions.SetupClient("Copy row to another sheet");
            CopyOrMoveRowResult result = smartsheet.SheetResources.RowResources.CopyRowsToAnotherSheet(
                1228520367122308,
                new CopyOrMoveRowDirective
                {
                    RowIds = new List<long>
                    {
                        2891150423025540
                    },
                    To = new CopyOrMoveRowDestination
                    {
                        SheetId = 799249123305348
                    }
                });
            Assert.AreEqual(result.DestinationSheetId, 799249123305348);
        }
    }
}
