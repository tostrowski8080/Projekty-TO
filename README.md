# Lab 2: Vector Library

A project featuring a custom implementation of 2D and 3D vectors with core functionalities, designed to be used as a foundational library in subsequent projects.

## Tech Stack
- **Language:** C# (.NET)
- **Paradigms:** Object-Oriented Programming (OOP), Composition over Inheritance
- **Design Patterns:** Adapter (Wrapper), Decorator

## Architecture & Design Patterns

The project establishes a base two-dimensional vector class (`Vector2D`) utilizing Cartesian coordinates. Its functionality is then extended using two distinctly different approaches (Inheritance vs. Composition), allowing for a direct, practical comparison of both methodologies.

### 1. Adapter Pattern
A 2D vector can be mathematically represented in two ways: Cartesian (x, y) and polar (angle, radius). Rather than bloating the Cartesian vector with direct polar logic, the Adapter pattern was employed:
- **`Polar2DInheritance`:** The rigid approach. This class directly inherits from `Vector2D` and adds a `getAngle()` method.
- **`Polar2DAdapter`:** The flexible approach. This class wraps any object that implements the `IVector` interface and adapts it to satisfy the `IPolar2D` interface. This demonstrates how to seamlessly translate one interface into another without relying on deep, brittle inheritance hierarchies.

### 2. Decorator Pattern
Extending the vector from 2D to 3D space (which introduces the Z-axis and the `cross` product).
- **`Vector3DInheritance`:** Again, the traditional, rigid inheritance-based approach.
- **`Vector3DDecorator`:** A decorator that implements the `IVector` interface. It internally holds a reference to another `IVector` (the base 2D vector) and wraps it to add the `Z` dimension. This allows any 2D vector to be dynamically "decorated" into a 3D vector at runtime, adhering strictly to the Open/Closed Principle.
