# AI Agent Workflows

This repository uses workflow-specific agents to handle different phases of SDK development. Each agent is backed by a detailed skill file that defines the workflow, and this document provides the project-specific context agents need to work effectively in this codebase.

## Available Agents

- **Implementation Agent** - Adding or modifying API endpoints
- **Review Agent** - Reviewing endpoint implementations before merge
- **Release Agent** - Cutting a new SDK release (version bump, changelog, tag, GitHub Release)

---

## Implementation Agent

### Purpose

Adding or modifying Smartsheet API endpoints in the C# SDK.

### Skill Reference

**Skill file:** `.claude/skills/implement-api-endpoint/SKILL.md`

### When to Use

Use the Implementation Agent when:
- User requests endpoint implementation
- Adding a new API endpoint to the SDK
- Modifying existing endpoint behavior

Do NOT use for:
- Bug fixes in existing endpoints (unless spec changed)
- Refactoring without behavior changes
- Documentation-only changes

---

## Review Agent

### Purpose

Systematic code review for API endpoint implementations before merge.

### Skill Reference

**Skill file:** `.claude/skills/review-api-endpoint/SKILL.md`

### When to Use

Use the Review Agent when:
- Reviewing a pull request with endpoint changes
- Verifying endpoint implementation before merge
- Quality check before release

Do NOT use for:
- Non-endpoint code reviews
- Documentation-only changes

---

## Release Agent

### Purpose

Cutting a new SDK release: determining the correct semver bump, updating the changelog, bumping the version in the `.csproj`, creating the release PR, tagging, and publishing via GitHub Release.

### Skill Reference

**Skill file:** `.claude/skills/releasing-smartsheet-csharp-sdk/SKILL.md`

**Full procedure:** `RELEASE.md` in the repository root — single source of truth for every step, decision rule, and checklist item.

### When to Use

Use the Release Agent when:
- User asks to cut a release or publish a new version
- Accumulated changes on `mainline` need to be shipped
- A hotfix needs to be released urgently

Do NOT use for:
- Implementing features or fixing bugs (merge those first)
- CI or tooling changes without a version bump

---

## Project-Specific Context

This section provides shared context that applies to all agents working in this repository.

### Repository Overview

**Name:** Smartsheet C# SDK

**Purpose:** Client library for the Smartsheet REST API, enabling .NET applications to interact with Smartsheet programmatically.

**Build tool:** .NET / MSBuild (`dotnet`)

**Target framework:** .NET 8.0

**Version file:** `smartsheet-csharp-sdk/smartsheet-csharp-sdk-v2.csproj` (`<Version>` field)

### Key Documentation Files

| File | Purpose | When to Read |
|------|---------|--------------|
| `README.md` | Installation, basic usage, example code | Getting started |
| `ADVANCED.md` | SDK architecture and patterns | Implementing endpoints |
| `TESTING.md` | Test structure and standards | Writing tests |
| `RELEASE.md` | Release procedure, version bump, changelog, tagging | Cutting a new SDK release |
