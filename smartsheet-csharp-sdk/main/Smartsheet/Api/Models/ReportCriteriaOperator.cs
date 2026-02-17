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
    /// Enum representing the condition operator for filter criteria.
    /// </summary>
    public enum ReportCriteriaOperator
    {
        /// <summary>
        /// Equal to.
        /// </summary>
        EQUAL,

        /// <summary>
        /// Not equal to.
        /// </summary>
        NOT_EQUAL,

        /// <summary>
        /// Greater than.
        /// </summary>
        GREATER_THAN,

        /// <summary>
        /// Less than.
        /// </summary>
        LESS_THAN,

        /// <summary>
        /// Contains the value.
        /// </summary>
        CONTAINS,

        /// <summary>
        /// Between two values.
        /// </summary>
        BETWEEN,

        /// <summary>
        /// Is today.
        /// </summary>
        TODAY,

        /// <summary>
        /// Is in the past.
        /// </summary>
        PAST,

        /// <summary>
        /// Is in the future.
        /// </summary>
        FUTURE,

        /// <summary>
        /// Is in the last N days.
        /// </summary>
        LAST_N_DAYS,

        /// <summary>
        /// Is in the next N days.
        /// </summary>
        NEXT_N_DAYS,

        /// <summary>
        /// Is blank.
        /// </summary>
        IS_BLANK,

        /// <summary>
        /// Is not blank.
        /// </summary>
        IS_NOT_BLANK,

        /// <summary>
        /// Is a number.
        /// </summary>
        IS_NUMBER,

        /// <summary>
        /// Is not a number.
        /// </summary>
        IS_NOT_NUMBER,

        /// <summary>
        /// Is a date.
        /// </summary>
        IS_DATE,

        /// <summary>
        /// Is not a date.
        /// </summary>
        IS_NOT_DATE,

        /// <summary>
        /// Is checked (for checkbox columns).
        /// </summary>
        IS_CHECKED,

        /// <summary>
        /// Is unchecked (for checkbox columns).
        /// </summary>
        IS_UNCHECKED,

        /// <summary>
        /// Is one of the specified values.
        /// </summary>
        IS_ONE_OF,

        /// <summary>
        /// Is not one of the specified values.
        /// </summary>
        IS_NOT_ONE_OF,

        /// <summary>
        /// Less than or equal to.
        /// </summary>
        LESS_THAN_OR_EQUAL,

        /// <summary>
        /// Greater than or equal to.
        /// </summary>
        GREATER_THAN_OR_EQUAL,

        /// <summary>
        /// Does not contain the value.
        /// </summary>
        DOES_NOT_CONTAIN,

        /// <summary>
        /// Not between two values.
        /// </summary>
        NOT_BETWEEN,

        /// <summary>
        /// Is not today.
        /// </summary>
        NOT_TODAY,

        /// <summary>
        /// Is not in the past.
        /// </summary>
        NOT_PAST,

        /// <summary>
        /// Is not in the future.
        /// </summary>
        NOT_FUTURE,

        /// <summary>
        /// Is not in the last N days.
        /// </summary>
        NOT_LAST_N_DAYS,

        /// <summary>
        /// Is not in the next N days.
        /// </summary>
        NOT_NEXT_N_DAYS,

        /// <summary>
        /// Has any of the specified values (multi-value columns).
        /// </summary>
        HAS_ANY_OF,

        /// <summary>
        /// Has none of the specified values (multi-value columns).
        /// </summary>
        HAS_NONE_OF,

        /// <summary>
        /// Has all of the specified values (multi-value columns).
        /// </summary>
        HAS_ALL_OF,

        /// <summary>
        /// Does not have all of the specified values (multi-value columns).
        /// </summary>
        NOT_ALL_OF,

        /// <summary>
        /// Multi-value is equal to.
        /// </summary>
        MULTI_IS_EQUAL,

        /// <summary>
        /// Multi-value is not equal to.
        /// </summary>
        MULTI_IS_NOT_EQUAL
    }
}
