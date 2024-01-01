# Resilience Project .NET

This project outlines the development and implementation of a resilient backend service designed to demonstrate the principles of robust API design, error handling and system resilience.

## ResilientApi

### Features:
- **Dedicated Package**: Maintains a separate package for the REST API to ensure separation of concerns.
- **CRUD Operations**: Provides complete CRUD operations for `Owner` and `Car` entities.
- **Simulated Random API Errors**: Implements simulated random API errors (e.g., error 500) to test the resilience of the system.

### Design and Architecture:
- **Singleton and Builder Patterns**: Implements Singleton and Builder patterns, particularly in client usage (e.g., C# HTTP Client).
- **Strategy Pattern**: Utilizes the Strategy pattern for creating resilient API calls, enhancing the robustness of client-server interactions.
- **Error Handling Middleware** Implements a custom error handling middleware to handle exceptions and errors.

## ResilientAPI.Data

### Features:
- **In-Memory Database**: Utilizes an in-memory database to simulate entity storage with two main entities: `Owner` and `Car`.
- **Code-First Approach**: The database schema for the in-memory database is created using a code-first methodology.
- **BaseRepository**: Encapsulates database operations within a `BaseRepository` class, adhering to the DRY (Don't Repeat Yourself) principle for streamlined entity operations.
- **Data Seeding**: Implements a seeder for initializing the database with preliminary data.

### Design Principles:
- **Separation of Concerns**: Carefully separates interface definitions for more straightforward mocking and testing.
- **SOLID Principles**: Throughout the development, SOLID principles are keenly followed to ensure a robust and maintainable codebase.
- **Additional Abstraction Layer**: Introduces an additional layer of abstraction over the in-memory database for enhanced modularity and encapsulation.

### ResilientApi.Tests
- **Unit Tests**: Implements unit tests for the `CarRepository` and `OwnerRepository` class to ensure the database operations are functioning as expected.
- **Integration Tests**: Implements integration tests for the `CarController` classes to ensure the API endpoints are functioning as expected.

## ResilientApiGateway

### Client with Implemented Resilience:
- **Poly 8 Strategies**: Employs Poly 8 library strategies to construct a resilient API, ensuring the client can gracefully handle and recover from various failure scenarios.
- **Controller Strategies**: The controller interacts with the API using different strategies, demonstrating the system's resilience in face of errors and downtimes.

### Additional Features:
- **Logging**: Implements comprehensive logging, particularly within the database layer, to aid in monitoring and debugging.
- **SOLID Principles**: Continued adherence to SOLID principles is evident in all layers of the application, promoting a well-structured and maintainable codebase.
---

By employing a combination of design patterns, SOLID principles and strategic error handling, this backend service exemplifies a resilient and maintainable system. 
The application's architecture and its implementation of Poly 8 strategies demonstrate an effective approach to building robust APIs capable of withstanding various failure modes.