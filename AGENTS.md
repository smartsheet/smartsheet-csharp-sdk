# AI Agent Workflows

This repository uses workflow-specific agents to handle different phases of SDK development.

## Available Agents

- **Release Agent** - Cutting a new SDK release (version bump, changelog, tag, GitHub Release)

---

## Release Agent

### Purpose

Cutting a new SDK release: determining the correct semver bump, updating the changelog, bumping the version in `smartsheet-csharp-sdk-v2.csproj`, creating the release PR, tagging, and publishing via GitHub Release.

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
