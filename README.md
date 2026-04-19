# Lab 1: Currency Calculator (NBP API)

A console-based currency exchange application that fetches live exchange rates from the National Bank of Poland (NBP) API. 

The primary goal of this project was not just to build a functional calculator, but to design it in a fully object-oriented manner, ensuring high testability and flexibility for future extensions.

## Tech Stack
- **Language:** C# (.NET)
- **Network Communication:** `HttpClient` (asynchronous HTTP requests)
- **Data Processing:** `System.Xml` (XML document parsing)
- **Paradigms:** Asynchronous Programming (TPL - Task Parallel Library), Object-Oriented Programming (OOP)

## Architecture & Design Patterns

The codebase is divided into logical layers, heavily relying on interfaces. As a result, the project strictly adheres to SOLID principles.

### Key concepts implemented in the codebase:

1. **Command Pattern**
   Instead of a massive `switch` statement containing logic within the main controller, each user command is a separate class (e.g., `ExchangeAction`, `UpdateAction`, `CurrenciesAction`) implementing an `Action` interface. This facilitates adding new features without modifying existing code (adhering to the Open/Closed Principle).

2. **Dependency Injection**
   The `ExchangeController` class does not instantiate its own dependencies (such as data fetching or UI components). Instead, they are "injected" via the constructor at the application's entry point (`Program.cs` - the Composition Root). This makes the controller class highly testable using mocks.

3. **Separation of Concerns (Data & Presentation Layers)**
   - **`RemoteRepository` (Interface):** Responsible solely for fetching raw bytes from the network. The `Rest` implementation utilizes `HttpClient`.
   - **`Document` (Interface):** Responsible purely for processing raw text into the domain object (`ExchangeTable`). The `XML` implementation decodes the specific NBP data format.
   - **`UserInterface` (Interface):** Decouples business logic from data presentation. The `ConsoleUI` implementation strictly handles terminal interaction. Migrating to a graphical interface (like WPF or WinForms) would only require writing a new implementation of this interface, without ever touching the core controller.

## Available Features (Commands)

Upon launching the application, the user is presented with an interactive command-line interface supporting the following commands:

- `help` - Displays a list of available commands.
- `currencies` - Prints an alphabetical list of all available currencies (ISO codes) successfully loaded from the API.
- `update` - Forces an asynchronous fetch of the latest XML exchange rate table directly from the NBP servers.
- `exchange` - Initiates the conversion process. It sequentially prompts the user for the source currency, target currency, and amount, automatically validating the inputted currency codes.
- `exit` - Safely terminates the program.
