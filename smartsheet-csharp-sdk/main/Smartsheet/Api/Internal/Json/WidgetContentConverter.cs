//    #[ license]
//    SmartsheetClient SDK for C#
//    %%
//    Copyright (C) 2014-2019 SmartsheetClient
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

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Smartsheet.Api.Models;

namespace Smartsheet.Api.Internal.Json
{
    /// <summary>
    /// Converter class for widget content
    /// </summary>
    class WidgetContentConverter : JsonConverter
    {
        /// <summary>
        /// Helper function to know if conversion can be done
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(IWidgetContent).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Read object from json
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            IWidgetContent content;

            WidgetContentSuperset superset = new WidgetContentSuperset();
            serializer.Populate(reader, superset);

            WidgetType parsedType;
            if (!Enum.TryParse(superset.type, true, out parsedType))
            {
                // If a new object type is introduced to the Smartsheet API that this version of the SDK doesn't support, 
                // return null instead of throwing an exception.
                if (superset.type == "WidgetWebContent")
                {
                    parsedType = WidgetType.WEBCONTENT;
                }
                else
                    return null;
            }

            switch (parsedType)
            {
                case WidgetType.CHART:
                    ChartWidgetContent chartWidgetContent = new ChartWidgetContent();
                    chartWidgetContent.ReportId = superset.reportId;
                    chartWidgetContent.SheetId = superset.sheetId;
                    chartWidgetContent.Axes = superset.axes;
                    chartWidgetContent.Hyperlink = superset.hyperlink;
                    chartWidgetContent.IncludedColumnIds = superset.includedColumnIds;
                    chartWidgetContent.Legend = superset.legend;
                    chartWidgetContent.SelectionRanges = superset.selectionRanges;
                    chartWidgetContent.Series = superset.series;
                    content = chartWidgetContent;
                    break;

                case WidgetType.IMAGE:
                    ImageWidgetContent imageWidgetContent = new ImageWidgetContent();
                    imageWidgetContent.PrivateId = superset.privateId;
                    imageWidgetContent.FileName = superset.fileName;
                    imageWidgetContent.Format = superset.format;
                    imageWidgetContent.Height = superset.height;
                    imageWidgetContent.Hyperlink = superset.hyperlink;
                    imageWidgetContent.Width = superset.width;
                    content = imageWidgetContent;
                    break;

                case WidgetType.METRIC:
                    CellLinkWidgetContent cellLinkWidgetContent = new CellLinkWidgetContent();
                    cellLinkWidgetContent.SheetId = superset.sheetId;
                    cellLinkWidgetContent.CellData = superset.cellData;
                    cellLinkWidgetContent.Columns = superset.columns;
                    cellLinkWidgetContent.Hyperlink = superset.hyperlink;
                    content = cellLinkWidgetContent;
                    break;

                case WidgetType.GRIDGANTT:
                    ReportWidgetContent reportWidgetContent = new ReportWidgetContent();
                    reportWidgetContent.ReportId = superset.reportId;
                    reportWidgetContent.HtmlContent = superset.htmlContent;
                    reportWidgetContent.Hyperlink = superset.hyperlink;
                    content = reportWidgetContent;
                    break;

                case WidgetType.RICHTEXT:
                case WidgetType.TITLE:
                    TitleRichTextWidgetContent titleRichtTextWidgetContent = new TitleRichTextWidgetContent();
                    titleRichtTextWidgetContent.BackgroundColor = superset.backgroundColor;
                    titleRichtTextWidgetContent.HtmlContent = superset.htmlContent;
                    content = titleRichtTextWidgetContent;
                    break;

                case WidgetType.SHORTCUT:
                    ShortcutWidgetContent shortcutWidgetContent = new ShortcutWidgetContent();
                    shortcutWidgetContent.ShortcutData = superset.shortcutData;
                    content = shortcutWidgetContent;
                    break;

                case WidgetType.WEBCONTENT:
                    WebContentWidgetContent webContentWidgetContent = new WebContentWidgetContent();
                    webContentWidgetContent.Url = superset.url;
                    content = webContentWidgetContent;
                    break;

                default:
                    content = null;
                    break;
            }
            return content;
        }

        /// <summary>
        /// Write object to json.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
        }

        /// <summary>
        /// Private helper class to encapsulate widget content possibilies
        /// </summary>
        private class WidgetContentSuperset
        {
            // Common
            public string type;
            public long? sheetId;
            public long? reportId;
            public WidgetHyperlink hyperlink;
            public string htmlContent;

            // CellLinkWidgetContent
            public IList<CellDataItem> cellData;
            public IList<Column> columns;

            // ChartWidgetContent
            public IList<Object> axes;
            public IList<long> includedColumnIds;
            public Object legend;
            public IList<SelectionRange> selectionRanges;
            public IList<Object> series;

            // ImageWidgetContent
            public string privateId;
            public string fileName;
            public string format;
            public int? height;
            public int? width;

            // ReportWidgetContent

            // RichTextWidgetContent

            // ShortcutWidgetContent
            public IList<ShortcutDataItem> shortcutData;

            // TitleWidgetContent
            public string backgroundColor;

            // WebContentWidgetContent
            public string url;
        }
    }
}

