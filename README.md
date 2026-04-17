# Symulator Kolonii Mrówek

Aplikacja okienkowa symulująca życie kolonii mrówek, poszukiwanie pożywienia oraz unikanie zagrożeń.

## Wykorzystany Tech Stack
- **Język:** C# (.NET)
- **Interfejs Użytkownika:** WPF (Windows Presentation Foundation)
- **Architektura UI:** MVVM (Model-View-ViewModel) z wykorzystaniem `INotifyPropertyChanged` i bindowania danych (Data Binding).
- **Wzorce Projektowe (Design Patterns):**
  - *Behawioralne:* State (Stan), Strategy (Strategia), Observer (Obserwator), Memento (Pamiątka), Iterator.
  - *Strukturalne:* Decorator (Dekorator), Adapter.

## Architektura i Wzorce Projektowe

### 1. Architektura MVVM
Aplikacja oddziela logikę symulacji od warstwy widoku. 
- Obiekty symulacji nie mają pojęcia o elementach interfejsu. Posiadają jedynie swoje reprezentacje `ViewModel` (np. `EntityViewModel`), które powiadamiają interfejs o zmianach.
- Panel boczny sterowany jest za pomocą `MainViewModel` i wstrzykiwanych komend (`RelayCommand`).

### 2. State & Strategy (Połączenie)
Inteligencja każdej mrówki (`AntContext`) opiera się na dwóch ściśle współpracujących wzorcach:
- **State (Stan):** Decyduje *co* mrówka obecnie robi (np. `ForagingState` - szuka jedzenia, `ReturningState` - niesie jedzenie do mrowiska, `FleeingState` - ucieka przed zagrożeniem).
- **Strategy (Strategia):** Decyduje *jak* mrówka się porusza. Stany delegują obliczanie wektorów ruchu do odpowiednich strategii (`WanderStrategy` wykorzystuje feromony i błądzenie losowe, `FleeStrategy` oblicza wektor ucieczki od drapieżnika).

### 3. Decorator (Dekorator)
Wygląd i atrybuty fizyczne mrówki modyfikowane są w locie za pomocą dekoratorów implementujących `IAntAppearance`.
Kiedy mrówka podnosi jedzenie, "owijana" jest w `CarrierAppearanceDecorator`, co natychmiast zmienia jej kolor w interfejsie (na zielony) oraz nakłada modyfikator spowalniający prędkość poruszania się. Po zrzuceniu jedzenia w mrowisku, dekorator jest zdejmowany.

### 4. Observer (Obserwator)
Mechanika czasu rzeczywistego (Update Tick). Główny silnik `World` pełni rolę `ISimulationSubject`. Co każdą klatkę symulacji powiadamia on subskrybentów (w tym `MainViewModel`) o konieczności odświeżenia danych (np. zaktualizowania statystyk populacji i ilości zebranego pożywienia).

### 5. Memento (Pamiątka)
System Save/Load. Zamiast naruszać hermetyzację obiektów, świat symulacji potrafi wygenerować `ColonyMemento` zawierające bezpieczne migawki danych (np. `AntSnapshot`). Pozwala to na zamrożenie stanu symulacji i przywrócenie go w późniejszym czasie.

### 6. Iterator
Do przeglądania całego świata (np. w celu aktualizacji wszystkich obiektów na ekranie) zaimplementowano `WorldIterator`. Posiada on zdolność płynnego iterowania po zupełnie różnych kolekcjach listowych (mrówki, zagrożenia, pożywienie) pod jedną, spójną abstrakcją `IWorldIterator`.

### 7. Adapter
Dla zachowania czystości kodu, wprowadzono `UIConfigAdapter`. Jego zadaniem jest przetłumaczenie parametrów podanych przez użytkownika w suwakach i polach interfejsu WPF na bezpieczny obiekt `SimulationConfig` wymagany przez konstruktor świata.

## Mechanika Symulacji
- **Feromony:** Mrówki wracające z jedzeniem zostawiają ślad feromonowy. Inne mrówki w stanie błądzenia wykrywają te ślady i modyfikują swój wektor ruchu, by podążać za "zapachem", co naturalnie tworzy szlaki transportowe. Ślady z czasem słabną (wyparowują).
- **Zagrożenia:** Po mapie poruszają się czerwone, większe kule odbijające się od krawędzi mapy. Gdy mrówka wykryje zagrożenie w swoim promieniu, porzuca dotychczasowe zadania, rzuca pożywienie i przechodzi w stan ucieczki.
- **Ekonomia Kolonii:** Znoszenie pożywienia zwiększa zasoby mrowiska. Gdy zgromadzi się odpowiednia ilość jedzenia, mrowisko tworzy nowe mrówki. Z drugiej strony, powrót do mrowiska konsumuje jedną porcję żywności (symulacja żywienia mrówki) - jeśli jedzenia zabraknie, mrówka umiera.
