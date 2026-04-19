# Ant Colony Simulator

A desktop application simulating the life of an ant colony, focusing on foraging for food and avoiding threats.

## Tech Stack
- **Language:** C# (.NET)
- **User Interface:** WPF (Windows Presentation Foundation)
- **UI Architecture:** MVVM (Model-View-ViewModel) utilizing `INotifyPropertyChanged` and Data Binding.
- **Design Patterns:**
  - *Behavioral:* State, Strategy, Observer, Memento, Iterator.
  - *Structural:* Decorator, Adapter.

## Architecture & Design Patterns

### 1. MVVM Architecture
The application strictly separates simulation logic from the view layer. 
- Simulation entities are completely unaware of UI elements. They only possess their `ViewModel` representations (e.g., `EntityViewModel`), which notify the interface of state changes.
- The side control panel is managed via the `MainViewModel` and injected commands (`RelayCommand`).

### 2. State & Strategy (Combined)
The intelligence of each ant (`AntContext`) relies on two tightly collaborating patterns:
- **State:** Determines *what* the ant is currently doing (e.g., `ForagingState` - looking for food, `ReturningState` - carrying food back to the anthill, `FleeingState` - escaping a threat).
- **Strategy:** Determines *how* the ant moves. States delegate the calculation of movement vectors to appropriate strategies (e.g., `WanderStrategy` utilizes pheromones and random wandering, while `FleeStrategy` calculates an escape vector away from a predator).

### 3. Decorator
The physical attributes and visual appearance of an ant are modified on the fly using decorators that implement `IAntAppearance`.
When an ant picks up food, it is "wrapped" in a `CarrierAppearanceDecorator`, which instantly changes its UI color (to green) and applies a modifier that reduces its movement speed. Once the food is dropped off at the anthill, the decorator is removed.

### 4. Observer
Real-time mechanics (Update Tick). The main `World` engine acts as the `ISimulationSubject`. On every simulation frame, it notifies its subscribers (including `MainViewModel`) to refresh their data (e.g., updating population statistics and total gathered food).

### 5. Memento
Save/Load system. Instead of breaking object encapsulation, the simulation world can generate a `ColonyMemento` containing safe data snapshots (e.g., `AntSnapshot`). This allows the simulation state to be frozen and completely restored at a later time.

### 6. Iterator
To traverse the entire world (e.g., to update all objects on the screen), a `WorldIterator` was implemented. It seamlessly iterates over completely different list collections (ants, threats, food items) under one unified abstraction (`IWorldIterator`).

### 7. Adapter
To maintain clean code, a `UIConfigAdapter` was introduced. Its purpose is to translate user inputs from WPF sliders and text fields into a safe, strongly-typed `SimulationConfig` object required by the world constructor.

## Simulation Mechanics
- **Pheromones:** Ants returning with food leave a pheromone trail. Other ants in the wandering state detect these trails and modify their movement vectors to follow the "scent," naturally establishing supply routes. Pheromone trails gradually weaken (evaporate) over time.
- **Threats:** Large red spheres bounce off the map edges, acting as predators. When an ant detects a threat within its radius, it abandons its current task, drops any carried food, and transitions into a fleeing state.
- **Colony Economy:** Delivering food increases the anthill's resources. Once enough food is gathered, the colony spawns new ants. Conversely, returning to the anthill consumes one food portion (simulating the ant feeding itself)—if the colony runs out of food, the ant dies.
