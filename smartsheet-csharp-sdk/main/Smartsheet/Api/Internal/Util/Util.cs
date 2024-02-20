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
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Smartsheet.Api.Internal.Utility
{
    /// <summary>
    /// Utility class for common functions.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Utility()
        {
        }

        /// <summary>
        /// Helper function that throws an IllegalArgumentException if one of the parameters is null.
        /// @param objects the parameters to 
        /// </summary>
        /// <param name="objects"></param>
        /// <exception cref="System.ArgumentException"></exception>       
        public static void ThrowIfNull(params object[] objects)
        {
            foreach (object obj in objects)
            {
                if (obj == null)
                {
                    throw new System.ArgumentException();
                }
            }
        }

        /// <summary>
        /// Helper function that throws an IllegalArgumentException if one of the parameters is empty.
        /// @param objects the parameters to 
        /// </summary>
        /// <param name="strings"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void ThrowIfEmpty(params string[] strings)
        {
            foreach (string @string in strings)
            {
                if (@string != null && @string.Length == 0)
                {
                    throw new System.ArgumentException();
                }
            }
        }

        /// <summary>
        /// Helper function that reads a binary reader into a byte array.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                {
                    ms.Write(buffer, 0, count);
                }
                ms.Position = 0;
                return ms.ToArray();
            }
        }
    }
}