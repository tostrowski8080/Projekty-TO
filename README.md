# Lab 3: Symulacja Rozprzestrzeniania się Wirusa

Okienkowa aplikacja (GUI) symulująca w czasie rzeczywistym dynamikę rozprzestrzeniania się wirusa w zamkniętym pomieszczeniu. Ludzie poruszają się po dwuwymiarowej przestrzeni i zarażają się na podstawie określonych reguł dystansu społecznego i czasu ekspozycji.

## Wykorzystany Tech Stack
- **Język:** C# (.NET)
- **Interfejs Użytkownika:** WPF (Windows Presentation Foundation) z wykorzystaniem XAML.
- **Renderowanie:** `Canvas` oraz `DispatcherTimer` (pętla symulacji działająca w 25 FPS).
- **Serializacja:** `System.Text.Json` (zapis i odczyt stanu symulacji).
- **Paradygmaty i Wzorce:** Object-Oriented Programming (OOP), State (Stan), Memento (Pamiątka).

## Architektura i Wzorce Projektowe

### 1. Wzorzec Stan (State Pattern)
Każdy stan to osobna klasa, która sama decyduje o swoim zachowaniu i przejściach w inne stany:
- **`SusceptibleState` (Podatny):** Śledzi czas kontaktu z zarażonymi. Po przekroczeniu progu czasowego (oraz wylosowaniu szansy), zmienia stan na `InfectedState`.
- **`InfectedState` (Zarażony):** Odlicza czas do wyzdrowienia. Występuje w dwóch wariantach (z symptomami lub bez), co wpływa na szansę zarażenia innych. Po upływie czasu automatycznie zmienia się na `ImmuneState`.
- **`ImmuneState` (Odporny):** Ignoruje kontakty z zarażonymi.

Klasa `Person` po prostu deleguje metody `Update()` i `HandleContact()` do swojego aktualnego stanu.

### 2. Wzorzec Pamiątka (Memento Pattern)
Aplikacja posiada funkcjonalność zapisu i wczytywania (Save/Load) w dowolnym momencie. Zrealizowano to używając wzorca Memento:
- Obiekty (takie jak `Person` czy konkretne stany zdrowia) generują swoje memento (`PersonMemento`, `StateMemento`), które są czystymi klasami DTO (Data Transfer Object).
- Wewnętrzny stan obiektów (np. prywatne liczniki czasu czy słowniki ekspozycji) nie jest wystawiany publicznie przez properties, lecz zamykany wewnątrz Memento.
- Główny silnik zbiera je w `SimulationSnapshot` i serializuje do pliku JSON. Przy wczytywaniu, obiekty są odtwarzane na podstawie dostarczonych Pamiątek.

## Mechanika Symulacji

Silnik symulacji (`SimulationEngine`) w każdej klatce oblicza nową pozycję ludzi z wykorzystaniem wektorów 2D.
- **Ruch:** Fizyka 2D z odbijaniem się od ścian pomieszczenia. Zaimplementowano również losowe zaburzenia (perturbacje) wektora prędkości, co symuluje naturalny spacer.
- **Rotacja populacji:** Ludzie przy krawędziach mają szansę opuścić pomieszczenie (`ShouldLeave`), a na ich miejsce pojawiają się nowi ludzie.

## Interfejs

Aplikacja posiada panel boczny, z którego użytkownik może kontrolować parametry symulacji w czasie jej trwania.
Możliwe jest dostosowanie:
- Rozmiaru populacji
- Rozmiaru symulowanego pomieszczenia
- Współczynnika początkowej odporności
- Odsetka początkowo zakażonych
- Zapisywania i wczytywania stanu z plików `.json`.
