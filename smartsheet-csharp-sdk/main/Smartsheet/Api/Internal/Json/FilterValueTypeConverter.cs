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
    /// Helper class to convert filter value types for JSON serialization/deserialization
    /// </summary>
    class FilterValueTypeConverter : JsonConverter
    {
        /// <summary>
        /// Determines if this converter can handle the given type
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(FilterValue).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Read filter value from JSON
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            FilterValue filterValue;

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
                        filterValue = new DateFilterValue(dateValue);
                        break;

                    case "CURRENT_USER":
                        filterValue = new CurrentUserFilterValue();
                        break;

                    default:
                        // Unknown objectType, return null
                        return null;
                }
            }
            else if (reader.TokenType == JsonToken.Null)
            {
                filterValue = new NullFilterValue();
            }
            else if (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float)
            {
                filterValue = new NumberFilterValue(Convert.ToDouble(reader.Value));
            }
            else if (reader.TokenType == JsonToken.String)
            {
                filterValue = new StringFilterValue((string)reader.Value);
            }
            else
            {
                return null;
            }

            return filterValue;
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

            FilterValue filterValue = (FilterValue)value;

            switch (filterValue.ValueType)
            {
                case FilterValueType.STRING:
                    StringFilterValue stringValue = (StringFilterValue)filterValue;
                    writer.WriteValue(stringValue.Value);
                    break;

                case FilterValueType.NUMBER:
                    NumberFilterValue numberValue = (NumberFilterValue)filterValue;
                    writer.WriteValue(numberValue.Value);
                    break;

                case FilterValueType.NULL:
                    writer.WriteNull();
                    break;

                case FilterValueType.DATE:
                    DateFilterValue dateValue = (DateFilterValue)filterValue;
                    writer.WriteStartObject();
                    writer.WritePropertyName("objectType");
                    writer.WriteValue(dateValue.ObjectType);
                    writer.WritePropertyName("value");
                    writer.WriteValue(dateValue.Value);
                    writer.WriteEndObject();
                    break;

                case FilterValueType.CURRENT_USER:
                    CurrentUserFilterValue currentUserValue = (CurrentUserFilterValue)filterValue;
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
    }
}
