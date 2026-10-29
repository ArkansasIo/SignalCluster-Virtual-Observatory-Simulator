# SignalCluster repository instructions

## Goal
Build a reliable, clearly labelled simulation-only virtual radio astronomy observatory prototype using C#/.NET 8, ASP.NET Core, and the existing browser UI.

## Before editing
- Inspect the current implementation, project files, tests, configuration, and recent git history before changing anything.
- Preserve existing working behavior and public API contracts unless a change is necessary; explain any breaking change.
- Do not recreate or overwrite an existing file blindly. Prefer small, reviewable diffs.
- Treat the current repository as the source of truth. Do not assume a feature exists just because documentation mentions it.

## Technical standards
- Target .NET 8 and use nullable reference types / implicit usings as configured by the projects.
- Keep core algorithms in SignalCluster.Core and HTTP endpoints / transport models in SignalCluster.Service.
- Validate API input and return appropriate 4xx responses for invalid requests.
- Make simulation deterministic where practical by supporting explicit seeds.
- Avoid unbounded allocations, unbounded loops, and unsafe parsing of user-supplied values.
- Keep the UI responsive and ensure API base URLs and CORS settings agree with documented local run instructions.
- Do not add real SDR control, real RF transmission, or claims of real telescope access without a separately reviewed hardware scope.
- The 24 logical drives are simulated inventory only; never imply they are physical disks.
- Treat Lua files as examples unless a sandboxed runtime loader and security model are implemented.
- Do not add secrets, credentials, tokens, or machine-specific paths to source control.

## Required workflow
1. Inspect the relevant code and identify dependencies.
2. Implement a coherent vertical slice rather than generating placeholder files only.
3. Add or update unit tests for changed logic and edge cases.
4. Update README/docs when behavior, endpoints, configuration, or run commands change.
5. Run dotnet restore, dotnet build -c Release --no-restore, and dotnet test -c Release --no-build when the .NET SDK is available.
6. If a command cannot run, report that as unverified; never claim tests passed without actual output.
7. Review the diff for accidental deletions, unrelated formatting, insecure defaults, and inconsistent names.
8. Open a pull request with a summary, test results, known limitations, and a list of files changed. Do not merge automatically.

## Prototype priorities
- Make solution/project structure build cleanly first.
- Align service launch URL, CORS, and UI API configuration.
- Add validated channel-planning and synthetic-sample API endpoints.
- Add tests for channel bounds, sample counts, deterministic simulation, FFT input constraints, and dispersion-delay units.
- Keep API and UI status truthful about simulation-only capabilities.
