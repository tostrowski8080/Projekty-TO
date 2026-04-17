# Lab 1: Kalkulator Walutowy (NBP API)

Konsolowa aplikacja do wymiany walut, która na żywo pobiera aktualne kursy z API Narodowego Banku Polskiego (NBP). 

Głównym celem tego projektu nie było samo napisanie kalkulatora, ale zaprojektowanie go w sposób w pełni zorientowany obiektowo, z zachowaniem wysokiej testowalności i elastyczności na przyszłe zmiany. 

## Wykorzystany Tech Stack
- **Język:** C# (.NET)
- **Komunikacja sieciowa:** `HttpClient` (asynchroniczne zapytania HTTP)
- **Przetwarzanie danych:** `System.Xml` (parsowanie dokumentów XML)
- **Paradygmaty:** Asynchroniczność (TPL - Task Parallel Library), Object-Oriented Programming (OOP)

## Architektura i Wzorce Projektowe

Kod został podzielony na logiczne warstwy, mocno opierając się na interfejsach. Dzięki temu projekt ściśle przestrzega zasad **SOLID**.

### Kluczowe koncepcje zaimplementowane w kodzie:

1. **Wzorzec Polecenia (Command Pattern)**
   Zamiast ogromnej instrukcji `switch` z logiką w głównym kontrolerze, każda komenda użytkownika jest osobną klasą (np. `ExchangeAction`, `UpdateAction`, `CurrenciesAction`) implementującą interfejs `Action`. Ułatwia to dodawanie nowych funkcji bez modyfikowania istniejącego kodu (zasada Open/Closed).

2. **Wstrzykiwanie Zależności (Dependency Injection)**
   Klasa `ExchangeController` nie tworzy swoich zależności (takich jak pobieranie danych czy UI). Są one "wstrzykiwane" przez konstruktor w punkcie wejścia aplikacji (`Program.cs` - Composition Root). Dzięki temu klasę można łatwo przetestować za pomocą mocków.

3. **Oddzielenie Warstwy Danych i Prezentacji (Separation of Concerns)**
   - **`RemoteRepository` (Interfejs):** Odpowiada tylko za pobranie surowych bajtów. Implementacja `Rest` korzysta z `HttpClient`.
   - **`Document` (Interfejs):** Odpowiada tylko za przetworzenie surowego tekstu na obiekt domenowy `ExchangeTable`. Implementacja `XML` dekoduje format NBP.
   - **`UserInterface` (Interfejs):** Odseparowuje logikę biznesową od sposobu wyświetlania danych. Implementacja `ConsoleUI` zajmuje się wyłącznie interakcją w terminalu. Zmiana interfejsu na okienkowy (WPF/WinForms) wymagałaby tylko napisania nowej implementacji tego interfejsu, bez ruszania kontrolera.

## Dostępne Funkcjonalności (Komendy)

Po uruchomieniu aplikacji użytkownik ma do dyspozycji interaktywny wiersz poleceń z obsługą następujących komend:

- `help` - Wyświetla listę dostępnych komend.
- `currencies` - Wypisuje alfabetyczną listę wszystkich dostępnych walut (kodów ISO), załadowanych z API.
- `update` - Wymusza asynchroniczne pobranie najnowszej tabeli kursów XML z serwerów NBP.
- `exchange` - Rozpoczyna proces konwersji. Krok po kroku pyta użytkownika o walutę źródłową, docelową oraz kwotę. Automatycznie waliduje poprawne kody walut.
- `exit` - Bezpiecznie zamyka program.
