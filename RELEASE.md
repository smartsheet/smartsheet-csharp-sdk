# Release Procedure

This document is the single source of truth for releasing the Smartsheet C# SDK. Publishing to NuGet is automated via GitHub Actions, but the version bump and changelog update are done by hand.

## Overview

Releases follow [Semantic Versioning](https://semver.org/) and [Keep a Changelog](https://keepachangelog.com/) conventions. Every release consists of:

1. A "Prepare for release" PR that bumps the version and closes out the changelog.
2. A merged commit on `mainline` published as a GitHub Release (which also creates the tag).
3. Automated publishing to NuGet triggered by the GitHub Release event.

## Prerequisites

- Write access to the `smartsheet/smartsheet-csharp-sdk` repository.
- NuGet credentials are not needed locally — publishing uses a `NUGET_TOKEN` stored as a GitHub Actions secret.

## Step-by-Step Process

### 1. Decide the version bump

Review the `## [X.X.X] - Unreleased` section in `CHANGELOG.md` and apply semver rules:

| Change type | Bump |
| --- | --- |
| New endpoints, non-breaking additions | `minor` |
| Bug fixes, dependency updates | `patch` |
| Breaking API changes, removed types/methods | `major` |

### 2. Create a "Prepare for release" pull request

Open a branch from `mainline` (e.g., `release/v7.2.0`) and make the following changes:

#### a. Update `CHANGELOG.md`

Feature PRs accumulate entries under `## [X.X.X] - Unreleased`. For the release, insert the new versioned header between that placeholder and its content:

```diff
 ## [X.X.X] - Unreleased

+## [7.2.0] - 2026-07-15
+
 ### Added
```

The `## [X.X.X] - Unreleased` placeholder header is never removed — it stays at the top of the file permanently so future PRs have somewhere to add entries.

#### b. Update `smartsheet-csharp-sdk/smartsheet-csharp-sdk-v2.csproj`

```diff
-    <Version>7.1.0</Version>
+    <Version>7.2.0</Version>
```

#### c. PR title convention

```
Prepare for release vX.X.X
```

Example: `Prepare for release v7.2.0`

### 3. Merge the PR

CI must pass before merging. The `main.yml` workflow runs the build and mock API tests.

### 4. Create and publish the GitHub Release

1. Go to **Releases → Draft a new release** in the GitHub UI.
2. In the **Choose a tag** field, type the new version (e.g. `v7.2.0`) and select **Create new tag on publish**.
3. Set the title to `v7.2.0`.
4. Set the description to the changelog entries for this version (copy from `CHANGELOG.md`).
5. Click **Publish release**.

The tag is created automatically when the release is published — no separate `git tag` step needed.

Publishing the release (not just saving as a draft) triggers two workflows:

- **publish.yml**: restores dependencies, builds, runs mock API tests, packages with `dotnet pack`, and publishes to NuGet using the `NUGET_TOKEN` secret.
- **docs.yml**: generates DocFX documentation and deploys to GitHub Pages.

### 5. Verify the publish workflow

Go to [Workflow runs](https://github.com/smartsheet/smartsheet-csharp-sdk/actions) and confirm both the `Publish Nuget Package` and docs jobs succeeded.

### 6. Verify on NuGet

```
https://www.nuget.org/packages/smartsheet-csharp-sdk/7.2.0
```

The new version should appear shortly after the workflow succeeds. NuGet indexing can take a few minutes.

## Files Changed in Every Release

| File | What changes |
| --- | --- |
| `CHANGELOG.md` | New versioned header inserted below the permanent `Unreleased` placeholder |
| `smartsheet-csharp-sdk/smartsheet-csharp-sdk-v2.csproj` | `<Version>` field bumped |

## Automation

Publishing is fully automated once the GitHub Release is published:

```
GitHub Release (published) → publish.yml → dotnet pack → dotnet nuget push → NuGet
                           → docs.yml → DocFX → GitHub Pages
```

The workflow uses a `NUGET_TOKEN` GitHub Actions secret — no local credentials needed.

## Troubleshooting

**CI fails on the PR**

Check the `main.yml` run for build or mock API test failures.

**Publish workflow fails after the release is published**

The tag and GitHub Release already exist — do not delete them. Instead:

1. Investigate the failure in the Actions log (common cause: expired `NUGET_TOKEN`).
2. Re-trigger via **Actions → Publish Nuget Package → Run workflow** (the workflow supports `workflow_dispatch`) after fixing the root cause.
3. If the root cause requires a code fix, cut a patch release instead.

## Rollback

NuGet does not support deleting published versions. If a bad release ships:

1. Publish a patch release immediately with the fix.
2. Unlist the bad version via the NuGet UI (unlisted versions still exist but are hidden from search and `dotnet add package` without an explicit version pin).

## Checklist

- [ ] Determined correct semver bump
- [ ] `CHANGELOG.md` versioned header inserted below the permanent `Unreleased` placeholder
- [ ] `smartsheet-csharp-sdk-v2.csproj` `<Version>` bumped
- [ ] PR title: `Prepare for release vX.X.X`
- [ ] CI passes on the PR
- [ ] PR merged to `mainline`
- [ ] GitHub Release created: tag `vX.X.X` set to **Create new tag on publish**, description set to changelog entries, **published** (not draft)
- [ ] `Publish Nuget Package` and docs workflow jobs verified as succeeded
- [ ] Version confirmed on NuGet: `https://www.nuget.org/packages/smartsheet-csharp-sdk/X.X.X`
