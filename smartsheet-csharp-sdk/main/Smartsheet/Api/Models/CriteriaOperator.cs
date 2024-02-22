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
    /// Represents operator.
    /// </summary>
    public enum CriteriaOperator
    {
        /// <summary>
        /// Equality operator
        /// </summary>
        EQUAL,
        /// <summary>
        /// Inequality operator
        /// </summary>
        NOT_EQUAL,
        /// <summary>
        /// Greater than operator
        /// </summary>
        GREATER_THAN,
        /// <summary>
        /// Greater than or equal operator
        /// </summary>
        GREATER_THAN_OR_EQUAL,
        /// <summary>
        /// Less than operator
        /// </summary>
        LESS_THAN,
        /// <summary>
        /// Less than or equal operator
        /// </summary>
        LESS_THAN_OR_EQUAL,
        /// <summary>
        /// Contains in collection operator
        /// </summary>
        CONTAINS,
        /// <summary>
        /// Not contained in collection operator
        /// </summary>
        DOES_NOT_CONTAIN,
        /// <summary>
        /// Between two values operator
        /// </summary>
        BETWEEN,
        /// <summary>
        /// Not between two values operator
        /// </summary>
        NOT_BETWEEN,
        /// <summary>
        /// Date is today operator
        /// </summary>
        TODAY,
        /// <summary>
        /// Date is not today operator
        /// </summary>
        NOT_TODAY,
        /// <summary>
        /// Date is in past operator
        /// </summary>
        PAST,
        /// <summary>
        /// Date is not in the past operator
        /// </summary>
        NOT_PAST,
        /// <summary>
        /// Date is in the future operator
        /// </summary>
        FUTURE,
        /// <summary>
        /// Date is not in the future operator
        /// </summary>
        NOT_FUTURE,
        /// <summary>
        /// Date is in the last N days operator
        /// </summary>
        LAST_N_DAYS,
        /// <summary>
        /// Date is not in the last N days operator
        /// </summary>
        NOT_LAST_N_DAYS,
        /// <summary>
        /// Date is in the next N days operator
        /// </summary>
        NEXT_N_DAYS,
        /// <summary>
        /// Date is not in the next N days operator
        /// </summary>
        NOT_NEXT_N_DAYS,
        /// <summary>
        /// Operator for value is blank
        /// </summary>
        IS_BLANK,
        /// <summary>
        /// Operator for value is not blank
        /// </summary>
        IS_NOT_BLANK,
        /// <summary>
        /// Operator for value is a number
        /// </summary>
        IS_NUMBER,
        /// <summary>
        /// Operator for value is not a number
        /// </summary>
        IS_NOT_NUMBER,
        /// <summary>
        /// Operator for value is a date
        /// </summary>
        IS_DATE,
        /// <summary>
        /// Operator for value is not a date
        /// </summary>
        IS_NOT_DATE,
        /// <summary>
        /// Operator for value is checked
        /// </summary>
        IS_CHECKED,
        /// <summary>
        /// Operator for value is not checked
        /// </summary>
        IS_NOT_CHECKED,
        /// <summary>
        /// Operator for value is one of a set of options
        /// </summary>
        IS_ONE_OF,
        /// <summary>
        /// Operator for value is not one of a set of options
        /// </summary>
        IS_NOT_ONE_OF,
        /// <summary>
        /// Is current user operator
        /// </summary>
        IS_CURRENT_USER,
        /// <summary>
        /// Is not the current user operator
        /// </summary>
        IS_NOT_CURRENT_USER,
        /// <summary>
        /// Is on critical path operator
        /// </summary>
        ON_CRITICAL_PATH,
        /// <summary>
        /// Is not on critical path operator
        /// </summary>
        NOT_ON_CRITICAL_PATH,
        /// <summary>
        /// Has attachments operator
        /// </summary>
        HAS_ATTACHMENTS,
        /// <summary>
        /// Has no attachments operator
        /// </summary>
        NO_ATTACHMENTS,
        /// <summary>
        /// Has comments operator
        /// </summary>
        HAS_COMMENTS,
        /// <summary>
        /// Has no comments operator
        /// </summary>
        NO_COMMENTS,
        /// <summary>
        /// Has any of these values operator
        /// </summary>
        HAS_ANY_OF,
        /// <summary>
        /// Has none of these values operator
        /// </summary>
        HAS_NONE_OF,
        /// <summary>
        /// Has all of these values operator
        /// </summary>
        HAS_ALL_OF,
        /// <summary>
        /// Has not all of these values operator
        /// </summary>
        NOT_ALL_OF
    }
}