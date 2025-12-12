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
using Smartsheet.Api.Internal.Util;

namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Unified class for deserializing children resources based on the OpenAPI specification.
    /// Contains properties returned by the /children endpoints before converting to the
    /// appropriate specific type based on resourceType.
    /// </summary>
    public class ChildResource : NamedModel
    {
        /// <summary>
        /// The resource type (sheet, report, sight, folder)
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// The user's access level for the resource
        /// </summary>
        public AccessLevel? AccessLevel { get; set; }

        /// <summary>
        /// The resource permalink
        /// </summary>
        public string Permalink { get; set; }

        /// <summary>
        /// The resource creation date
        /// Either a DateTime object, or Long if numericDates parameter is true on API call.
        /// </summary>
        public object CreatedAt { get; set; }

        /// <summary>
        /// The resource last updated date
        /// Either a DateTime object, or Long if numericDates parameter is true on API call.
        /// </summary>
        public object ModifiedAt { get; set; }

        /// <summary>
        /// Data on the source object from which this asset was instantiated.
        /// Only present if 'source' is listed for the 'include' query parameter.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// The ID of the owner, or the user with admin permissions if there is no owner assigned.
        /// Only present for sheets if 'ownerInfo' is listed for the 'include' query parameter.
        /// </summary>
        public long? OwnerId { get; set; }

        /// <summary>
        /// The email address of the owner, or the user with admin permissions if there is no owner assigned.
        /// Only present for sheets if 'ownerInfo' is listed for the 'include' query parameter.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Converts a ChildResource to the appropriate specific type based on resourceType
        /// </summary>
        /// <param name="childResource">The ChildResource to convert</param>
        /// <returns>The converted object (Sheet, Report, Sight, Folder, or Template)</returns>
        public static object ConvertToSpecificType(ChildResource childResource)
        {
            if (childResource == null)
                return null;

            switch (childResource.ResourceType?.ToLower())
            {
                case "sheet":
                    return ConvertToSheet(childResource);
                case "report":
                    return ConvertToReport(childResource);
                case "sight":
                    return ConvertToSight(childResource);
                case "folder":
                    return ConvertToFolder(childResource);
                case "template":
                    return ConvertToTemplate(childResource);
                default:
                    throw new InvalidOperationException($"Unknown resourceType: {childResource.ResourceType}");
            }
        }

        /// <summary>
        /// Converts a ChildResource to a Sheet
        /// </summary>
        private static Sheet ConvertToSheet(ChildResource childResource)
        {
            return new Sheet
            {
                Id = childResource.Id,
                Name = childResource.Name,
                AccessLevel = childResource.AccessLevel,
                Permalink = childResource.Permalink,
                CreatedAt = DateTimeConverter.ConvertToDateTime(childResource.CreatedAt),
                ModifiedAt = DateTimeConverter.ConvertToDateTime(childResource.ModifiedAt),
                Source = childResource.Source,
                OwnerId = childResource.OwnerId,
                Owner = childResource.Owner
            };
        }

        /// <summary>
        /// Converts a ChildResource to a Report
        /// </summary>
        private static Report ConvertToReport(ChildResource childResource)
        {
            return new Report
            {
                Id = childResource.Id,
                Name = childResource.Name,
                AccessLevel = childResource.AccessLevel,
                Permalink = childResource.Permalink,
                CreatedAt = DateTimeConverter.ConvertToDateTime(childResource.CreatedAt),
                ModifiedAt = DateTimeConverter.ConvertToDateTime(childResource.ModifiedAt),
                Source = childResource.Source
            };
        }

        /// <summary>
        /// Converts a ChildResource to a Sight
        /// </summary>
        private static Sight ConvertToSight(ChildResource childResource)
        {
            return new Sight
            {
                Id = childResource.Id,
                Name = childResource.Name,
                AccessLevel = childResource.AccessLevel,
                Permalink = childResource.Permalink,
                CreatedAt = DateTimeConverter.ConvertToDateTime(childResource.CreatedAt),
                ModifiedAt = DateTimeConverter.ConvertToDateTime(childResource.ModifiedAt),
                Source = childResource.Source
            };
        }

        /// <summary>
        /// Converts a ChildResource to a Folder
        /// </summary>
        private static Folder ConvertToFolder(ChildResource childResource)
        {
            return new Folder
            {
                Id = childResource.Id,
                Name = childResource.Name,
                Permalink = childResource.Permalink,
                CreatedAt = childResource.CreatedAt,
                ModifiedAt = childResource.ModifiedAt,
                Source = childResource.Source
            };
        }

        /// <summary>
        /// Converts a ChildResource to a Template
        /// </summary>
        private static Template ConvertToTemplate(ChildResource childResource)
        {
            return new Template
            {
                Id = childResource.Id,
                Name = childResource.Name,
                AccessLevel = childResource.AccessLevel
            };
        }
    }
}
