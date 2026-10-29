# Using GitHub Copilot coding agent with SignalCluster

GitHub provides a Copilot coding agent workflow that can work on a repository branch, edit code, run available checks, and propose a pull request. Availability depends on the account's Copilot plan, repository policy, and whether the feature is enabled. Adding the files in this repository does not itself start an agent session.

## Start an agent task
1. Open this repository on GitHub.
2. Open the Agents tab/panel, or go to https://github.com/copilot/agents.
3. Select ArkansasIo/SignalCluster-Virtual-Observatory-Simulator as the repository.
4. Select the SignalCluster Builder custom agent if it appears in the agent picker. Otherwise use the default Copilot coding agent.
5. Paste the task prompt below and ask it to open a pull request. Review the resulting diff and checks before merging.

## Recommended first task prompt
> Work in ArkansasIo/SignalCluster-Virtual-Observatory-Simulator. Read .github/copilot-instructions.md and docs/project-status.md. Inspect the entire current solution before editing. First repair any solution/project/build configuration issues, then correct and unit-test signal-processing formula units and input bounds. Next implement the smallest end-to-end vertical slice for channel-plan listing and bounded synthetic sample generation, connect the existing UI, and update docs. Preserve existing working files. Run .NET 8 restore, Release build, and tests; report exact commands and actual results. Do not claim unrun checks passed. Keep all data synthetic and open a pull request; do not merge it.

## Work in reviewable batches
Prefer one pull request per coherent batch. Merge only after reviewing:
- the diff and file list;
- test/build results and CI checks;
- API/UI compatibility;
- input limits and error handling;
- whether documentation accurately describes what works.

The agent profile is guidance, not a guarantee that every task will complete or that all generated code is correct.
