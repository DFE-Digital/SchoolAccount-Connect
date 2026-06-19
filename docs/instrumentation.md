# Instrumentation

SchoolAccount-Connect uses [OpenTelemetry](https://opentelemetry.io/) for all observability, structured logging, metrics, and distributed 
tracing, exporting to **Azure Application Insights** in production and **Seq** locally.

## Configuration

| Key                                    | Purpose                                                                                                                     |
|----------------------------------------|-----------------------------------------------------------------------------------------------------------------------------|
| `ApplicationInsights:ConnectionString` | Enables all OpenTelemetry exports to Azure Monitor. When blank, App Insights is skipped and a warning is logged at startup. |
| `Seq:Endpoint`                         | Enables OTLP/HTTP export to Seq (e.g. `http://localhost:5341/ingest/otlp`). Only active when the value is non-empty.        |

Both sinks are additive, setting both sends telemetry to both destinations.

## Logging

Structured logs are written via `Microsoft.Extensions.Logging`. The pipeline is configured in 
`DependencyInjection.AddPresentation` (`src/SchoolAccount.Web.Connect/DependencyInjection.cs`) and includes:

- **Console** — active in the `Development` environment only.
- **OpenTelemetry → Azure Monitor** — active when `ApplicationInsights:ConnectionString` is set. Log messages include 
  formatted message text, scope values, and parsed state properties.
- **OpenTelemetry → Seq (OTLP)** — active when `Seq:Endpoint` is set. Uses the `HttpProtobuf` OTLP protocol.

A `BootstrapLogger` (`src/SchoolAccount.Web.Connect/Logging/BootstrapLogger.cs`) is created before the DI container is 
built so that startup failures (including Azure App Configuration and infrastructure registration errors) are 
captured and exported with the same sinks.

### CQRS logging decorator

Every command and query handler is automatically wrapped by `LoggingDecorator` 
(`src/SchoolAccount.Application/Abstractions/Behaviours/LoggingDecorator.cs`). This logs command and query lifecycle 
events at `Debug` level (event IDs 2001–2004 for commands, 3001–3003 for queries) and promotes failures to 
`Error` level with the structured `Error` object attached as a log scope property.

| Event ID   | Level   | Meaning                                    |
|------------|---------|--------------------------------------------|
| 2001       | Debug   | Command handler invoked                    |
| 2002       | Debug   | Command completed successfully             |
| 2003       | Debug   | Command completed with a validation error  |
| 2004       | Error   | Command failed with a non-validation error |
| 3001       | Debug   | Query handler invoked                      |
| 3002       | Debug   | Query completed successfully               |
| 3003       | Error   | Query failed                               |

## Metrics

Metrics use `System.Diagnostics.Metrics` and are exported to Azure Monitor via the OpenTelemetry SDK. 
Two meters are registered:

| Meter name                | Constant                                | Purpose                                        |
|---------------------------|-----------------------------------------|------------------------------------------------|
| `SchoolAccount.Analytics` | `MeterConstants.SchoolAccountAnalytics` | Page visits and user journey counters          |
| `SchoolAccount.Feedback`  | `MeterConstants.SchoolAccountFeedback`  | Feedback banner exposure and response counters |

Counters are created lazily by `TrackAnalyticsTelemetryCommandHandler` and cached in a `ConcurrentDictionary`.

## Analytics events

The `AppInsightsFilter` MVC action filter (`src/SchoolAccount.Web.Connect/Filters/AppInsightsFilter.cs`) fires after 
every authenticated HTTP GET action and dispatches a `TrackAnalyticsTelemetryCommand` that records:

1. **A metric counter** (`connect_user_journey`) with tags for event name, page ID, feature, journey, and client.
2. **A log event** (`connect_page_visited`) with the full set of tags including the hashed user ID, organisation UKPRN, 
   and session ID.

### Event catalogue

| Event name                         |  Trigger                                           |
|------------------------------------|----------------------------------------------------|
| `connect_login_succeeded`          | Successful DfE Sign-In authentication              |
| `connect_login_failed`             | Failed authentication attempt                      |
| `connect_page_visited`             | Authenticated GET request to any controller action |
| `connect_banner_exposure_assigned` | Feedback banner shown to user                      |
| `connect_cta_yes_no_interaction`   | User interacts with a yes/no CTA                   |
| `connect_cta_feedback_exit`        | User exits via CTA feedback prompt                 |
| `connect_cta_cancelled`            | User cancels a CTA                                 |
| `connect_cta_dismissed`            | User dismisses a CTA                               |

### Tag names

Tags are defined in `AnalyticsTagNames` (`src/SchoolAccount.Application/Constants/AnalyticsTagNames.cs`). The most 
common ones are:

| Tag              | PII?                 | Present on metrics   | Present on events   |
|------------------|----------------------|----------------------|---------------------|
| `EventName`      | No                   | Yes                  | Yes                 |
| `PageId`         | No                   | Yes                  | Yes                 |
| `Feature`        | No                   | Yes                  | Yes                 |
| `Journey`        | No                   | Yes                  | Yes                 |
| `Client`         | No                   | Yes                  | Yes                 |
| `UserId`         | Yes (hashed)         | **No**               | Yes                 |
| `OrganisationId` | Pseudonymous (UKPRN) | **No**               | Yes                 |
| `SessionId`      | No                   | **No**               | Yes                 |

## Distributed tracing

Azure Monitor OpenTelemetry captures ASP.NET Core request traces automatically. The active `TraceId` is surfaced to 
application code through the `IRequestContext` abstraction 
(`src/SchoolAccount.Application/Abstractions/Telemetry/IRequestContext.cs`), implemented by `RequestContext` 
in the web layer. Command handlers use this to correlate analytics log events with the parent HTTP trace.

## Privacy

User IDs are SHA-256 hashed before inclusion in any telemetry 
(see `TelemetryUserIdFactory`, `src/SchoolAccount.Web.Connect/Telemetry/TelemetryUserIdFactory.cs`). 
Raw user identifiers are never written to logs or metrics. Organisation identifiers are included as UKPRNs, 
which are public reference numbers.

Analytics session IDs are generated as a new GUID on first authenticated request for a sign-in session and stored as a 
claim on the `ClaimsPrincipal` 
(see `AnalyticsSessionIdProvider`, `src/SchoolAccount.Web.Connect/Telemetry/AnalyticsSessionIdProvider.cs`). 
They are not persisted beyond the authentication cookie lifetime.