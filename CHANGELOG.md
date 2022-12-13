# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

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
