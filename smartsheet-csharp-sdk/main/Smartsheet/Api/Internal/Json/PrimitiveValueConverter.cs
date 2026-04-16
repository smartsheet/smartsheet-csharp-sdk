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

namespace Smartsheet.Api.Internal.Json
{
    /// <summary>
    /// Base class for converters that handle primitive value types (string, number, boolean, null)
    /// Provides common logic for reading primitive JSON tokens and delegates to subclasses
    /// to create the appropriate value objects.
    /// </summary>
    internal abstract class PrimitiveValueConverter : JsonConverter
    {
        protected readonly bool _enableDecimalObjectValue;

        /// <summary>
        /// Constructor that accepts configuration for decimal value handling.
        /// </summary>
        /// <param name="enableDecimalObjectValue">
        /// If true, numeric values are deserialized with decimal precision.
        /// If false, numeric values are deserialized as double (default behavior).
        /// </param>
        protected PrimitiveValueConverter(bool enableDecimalObjectValue)
        {
            _enableDecimalObjectValue = enableDecimalObjectValue;
        }

        /// <summary>
        /// Read a primitive value (string, number, boolean, null) from JSON.
        /// Delegates to subclass factory methods to create the appropriate value objects.
        /// </summary>
        protected object ReadPrimitiveValue(JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Boolean:
                    return CreateBooleanValue((bool)reader.Value);

                case JsonToken.Integer:
                    if (_enableDecimalObjectValue)
                    {
                        return CreateDecimalValue(Convert.ToDecimal(reader.Value));
                    }
                    else
                    {
                        return CreateNumberValue(Convert.ToDouble(reader.Value));
                    }

                case JsonToken.Float:
                    if (_enableDecimalObjectValue)
                    {
                        return CreateDecimalValue((decimal)reader.Value);
                    }
                    else
                    {
                        // reader.Value is decimal due to FloatParseHandling.Decimal, convert to double
                        return CreateNumberValue(Convert.ToDouble(reader.Value));
                    }

                case JsonToken.Date:
                    return CreateStringValue(((DateTime)reader.Value).ToString("yyyy-MM-ddTHH:mm:ssZ"));

                case JsonToken.Null:
                    return CreateNullValue();

                case JsonToken.String:
                    return CreateStringValue((string)reader.Value);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Create a boolean value object. Subclasses must implement this.
        /// Return null if the subclass doesn't support boolean values.
        /// </summary>
        protected abstract object CreateBooleanValue(bool value);

        /// <summary>
        /// Create a number value object (double). Subclasses must implement this.
        /// </summary>
        protected abstract object CreateNumberValue(double value);

        /// <summary>
        /// Create a decimal value object. Subclasses must implement this.
        /// </summary>
        protected abstract object CreateDecimalValue(decimal value);

        /// <summary>
        /// Create a string value object. Subclasses must implement this.
        /// </summary>
        protected abstract object CreateStringValue(string value);

        /// <summary>
        /// Create a null value object. Subclasses must implement this.
        /// Return null if the subclass doesn't support a null value object.
        /// </summary>
        protected abstract object CreateNullValue();
    }
}
