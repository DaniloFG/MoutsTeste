# DeveloperStore Team

This project serves as an evaluation for senior developer candidates, designed to assess various skills and competencies, including C#, .NET 8, layer separation, databases (PostgreSQL & MongoDB), design patterns (Mediator, Domain-Driven Design), unit testing, API design, containerization (Docker), and deployment (Kubernetes).

## Tech Stack & Frameworks

-   **Backend**: .NET 8, C#
-   **Patterns**: Minimal API, CQRS, Rich Domain Model, Event-Driven, SOLID
-   **Frameworks**:
    -   MediatR (for Mediator/CQRS pattern)
    -   AutoMapper (for object mapping)
    -   Entity Framework Core (for ORM with PostgreSQL)
-   **Databases**: PostgreSQL (Write Model), MongoDB (Read Model/Secondary Store)
-   **Testing**: xUnit, Moq, FluentAssertions
-   **Data Generation**: Bogus
-   **Containerization**: Docker
-   **Orchestration**: Kubernetes

## Architecture

The project follows a Clean Architecture approach with event-driven principles for data synchronization between databases.

-   **Domain**: Contains the core business logic, entities, and domain events. It has no external dependencies.
-   **Application**: Orchestrates the domain layer. It contains CQRS handlers, DTOs, and event handlers.
-   **Infrastructure**: Implements interfaces from upper layers, handling database access (EF Core for Postgres, MongoDB Driver), and other external concerns.
-   **Api**: The presentation layer, using .NET Minimal APIs to expose endpoints.