# Lab 3: Virus Spread Simulation

A desktop application (GUI) simulating the real-time dynamics of a virus spreading within a closed environment. Individuals move across a 2D space and infect one another based on specific social distancing rules and exposure times.

## Tech Stack
- **Language:** C# (.NET)
- **User Interface:** WPF (Windows Presentation Foundation) using XAML.
- **Rendering:** `Canvas` and `DispatcherTimer` (simulation loop running at 25 FPS).
- **Serialization:** `System.Text.Json` (saving and loading the simulation state).
- **Paradigms & Patterns:** Object-Oriented Programming (OOP), State Pattern, Memento Pattern.

## Architecture & Design Patterns

### 1. State Pattern
Each health state is encapsulated in a separate class that dictates its own behavior and determines transitions to other states:
- **`SusceptibleState`:** Tracks contact time with infected individuals. Once the exposure time exceeds a specific threshold (and a probability check is passed), it transitions into the `InfectedState`.
- **`InfectedState`:** Counts down the time until recovery. It exists in two variants (symptomatic and asymptomatic), which directly impacts the probability of infecting others. Once the timer elapses, it automatically transitions into the `ImmuneState`.
- **`ImmuneState`:** Completely ignores further contacts with infected individuals.

The `Person` class acts as the context, simply delegating the `Update()` and `HandleContact()` methods to its current active state.

### 2. Memento Pattern
The application features a robust Save/Load system that can capture the simulation at any exact moment, implemented via the Memento pattern:
- Objects (such as `Person` entities or specific health states) generate their own mementos (`PersonMemento`, `StateMemento`), which act purely as Data Transfer Objects (DTOs).
- Internal object states (e.g., private countdown timers or exposure tracking dictionaries) are not exposed publicly via properties; instead, they are strictly encapsulated within the Memento.
- The main engine collects these mementos into a comprehensive `SimulationSnapshot` and serializes it to a JSON file. Upon loading, objects are seamlessly reconstructed using their respective Mementos.

## Simulation Mechanics

The `SimulationEngine` recalculates the positions of all individuals every frame using 2D vectors.
- **Movement:** Features 2D physics with bounding box collisions (individuals bounce off the room's walls). Random perturbations are frequently applied to the velocity vectors to simulate natural, unpredictable wandering.
- **Population Rotation:** Individuals lingering near the edges of the simulation have a chance to leave the room (`ShouldLeave`), and new, healthy individuals spawn to replace them, keeping the simulation dynamic.

## User Interface

The application features an interactive side panel that allows the user to dynamically control simulation parameters while the engine is running. 
Adjustable parameters include:
- Total population size
- Simulated room dimensions
- Initial immunity rate of the population
- Percentage of initially infected individuals
- Saving and loading simulation states via `.json` files
