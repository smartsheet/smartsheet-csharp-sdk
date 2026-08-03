# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).
## [X.X.X] - Unreleased

### Added
- `Proof` model and `ProofType` enum
- `Proof` property on rows returned by `GetSheet` and `GetReport`
- `PROOFS` include value for `SheetLevelInclusion`, `ReportInclusion`, and `RowInclusion`

### Fixed
- `COMMENTER` value for `AccessLevel`, which previously deserialized to `null` on sheets, reports, workspaces, and shares, and could not be used to create or update a share. Fixes [#218](https://github.com/smartsheet/smartsheet-csharp-sdk/issues/218)

## [7.3.0] - 2026-07-20

### Added
- `ChildResourceType` enum 
- Test cases for a template resource type

### Fixed
- `NullReferenceException` when the API returns an error response with an empty or body-less payload (e.g. `GetSheetAsCSV` with a low-privilege token); the SDK now raises a `SmartsheetException` instead. Fixes [#211](https://github.com/smartsheet/smartsheet-csharp-sdk/issues/211)

## [7.2.0] - 2026-07-09

### Added
- Support for GET /2.0/reports/{reportId}/definition (Get Report Definition) via `ReportResources.GetReportDefinition`
- Support for GET /2.0/reports/{reportId}/columns (List Report Columns) via `ReportResources.ListReportColumns`
- Support for GET /2.0/reports/{reportId}/columns/{columnVirtualId} (Get Report Column) via `ReportResources.GetReportColumn`
- Support for PUT /2.0/reports/{reportId}/columns/{columnVirtualId} (Update Report Column) via `ReportResources.UpdateReportColumn`
- Support for DELETE /2.0/reports/{reportId}/columns/{columnVirtualId} (Delete Report Column) via `ReportResources.DeleteReportColumn`
- Support for GET /2.0/reports/{reportId}/scope (List Report Scope) via `ReportResources.GetReportScope`

### Deprecated

- Deprecated `Event.ObjectId`; use `Event.ObjectIdStr` instead. `ObjectId` is numeric only and returns -1 for non-numeric identifiers. It is not scheduled for removal.

## [7.1.0] - 2026-06-26

### Fixed

- Deprecation related corrections

### Added

- Hardcode `paginationType=token` for `listWorkspaces`.
- Added support for GET /2.0/sheets/{sheetId}/path endpoint (`GetSheetPath`)
- Added support for GET /2.0/reports/{reportId}/path endpoint (`GetReportPath`)
- Added support for GET /2.0/sights/{sightId}/path endpoint (`GetSightPath`)
- Added support for GET /2.0/folders/{folderId}/path endpoint (`GetFolderPath`)
- Added helper methods `GetLeaf<Asset>()` and `GetLeaf<Asset>Path()` to the responses of the path endpoints for convenient traversal
- Added asynchronous (`*Async`) counterparts to `SheetResources.RowResources`: `AddRowsAsync`, `AddRowsAllowPartialSuccessAsync`, `GetRowAsync`, `CopyRowsToAnotherSheetAsync`, `DeleteRowsAsync`, `MoveRowsToAnotherSheetAsync`, `SendRowsAsync`, `CreateResourceAsync`, `UpdateRowsAsync`, and `UpdateRowsAllowPartialSuccessAsync`.
- Added asynchronous (`*Async`) counterparts to `ReportResources`: `GetReportAsync`, `ListReportsAsync`, `SendReportAsync`, `DeleteReportAsync`, `GetPublishStatusAsync`, `UpdatePublishStatusAsync`, `UpdateReportDefinitionAsync`, `AddReportScopeAsync`, `RemoveReportScopeAsync`, `AddReportColumnsAsync`, `CreateReportAsync`, and `GetReportPathAsync`.

## [7.0.0] - 2026-06-08

### Added
- Added `ObjectIdStr` property to `Event` model to support alphanumeric object identifiers (AUD-905)
- AI assisted workflows via claude skills (`implement-api-endpoint` and `review-api-endpoint`).
- ⚠️ **BREAKING**: Added `RequestAsync()` to `HttpClient` interface, which must be overridden in custom HttpClient implementations to affect async resource methods.
- Added asynchronous (`*Async`) counterparts to `SheetResources`: `ListSheetsAsync`, `GetSheetAsync`, `GetSheetVersionAsync`, `CreateSheetFromTemplateAsync`, `CopySheetAsync`, `MoveSheetAsync`, `UpdateSheetAsync`, `DeleteSheetAsync`, `SortSheetAsync`, `SendSheetAsync`, `GetPublishStatusAsync`, and `UpdatePublishStatusAsync`.

### Removed
- ⚠️ **BREAKING**: Removed deprecated `ListSights(PaginationParameters?, DateTime?)` overload and `modifiedSince` parameter from `ListSights`. These were [deprecated by the Smartsheet API](https://developers.smartsheet.com/api/smartsheet/changelog#deprecated-includeall-and-offset-based-pagination-for-dashboards) (sunset Jun-03-2026). `ListSights` now accepts only `TokenPaginationParameters?` and returns `TokenPaginatedResult<Sight>`. The response no longer includes `TotalCount`, `TotalPages`, `PageNumber`, or `PageSize`.
- ⚠️ **BREAKING**: Removed deprecated `ListWorkspaces(PaginationParameters?)` overload returning `PaginatedResult<Workspace>`. These offset parameters were [deprecated by the Smartsheet API](https://developers.smartsheet.com/api/smartsheet/changelog#2025-08-04) (sunset Jun-03-2026). `ListWorkspaces` now accepts only `TokenPaginationParameters?` and returns `TokenPaginatedResult<Workspace>`. The new shape mirrors `ListSights`.
- ⚠️ **BREAKING**: Removed `ListWorkspacesTokenPaginationParameters` class. It only added an explicit `PaginationType` property that is already hardcoded to `"token"` in the base `TokenPaginationParameters.toDictionary()`. Use `TokenPaginationParameters` directly with `ListWorkspaces`.
- ⚠️ **BREAKING**: Removed `ListPublicTemplates` and `ListUserCreatedTemplates` from `TemplateResources`. The `TemplateResources` interface, implementation, and `SmartsheetClient.TemplateResources` accessor have been removed entirely. The underlying `GET /templates` and `GET /templates/public` endpoints were [deprecated by the Smartsheet API](https://developers.smartsheet.com/api/smartsheet/changelog#2025-08-04) (sunset Jun-03-2026). Migrate to `GetWorkspaceChildren` / `GetFolderChildren` with `ChildrenResourceTypes` including `TEMPLATES,SHEETS` to list templates within a specific workspace or folder.
- ⚠️ **BREAKING**: Removed `GetFolder` and `ListFolders` from `FolderResources`, `GetWorkspace` from `WorkspaceResources`, and `ListFolders` from `WorkspaceFolderResources`. (`WorkspaceFolderResources.CreateFolder` is retained.) `FolderInclusion` and `WorkspaceInclusion` no longer include `OWNER_INFO` or `SHEET_VERSION`; only `SOURCE` is retained. The underlying `GET /folders/{folderId}`, `GET /folders/{folderId}/folders`, `GET /workspaces/{workspaceId}`, and `GET /workspaces/{workspaceId}/folders` endpoints were [deprecated by the Smartsheet API](https://developers.smartsheet.com/api/smartsheet/changelog#2025-08-04) (sunset Jun-03-2026). Migrate to `GetFolderMetadata` + `GetFolderChildren` and `GetWorkspaceMetadata` + `GetWorkspaceChildren`. Use `ChildrenResourceTypes` to filter the children response (e.g., `FOLDERS` to replicate the old list-folders behavior).
- ⚠️ **BREAKING**: Removed the deprecated `ShareResources` interface and the `ShareResources()` accessor from `SheetResources`, `ReportResources`, `SightResources`, and `WorkspaceResources`. The underlying asset-specific sharing endpoints (`GET`/`POST`/`PUT`/`DELETE` on `/sheets/{id}/shares`, `/reports/{id}/shares`, `/sights/{id}/shares`, `/workspaces/{id}/shares` and their `/{shareId}` variants) were [deprecated by the Smartsheet API](https://developers.smartsheet.com/api/smartsheet/changelog#2025-08-04) (sunset Jun-03-2026). Migrate to `AssetSharingResources` (`ListAssetShares`, `GetAssetShare`, `ShareAsset`, `UpdateAssetShare`, `DeleteAssetShare`), passing `AssetType` and `assetId`. Note updates now use `PATCH` instead of `PUT`.

### Changed
- `ListWebhooks` XML documentation updated to reflect Smartsheet API behavior changes effective Jun-03-2026: `includeAll` is no longer honored by the server for this endpoint and is ignored if set on `PaginationParameters` (`PaginationParameters` remains a shared class — other endpoints still support `includeAll`), `PageSize` is server-capped at 10,000, `TotalCount` and `TotalPages` are returned as `-1`, and webhooks are sorted by creation date (most recent first) instead of name. SDK signature unchanged. See [Smartsheet API changelog 2025-08-04](https://developers.smartsheet.com/api/smartsheet/changelog#2025-08-04).
- ⚠️ **BREAKING**: Change `DefaultHttpClient.RetrySleep()` into an async method that returns a boolean wrapped in a Task

## [6.7.0] - 2026-04-30
### Added
- Add support for PUT /reports/{id}/definition endpoint, Update Report Definition
- Add 'Add Report Scope' and 'Remove Report Scope' endpoint support to ReportResources
- Add 'Add Report Columns' endpoint support to ReportResources (POST /2.0/reports/{reportId}/columns)
- Add support for DELETE /reports/{id} endpoint, Delete Report
- Add 'Create Report' endpoint support to ReportResources (POST /2.0/reports)
- Support for CONTRIBUTOR seat type in SeatType enum
- Support for CONTRIBUTOR seat type in DowngradeSeatType enum
- Support for `displayContributorSeatType` query parameter in GET /2.0/users endpoint
- Support for `displayContributorSeatType` query parameter in GET /2.0/users/{userId}/plans endpoint
- WireMock integration tests for `displayContributorSeatType` parameter functionality
- Stream-based attachment upload support. Added `AttachFile` and `AttachNewVersion` method overloads that accept `Stream` parameters for `RowAttachmentResources`, `SheetAttachmentResources`, `CommentAttachmentResources`, and `AttachmentVersioningResources`. This enables direct upload from memory without requiring temporary files. Fixes [#178](https://github.com/smartsheet/smartsheet-csharp-sdk/issues/178)


### Refactor
- Resolved double assignment of parameter in AbstractResources.cs
- Refactored ObjectValueTypeConverter and FilterValueTypeConverter to extend a common PrimitiveValueConverter base class, reducing code duplication for primitive type handling (string, number, boolean, null)

## [6.6.6] - 2026-03-25
### Fixed
- Remove redundant http client and json serializer creation. Fixes [#168](https://github.com/smartsheet/smartsheet-csharp-sdk/issues/168)

### Added
- `AssetSharingResources.ShareAsset` now uses a separate DTO (`CreateShareRequest`) for asset sharing. Fixes [#166](https://github.com/smartsheet/smartsheet-csharp-sdk/issues/166)

## [6.6.5] - 2026-02-04
### Fixed
- Fix retry logic. Previously the SDK's retry logic was hindered by a generic error check and as a result the code could never reach the retry part.
- Fix user agent for default HTTP client.

## [6.6.4] - 2026-01-19
### Fixed
- Revert the default float parsing handler to double - [#157](https://github.com/smartsheet/smartsheet-csharp-sdk/pull/157)

## [6.6.3] - 2025-12-23
### Added
- Added `SetEnableDecimalObjectValue` method to `SmartsheetBuilder` to enable opt-in support for `DecimalObjectValue` when deserializing numeric cell values, preserving full decimal precision instead of converting to `double`. See [ADVANCED.md](ADVANCED.md#preserving-decimal-precision-with-decimalobjectvalue) for usage details and important considerations.

### Fixed
- Reverted [#134](https://github.com/smartsheet/smartsheet-csharp-sdk/pull/134) because it introduced breaking changes. The addition above adds support for decimal conversion with an opt-in flag.

## [6.6.2] - 2025-12-12
### Fixed
- Fixed "Unknown resourceType" exception when template resources are returned in folder/workspace children endpoints by adding template support to `ChildResource.ConvertToSpecificType()` method
- Added `TEMPLATES` enum value to `ChildrenResourceType` for consistency with supported child resource types

## [6.6.1] - 2025-12-10
### Fixed
- Fix `DateTime` conversions in the ChildResource class

## [6.6.0] - 2025-12-04
### Added
- Support for POST /2.0/users/{userId}/reactivate endpoint
- Support for POST /2.0/users/{userId}/deactivate endpoint
### Updated
- Update csproj file for backwards compatibility

### Fixed
- Serialization of float numbers to double caused precision loss. Switched to decimal.
- Renamed `IncludeColumnIds` to `IncludedColumnIds` in [`AutomationAction`](smartsheet-csharp-sdk/main/Smartsheet/Api/Models/AutomationAction.cs) class to match Smartsheet API specification and align with Python and Java SDK implementations. Fixes [issue #32](https://github.com/smartsheet/smartsheet-csharp-sdk/issues/32).

## [6.5.0] - 2025-11-25
### Added
- WireMock integration tests for contract testing for GET /2.0/users/{userId}/plans and GET /2.0/users endpoints
- WireMock integration tests for contract testing for POST /2.0/users/{userId}/plans/{planId}/upgrade and POST /2.0/users/{userId}/plans/{planId}/downgrade
- WireMock integration tests for contract testing for DELETE /2.0/users/{userId}/plans/{planId} endpoint
- Added new `AssetShare` class
- Added new asset-based sharing endpoints through `SharingResources` interface
- Added `AssetType` enum to support multiple asset types (sheets, reports, sights, workspaces)
- Added methods for listing, getting, creating, updating, and deleting shares for any asset type

### Updated
- Folder structure for the Users related WireMock tests
- Replaced `Share` in `AssetSharingResources` classes with `AssetShare`

### Changed
- Deprecated old asset-specific sharing methods in `ShareResources` with notices to use the new asset-based methods

### Removed
- Remove integration tests from the sdk test suite and workflows

### Fixed
- Fix [issue #128](https://github.com/smartsheet/smartsheet-csharp-sdk/issues/128) - in the implementation of the DefaultHttpClient class that recreates the httpClient (RestClient) instance on every request

## [6.4.0] - 2025-10-27
### Added
- Add provisionalExpirationDate field to the User and UserPlan models

## [6.3.0] - 2025-09-25
### Added
- Support for POST /2.0/users/{userId}/plans/{planId}/downgrade
- Support for POST /2.0/users/{userId}/plans/{planId}/upgrade
- Support for GET /2.0/users/{userId}/plans
- Support for GET /2.0/users?planId={planId}&seatType={seatType}
- Support for DELETE /2.0/users/{userId}/plans/{planId}

## [5.1.0] - 2025-08-25
### Added
- Add support for token-based pagination in WorkspaceResources.ListWorkspaces()
- New TokenPaginationParameters class to support paginationType, lastKey and maxItems parameters
- New TokenPaginatedResult<T> class for token-based pagination responses with data and lastKey properties
- GetWorkspaceChildren endpoint with support for token-based pagination
- GetWorkspaceMetadata endpoint with support for numericDates and accessApiLevel parameters
- GetFolderChildren endpoint with support for token-based pagination
- GetFolderMetadata endpoint with support for numericDates parameter

### Changed
- Updated Folder model with CreatedAt and ModifiedAt properties supporting both DateTime and Long values

### Deprecated
- GetFolder method in FolderResources (use GetFolderChildren and GetFolderMetadata instead)
- GetWorkspace method in WorkspaceResources (use GetWorkspaceChildren and GetWorkspaceMetadata instead)
- ListFolders method in FolderResources (use GetFolderChildren instead)
- ListFolders method in WorkspaceFolderResources (use GetWorkspaceChildren instead)
- HomeFolderResources interface and all its methods (See the API docs article on [migrating off the Sheets folder](https://developers.smartsheet.com/api/smartsheet/guides/updating-code/migrate-from-using-the-sheets-folder))

## [3.0.0] - 2022-12-07
### Updated
- Migrated SDK to new project
- Migrated SDK to .Net 6.0

### Added
- Add Github Actions pipeline

## [2.126.0] - 2021-05-24
### Added
- add support for column formulas

## [2.101.0] - 2020-07-28
### Fixed
- [image.id versus image.imageId #119](https://github.com/smartsheet-platform/smartsheet-csharp-sdk/issues/119)

## [2.93.2] - 2020-06-11
### Fixed
- [Double url escaping for the search api call #120](https://github.com/smartsheet-platform/smartsheet-csharp-sdk/issues/120)

## [2.93.1] - 2020-03-24
### Fixed
- [Unable to Search Sheet Summary #117](https://github.com/smartsheet-platform/smartsheet-csharp-sdk/issues/117)

## [2.93.0] - 2020-03-12
### Added
- Webhooks for columns support

### Fixed
- [Json deserialization error #113](https://github.com/smartsheet-platform/smartsheet-csharp-sdk/issues/113)

### Changed
- disable Newtonsoft default configuration of deserializing strings that "look like" dates into C# DateTime objects,
see [README](https://github.com/smartsheet-platform/smartsheet-csharp-sdk/blob/master/README.md) for details on how
to opt-out of this change if required.

## [2.86.0] - 2019-11-07
### Added
- type and object definitions to support multi-picklist columns

### Changed
- dashboard widget model to support widgets that are in an error state
- additions to CellDataItem widget contents to support METRIC widgets containing sheet summary fields

## [2.83.0] - 2019-08-23
### Added
- support for sheet profiles
- include format and objectValue in the includes for GetCellHistory (Issue #109)

### Changed
- continue to support level 0 widget types

## [2.77.0] - 2019-07-25
### Added
- CARD_DONE tag to column tags enumeration
- `Description` property to Column model
- ListUsers accepts an `includes` parameter - the only currently accepted argument value is `LAST_LOGIN`
- 'DateFormat' property to FormatTables model
- `SOURCE` as include flag to GetSight
- `SOURCE` and `SCOPE` as include flags to GetReport

### Changed
- Significant overhaul to Sights (AKA dashboards) - added separate content models for each widget type
- Started a revamp of ITs to provide better test coverage

## [2.68.3] - 2019-06-21
### Added
- rules, ruleRecipients and shares to inclusions
- exclusion to CopySheet method

## [2.68.2] - 2019-05-29
### Added
- Support for .NET Standard 2.0. Nuget.org package contains assemblies for both .NET Framework 4.5.2 and
.NET Standard 2.0.

## [2.68.1] - 2019-05-15
### Fixed
- Added missing public class declarations for some of the ObjectValue types

## [2.68.0] - 2019-05-13
### Added
- Implement Event Reporting

## [2.6.0] - 2019-01-31
### Added
- Added group inclusion to GetCurrentUser
- Added BASE URI definition for Smartsheetgov

### Fixed
- Fixed GetRow to process include and exclude parameters

## [2.5.0] - 2018-12-11
### Added
- Added opt-in for TLS 1.2. TLS 1.0 will be disabled soon for Smartsheet API.

## [2.4.0] - 2018-11-28
### Added
- WebContent as a valid widget type for Sights
- Missing Workspace item to Sheet model
- Support for Multi-Assign feature
- deleteAllForApiClient parameter to OAuth revoke

### Changed
- ProjectSettings nonWorkingDays should be a string (modified from DateTime)

## [2.3.0] - 2018-04-18
### Added
- [Automation rules](http://smartsheet-platform.github.io/api-docs/?shell#automation-rules)
- [Cross sheet references](http://smartsheet-platform.github.io/api-docs/?shell#cross-sheet-references)
- Added import API endpoints to allow SDK users to import Sheets from XLSX and CSV files
- Data validation (more information about cell value parsing including validation can be found [here](http://smartsheet-platform.github.io/api-docs/#cell-reference))
- Passthrough mechanism to pass raw JSON requests through to the API (documented in README/Advanced Topics)
- Sheet filter implementation
- Row sort feature
- User profile properties (including profileImage) to UserModel
- Scope, location, and favoriteFlag inclusion to search
- getSheet() ifVersionAfter parameter
- Expose Change-Agent, Assumed-User, and User-Agent on Smartsheet client
- Bulk access to sheet version through sheetVersion inclusion
- Missing report and sheet publish flags
- Missing title widget for Sights
- Deserialization of error detail
- Cleaned up logging for binary HTTP entities
- Methods to clear hyperlink and cellLink (examples can be found in the `RowTests.cs` mock tests)

### Changed
- Implementation of objectValue to better support PredecessorList and objectValue primitives (examples of how to set and clear Predecessor list can be found in the `RowTests.cs` mock tests)
- HttpClient interface to allow SDK users to inject HTTP headers or implement an HTTP proxy by extending
DefaultHttpClient (a proxy sample is provided in the Advanced Topics section of the README)
- Removed outdated Link model and replaced all references with current Hyperlink model
- Removed ShouldRetry and CalcBackoff interfaces and replaced with HttpClient interface methods. You can now customize
shouldRetry or calcBackoff using the same method as proxy or request header injection (i.e., extend DefaultHttpClient).

### Fixed
- Several deserialization issues with Sights
- Changed Duration values to floats to be consistent with the API
- Don't modify comments array when setComment is called for outbound comment
- Don't attempt to rety non-JSON responses

## Earlier releases
- Documented in [Github releases page](https://github.com/smartsheet-platform/smartsheet-csharp-sdk/releases)
