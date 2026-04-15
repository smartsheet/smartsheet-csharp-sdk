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

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Smartsheet.Api.Models;

namespace Smartsheet.Api.Internal.Json
{
    /// <summary>
    /// Helper class to convert report filter value types for JSON serialization/deserialization
    /// </summary>
    class ReportFilterValueTypeConverter : PrimitiveValueConverter
    {
        /// <summary>
        /// Constructor with default decimal handling (disabled)
        /// </summary>
        public ReportFilterValueTypeConverter() : base(false)
        {
        }

        /// <summary>
        /// Determines if this converter can handle the given type
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ReportFilterValue).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Read filter value from JSON
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject obj = JObject.Load(reader);
                string objectTypeValue = obj["objectType"]?.Value<string>();

                if (objectTypeValue == null)
                {
                    return null;
                }

                switch (objectTypeValue.ToUpper())
                {
                    case "DATE":
                        string dateValue = obj["value"]?.Value<string>();
                        return new DateReportFilterValue(dateValue);

                    case "CURRENT_USER":
                        return new CurrentUserReportFilterValue();

                    default:
                        // Unknown objectType, return null
                        return null;
                }
            }

            // Handle primitives using base class
            return ReadPrimitiveValue(reader);
        }

        /// <summary>
        /// Write filter value to JSON
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            ReportFilterValue filterValue = (ReportFilterValue)value;

            switch (filterValue.ValueType)
            {
                case ReportFilterValueType.STRING:
                    StringReportFilterValue stringValue = (StringReportFilterValue)filterValue;
                    writer.WriteValue(stringValue.Value);
                    break;

                case ReportFilterValueType.NUMBER:
                    NumberReportFilterValue numberValue = (NumberReportFilterValue)filterValue;
                    writer.WriteValue(numberValue.Value);
                    break;

                case ReportFilterValueType.NULL:
                    writer.WriteNull();
                    break;

                case ReportFilterValueType.DATE:
                    DateReportFilterValue dateValue = (DateReportFilterValue)filterValue;
                    writer.WriteStartObject();
                    writer.WritePropertyName("objectType");
                    writer.WriteValue(dateValue.ObjectType);
                    writer.WritePropertyName("value");
                    writer.WriteValue(dateValue.Value);
                    writer.WriteEndObject();
                    break;

                case ReportFilterValueType.CURRENT_USER:
                    CurrentUserReportFilterValue currentUserValue = (CurrentUserReportFilterValue)filterValue;
                    writer.WriteStartObject();
                    writer.WritePropertyName("objectType");
                    writer.WriteValue(currentUserValue.ObjectType);
                    writer.WriteEndObject();
                    break;

                default:
                    writer.WriteNull();
                    break;
            }
        }

        protected override object CreateBooleanValue(bool value)
        {
            // ReportFilterValue doesn't support boolean primitives, return null
            return null;
        }

        protected override object CreateNumberValue(double value)
        {
            return new NumberReportFilterValue(value);
        }

        protected override object CreateDecimalValue(decimal value)
        {
            return new NumberReportFilterValue((double)value);
        }

        protected override object CreateStringValue(string value)
        {
            return new StringReportFilterValue(value);
        }

        protected override object CreateNullValue()
        {
            return new NullReportFilterValue();
        }
    }
}

