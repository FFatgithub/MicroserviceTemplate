
# MicroserviceTemplate

## Beschreibung

**MicroserviceTemplate** ist ein Minimal-Template für den Aufbau eines Microservices mit .NET 8. Es implementiert das **CQRS-Muster** (Command Query Responsibility Segregation) und trennt Anliegen für Lese- und Schreiboperationen. Dieses Template bietet eine saubere und modulare Grundlage für die Entwicklung von Microservices, die in einem Produktionsumfeld eingesetzt werden können.

Es dient als Ausgangspunkt für die Erstellung von Microservices und enthält grundlegende Komponenten wie Controller, Services, Datenzugriff und Command/Query-Handler. Das Template kann an spezifische Anforderungen angepasst und weiterentwickelt werden.

### Wichtige Merkmale:
- **MediatR** zur Implementierung von Command- und Query-Handlern.
- Verwendung des **ApplicationDbContext** für den Datenzugriff.
- Trennung der Geschäftslogik, des Datenbankzugriffs und der API-Endpunkte.
- Ein einfaches Template für den schnellen Start eines Microservices.

## Verzeichnisstruktur

```
-- MicroserviceTemplate
   ├── Controllers           			  # API-Controller
   ├── Data                  			  # DbContext
   ├── Handlers              			  # Command- und Query-Handler (CQRS)
   ├── Models                			  # Query-Handler Modele
   ├── Properties            			  # Projektkonfigurationen und Einstellungen
   ├── Services              			  # Geschäftslogik
   ├── appsettings.json      			  # Umgebungsvariablen und Verbindungszeichenfolgen
   ├── Program.cs                         # Startdatei der Anwendung
   ├── MicroserviceTemplate.csproj        # .NET Projektdatei
   ├── stylecop.json                      # Codierungsrichtlinien (StyleCop)
   └── ...
```

## Installation

### Voraussetzungen:
- **.NET 8** (oder eine höhere Version)
- **Visual Studio** oder ein anderer C#-Editor
- **SQL Server** oder eine andere kompatible Datenbank

### Schritte zur Installation:

1. **Repository klonen:**

   ```bash
   git clone https://github.com/FFatgithub/MicroserviceTemplate.git
   cd MicroserviceTemplate
   ```

2. **Datenbank einrichten:**

   Die **Verbindungszeichenfolge** für die Datenbank muss in der Datei `appsettings.json` festgelegt werden. Beispiel:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=MicroserviceTemplateDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. **Migrationen anwenden (falls erforderlich):**

   Wenn **EF Core** verwendet wird, müssen Migrationen erstellt und die Datenbank aktualisiert werden:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Projekt starten:**

   Das Projekt kann mit dem folgenden Befehl ausgeführt werden:

   ```bash
   dotnet run
   ```

   Der Webserver wird gestartet und die API-Endpunkte sind unter **https://localhost:5001** verfügbar (oder einer anderen URL, die in `Program.cs` definiert ist).

## Verwendung

Das Template implementiert grundlegende **CRUD**-Operationen über Command- und Query-Handler. Zwei grundlegende Beispiele:

1. **Erstellen einer Entität (über POST):**
   - **Endpunkt**: `POST /api/entity`
   - **Beschreibung**: Fügt eine neue Entität in die Datenbank ein und gibt die ID der neu erstellten Entität zurück. Das Model beninhaltet nur die Attribute die wirklich geändert werden, oder hinzugefügt werden.

2. **Abrufen einer Entität (über GET):**
   - **Endpunkt**: `GET /api/entity/{id}`
   - **Beschreibung**: Gibt eine Entität basierend auf der ID zurück. Hier wird das Database Objekt zurückgeliefert.

### Beispiel: POST-Anfrage (Create)

```bash
POST /api/entity
Content-Type: application/json

{
  "articleNumber": "12356456123",
  "purchaseDate": "2025-01-01T00:00:00"
}
```

Antwort:

```json
{
  "id": "f6fbd67e-bd6e-4d6b-bd25-88bfb3b4828e"
}
```

### Beispiel: GET-Anfrage (Get by ID)

```bash
GET /api/entity/f6fbd67e-bd6e-4d6b-bd25-88bfb3b4828e
```

Antwort:

```json
{
  "id": "f6fbd67e-bd6e-4d6b-bd25-88bfb3b4828e",
  "articleNumber": "12356456123",
  "purchaseDate": "2025-01-01T00:00:00",
  "color": "5"
}
```

## Architektur

### CQRS (Command Query Responsibility Segregation)
Das Template nutzt das **CQRS-Muster**, bei dem Leseoperationen (Queries) und Schreiboperationen (Commands) getrennt behandelt werden.

- **Command**: Führt eine Aktion aus, die den Zustand verändert (z. B. Entität erstellen oder aktualisieren).
- **Query**: Leseoperationen, die den Zustand abfragen, aber nicht verändern.

### MediatR
**MediatR** wird verwendet, um die Commands und Queries zu verwalten und die Logik aus den Controllern herauszuhalten, was zu einer klareren Architektur führt.

- **Handlers**: Sie verarbeiten die Commands und Queries, die über MediatR gesendet werden.
- **Services**: Implementieren die Geschäftslogik und kümmern sich um die Datenverarbeitung.

## Tests

Da dieses Template hauptsächlich als Ausgangspunkt dient, sind keine Unit-Tests enthalten. Es können jedoch problemlos Unit-Tests für den Service und die Handler hinzugefügt werden, indem eine Testprojekt-Datei (.csproj) erstellt wird und Frameworks wie **xUnit** oder **NUnit** verwendet werden.

Beispiel für Unit-Tests mit xUnit:

```bash
dotnet new xunit -n MicroserviceTemplate.Tests
dotnet add reference ../MicroserviceTemplate/MicroserviceTemplate.csproj
```

## Anpassungen

Dieses Template ist ein Ausgangspunkt, der nach Belieben angepasst und erweitert werden kann. Zu den typischen Anpassungen gehören:

- Hinzufügen weiterer Entitäten und Datenbanktabellen.
- Erstellen zusätzlicher Commands und Queries für verschiedene Anwendungsfälle.
- Implementierung von Authentifizierung und Autorisierung.

## Lizenz

Dieses Projekt ist unter der **MIT-Lizenz** lizenziert – siehe die **`LICENSE`**-Datei für Details.
