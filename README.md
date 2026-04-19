# Lab 4: Fire Department Dispatch System Simulation (SKKM)

*(Note: While the source code and architectural logic are written entirely in English, the application's Graphical User Interface is in Polish).*

A desktop application (WPF) simulating the operations of the Municipal Commander's Dispatch Center (SKKM) of the State Fire Service in Kraków. The system automatically generates incidents on a map and dispatches the appropriate resources and personnel from the nearest Fire and Rescue Units (JRG).

## Tech Stack

* **Language:** C# (.NET)
* **User Interface:** WPF (Windows Presentation Foundation)
* **Real-Time Logic:** Custom simulation loop based on delta-time (dt) and `DispatcherTimer`.
* **Paradigms & Patterns:** Object-Oriented Programming (OOP), Event-Driven Architecture, Strategy, Iterator, State, Observer.

## Architecture & Design Patterns

### 1. Strategy Pattern
Each incident can have a different nature (e.g., Fire, Local Threat), requiring a different amount of resources.
* `PZStrategy` – Fire (*Pożar*), requires 3 fire trucks.
* `MZStrategy` – Local Threat (*Miejscowe Zagrożenie*), requires 2 fire trucks.

### 2. Iterator Pattern
A key logistical challenge is dispatching trucks from the unit closest to the incident. To solve this, a dedicated `UnitCollection` and a custom `ClosestUnitIterator` were created. It iterates through the JRG units strictly in order from closest to farthest relative to a specific incident point (utilizing dynamic sorting based on 2D vector distances).

### 3. State Pattern
Each fire truck possesses its own internal `ICarState` that fully dictates its behavior within the `Update` loop:
* `FreeState`: The truck is waiting at the base.
* `MovingState`: The truck is traveling to the destination (position interpolation over time).
* `WaitingState`: The truck has arrived at the scene but is waiting for the rest of the dispatched forces to arrive.
* `ActionState`: All assigned trucks have arrived; the rescue operation is in progress.
* `ReturningState`: The operation is complete (or it was a false alarm), and the truck is returning to its home unit.

### 4. Observer Pattern
Instead of continuously polling objects for their status, an event-driven architecture (`IObservable` / `IObserver`) was implemented:
* **Truck -> Incident:** When a truck changes its state (e.g., from `Moving` to `Waiting`), it notifies the incident it is assigned to. The incident counts the arriving trucks, and once all are on-site, it automatically switches them into `ActionState`.
* **Incident -> SKKM:** When all trucks finish their tasks and leave the scene, the incident sends a notification to the Main Dispatcher (SKKM) requesting to be removed from the map and cleared from memory.

## Simulation Flow & Mechanics

1. The generator spawns a new random event on the map (within the geographical boundaries of Kraków).
2. The SKKM determines the required resources for the event using its designated **Strategy**.
3. The system utilizes the **Iterator** to scan stations starting from the closest one, reserving available fire trucks.
4. Assigned trucks shift their **State** to traveling and begin moving toward the coordinates.
5. Once all required forces arrive, the incident may turn out to be a false alarm (trucks return immediately) or a real operation (the duration of the action is randomized).
6. Using the **Observer** pattern, the system automatically cleans up the map once all units have returned to their barracks.

### Real-Time Dashboard
The application features a clear, readable legend on the side panel. It provides real-time visual feedback, including:
* Colored dots symbolizing the specific type of incident.
* Dynamically changing colors of the fire truck icons depending on their current state (Traveling, Action, Returning).
* Live numerical tracking of available (free) trucks stationed at each respective Fire and Rescue Unit.
