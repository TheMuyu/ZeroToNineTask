#### Design Choices;
- The design of the Loan Application API was guided by DDD principles and onion architecture to ensure a modular, maintainable, and production-realistic solution. 
- Key decisions included separating the solution into Domain, Application, Infrastructure, and Api layers, with the Domain layer encapsulating business logic and entities like LoanApplication with immutable properties (e.g., Id with private set) to enforce integrity. 
- The use of MediatR for CQRS was for a clean separation of commands and queries.
- Middleware was implemented for global exception handling, returning standardized 400 and 500 responses, and dependency injection was leveraged to wire services, enhancing testability and scalability. 
- Unit tests were developed using xUnit and Moq to validate key components like command and query handlers.


#### With more time, several improvements could be made. 
- Replacing the static in-memory list with a proper database.
- Adding integration tests to verify end-to-end API behavior and enhancing unit tests with more edge cases. 
- Implementing async communication mechanisms, such as integrating a message queue.
- With a logging framework would enhance traceability in a production environment. 
- API versioning to support future enhancements.
- Implementing basic rate limiting to prevent abuse.
- Adding Swagger/OpenAPI documentation to improve developer experience. 