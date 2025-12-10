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

namespace Smartsheet.Api.Internal.Util
{
    /// <summary>
    /// Utility class for DateTime conversion operations
    /// </summary>
    public static class DateTimeConverter
    {
        /// <summary>
        /// Converts an object (string, or long) to DateTime?
        /// </summary>
        /// <param name="value">The value to convert (string or int)</param>
        /// <returns>A DateTime? representing the converted value, or null if conversion fails</returns>
        public static DateTime? ConvertToDateTime(object value)
        {
            // String format (ISO-8601)
            if (value is string dateString)
            {
                if (DateTime.TryParse(dateString, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime parsedDate))
                {
                    return parsedDate;
                }
            }

            // Try as int (Unix epoch in seconds)
            if (value is int unixTimeInt)
            {
                return DateTimeOffset.FromUnixTimeSeconds(unixTimeInt).DateTime;
            }

            return null;
        }
    }
}
