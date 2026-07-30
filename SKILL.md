[//]: # (Source of truth: .ai/base-instructions.md + .ai/stacks/dotnet-library.md — update those, then regenerate by re-running /sync-ai-instructions)

# SKILL.md — OpenClaw Agent Skill

This skill configures OpenClaw for this project.

# AI Agent Base Instructions

Canonical, **stack-agnostic** reference for all AI coding agents. Applies to every project regardless of language or framework. Stack-specific overlays live in `.ai/stacks/<stack>.md` and are loaded alongside this file. A project loads **base + exactly one stack overlay**. Tool-specific files (`CLAUDE.md`, `.github/copilot-instructions.md`, `SKILL.md`) derive from base + the chosen stack.

> **Workflow role:** If a `WORKFLOW-ROLE.md` exists at the repo root, read it before continuing — it describes this repo's place in the personal dev workflow (implementer / consumer / workflow infrastructure). See `ai-instructions/workflows/personal-dev-workflow.md` for the workflow doc itself.
>
> **Project context:** If a `PROJECT-OVERVIEW.md` exists at the repo root, read it before continuing — it describes this repo's product/project context (name, purpose, stakeholders, vision, core customer need, key features, architecture in one paragraph). Per-feature PRDs live under `docs/specs/` or `designs/`; ADRs under `docs/adr/`.
>
> **Agent notes:** If an `AGENT-NOTES.md` exists at the repo root, read it before continuing — it holds project-specific agent-facing context that doesn't fit in the regenerated CLAUDE.md: operational gotchas, project-specific commands, repo-local workflow conventions (branch naming, PR conventions, etc.).

---

## Working Method (before any code)

Meta-rules for *how* to approach a task. Framing adapted from [multica-ai/andrej-karpathy-skills](https://github.com/multica-ai/andrej-karpathy-skills).

- **State assumptions explicitly.** If multiple interpretations exist, present them — don't pick silently.
- **Ask when unclear.** Don't hide confusion behind plausible-looking code.
- **Push back when a simpler approach exists.** Minimum code that solves the problem; nothing speculative (no unrequested flexibility, configurability, or error handling for impossible cases).
- **Surgical edits.** Every changed line must trace to the request. Don't "improve" adjacent code, comments, or formatting. Match existing style. Remove orphans *your* change created — leave pre-existing dead code alone (mention it instead).
- **Goal-driven execution.** Restate the task as a verifiable success criterion before starting. For multi-step work, write a brief numbered plan with a `verify:` check per step, then loop until each check passes.

---

## Clean Code Principles

Apply to all generated and modified code, regardless of language:

- **Small methods/functions** — each does one thing at one level of abstraction; aim for ≤20 lines
- **Guard clauses** — validate and return/throw early at the top; avoid nested `if/else` pyramids
- **Command-Query Separation** — a function either performs an action (command, returns nothing) or returns data (query), never both
- **No flag arguments** — avoid boolean parameters that switch behaviour; split into two clearly named functions instead
- **Meaningful names** — names reveal intent; no abbreviations (`cnt`, `mgr`, `svc`) except universally understood ones (`id`, `url`, `dto`)
- **One level of abstraction per function** — don't mix high-level orchestration with low-level detail; extract helpers
- **Fail fast** — detect invalid state as early as possible and throw specific errors; don't let bad data travel deep into the call stack
- **DRY** — if the same logic exists in two places, extract it; but prefer duplication over the wrong abstraction — wait until the pattern is clear before generalising
- **No dead code** — delete unreachable branches, unused parameters, and vestigial methods; git has history
- **No commented-out code blocks** — delete them, git has history

---

## Testing — TDD, Tests First, No Shortcuts

Applies to every language and framework:

1. Write the failing test first
2. Write the minimum implementation to make it pass
3. Refactor
4. **Never modify a test to make it green** — fix the implementation
5. **Never hardcode return values, mock results, or stub logic** to satisfy a test
6. **Never silently swallow exceptions** to make a test green
7. **After implementation, run the full test suite** — not just the new test
8. **If a test fails after 3 attempts, STOP** and explain what's going wrong instead of continuing to iterate
9. Test naming: `MethodName_StateUnderTest_ExpectedBehavior` (or the idiomatic equivalent for the target language)
10. E2E tests must be independent and idempotent — seed and clean up their own data

Framework-specific test project layout, mocking library choice, and assertion library live in the stack overlay.

---

## UI Development Workflow (Mandatory Phase Order)

**Never skip phases. Never write component code before wireframe approval.**

| Phase | Command | Gate |
|---|---|---|
| 1 — Brainstorm | `/ui:brainstorm` | ASCII wireframe approved |
| 2 — Flow       | `/ui:flow`       | Mermaid diagrams approved |
| 3 — Build      | `/ui:build`      | Shell → logic → interactions → polish |
| 4 — Review     | `/ui:review`     | Checklist passes |

These commands ship from the global operator console (`agent-workflow`), installed once into `~/.claude/commands/ui/` — they are **not** synced per-project. They are stack-neutral: UI component library preferences (e.g. MudBlazor, shadcn/ui, Material, Flutter widgets) are read from the active stack overlay when one is present, otherwise inferred from the existing codebase.

### What to check before writing UI code

- [ ] Does a similar component already exist in a shared folder?
- [ ] Has the ASCII wireframe been approved?
- [ ] Has the Mermaid flow been approved?
- [ ] Are you building the shell first (no business logic yet)?
- [ ] Does the component need a unit/component test?

---

## Localization (i18n) & Regional Formatting

User-facing apps support **`de` and `en`** (CI/dev tooling exempt). Regional formatting follows the **OS region**, not the UI language; `de` with an unknown region falls back to **`de-CH`**. Render via the platform localization API, never `string.Format` / `toString()`.

Full rules: [`localization.md`](https://github.com/freaxnx01/ai-instructions/blob/main/.ai/references/base/localization.md)

---

## Versioning (SemVer)

All projects follow [Semantic Versioning 2.0.0](https://semver.org/): `MAJOR.MINOR.PATCH` — `MAJOR` = breaking, `MINOR` = new feature (backwards-compatible), `PATCH` = bug fix.

Conventional Commits mapping: `BREAKING CHANGE:` footer or `!` after type → MAJOR; `feat` → MINOR; `fix`, `perf` → PATCH; `chore`, `docs`, `ci`, `test`, `refactor` → no bump.

- Git tags follow `v<MAJOR>.<MINOR>.<PATCH>` (e.g. `v1.3.0`) — tag on `main` after merge
- Pre-release: `v1.0.0-alpha.1`, `v1.0.0-beta.2`, `v1.0.0-rc.1`
- **git-cliff** is the changelog and release notes tool — configured via `cliff.toml`
- Where the version is declared in the project (build file, manifest, etc.) is defined by the stack overlay — but it must be declared in **exactly one place**

---

## Changelog

All projects maintain a `CHANGELOG.md` in the repo root following [Keep a Changelog](https://keepachangelog.com) conventions. **Sections per release:** `Added`, `Changed`, `Deprecated`, `Removed`, `Fixed`, `Security`.

- `[Unreleased]` section accumulates changes until a release is cut
- Auto-generation: **git-cliff** with `cliff.toml` configured for Conventional Commits
- CI integration: `orhun/git-cliff-action` in GitHub Actions generates release notes into GitHub Releases
- CI can validate that `[Unreleased]` is not empty before allowing a release branch

Example: [`.ai/references/base/changelog-example.md`](https://github.com/freaxnx01/ai-instructions/blob/main/.ai/references/base/changelog-example.md)

---

## 12-Factor App Compliance

Projects follow the [12-Factor App](https://www.12factor.net/) methodology: one repo per service, all deps declared, env-var config, attached backing services, separate build/release/run stages, stateless processes, port binding, scale via replicas not threads, fast disposability, dev/prod parity, logs to stdout, admin processes as one-offs.

Stack-specific enforcement details (logging library, migrations, etc.) live in the stack overlay.

Full per-factor table: [`.ai/references/base/12-factor.md`](https://github.com/freaxnx01/ai-instructions/blob/main/.ai/references/base/12-factor.md)

---

## Branching Strategy (GitHub Flow + protection rules)

```text
main              ← always deployable, protected
  └── feature/<issue-id>-short-description
  └── fix/<issue-id>-short-description
  └── chore/<short-description>
  └── release/<version>   ← only if needed for staged releases
```

- `main` requires: passing CI, at least 1 PR review, no direct push
- Branch from `main`, PR back to `main`
- Delete branch after merge
- Rebase or squash merge — no merge commits on `main`

All changes go through a PR, including docs-only ones. There is no trivial-edit exception: a direct push to a protected `main` lands before the required checks report, so they become a postmortem instead of a gate, and it leaves open PRs' branches stale.

---

## Git Worktrees

### Worktree directory

- Use **project-local** worktrees under `.worktrees/` at the repo root (hidden directory)
- `.worktrees/` must be listed in `.gitignore` — add and commit it before creating the first worktree in a repo
- Use a **random, short branch name** when the user does not specify one (e.g. `wt/<8-hex-chars>`); do not prompt for a branch name

Agent tooling that automates worktree creation should discover these rules from `CLAUDE.md` / `AGENTS.md` (e.g. a `worktree.*director` grep) and honour them without asking.

---

## Commit Messages (Conventional Commits)

```text
<type>(<scope>): <short summary>

[optional body]

[optional footer: Closes #<issue>]
```

**Types:** `feat`, `fix`, `test`, `refactor`, `chore`, `docs`, `ci`, `perf`
**Scope:** module or layer name, e.g. `orders`, `auth`, `infra`, `ui`

```text
feat(orders): add order cancellation endpoint

Implements POST /api/v1/orders/{id}/cancel.
Validates order is in Pending state before cancelling.

Closes #42
```

- Subject line: imperative mood, ≤72 chars, no period
- Body: explain *why*, not *what*
- Breaking changes: add `BREAKING CHANGE:` footer (or `!` after the type)

---

## Pull Request Conventions

### PR Title

Follow Conventional Commits format: `feat(orders): add cancellation endpoint`

### PR Description Template

Body sections: **Summary** · **Changes** · **Testing** (unit, component/integration, E2E, local) · **Checklist** (tests pass, no new vulnerable deps, no secrets, migrations included if schema changed, API/OpenAPI spec still valid).

Template: [`.ai/references/base/pr-description-template.md`](https://github.com/freaxnx01/ai-instructions/blob/main/.ai/references/base/pr-description-template.md)

### Review Guidelines

- PRs should be small and focused — one concern per PR
- Reviewers check: architecture adherence, test quality, security, no shortcuts that make tests green
- Auto-assign reviewers via `CODEOWNERS`

---

## CI/CD (generic outline)

Pipeline stages: `build` → `test` → `security-scan` → `container-build` → `push`

- Build and test run on every PR
- Vulnerable-dependency scan fails the build on HIGH/CRITICAL
- Container image built and pushed only on `main` after tests pass
- E2E tests run against the built image before it is marked as a release candidate

Concrete CI configuration (GitHub Actions YAML, commands, package scanners) lives in the stack overlay.

---

## Scripting

**PowerShell — customer-delivered scripts target Windows PowerShell 5.1.** Anything a customer runs (`build.ps1`, install/deploy scripts, release artifacts) must run on 5.1 unless the project documents a PS 7+ floor; `pwsh` is not installed there.

- **Never** use `??`, `??=`, ternary `? :`, `?.`, `&&` / `||` chains — *parse* errors on 5.1, so the script dies before its first line — nor `ForEach-Object -Parallel`, `Sort-Object -Stable`, `-SslProtocol`
- `$IsWindows` / `$IsLinux` / `$IsMacOS` **do not exist** on 5.1 — they are `$null`, so the branch is silently skipped. Use `$env:OS -eq 'Windows_NT'`
- Pass `-Depth` to `ConvertTo-Json` (defaults to 2, truncates silently) and `-UseBasicParsing` to the web cmdlets (a patched host prompts and hangs)
- Start with `#requires -Version 5.1`, pin encoding, verify with PSScriptAnalyzer
- **Exempt:** dev-loop tooling (`justfile` recipes) may require `pwsh`

Full rules: [`powershell-5.1.md`](https://github.com/freaxnx01/ai-instructions/blob/main/.ai/references/base/powershell-5.1.md)

---

## Documentation Structure

Repo-root `docs/` contains:

- `design/<feature-name>/` — UI wireframes (`wireframe.md`) & Mermaid flows (`flow.md`) per feature
- `adr/` — Architecture Decision Records
- `ai-notes/` — AI agent working notes

Rules:

- `README.md` and `CHANGELOG.md` live in the repo root
- UI design artifacts are saved per feature during the UI workflow phases
- AI agents write working notes to `docs/ai-notes/`, not `.ai/`
- `.ai/` is reserved for agent instructions and skill files only

Layout: [`.ai/references/base/documentation-structure.md`](https://github.com/freaxnx01/ai-instructions/blob/main/.ai/references/base/documentation-structure.md)

---

## Security (baseline)

- Transport security enforced (HTTPS + HSTS)
- No secrets in source files or per-environment config files — environment variables or a secrets manager only
- Validate all inputs at system boundaries before any domain logic
- Run a vulnerable-dependency scan in CI — fail the build on HIGH/CRITICAL findings
- Standard security response headers on every HTTP response

Language- and framework-specific enforcement (specific scanners, validation libraries, header mechanisms) lives in the stack overlay.

---

## Agent Guardrails

- Do not install additional packages without asking first
- Do not change the project's target runtime or framework version
- Do not modify build/project files unless the task requires it
- Do not introduce new architectural patterns unless explicitly asked
- Do not touch files outside the scope of the current task
- Keep changes minimal and focused — do not refactor unrelated code unless asked
- Never skip git hooks (`--no-verify`) unless the user explicitly asks
- Never commit secrets or credential files

Stack-specific guardrails (e.g. "do not add NuGet packages") live in the stack overlay.

---

## Project Scaffold Checklist (baseline)

Init-time checklist (every project, regardless of stack) — including baseline, .NET, and WebAPI layers — lives at [`.ai/references/scaffold-checklists.md`](https://github.com/freaxnx01/ai-instructions/blob/main/.ai/references/scaffold-checklists.md). Stack-specific additions are in the same file under their respective sections.

[//]: # (Stack overlay — loaded together with .ai/base-instructions.md for modern .NET class libraries and console tools)

# .NET Library / Console Tool Stack Overlay

Applies on top of `.ai/base-instructions.md` for **modern SDK-style .NET class
libraries and small console utilities** — repos like `CommonLibrary`,
`Extensions` (a published NuGet extension-method package), `StringKing` (a
library + code-generator console app), `CodeConverterSingleFile`, and
`SaveOutlookCalendar`. These are not services: there's no ASP.NET Core host, no
web-facing surface, and often no long-running process at all — the deliverable
is a `.dll` (library) or a `.exe` you run once and it exits.

This is a **standalone overlay, not built on the shared `dotnet-core` partial**
that `dotnet-blazor`/`dotnet-webapi` share — that partial assumes an ASP.NET
Core service shape (Modular Monolith architecture, Minimal API, EF Core,
Docker, Serilog/OpenTelemetry), none of which applies to a plain library or a
run-once CLI tool. If a repo in this stack later grows a web-facing host, it
has outgrown this overlay — switch it to `dotnet-webapi`/`dotnet-blazor`
instead of bolting service concerns onto a library overlay.

---

## Tech Stack

Modern SDK-style `.csproj` (`<Project Sdk="Microsoft.NET.Sdk">`) · current .NET
LTS/STS target (`net8.0`+; check the project's actual `<TargetFramework>` before
assuming) · `netstandard2.0` for libraries meant to be consumable from both
.NET Framework and modern .NET · `PackageReference`, never `packages.config` ·
xUnit for tests, where a test project exists · NuGet publishing via
`dotnet pack`/`dotnet nuget push` for libraries meant to be shared.

---

## Project Shape

Two shapes cover this stack; identify which one a given project is before
touching it:

- **Library** (`OutputType` implicit/omitted or `Library`): a `.csproj`
  producing a `.dll`, consumed by other projects or published as a NuGet
  package. No `Main` entry point; no hosting, no DI container of its own.
- **Console tool** (`OutputType=Exe` or `WinExe`): a small, usually single-
  purpose CLI utility with a `Main`/top-level-statements entry point that runs
  to completion and exits — not a long-running host, not a background worker.
  `WinExe` specifically means a Windows-only tool (e.g. one using COM Interop
  like Outlook automation) — don't assume cross-platform for those.

A repo may contain **both** (e.g. a library plus a small companion CLI that
exercises it, or a code-generator console app alongside the library it
generates code for) — treat each project by its own shape, not the repo as a
whole.

---

## C# Conventions

- File-scoped namespaces; `record` types for immutable DTOs/value objects;
  `sealed` by default on non-base classes.
- Public API surface gets XML doc comments (`/// <summary>`) — this is what
  IntelliSense and a consuming project see; undocumented public members on a
  published library are a gap, not a style nit.
- Nullable reference types enabled (`<Nullable>enable</Nullable>`) for new
  SDK-style projects; no `#nullable disable` or unexplained `!` suppression.
- Specific exception types, not bare `catch (Exception)`; a library throws
  exceptions a caller can reasonably catch and handle, not opaque wrapper
  exceptions that hide the real cause.
- `async`/`await` end-to-end for any I/O the library performs — never
  `.Result`/`.GetAwaiter().GetResult()`. A library that offers only sync
  wrappers over async I/O forces that same trap onto every consumer.
- No `Console.WriteLine` inside library code — a library has no business
  writing to a console it doesn't own. Console output is legitimate only in
  the console-tool projects themselves.

---

## Public API Design (libraries)

- Keep the public surface intentional: `internal` by default, `public` only
  for what's meant to be consumed. A wide public surface with no consumer
  outside the same solution is a sign something should be `internal`.
- Semantic Versioning applies to the **public API**, not just the package
  version number — removing or changing the signature of a public member is a
  breaking (MAJOR) change regardless of how small it looks internally.
- Prefer extension methods and small, composable types over large "manager"/
  "helper" god-classes — `Extensions`-style focused packages are the model to
  follow, not a `CommonLibrary`-style grab-bag (existing grab-bag libraries
  don't need a forced split, but new code shouldn't grow one further).
- Target `netstandard2.0` when the library needs to be usable from both
  .NET Framework 4.8+ consumers and modern .NET; target the current LTS
  directly when the library is only ever consumed by modern .NET projects.

---

## Console Tool Design

- Parse arguments with a real parser (e.g. `CommandLineParser`,
  `System.CommandLine`) once a tool takes more than one or two positional
  arguments — don't hand-roll `args[0]`/`args[1]` indexing past a trivial
  case.
- Exit codes are meaningful: `0` success, non-zero on failure, and a tool that
  another script might invoke documents what its codes mean.
- Errors intended for the user go to stderr with a clear message; don't dump a
  raw stack trace as the only output unless a `--verbose`/`--debug` flag was
  passed.
- Platform-specific tools (COM Interop, Windows-only APIs) fail fast and
  clearly on the wrong platform rather than throwing an opaque
  `PlatformNotSupportedException` deep in a call stack — check and message
  early.

---

## Testing

Base TDD rules (tests first, never modify a test to make it green, no stubbed
logic to satisfy a test, full suite after implementation) apply wherever a
test project exists.

- xUnit is the default test framework for this stack.
- One test project per library/tool that has meaningful logic
  (`<Project>.Tests`), targeting the same or a compatible framework.
- Naming: `MethodName_StateUnderTest_ExpectedBehavior`.
- `[Theory]` + `[InlineData]`/`[MemberData]` over logic embedded in a `[Fact]`.
- A library with no test project is a gap to flag, not a precedent to extend —
  don't add new untested surface area to a library on the assumption "it
  already has none."

---

## Versioning & Publishing

Base SemVer/Conventional-Commits/`git-cliff` rules apply.

- **Libraries published as NuGet packages**: the package version is the git
  tag (`vX.Y.Z` → `X.Y.Z` package version), set once via
  `Directory.Build.props` `<Version>`, never hardcoded per-`.csproj`.
- `dotnet pack` produces the `.nupkg`; `dotnet nuget push` publishes it — keep
  the API key in environment/secrets, never committed.
- **Console tools with no external consumers** (a personal utility, not
  published anywhere) still tag releases per base SemVer if the tool has
  users beyond the author; a purely personal one-off tool can skip formal
  releases but still keeps `CHANGELOG.md`'s `[Unreleased]` section current.

---

## Security

Base security rules apply. For this stack specifically:

- Run `dotnet list package --vulnerable --fail-on-severity high` in CI —
  same as the service-shaped .NET stacks.
- A published NuGet package's dependency tree is part of every consumer's
  supply chain — keep dependencies minimal and justify each one.
- Tools that read credentials (mail/calendar/API access, e.g.
  `SaveOutlookCalendar`'s Outlook COM access) never hardcode them — configured
  via environment variable, a local untracked config file, or the platform
  credential store.

---

## Agent Guardrails (this stack)

In addition to the base guardrails:

- Do not add NuGet packages or change the target framework without asking.
- Do not change a library's public API shape (rename/remove a public member,
  change a signature) without flagging it as a breaking (MAJOR) change.
- Do not add ASP.NET Core, EF Core, or web-hosting packages to a project in
  this stack — if the project genuinely needs to become a service, that's a
  stack change to `dotnet-webapi`/`dotnet-blazor`, not an addition here.
- Do not convert a `netstandard2.0` library to a newer-only TFM without
  confirming nothing still consumes it from .NET Framework.
- Detect library vs. console-tool shape from the actual `OutputType`/entry
  point before applying conventions — don't assume one shape for the whole
  repo when it contains both.

### Never generate (this stack)

- `Console.WriteLine` inside library (non-entry-point) code
- `.Result` / `.GetAwaiter().GetResult()` on a `Task` — always `await`
- A public API break (renamed/removed public member, changed signature)
  treated as a non-breaking change
- ASP.NET Core / EF Core / web-hosting references added to a library or
  console-tool project
- Hardcoded credentials or API keys in a console tool that authenticates to an
  external service
- A hand-set `<Version>` in an individual `.csproj` instead of
  `Directory.Build.props`
