//    #[license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2014 SmartsheetClient
//    %%
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//            http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//    %[license]

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Object used to match a sheet column for a report. One of [type, systemColumnType] or [primary=true] is required.
    ///
    /// systemColumnType should be specified if you want to match a system column. Use primary=true to match primary columns.
    /// When matching primary columns, title can be used to customize primary column name in the rendered report.
    ///
    /// Note: Columns in the report are matched by the combination of title and type (and systemColumnType if specified).
    ///
    /// Note: symbol is not used for matching and as a result CHECKBOX or PICKLIST columns with different symbols
    /// (from different sheets) can be combined into the same column in the report. You cannot combine CHECKBOX with
    /// PICKLIST into the same column in the report because they are different types.
    /// </summary>
    public class ReportColumnIdentifier
    {
        /// <summary>
        /// Column title to be matched from the source sheets.
        /// If primary is true, then this property can be used to customize the primary column title.
        /// </summary>
        private string? title;

        /// <summary>
        /// Column type to be matched from the source sheets.
        /// </summary>
        private ColumnType? type;

        /// <summary>
        /// System column type to be matched from the source sheets.
        /// SHEET_NAME is available as an extra option for reports.
        /// </summary>
        private ReportSystemColumnType? systemColumnType;

        /// <summary>
        /// Indicates if the matched column is primary.
        /// </summary>
        private bool? primary;

        /// <summary>
        /// Gets the column title to be matched from the source sheets.
        /// </summary>
        /// <returns> the title </returns>
        public string? Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Gets the column type to be matched from the source sheets.
        /// </summary>
        /// <returns> the type </returns>
        public ColumnType? Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets the system column type to be matched from the source sheets.
        /// </summary>
        /// <returns> the system column type </returns>
        public ReportSystemColumnType? SystemColumnType
        {
            get { return systemColumnType; }
            set { systemColumnType = value; }
        }

        /// <summary>
        /// Gets whether the matched column is primary.
        /// </summary>
        /// <returns> true if primary, false otherwise </returns>
        public bool? Primary
        {
            get { return primary; }
            set { primary = value; }
        }
    }
}
