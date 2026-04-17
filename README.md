# Lab 2: Biblioteka Wektorów

Projekt zawierający własną implementację wektorów 2D i 3D z podstawowymi funkcjonalnościami, do użycia w kolejnych projektach.

## Wykorzystany Tech Stack
- **Język:** C# (.NET)
- **Paradygmaty:** Object-Oriented Programming (OOP), Composition over Inheritance
- **Wzorce projektowe:** Adapter (Wrapper), Decorator (Dekorator)

## Architektura i Wzorce Projektowe

W ramach projektu zaimplementowano bazową klasę wektora dwuwymiarowego (`Vector2D`) opartego na współrzędnych kartezjańskich. Następnie jej funkcjonalność została rozszerzona na dwa zupełnie różne sposoby (dziedziczenie vs kompozycja), co pozwala na bezpośrednie porównanie obu podejść.

### 1. Wzorzec Adapter (Adapter Pattern)
Wektor 2D można reprezentować na dwa sposoby: kartezjańsko (x, y) oraz biegunowo (kąt, promień). Zamiast dodawać logikę biegunową bezpośrednio do wektora kartezjańskiego, wykorzystano wzorzec Adaptera:
- **`Polar2DInheritance`:** Rozwiązanie sztywne. Klasa dziedziczy po `Vector2D` i dodaje metodę `getAngle()`.
- **`Polar2DAdapter`:** Rozwiązanie elastyczne. Klasa "owija" (wraps) dowolny obiekt implementujący `IVector` i adaptuje go do interfejsu `IPolar2D`. Pokazuje to, jak w łatwy sposób tłumaczyć jeden interfejs na drugi bez głębokich hierarchii dziedziczenia.

### 2. Wzorzec Dekorator (Decorator Pattern)
Rozszerzenie wektora z przestrzeni 2D do 3D (dodanie osi Z i iloczynu wektorowego `cross`).
- **`Vector3DInheritance`:** Ponownie, sztywne podejście klasyczne.
- **`Vector3DDecorator`:** Dekorator implementujący interfejs `IVector`, który wewnątrz przechowuje referencję do innego `IVector` (bazowego wektora 2D) i dodaje do niego wymiar `Z`. Pozwala to na dynamiczne "udekorowanie" dowolnego wektora dwuwymiarowego w czasie działania programu, zgodnie z zasadą Otwarty-Zamknięty (Open/Closed Principle).
