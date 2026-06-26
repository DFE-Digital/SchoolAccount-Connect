# ServiceNow Change Requests for GitHub Actions

## Introduction

The actions within the `./.github/actions/servicenow/change-request` allow you to create and close ServiceNow change 
requests (CRs) directly from your deployment workflows. This is the GitHub Actions replacement for the Azure DevOps 
pipeline that previously created and closed CRs against the ServiceNow `sn_chg_rest` API.

The setup is two reusable **composite actions** plus a JSON file holding the static, org-standard CR text:
- **Create** a CR before deploying; returns the CR number and `sys_id`.
- **Close** the CR after deploying; sets the work window, close code and notes.

### Repository layout

```
.github/
  actions/
    servicenow/
      close/action.yml            # raises the CR
      create/action.yml           # closes the CR
infrastructure/
  servicenow/
    change-defaults.json          # static CR text, a single source of truth, this allows common messages to be 
                                    versioned, additionally we could introduce versioned responses.
```

### How it fits together

```
checkout --> create CR --> deploy --> close CR
                |                         ^
            sys-id, start ----------------|
```

The create action outputs the `sys-id` and `start` time; the close action takes them back as inputs so it can update 
the exact record that was raised. The close step runs with `if: always()`, so the CR is still closed (as `unsuccessful`) 
if the deploy fails, we might also do a check to see if the CR was created and returned us a `id`.

## Prerequisites

- A ServiceNow account with permission to create and update change requests via the REST API.
- The deploy job must run `actions/checkout` **before** the create action, the defaults file is read from the 
  checked-out workspace.
- GitHub-hosted `ubuntu-latest` runners are fine (PowerShell `pwsh` is preinstalled and runs in UTC).

## Configuration

Set these at the repository level or, preferably, on a GitHub **Environment** (e.g. `production`) so you can scope them 
and require approvals.

### Secrets

| Secret | Description |
| --- | --- |
| `SERVICE_NOW_USERNAME` | ServiceNow API username |
| `SERVICE_NOW_PASSWORD` | ServiceNow API password |

### Variables

| Variable | Description |
| --- | --- |
| `SERVICE_NOW_URL` | Instance base URL, e.g. `https://acme.service-now.com` |
| `SERVICE_NOW_ENVIRONMENT` | Target environment (`u_environment`) |
| `SERVICE_NOW_SERVICE_OFFERING` | Service offering / configuration item |
| `SERVICE_NOW_IMPLEMENTATION_GROUP` | Assignment group |
| `SERVICE_NOW_IMPLEMENTOR` | Assigned-to user |

## The _defaults_ file

`./infrastructor/servicenow/change-defaults.json` holds the **static text that is the same for every release**,
descriptions, justification, and the risk, backout, test and communication plans, plus the default expected duration. 
The create action reads it at runtime (`ConvertFrom-Json`) and maps each key onto a field in the CR request body.

To change the wording of any CR field, edit the JSON and commit, no change to the action is needed. To keep variants 
(per team or service), commit additional files and point the action's `defaults-file` input at them, e.g.
`[...]/change-defaults.payments.json` or `[...]/change-defaults.2026.6.*.json`.

These are action inputs vs ServiceNow's API fields:

| JSON key | ServiceNow field |
| --- | --- |
| `longDescription` | `description` |
| `shortDescription` | `short_description` |
| `justification` | `justification` |
| `implementationPlan` | `implementation_plan` |
| `riskImpactAnalysis` | `risk_impact_analysis` |
| `backoutPlan` | `backout_plan` |
| `testPlan` | `test_plan` |
| `communicationPlan` | `u_communication_plan` |
| `expectedDurationMinutes` | derives `end_date` from `start_date` |

## Usage

```yaml
env:
  SERVICENOW_CREATE_CR: true
jobs:
  deploy:
    runs-on: ubuntu-latest
    environment: production
    steps:
      - uses: actions/checkout@v4

      - id: change-request
        if: env.SERVICENOW_CREATE_CR == 'true'
        uses: ./.github/actions/servicenow/change-request/create
        with:
          service-now-url: ${{ vars.SERVICE_NOW_URL }}
          username: ${{ secrets.SERVICE_NOW_USERNAME }}
          password: ${{ secrets.SERVICE_NOW_PASSWORD }}
          environment: ${{ vars.SERVICE_NOW_ENVIRONMENT }}
          service-offering: ${{ vars.SERVICE_NOW_SERVICE_OFFERING }}
          implementation-group: ${{ vars.SERVICE_NOW_IMPLEMENTATION_GROUP }}
          implementor: ${{ vars.SERVICE_NOW_IMPLEMENTOR }}

      - id: deploy
        [...]

      - name: Close ServiceNow CR
        if: always() && steps.change-request.outputs.sys-id != ''
        uses: ./.github/actions/servicenow/change-request/close
        with:
          service-now-url: ${{ vars.SERVICE_NOW_URL }}
          username: ${{ secrets.SERVICE_NOW_USERNAME }}
          password: ${{ secrets.SERVICE_NOW_PASSWORD }}
          sys-id: ${{ steps.change-request.outputs.sys-id }}
          change-request-number: ${{ steps.change-request.outputs.change-request-number }}
          work-start: ${{ steps.change-request.outputs.start }}
          close-code: ${{ steps.deploy.outcome == 'success' && 'successful' || 'unsuccessful' }}
          close-notes: ${{ steps.deploy.outcome == 'success' && 'Automated deployment was a success' || 'Automated deployment failed; see run logs' }}
```

The above example shows how it could be implemented in to either `.github/workflows/_deploy.yml` as a template action 
where the `env:` could be a variable or the `build.yml` file could just have added steps.

## Action Variable Inputs

### Create action

#### Inputs

| Input | Required | Default                                            | Description |
| --- | --- |----------------------------------------------------| --- |
| `service-now-url` | yes |                                                    | Instance base URL |
| `username` | yes |                                                    | API username |
| `password` | yes |                                                    | API password (use a secret) |
| `environment` | yes |                                                    | `u_environment` |
| `service-offering` | yes |                                                    | Service offering / CI |
| `implementation-group` | yes |                                                    | Assignment group |
| `implementor` | yes |                                                    | Assigned-to user |
| `defaults-file` | no | `infrastructure/ servicenow/ change-defaults.json` | Path to the defaults JSON |
| `expected-duration-minutes` | no | (from defaults)                                    | Override the duration |

#### Outputs

| Output | Description |
| --- | --- |
| `change-request-number` | The created CR number (e.g. `CHG0012345`) |
| `sys-id` | The `sys_id` of the created CR |
| `start` | Scheduled start, `yyyy-MM-dd HH:mm:ss` |

### Close action

#### Inputs

| Input | Required | Default | Description |
| --- | --- | --- | --- |
| `service-now-url` | yes | | Instance base URL |
| `username` | yes | | API username |
| `password` | yes | | API password (use a secret) |
| `sys-id` | yes | | CR `sys_id` from the create action |
| `change-request-number` | no | `""` | CR number, for logging / summary |
| `work-start` | yes | | The create action's `start` output |
| `close-code` | no | `successful` | `successful` \| `successful_with_issues` \| `unsuccessful` |
| `close-notes` | no | `Automated deployment was a success` | Free-text close notes |

#### Outputs

_None_

## Job summary

Both actions write a panel to the run's **Summary** page (via `$GITHUB_STEP_SUMMARY`) showing the CR details, with 
the CR number linked straight to the record in ServiceNow. The raw request/response is also logged to the step output 
for debugging.

The CR link uses the classic UI form `{url}/nav_to.do?uri=change_request.do?sys_id={id}`. If your instance uses the 
Next Experience / Workspace UI, change it to `{url}/now/sow/record/change_request/{id}` in both action files.

## Migration notes 

Here is some of the nice to notes from the migration of **Azure DevOps** to **GitHub Actions**.

### Environment variables

| Azure DevOps | GitHub Actions |
| --- | --- |
| `$(Build.BuildId)` | `${{ github.run_id }}` |
| `$(Build.BuildNumber)` | `${{ github.run_number }}` |
| `$(Build.DefinitionName)` | `${{ github.workflow }}` |
| `$(Build.Repository.Uri)` | `${{ github.server_url }}/${{ github.repository }}` |
| `$(System.StageDisplayName)` | `${{ github.job }}` |
| `##vso[task.setvariable …;isoutput=true]` | `"name=value" >> $GITHUB_OUTPUT` |
| `variables/service-now-details.yaml` template | `change-request-defaults.json` + `vars`/`secrets` |


### Pipeline changes

Some behaviour changes from the original pipeline:
- **Password is no longer base64-encoded.** Azure DevOps stored a base64 value to handle special characters; GitHub 
  secrets handle them natively, so the password is passed directly. (The base64 decode can be re-added if needed.)
- **Close code reflects the deploy outcome.** The original always closed as `successful`; the example workflow closes 
  as `unsuccessful` when the deploy step fails. 
- A little side note of the `x_mioms_azpipeline_*` fields are ServiceNow are assumed to be custom field names and so 
  kept as-is (populated with GitHub values); renaming them requires a ServiceNow-side schema change.
