<!-- File: README.md -->

# 🧱 Microservice Template – Unternehmens‑Vorlage & Bewerbungsbeispiel

<p align="center">
  <!-- Badges: OWNER/REPO & Registries ersetzen -->
  <a href="https://github.com/OWNER/REPO/actions"><img alt="CI" src="https://img.shields.io/github/actions/workflow/status/OWNER/REPO/ci.yml?branch=main"></a>
  <a href="https://github.com/OWNER/REPO/releases"><img alt="Release" src="https://img.shields.io/github/v/release/OWNER/REPO"></a>
  <a href="#lizenz"><img alt="Lizenz" src="https://img.shields.io/badge/license-MIT-green"></a>
  <a href="#qualitätssicherung"><img alt="Coverage" src="https://img.shields.io/badge/coverage-80%25%2B-brightgreen"></a>
</p>

> **Wofür?**
>
> * **Bewerbung**: Zeigt Arbeitsweise, Qualitätsanspruch & DevOps‑Denken. Zu diesem Zeitpunkt noch in Arbeit – es fehlen Tests (UI), Authentication und weiteres.
> * **Unternehmen**: Startpunkt für *neue* Microservices mit gemeinsamen Standards.

---

## Inhaltsverzeichnis

* [Zweck & Prinzipien](#zweck--prinzipien)
* [Schnellstart (als Vorlage nutzen)](#schnellstart-als-vorlage-nutzen)
* [Struktur](#struktur)
* [Konventionen](#konventionen)
* [API & Verträge](#api--verträge)
* [Lokale Entwicklung](#lokale-entwicklung)
* [CI/CD](#cicd)
* [Qualitätssicherung](#qualitätssicherung)
* [Sicherheit](#sicherheit)
* [Release & Versionierung](#release--versionierung)
* [Checkliste: Neuer Service aus Template](#checkliste-neuer-service-aus-template)
* [Roadmap](#roadmap)
* [Lizenz](#lizenz)
* [Kontakt](#kontakt)
* [FAQ](#faq)

---

## Zweck & Prinzipien

Dieses Repository ist eine **produktive Vorlage** für Microservices. Es erzwingt klare Standards, damit Teams schneller starten und Services konsistent betreiben können.

**Design‑Prinzipien**

* **API‑first**, KISS
* Einfache, kleine Services – leicht zu pflegen
* Automatisierung: Build, Test, Release, Infra as Code
* Reproduzierbarkeit: Container, deterministische Builds

**Referenz‑Endpoints (sollte jeder Service perspektivisch bieten)**

* `GET /docs` (Swagger UI) – **aktuell vorhanden**
* `GET /health` (Liveness) – *geplant*
* `GET /ready` (Readiness) – *geplant*
* `GET /metrics` (Prometheus) – *geplant*
* `GET /info` (Build‑Infos) – *geplant*

Mermaid‑Skizze (Beispiel):

```mermaid
flowchart LR
  Frontend -->|RESTful API| API[Kunden‑Service]
  Frontend -->|RESTful API| API2[Katalog‑Service]
  Frontend -->|RESTful API| API3[Bestell‑Service]
  API --> DB[(Persistenz)]
  API2 --> DB[(Persistenz)]
  API3 --> DB[(Persistenz)]
```

## Schnellstart (als Vorlage nutzen)

**GitHub UI** → *Use this template* → neuen Repo‑Namen vergeben → **Create**.

**GitHub CLI**

```bash
# Voraussetzung: gh auth login
gh repo create ORG/NEW_SERVICE --template OWNER/REPO --public
```

**Lokale Umbenennung**

```bash
# Platzhalter ersetzen
export SERVICE_NAME=my-service
export ORG_NAME=my-org
rg -uu "OWNER/REPO|SERVICE_NAME|ORG_NAME" -nl | xargs sed -i "" \
  -e "s#OWNER/REPO#$ORG_NAME/$SERVICE_NAME#g" \
  -e "s#SERVICE_NAME#$SERVICE_NAME#g" \
  -e "s#ORG_NAME#$ORG_NAME#g"
```

> ⚠️ **Warum:** Einheitliche Namen erleichtern Telemetrie, CI/CD und Discovery.

## Struktur

```text
.
├─ Properties/                         # .NET-Projekt-/Laufzeitkonfig (z. B. launchSettings.json, AssemblyInfo)
├─ Controllers/                        # REST-Endpoints, nur Orchestrierung, keine Geschäftslogik
├─ Data/                               # Persistenzschicht: DbContext/Repos/Migrations, Zugriff auf die DB bündeln
├─ Handlers/                           # Use-Case-/MediatR-Handler, kapselt Anwendungsfälle. Nur hier findet die Logik statt.
├─ Models/                             # Nur DTOs, alle Models sind ohne Logik.
├─ Service/                            # Geschäftslogik (Interfaces + Implementierungen), wiederverwendbar/ später dann testbar.
├─ appsettings.json                    # Konfiguration (per ENV übersteuerbar), keine Secrets hier ablegen.
├─ MicroserviceTemplate.http           # Beispiel-HTTP-Requests (VS/VSCode REST Client) zum manuellen Testen.
├─ Program.cs                          # Registriert Service, startet die App.
├─ stylecop.json                       # Beinhaltet Informationen über die Firma, welche als Head in jeder Datei dienen.
```

## Konventionen

* **Ports**: Standard `8080` (konfigurierbar via `PORT`).
* **Env‑Präfix**: `SERVICE_` (z. B. `SERVICE_DB_URL`).
* **Logging**: JSON, Level per Env (`LOG_LEVEL`), Korrelations‑ID `X‑Request‑ID`.
* **Metriken**: Prometheus‑Format, Namespace = `service_name`.
* **Tracing**: OpenTelemetry (`OTEL_EXPORTER_OTLP_ENDPOINT`).
* **API Style**: REST, plural Nomen, Snake im Body, Kebab im Pfad; Pagination `?page=&page_size=`.
* **Fehlerformat**: RFC7807‑ähnlich (`type`, `title`, `status`, `detail`, `traceId`).
* **Migrations**: idempotent, forward‑only.
* **Zeit**: UTC, ISO‑8601, explizite Zeitzonen.

## API & Verträge

Der API‑Vertrag liegt unter `openapi/openapi.yaml` und wird in CI validiert. Swagger UI wird in Dev unter `/docs` ausgeliefert. Generierte Clients/Server sind optional (z. B. `openapi-generator`).

## Lokale Entwicklung

**.NET (ohne Container)**

```bash
# Wiederherstellen, Build, Start
dotnet restore
dotnet build -c Debug
dotnet run --project ./ # ggf. Projektpfad anpassen

# Swagger UI erreichbar unter /docs (Dev)
```

**Docker Compose (Dev‑Stack, optional)**

```yaml
# ops/docker-compose.dev.yml
version: "3.9"
services:
  service:
    build: { context: .., dockerfile: ops/docker/Dockerfile }
    env_file: [../.env]
    ports: ["8080:8080"]
    depends_on: [db]
  db:
    image: postgres:16
    environment:
      POSTGRES_PASSWORD: example
      POSTGRES_USER: app
      POSTGRES_DB: app
    ports: ["5432:5432"]
  prometheus:
    image: prom/prometheus:latest
    command: ["--config.file=/etc/prometheus/prometheus.yml"]
```

**.env Beispiel**

```env
PORT=8080
SERVICE_DB_URL=postgres://app:example@localhost:5432/app
LOG_LEVEL=info
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4318
```

## Makefile‑Befehle

```makefile
# Standard‑Kommandos (passen zu beliebigen Stacks)
.PHONY: dev test lint build docker-build run migrate fmt

dev:        ## Starte lokalen Dev‑Modus
	@echo "dev server…"

test:       ## Tests ausführen
	@echo "run tests…"

lint:       ## Linting
	@echo "lint…"

fmt:        ## Formatierung
	@echo "format…"

build:      ## Produktionsbuild
	@echo "build…"

docker-build: ## Image bauen
	docker build -f ops/docker/Dockerfile -t REGISTRY/IMAGE:dev .

run:        ## Container starten
	docker run --rm -p 8080:8080 REGISTRY/IMAGE:dev

migrate:    ## DB‑Migrationen anwenden
	@echo "migrate…"
```

> 💡 **Warum Make?** Vereinheitlicht Entwickler‑Workflows über Sprachen hinweg.

## CI/CD

* **CI (Push/PR)**: Restore → Build → Tests → (optional) Coverage → Docker Build → Image Scan → Artefakte.
* **CD (Release)**: Tag `vX.Y.Z` → Build & Push `REGISTRY/IMAGE:{vX.Y.Z,sha,latest}` → optional Deployment (ArgoCD/Helm/Kustomize).
* **Empfohlene Checks**: Container‑Scan (`trivy`/`grype`), Secret‑Scan (`gitleaks`), Dependency‑Updates (`dependabot`).

*Minimales GitHub‑Actions‑Gerüst für .NET (`.github/workflows/ci.yml`):*

```yaml
name: ci
on: [push, pull_request]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - name: Restore
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore -c Release
      - name: Test
        run: dotnet test --no-build -c Release
      - uses: docker/setup-buildx-action@v3
      - uses: docker/login-action@v3
        with:
          registry: ghcr.io
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}
      - uses: docker/build-push-action@v6
        with:
          context: .
          file: Dockerfile # ggf. anpassen
          push: false
          tags: ghcr.io/ORG_NAME/SERVICE_NAME:ci
```

## Qualitätssicherung

* Coverage ≥ **80 %** (Unit) – sobald Tests vorhanden
* .NET Analyzer + **StyleCop** (vorhanden), `dotnet format`
* Pre‑commit Hooks: Build, Tests, Secret‑Scan
* PR‑Template mit Akzeptanzkriterien & Screenshots/Logs

## Sicherheit

* Secrets **nie** im Repo, nur via CI‑Secrets/Env
* Least‑Privilege‑Service‑Accounts
* Read‑only Root‑FS, Non‑Root‑User im Container
* Dependency‑Updates automatisiert (Dependabot)

## Release & Versionierung

* **SemVer** (`MAJOR.MINOR.PATCH`)
* Docker‑Tags: `vX.Y.Z`, `sha-<short>`, `latest`
* Changelog nach *Keep a Changelog*

## Template‑Variablen

| Variable           | Beschreibung              | Beispiel            |
| ------------------ | ------------------------- | ------------------- |
| `ORG_NAME`         | GitHub‑Org/Benutzer       | `acme-corp`         |
| `SERVICE_NAME`     | Einheitlicher Servicename | `billing-service`   |
| `DEFAULT_PORT`     | Standardport              | `8080`              |
| `REGISTRY`         | Container‑Registry        | `ghcr.io/acme-corp` |
| `IMAGE`            | Image‑Name                | `billing-service`   |
| `MAINTAINER_EMAIL` | Kontakt                   | `dev@acme.example`  |

## Checkliste: Neuer Service aus Template

* [ ] Repo aus Template erzeugt (UI/CLI)
* [ ] `ORG_NAME`/`SERVICE_NAME`/Badges ersetzt
* [ ] `openapi/openapi.yaml` grob modelliert (Top‑3 Endpoints)
* [ ] Health/Ready/Metrics/Docs implementiert
* [ ] Make‑Targets lauffähig
* [ ] CI grün, Image baubar, Scan ok
* [ ] `.env.example` geprüft, Secrets ausgelagert

## Roadmap

* [ ] Beispiel‑Implementierung: ASP.NET Core (Minimal API) mit Health/Ready/Metrics/Docs
* [ ] Helm‑Chart & Kustomize‑Overlays
* [ ] Observability‑Dashboard (Grafana)
* [ ] Terraform‑Modul (Shared Infra)
* [ ] Sicherheit
* [ ] Tests
* [ ] Authentication

## Lizenz

MIT – siehe [`LICENSE`](./LICENSE).

## Kontakt

**MAINTAINER\_NAME** – `bewerbung@frankfriedrich.eu`

## FAQ

**Warum Microservices?** Kleine, unabhängig deploybare Einheiten, die Teams autonom machen.

**Kann ich jeden Tech‑Stack nutzen?** Ja. Diese Vorlage definiert *Prozesse & Qualitätsstandards*, nicht die Sprache.
