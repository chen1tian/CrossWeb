# AvWeb Agent Guide

This repository builds the cross-platform application described in `README.md`.
The product has three distinct layers:

- React web UI — not created yet.
- Avalonia host shell — loads the web UI in a WebView.
- PicoServer/ABP services — expose local APIs to the web UI.

`README.md` is the source of truth for product scope. `Host/AGENTS.md` contains
additional placement and framework rules for changes under `Host/`.

## Startup Workflow

Before editing:

1. Confirm the working directory is the repository root.
2. Read `README.md`, this file, `feature_list.json`, and `progress.md`.
3. Read `session-handoff.md` when resuming previous work.
4. Before changing `Host/`, also read `Host/AGENTS.md` and the linked documents
   relevant to the change.
5. Run `./init.ps1` on Windows or `./init.sh` in a POSIX shell.
6. Select exactly one unfinished feature whose dependencies are complete.

If baseline verification fails, record the failure in `progress.md` before
changing product code. Do not silently widen the active feature to fix unrelated
warnings or defects.

## Working Rules

- **One feature at a time:** work on exactly one active feature.
- Keep React UI, Avalonia shell, and ABP/API responsibilities distinct.
- Do not edit generated `bin/` or `obj/` content.
- Keep changes inside the active feature's completion criteria.
- Treat destructive operations, dependency replacements, and architecture
  changes as decisions requiring explicit user approval.
- Do not mark a feature done without recording verification evidence.
- Update `progress.md` and `feature_list.json` before ending the session.

## Scope Boundaries

- Stay in scope: only implement the active feature and its completion criteria.
- Phase 1 includes the React scaffold, PicoServer integration, Avalonia WebView,
  and the ping/pong MVP described in `README.md`.
- Unrelated warning cleanup, broad refactoring, additional product features,
  deployment, and later-phase platform work are out of scope unless the user
  explicitly adds them.

## Required State

- `feature_list.json` — feature dependencies, status, completion criteria, and evidence.
- `progress.md` — current work, decisions, blockers, and next action.
- `session-handoff.md` — restart information for a longer or interrupted session.
- `init.ps1` / `init.sh` — standard baseline verification.

Allowed feature statuses are `not-started`, `in-progress`, `blocked`, and `done`.
At most one feature may be `in-progress`.

## Definition of Done

A feature is done only when:

1. Every completion criterion in `feature_list.json` is satisfied.
2. Relevant build, test, lint, and type-check commands have run successfully.
3. User-visible behavior is manually verified when automation is unavailable.
4. Commands and results are recorded in `feature_list.json` or `progress.md`.
5. Documentation and session state reflect the implemented behavior.

For the Phase 1 MVP, completion additionally requires an end-to-end demonstration
that React sends `ping`, PicoServer returns `pong`, and the page renders inside
the Avalonia WebView.

## Verification

Run the platform-appropriate entrypoint:

```powershell
.\init.ps1
```

```bash
./init.sh
```

The current baseline builds the Desktop host:

```text
dotnet build Host/src/Host.Desktop/Host.Desktop.csproj --nologo
```

When the React project is created, add its install, lint, test, and build checks
to both initialization scripts.

## End of Session

1. Re-run verification relevant to the active feature.
2. Update feature status and evidence in `feature_list.json`.
3. Update `progress.md` with files changed, decisions, blockers, and the exact next step.
4. Update `session-handoff.md` for interrupted or multi-session work.
5. If this directory is version-controlled, leave the working tree in an
   intentional state and report any uncommitted changes.

The repository is restartable when the next session can follow the Startup
Workflow from a clean checkout, run the platform initialization script, identify
one active feature, and reproduce the recorded baseline.
