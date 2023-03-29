﻿using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace integration_test_sdk_net70
{
    [TestClass]
    public class ReportResourcesTest
    {
        [Ignore]
        [TestMethod]
        public void TestReportResources()
        {
            SmartsheetClient smartsheet = new SmartsheetBuilder().SetMaxRetryTimeout(30000).Build();

            //Must manually create a Report at smartsheet.com and paste reportId and reportName below.
            long reportId = 1176442848470916;
            string reportName = "New Blank Report";

            Report report = smartsheet.ReportResources.GetReport(reportId, new ReportInclusion[] { ReportInclusion.ATTACHMENTS, ReportInclusion.DISCUSSIONS }, null, null);
            Assert.IsTrue(report.Name == reportName);
            SheetEmail email = new SheetEmail.CreateSheetEmail(new Recipient[] { new Recipient { Email = "ericyan99@outlook.com" } }, SheetEmailFormat.PDF).SetCcMe(true).Build();
            smartsheet.ReportResources.SendReport(reportId, email);

            PaginatedResult<Report> reportResults = smartsheet.ReportResources.ListReports(null);
            Assert.IsTrue(reportResults.Data.Count == 1);
            Assert.IsTrue(reportResults.Data[0].Name == reportName);
        }
    }
}