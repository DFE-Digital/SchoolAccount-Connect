# Coding and Repository Standards

Standards for contributing to SchoolAccount Connect. They reflect the patterns already established in the codebase rather than aspirational rules.

---

## Repository

### Branches

Branch from `main` using one of these prefixes:

```
feature/<short-description>
bugfix/<short-description>
task/<short-description>
```

### Pull Requests

- Open PRs against `main` in [Azure DevOps](https://dfe-gov-uk.visualstudio.com/s268-schoolaccount/_git/SchoolAccount-Connect/pullrequests).
- Fill in the PR template checklist items
- Include screenshots or GIFs for any visible UX change.

### Commits

- Keep commit messages in the imperative present tense: _Add task detail caching_, not _Added_ or _Adds_.
- Prefer the why over the what
- Add more information using the description

#### Good example

```text
Ensure pages are valid before persistence

Pages are currently saved to the database with no validation
leading to incorrect data and errors upon retrieval 

Work Item #12345
```

See more guidance and examples [here](https://dfe-gov-uk.visualstudio.com/s268-schoolaccount/_wiki/wikis/s268-schoolaccount.wiki/15561/Git-Commit-Messages)

---

## Formatting

CSharpier is enforced via Husky pre-commit hooks (installed by `init.sh` / `init.ps1`). It runs automatically on every 
commit, do not bypass it with `--no-verify`.

| Rule            | Value          |
|-----------------|----------------|
| Indent          | 4 spaces       |
| Max line length | 120 characters |

Do not adjust `.editorconfig` or CSharpier settings without team agreement.

---

## Project Structure

The solution follows Clean Architecture. Dependencies flow inward only:

```
Domain ← Application ← Infrastructure
                    ↑
              Presentation (Web.Connect)
```

`SchoolAccount.Kernel` sits outside the ring - it has no dependencies and may be referenced by any layer.

Architecture conformance is verified automatically by `SchoolAccount.ConformanceTests`. These tests must not be excluded
or deleted to work around a violation.

### Layer responsibilities

| Project                               | Contains                                                                                                                   |
|---------------------------------------|----------------------------------------------------------------------------------------------------------------------------|
| `SchoolAccount.Domain`                | Entities, Value Objects, Domain Events, Domain Exceptions                                                                  |
| `SchoolAccount.Application`           | CQRS Commands/Queries and their handlers, pipeline behaviours, FluentValidation validators, Application-layer abstractions |
| `SchoolAccount.Infrastructure`        | EF Core DbContext and entity configurations, external service clients, background jobs                                     |
| `SchoolAccount.Web.Connect`           | MVC controllers, Razor views, view models, middleware, authentication configuration                                        |
| `SchoolAccount.Kernel`                | Shared primitives, anything used across layers, be judicious with what goes here                                           |
| `SchoolAccount.Integration.DfESignIn` | DfE Sign-In authentication integration                                                                                     |

See more about layers at [Clean Architecture](clean-architecture.md)

---

## Feature Organisation

Application features use [package by feature](https://www.milanjovanovic.tech/blog/clean-architecture-the-missing-chapter#package-by-feature) 
under `SchoolAccount.Application/Features/`. Each feature operation gets its own sub-folder containing all related 
classes:

```
Features/
└── Tasks/
    └── GetById/
        ├── GetTaskByIdQuery.cs
        ├── GetTaskByIdHandler.cs
        ├── GetTaskByIdResponse.cs
        ├── GetTaskByIdErrors.cs
        └── GetTaskByIdProjection.cs
```

We have also applied a feature-based organisation with the presentation layer which is an MVC project `SchoolAccount.Web.Connect` 
You can find a more detailed explanation at [Project Structure](project-structure.md)

One file per class as a rule, the only exception to this is in the response classes. 
Name files after their feature action.

---

## CQRS Pattern

Every use case is either a **Command** (mutates state) or a **Query** (reads state).

### Queries

```csharp
public sealed record GetTaskByIdQuery(long Id) : IQuery<GetTaskByIdResponse>;

public sealed class GetTaskByIdHandler(IApplicationDbContext db, IOrganisationContext org)
    : IQueryHandler<GetTaskByIdQuery, GetTaskByIdResponse>
{
    public async Task<Result<GetTaskByIdResponse>> Handle(
        GetTaskByIdQuery query, CancellationToken cancellationToken)
    { ... }
}
```

- `IQuery<TResponse>` and `IQueryHandler<TQuery, TResponse>` from `SchoolAccount.Application.Abstractions.Messaging`.
- Handlers are `sealed` and use primary constructor injection.
- Queries return `Result<TResponse>`.

### Commands

Follow the same pattern with `ICommand` / `ICommand<TResponse>` and the corresponding handler interface.

### Error definitions

Errors for a feature live in a dedicated `*Errors` static class:

```csharp
public static class GetTaskByIdErrors
{
    public static Error NotFound(long taskId) =>
        Error.NotFound("Task.NotFound", $"The task with the Id = '{taskId}' was not found");
}
```

Use `Error.NotFound`, `Error.Validation`, `Error.Conflict`, or `Error.Failure` from `SchoolAccount.Kernel`.

### Result pattern

Return `Result<T>` from handlers; never throw for expected business failures.

```csharp
// success
return Result.Success(response);

// failure
return Result.Failure<GetTaskByIdResponse>(GetTaskByIdErrors.NotFound(query.Id));
```

Controllers unpack `Result<T>` - they do not inspect domain logic directly.

---

## C# Conventions

### Types

- Use `sealed` on concrete classes and records unless inheritance is required.
- Prefer `record` for DTOs, responses, and value objects; `record struct` for small value types.
- Use `init`-only properties on records.

### Naming

| Element           | Convention                  | Example                             |
|-------------------|-----------------------------|-------------------------------------|
| Interfaces        | `I` prefix                  | `IApplicationDbContext`             |
| Generics          | Descriptive parameter names | `Result<TResponse>`                 |
| Constants class   | `*Constants`                | `ApplicationConstants`              |
| Error class       | `*Errors`                   | `GetTaskByIdErrors`                 |
| Projection class  | `*Projection`               | `GetTaskByIdProjection`             |
| Test classes      | `*Tests`                    | `ProviderAuthorisationHandlerTests` |

### Dependency Injection

Classes are registered via Scrutor assembly scanning in each layer's `DependencyInjection.cs`. Add new services to the 
relevant scan block rather than registering them individually unless there is a specific reason.

### Null safety

- Enable nullable reference types - they are on by default in this solution.
- Initialise collection properties to `[]` rather than `null`.
- Use `string.Empty` rather than `""` for empty string defaults on record properties.

### Comments

Write comments only when the **why** is non-obvious - a hidden constraint, a workaround, or a subtle invariant. 
Do not describe what the code does; well-named identifiers and method names do that.

---

## Validation

Input validation uses **FluentValidation** with pipeline behaviours - do not validate inside handlers. 
Validators live in the same feature folder as the command they validate.

Presentation-layer model validation is handled by dedicated validators in `SchoolAccount.Web.Connect/Validation/Validators/`.

---

## Logging

Use **Serilog** structured logging. Log correlation IDs and user context where appropriate. Do not log sensitive 
personal data.

When running via Docker Compose, logs are viewable in Seq at `http://localhost:5341`.

---

## Testing

### Test projects

| Project | What it tests |
|---|---|
| `SchoolAccount.Application.UnitTests` | Application handlers and business logic in isolation |
| `SchoolAccount.Web.Connect.UnitTests` | View model builders, helpers, and presentation logic |
| `SchoolAccount.Application.IntegrationTests` | Application layer against a real database |
| `SchoolAccount.InfrastructureTests` | Infrastructure implementations against a real database |
| `SchoolAccount.IntegrationTests` | Full HTTP integration tests via `WebApplicationFactory` |
| `SchoolAccount.AuthenticationTests` | Authentication and authorisation handlers |
| `SchoolAccount.ConformanceTests` | Architecture layer dependency rules (NetArchTest) |
| `SchoolAccount.Tests.Common` | Shared builders, fakes, and test helpers |

### Test naming

Name tests as plain English sentences that describe the behaviour being verified not the method under test. 
Use underscores to separate words. Omit the name of the method being called; focus on what the application does, not how.

```csharp
// Avoid - encodes implementation detail, hard to read
public async Task ShouldFailWhenNoProvider() { ... }

// Prefer - reads like a behaviour statement
public async Task No_provider_throws_exception() { ... }
public async Task Valid_provider_succeeds_authorisation() { ... }
public async Task Delivery_with_a_past_date_is_invalid() { ... }
```

See [You are naming your tests wrong](https://enterprisecraftsmanship.com/posts/you-naming-tests-wrong/) for the full 
rationale and more examples.

### Key libraries

| Library                   | Purpose                      |
|---------------------------|------------------------------|
| XUnit v3                  | Test framework               |
| NSubstitute               | Mocking                      |
| AwesomeAssertions         | Fluent assertions            |
| Bogus                     | Fake data generation         |
| NetArchTest.Rules         | Architecture rule assertions |
| MockQueryable.NSubstitute | Mocking EF Core `IQueryable` |

### Arrange / Act / Assert

Structure every test with `// Arrange`, `// Act`, `// Assert` sections separated by a blank line.

### Database tests

Integration and infrastructure tests hit a **real database** - do not mock `IApplicationDbContext` in these test projects.
See existing `IClassFixture` setups in `SchoolAccount.Tests.Common` for the correct base class.

### Builders

Use builder classes from `SchoolAccount.Tests.Common/Builders/` to construct test entities and view models. 
Add new builders there rather than duplicating construction logic across tests.

---

## Packages

Package versions are centralised in `Directory.Packages.props`. Never specify a `Version` attribute on a 
`<PackageReference>` in a `.csproj` file - add or update the version in `Directory.Packages.props` instead.