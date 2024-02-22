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

using Smartsheet.Api;
using System;

namespace Smartsheet.Api.Internal.Json
{
    /// <summary>
    /// Class wrapping exceptions from JSON serialization
    /// </summary>
    public class JsonSerializationException : SmartsheetException
    {
        /// <summary>
        /// Constructor taking a string
        /// </summary>
        /// <param name="message"></param>
        public JsonSerializationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor taking a message and exception for cause
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cause"></param>
        public JsonSerializationException(string message, Exception cause)
            : base(message, cause)
        {
        }

        /// <summary>
        /// Constructor taking an exception for an argument
        /// </summary>
        /// <param name="e"></param>
        public JsonSerializationException(Exception e)
            : base(e)
        {
        }
    }
}