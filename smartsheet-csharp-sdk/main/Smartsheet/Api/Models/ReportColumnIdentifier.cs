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
    /// An object for matching a source sheet column for a report. It requires one of:
    /// - [type, title] for regular columns
    /// - [type, systemColumnType] for system columns
    /// - [type=TEXT_NUMBER, primary=true] for the primary column
    /// - [type=TEXT_NUMBER, sheetNameColumn=true] for the special sheet name report column
    ///
    /// Note: You can combine multiple CHECKBOX columns or multiple PICKLIST columns from different sheets
    /// into a single report column, even if their underlying symbols differ. However, you can't combine
    /// a CHECKBOX column with a PICKLIST column, because they're different types.
    ///
    /// Note: The system column type AUTO_NUMBER is matched together with columns having the same title
    /// and type=TEXT_NUMBER. Therefore, title is a required property in this case.
    /// </summary>
    public class ReportColumnIdentifier
    {
        /// <summary>
        /// Title of a column to match.
        /// Note: If you specified primary=true to match primary columns, you can set the resulting
        /// report column title to this value.
        /// </summary>
        private string? title;

        /// <summary>
        /// Type of column to match. See Column Types.
        /// </summary>
        private ColumnType type;

        /// <summary>
        /// System column type to match. See System Columns.
        /// </summary>
        private ReportSystemColumnType? systemColumnType;

        /// <summary>
        /// Set this to true to match the primary column.
        /// </summary>
        private bool? primary;

        /// <summary>
        /// Set this to true to match the special "Sheet Name" report column.
        /// </summary>
        private bool? sheetNameColumn;

        /// <summary>
        /// Gets or sets the title of a column to match.
        /// Note: If you specified primary=true to match primary columns, you can set the resulting
        /// report column title to this value.
        /// </summary>
        /// <returns> the title </returns>
        public string? Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Gets or sets the type of column to match.
        /// </summary>
        /// <returns> the type </returns>
        public ColumnType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets or sets the system column type to match.
        /// </summary>
        /// <returns> the system column type </returns>
        public ReportSystemColumnType? SystemColumnType
        {
            get { return systemColumnType; }
            set { systemColumnType = value; }
        }

        /// <summary>
        /// Gets or sets whether to match the primary column.
        /// </summary>
        /// <returns> true if primary, false otherwise </returns>
        public bool? Primary
        {
            get { return primary; }
            set { primary = value; }
        }

        /// <summary>
        /// Gets or sets whether to match the special "Sheet Name" report column.
        /// </summary>
        /// <returns> true if sheet name column, false otherwise </returns>
        public bool? SheetNameColumn
        {
            get { return sheetNameColumn; }
            set { sheetNameColumn = value; }
        }
    }
}
