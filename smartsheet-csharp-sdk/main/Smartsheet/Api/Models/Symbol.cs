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
    /// Represents the column symbols. </summary>
    /// <see href="http://help.Smartsheet.com/customer/portal/articles/504619-column-types#symbols">Symbols Help</see>
    public enum Symbol
    {
        /// <summary>
        /// Flag to check on/off to represent status
        /// </summary>
        FLAG,
        /// <summary>
        /// Star to signify favorite/important rows
        /// </summary>
        STAR,
        /// <summary>
        /// Circular graphics that are divided into segments to signify completion of a goal or task
        /// </summary>
        HARVEY_BALLS,
        /// <summary>
        /// Symbols to signify low or high priority
        /// </summary>
        PRIORITY,
        /// <summary>
        /// Red, yellow, green for decisions
        /// </summary>
        RYG,
        /// <summary>
        /// Symbols to signify high, medium, or low priority
        /// </summary>
        PRIORITY_HML,
        /// <summary>
        /// Yes (green check), hold (yellow exclamation) and no (red x)
        /// </summary>
        DECISION_SYMBOLS,
        /// <summary>
        /// Green circle, yellow triangle, and red circle
        /// </summary>
        DECISION_SHAPES,
        /// <summary>
        /// VCR buttons: Stop, Rewind, Play, Fast Forward, Pause  
        /// </summary>
        VCR,
        /// <summary>
        /// Red, yellow, green, blue
        /// </summary>
        RYGB,
        /// <summary>
        /// Red, yellow, green, gray
        /// </summary>
        RYGG,
        /// <summary>
        /// Sunny, Partly Sunny, Cloudy, Rainy, Stormy   
        /// </summary>
        WEATHER,
        /// <summary>
        /// Progress bar symbol: Empty, Quarter, Half, Three Quarter, Full   
        /// </summary>
        PROGRESS,
        /// <summary>
        /// Down, Sideways, Up   
        /// </summary>
        ARROWS_3_WAY,
        /// <summary>
        /// Down, Angle Down, Angle Up, Up   
        /// </summary>
        ARROWS_4_WAY,
        /// <summary>
        /// Down, Angle Down, Sideways, Angle Up, Up 
        /// </summary>
        ARROWS_5_WAY,
        /// <summary>
        /// Up, Unchanged, Down   
        /// </summary>
        DIRECTIONS_3_WAY,
        /// <summary>
        /// Down, Right, Up, Left 
        /// </summary>
        DIRECTIONS_4_WAY,
        /// <summary>
        /// Easy (green circle), Intermediate (blue square), Advanced (black diamond), Expert Only (double black diamond)   
        /// </summary>
        SKI,
        /// <summary>
        /// Like wifi signal: Empty, Quarter, Half, Three Quarter, Full   
        /// </summary>
        SIGNAL,
        /// <summary>
        /// Empty, One, Two, Three, Four, Five  (stars)
        /// </summary>
        STAR_RATING,
        /// <summary>
        /// Empty, One, Two, Three, Four, Five  (hearts)
        /// </summary>
        HEARTS,
        /// <summary>
        /// Empty, One, Two, Three, Four, Five (dollar signs)
        /// </summary>
        MONEY,
        /// <summary>
        /// Empty, One, Two, Three, Four, Five  (# of people symbols)
        /// </summary>
        EFFORT,
        /// <summary>
        /// No Pain, Mild, Moderate, Severe, Very Severe, Extreme (Smiley faces)
        /// </summary>
        PAIN
    }
}