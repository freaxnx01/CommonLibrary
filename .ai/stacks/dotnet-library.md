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
