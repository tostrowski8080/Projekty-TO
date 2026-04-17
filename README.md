# Lab 4: Symulacja Systemu Dyspozytorskiego Straży Pożarnej (SKKM)

Okienkowa aplikacja (WPF) symulująca pracę Stanowiska Kierowania Komendanta Miejskiego (SKKM) Państwowej Straży Pożarnej w Krakowie. System automatycznie generuje incydenty na mapie, a następnie dysponuje do nich odpowiednie siły i środki z najbliższych Jednostek Ratowniczo-Gaśniczych (JRG).

## 💻 Wykorzystany Tech Stack
- **Język:** C# (.NET)
- **Interfejs Użytkownika:** WPF (Windows Presentation Foundation)
- **Logika Czasu Rzeczywistego:** Własna pętla symulacji oparta na delta-time (`dt`) i `DispatcherTimer`.
- **Paradygmaty i Wzorce:** Object-Oriented Programming (OOP), Event-Driven Architecture, Strategy (Strategia), Iterator, State (Stan), Observer (Obserwator).

## Architektura i Wzorce Projektowe

### 1. Wzorzec Strategia (Strategy Pattern)
Każdy incydent może mieć inny charakter (Pożar, Miejscowe Zagrożenie), wymagając inną ilość środków.
- `PZStrategy` - Pożar, wymaga 3 wozów.
- `MZStrategy` - Miejscowe Zagrożenie, wymaga 2 wozów.

### 2. Wzorzec Iterator (Iterator Pattern)
Kluczowym problemem logistycznym jest zadysponowanie wozów z jednostki znajdującej się **najbliżej** miejsca zdarzenia. Stworzono dedykowaną kolekcję `UnitCollection` i niestandardowy iterator `ClosestUnitIterator`. Zwraca on jednostki JRG zawsze w kolejności od najbliższej do najdalszej względem konkretnego punktu (dynamiczne sortowanie po odległości wektora 2D).

### 3. Wzorzec Stan (State Pattern)
Każdy wóz posiada swój wewnętrzny `ICarState`, który w pełni kontroluje jego zachowanie w pętli `Update`:
- `FreeState`: Wóz czeka w bazie.
- `MovingState`: Wóz przemieszcza się do celu (interpolacja pozycji na podstawie czasu).
- `WaitingState`: Wóz dotarł na miejsce, ale czeka na dojazd pozostałych zadysponowanych sił.
- `ActionState`: Wszystkie wozy dotarły, trwa akcja ratownicza.
- `ReturningState`: Akcja zakończona (lub fałszywy alarm), wóz wraca do swojej jednostki macierzystej.

### 4. Wzorzec Obserwator (Observer Pattern)
Zamiast ciągłego odpytywania (tzw. *polling*) obiektów o ich status, wprowadzono architekturę zdarzeniową (`IObservable` / `IObserver`):
- **Wóz -> Incydent:** Kiedy wóz zmienia swój stan (np. z `Moving` na `Waiting`), powiadamia o tym incydent, do którego jest przypisany. Incydent zlicza wozy i gdy wszystkie są na miejscu, automatycznie przełącza je w tryb `ActionState`.
- **Incydent -> SKKM:** Kiedy wszystkie wozy zakończą działania i odjadą, incydent wysyła powiadomienie do Głównego Dyspozytora (SKKM) z prośbą o usunięcie z mapy i wyczyszczenie pamięci.

## Przebieg Symulacji i Mechanika
1. Generator losuje nowe zdarzenie na mapie (w granicach geograficznych Krakowa).
2. SKKM określa wymagania zdarzenia za pomocą jego *Strategii*.
3. System korzysta z *Iteratora*, by przeglądać jednostki od najbliższej i rezerwować wolne wozy.
4. Przypisane wozy zmieniają *Stan* na dojazd i ruszają w kierunku celu.
5. Po dotarciu na miejsce wszystkich sił, incydent może okazać się fałszywym alarmem (wozy natychmiast wracają) lub faktyczną akcją (czas trwania jest losowany).
6. Za pomocą *Obserwatora* system czyści mapę po powrocie sił do koszar.

Aplikacja zawiera czytelną legendę na panelu bocznym. Widać na niej w czasie rzeczywistym m.in.: kolorowe kropki symbolizujące rodzaj zdarzenia, zmieniające się kolory piktogramów wozów w zależności od ich aktualnego stanu (Dojazd, Akcja, Powrót) oraz stan liczebny wolnych wozów w poszczególnych Jednostkach Ratowniczo-Gaśniczych.
