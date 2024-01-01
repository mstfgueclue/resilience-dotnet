# Getting Started
This project demonstrates how to implement a resilient API using the `Polly` library. Below is an overview of the project structure.
![ResilientApi-Architecture](https://github.com/mstfgueclue/resilience-dotnet/assets/64474817/1a4e3fa2-568d-4951-96b3-a025e0abd012)

## How to Run
1. Clone the repository.
2. Open the solution in Visual Studio or Rider
3. Run the `ResilientApi` project.
4. Run the `ResilientApiGateway` project.
5. Navigate to the Swagger UI for the `ResilientApiGateway` project at 
   - `http://localhost:5224/swagger/index.html` or 
   - `https://localhost:7217/swagger/index.html`
6. Navigate to the Swagger UI for the `ResilientApi` project at 
   - `http://localhost:5282/swagger/index.html` or 
   - `https://localhost:7266/swagger/index.html`
7. Use the `ResilientApiGateway` Swagger UI to perform READ operations on the `Car` entity.
   - Note that you can trigger a manual error by using the `GET: api/cars/{id}/with-random-error` endpoint when you enter `-1`otherwise it will randomly cause error.
8. Use the `ResilientApi` Swagger UI to perform CRUD operations on the `Owner` and `Car` entities.


## ResilientApiGateway
The project `ResilientApiGateway` is the main project that contains the client implementation of the resilient API. 
The project is a `ASP.NET Core Web Api (8.0 Framework)` that can be run locally. `ResilientApiGateway` communicates with the `ResilientApi` project to perform READ operations on the `Car` entity to demonstrate the resilience of the system.

### Swagger UI (API Documentation)
![2024-01-01_17-08-46](https://github.com/mstfgueclue/resilience-dotnet/assets/64474817/1fd87aad-cd06-43e5-885a-340890be148f)

## ResilientApi
The project `ResilientApi` is the main project that contains the REST API implementation. 
The project is a `ASP.NET Core Web Api (8.0 Framework)` that can be run locally.The business logic of `ResilientApi` is contained within the `ResilientApi.Data` class library project. 
There is one specific endpoint that is used by the `ResilientApiGateway` project to demonstrate the resilience of the system. 
It is the `GET: api/cars/{id}/-with-random-error` endpoint for the `Car` entity.

### Swagger UI (API Documentation)
![2024-01-01_17-08-33](https://github.com/mstfgueclue/resilience-dotnet/assets/64474817/a02f1d55-a135-490b-b145-289763896310)

---
