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
    /// Represents page dimensions in the Smartsheet REST API.
    /// Dimensions pulled from https://community.smartsheet.com/discussion/4116/custom-print-sizes
    /// </summary>
    public enum PaperSize
    {
        /// <summary>
        /// 8.5in x 11in
        /// </summary>
        LETTER,
        /// <summary>
        /// 8.5in x 14in
        /// </summary>
        LEGAL,
        /// <summary>
        /// 11 x 17
        /// </summary>
        WIDE,
        /// <summary>
        /// 24in x 36in
        /// </summary>
        ARCHD,
        /// <summary>
        /// 210mm x 297mm
        /// </summary>
        A4,
        /// <summary>
        /// 297mm x 420mm
        /// </summary>
        A3,
        /// <summary>
        /// 420mm x 594mm
        /// </summary>
        A2,
        /// <summary>
        /// 594mm x 841mm
        /// </summary>
        A1,
        /// <summary>
        /// 841mm x 1189mm
        /// </summary>
        A0
    }
}