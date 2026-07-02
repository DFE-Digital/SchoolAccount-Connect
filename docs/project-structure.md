
---

## Solution Layout

```
SchoolAccount-Connect/
├── src/
│   ├── SchoolAccount.Domain/               # Innermost layer - business rules
│   ├── SchoolAccount.Application/          # Use cases and CQRS handlers
│   ├── SchoolAccount.Infrastructure/       # EF Core, SQL, external services
│   ├── SchoolAccount.Web.Connect/          # ASP.NET Core MVC presentation layer
│   └── SchoolAccount.Kernel/               # Shared primitives (no dependencies)
├── tests/
│   ├── SchoolAccount.Application.UnitTests/
│   ├── SchoolAccount.Application.IntegrationTests/
│   ├── SchoolAccount.Web.Connect.UnitTests/
│   ├── SchoolAccount.InfrastructureTests/
│   ├── SchoolAccount.IntegrationTests/
│   ├── SchoolAccount.AuthenticationTests/
│   ├── SchoolAccount.ConformanceTests/     # Architecture rule tests (NetArchTest)
│   └── SchoolAccount.Tests.Common/         # Shared builders, fakes, helpers
├── docs/
├── infrastructure/
├── scripts/
└── integration/                            # SchoolAccount.Integration.DfESignIn
```

---

## Layer Responsibilities

Dependencies point inward only. `SchoolAccount.Kernel` has no dependencies and can be used by any layer.

![Layer Responsibilities](images/layer_responsibilities.svg)

Architecture conformance is enforced automatically by `SchoolAccount.ConformanceTests` using NetArchTest. Never delete 
or disable these tests to resolve a violation, fix the dependency instead.

| Project                               | Contains                                                                                                             |
|---------------------------------------|----------------------------------------------------------------------------------------------------------------------|
| `SchoolAccount.Domain`                | Entities, Value Objects, Domain Events, Domain Exceptions                                                            |
| `SchoolAccount.Application`           | CQRS Commands/Queries and handlers, pipeline behaviours, FluentValidation validators, Application-layer abstractions |
| `SchoolAccount.Infrastructure`        | EF Core DbContext and entity configurations, external service clients, background jobs                               |
| `SchoolAccount.Web.Connect`           | MVC controllers, Razor views, view models, middleware, authentication configuration                                  |
| `SchoolAccount.Kernel`                | Shared primitives — anything used across layers. Be judicious with what goes here.                                   |
| `SchoolAccount.Integration.DfESignIn` | DfE Sign-In authentication integration                                                                               |

See [Clean Architecture](clean-architecture.md) for more detail on the layer rules.

---

## Application - Feature Organisation

Features are organised by domain area under `SchoolAccount.Application/Features/`, then by a use case inside each area. 
Each use case gets its own subfolder containing all related classes.

```
SchoolAccount.Application/
└── Features/
    ├── Tasks/
    │   ├── Common/                         # Shared models reused across Task operations
    │   │   └── Labels/
    │   │       ├── SubTaskAvailabilityLabel.cs
    │   │       └── SubTaskDueDateLabel.cs
    │   ├── GetAll/                         # One folder per operation
    │   │   ├── GetAllTasksQuery.cs         # The CQRS query record
    │   │   ├── GetAllTasksHandler.cs       # The query handler
    │   │   ├── GetAllTasksResponse.cs      # The response record(s)
    │   │   └── GetAllTasksProjection.cs    # EF Core projection expression
    │   ├── GetById/
    │   │   ├── GetTaskByIdQuery.cs
    │   │   ├── GetTaskByIdHandler.cs
    │   │   ├── GetTaskByIdResponse.cs
    │   │   ├── GetTaskByIdErrors.cs        # Named errors for this operation
    │   │   ├── GetTaskByIdProjection.cs
    │   │   └── GetTaskByIdResponseEnricher.cs
    └── Shared/                             # Cross-feature shared types (use sparingly)
```

### Use case folder conventions

| File                        | Purpose                                                    |
|-----------------------------|------------------------------------------------------------|
| `*Query.cs` / `*Command.cs` | The CQRS record - carries the input                        |
| `*Handler.cs`               | Implements the use case; returns `Result<TResponse>`       |
| `*Response.cs`              | One or more response records for this operation            |
| `*Errors.cs`                | Static class of `Error` factory methods for this operation |
| `*Projection.cs`            | EF Core expressions to only returned required data         |
| `*CommandValidator.cs`      | FluentValidation validator for a command                   |

---

## Web.Connect - Feature Organisation

Features are organised by domain area under `SchoolAccount.Web.Connect/Features/`, then by a use case inside each area.
Each use case gets its own subfolder containing all related classes. Cross-feature UI lives under `Features/Shared/`.

```
SchoolAccount.Web.Connect/
├── Infrastructure/
│   ├── FeatureConvention.cs                # Tags each controller with its feature folder path
│   └── FeatureViewLocationExpander.cs      # Resolves views from /Features/{feature}/
│
└── Features/
    ├── _ViewImports.cshtml                 # Shared using directives for all feature views
    ├── _ViewStart.cshtml                   # Layout declaration for all feature views
    │
    ├── Shared/                             # Cross-feature UI components
    │   ├── Breadcrumb/                     # Breadcrumb model, action filter and _Breadcrumbs partial
    │   ├── Feedback/                       # Feedback controller + _Feedback partial rendered by the layout
    │   ├── Filter/                         # Filter view models, _Filtration and _Checkbox partials
    │   ├── Images/                         # SVG partials (ArrowRight, ChevronRight, DividerLine)
    │   ├── Layout/                         # _Layout.cshtml and layout partials
    │   ├── List/                           # ListItemViewModel + _ListItem partial
    │   └── Pagination/                     # PaginatedListViewModel, _PaginatedList and _Pagination partials
    │
    └── Tasks/
        ├── GetAll/
        │   ├── GetAllTasksController.cs    # Controller - the GetAll use case only
        │   ├── GetAllTasksViewModel.cs     # View model for the list view
        │   ├── GetAllTasksViewModelBuilder.cs
        │   ├── GetAllTasksRequest.cs       # Request binding model to pass parameters
        │   └── AllTasks.cshtml             # Razor view
        │
        └── GetById/
            ├── GetTaskByIdController.cs    # Controller - the GetById use case only
            ├── GetTaskByIdViewModel.cs     # View model (wraps Application response, adds display logic)
            ├── Task.cshtml                 # Razor view
            ├── _Tabs.cshtml                # Partial view
            ├── _Guidance.cshtml            # Partial view
            ├── _Subtasks.cshtml            # Partial view
            └── _RelatedTasks.cshtml        # Partial view
```

### How the pieces fit together

**One controller per use case**

Each use case folder has its own sealed controller with a primary constructor for handler injection and attribute
routing via `RouteConstants`. This keeps each file focused on a single use case.

```
GetAll/GetAllTasksController.cs    → public sealed class GetAllTasksController(...) { [HttpGet] GetAll() }
GetById/GetTaskByIdController.cs   → public sealed class GetTaskByIdController(...) { [HttpGet] GetById() }
```

**View resolution by convention**

Views are never referenced by full `~/Features/...` paths. `FeatureConvention` derives each controller's feature
folder from its namespace (the segments after `Features`, e.g. `Features.Tasks.GetAll` → `Tasks/GetAll`), and
`FeatureViewLocationExpander` substitutes it into the view location formats registered in `DependencyInjection.cs`:

```
/Features/{feature}/{view}.cshtml           # The controller's own feature folder
/Features/Shared/Layout/{view}.cshtml
/Features/Shared/{view}.cshtml
```

Actions return `View()` when the view file matches the action name, or a short view name when it differs:

```csharp
return View(viewModel);                 // DashboardController.Dashboard → Dashboard.cshtml
return View("AllTasks", tasksViewModel); // GetAllTasksController.GetAll → AllTasks.cshtml
```

**Partial views**

Partial views are prefixed with `_` and referenced by name, never by path:

- Partials in the same use case folder use the bare name: `<partial name="_Tabs"/>`
- Shared partials use their subfolder-qualified name, resolved via `/Features/Shared/{view}.cshtml`:
  `<partial name="List/_ListItem"/>`, `<partial name="Pagination/_PaginatedList"/>`

**View models**

Each use case's view model lives in its sub-folder alongside the view. View models convert the Application response and 
add display-specific logic i.e. computed properties, formatting, conditional flags. A `*ViewModelBuilder.cs` maps the
Application response to the view model.

**Request binding models**

`*Request.cs` files within a use case folder bind the request parameters. These values are mapped from the request 
into an application layer query record i.e. `GetAllTasksQuery`.
