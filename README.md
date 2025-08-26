<!-- File: README.md -->

# 🧱 Microservice Template – Unternehmens‑Vorlage & Bewerbungsbeispiel

*Ein schlankes, technologie‑agnostisches Template, das du als Portfolio‑Projekt deinen Bewerbungen beilegen kannst und das Firmen später als einheitliche Basis für Microservices nutzen.*

<p align="center">
  <!-- Badges: OWNER/REPO & Registries ersetzen -->
  <a href="https://github.com/OWNER/REPO/actions"><img alt="CI" src="https://img.shields.io/github/actions/workflow/status/OWNER/REPO/ci.yml?branch=main"></a>
  <a href="https://github.com/OWNER/REPO/releases"><img alt="Release" src="https://img.shields.io/github/v/release/OWNER/REPO"></a>
  <a href="#lizenz"><img alt="Lizenz" src="https://img.shields.io/badge/license-MIT-green"></a>
  <a href="#qualitätssicherung"><img alt="Coverage" src="https://img.shields.io/badge/coverage-80%25%2B-brightgreen"></a>
</p>

> **Wofür?**
>
> * **Bewerbung**: Zeigt Arbeitsweise, Qualitätsanspruch & DevOps‑Denken.
> * **Unternehmen**: Startpunkt für *neue* Microservices mit gemeinsamen Standards.

---

## Inhaltsverzeichnis

* [Zweck & Prinzipien](#zweck--prinzipien)
* [Schnellstart (als Vorlage nutzen)](#schnellstart-als-vorlage-nutzen)
* [Struktur](#struktur)
* [Konventionen](#konventionen)
* [API & Verträge](#api--verträge)
* [Lokale Entwicklung](#lokale-entwicklung)
* [Makefile‑Befehle](#makefile-befehle)
* [CI/CD](#cicd)
* [Qualitätssicherung](#qualitätssicherung)
* [Sicherheit](#sicherheit)
* [Release & Versionierung](#release--versionierung)
* [Template‑Variablen](#template-variablen)
* [Checkliste: Neuer Service aus Template](#checkliste-neuer-service-aus-template)
* [Roadmap](#roadmap)
* [Beitragen](#beitragen)
* [Lizenz](#lizenz)
* [Kontakt](#kontakt)
* [FAQ](#faq)

---

## Zweck & Prinzipien

Dieses Repository ist eine **produktive Vorlage** für Microservices. Es erzwingt klare Standards, damit Teams schneller starten und Services konsistent betreiben können.

**Design‑Prinzipien**

* 12‑Factor App, **API‑first**, KISS
* Security‑by‑default (least privilege, immutables)
* Observability: Logs, Metriken, Traces (OTel)
* Automatisierung: Build, Test, Release, Infra as Code
* Reproduzierbarkeit: Container, deterministische Builds

**Referenz‑Endpoints (sollten jeder Service bieten)**

* `GET /health` (liveness), `GET /ready` (readiness)
* `GET /metrics` (Prometheus)
* `GET /docs` (Swagger UI), `GET /info` (Build‑Infos)

Mermaid‑Skizze (Beispiel):

```mermaid
flowchart LR
  Client -->|REST| API[Template Service]
  API --> DB[(Persistenz)]
  API --> MQ[(Queue/Broker)]
  API --> Ext[(Externer Dienst)]
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
├─ .github/workflows/ci.yml        # Build, Lint, Test, Scan, Build Image
├─ openapi/openapi.yaml            # API‑Vertrag (Single Source of Truth)
├─ ops/docker/Dockerfile           # Referenz‑Dockerfile
├─ ops/docker-compose.dev.yml      # Dev‑Stack (Service + DB + Tools)
├─ docs/architecture.md            # ADRs, Diagramme
├─ src/                            # Service‑Code (Sprache/Framework frei)
├─ tests/                          # Unit/Integration
├─ Makefile                        # Einheitliche Entwickler‑Kommandos
├─ .env.example                    # Konfigurations‑Beispiel
├─ CODE_OF_CONDUCT.md
├─ CONTRIBUTING.md
├─ LICENSE
└─ README.md
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

**Docker Compose (Dev‑Stack)**

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

* **CI (Push/PR)**: Lint → Test → Coverage‑Check (≥ 80 %) → SCA/SAST → Docker Build → Image Scan → Artefakte.
* **CD (Release)**: Tag `vX.Y.Z` → Build & Push `REGISTRY/IMAGE:{vX.Y.Z,sha,latest}` → optional Deployment (ArgoCD/Helm/Kustomize).
* **Empfohlene Checks**: `trivy`/`grype` (Images), `gitleaks` (Secrets), `dependabot` (Deps).

*Minimales GitHub‑Actions‑Gerüst (`.github/workflows/ci.yml`):*

```yaml
name: ci
on: [push, pull_request]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with: { node-version: 20 }
      - run: npm ci
      - run: npm test -- --coverage
      - run: npm run lint
      - uses: docker/setup-buildx-action@v3
      - uses: docker/login-action@v3
        with: { registry: ghcr.io, username: ${{ github.actor }}, password: ${{ secrets.GITHUB_TOKEN }} }
      - uses: docker/build-push-action@v6
        with:
          context: .
          file: ops/docker/Dockerfile
          push: false
          tags: ghcr.io/ORG_NAME/SERVICE_NAME:ci
```

## Qualitätssicherung

* Coverage ≥ **80 %** (Unit), kritische Pfade mit Integrationstests
* Lint/Format: ESLint/Prettier **oder** Ruff/Black (stack‑abhängig)
* Pre‑commit Hooks: Lint, Tests, Secret‑Scan
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

* [ ] Beispiel‑Implementierung: Node.js **oder** Python (wählbar)
* [ ] Helm‑Chart & Kustomize‑Overlays
* [ ] Observability‑Dashboard (Grafana)
* [ ] Terraform‑Modul (Shared Infra)

## Beitragen

PRs willkommen! Bitte lies [`CONTRIBUTING.md`](./CONTRIBUTING.md).

## Lizenz

MIT – siehe [`LICENSE`](./LICENSE).

## Kontakt

**MAINTAINER\_NAME** – `EMAIL_OR_SOCIAL`

## FAQ

**Warum Microservices?** Kleine, unabhängig deploybare Einheiten, die Teams autonom machen.

**Kann ich jeden Tech‑Stack nutzen?** Ja. Diese Vorlage definiert *Prozesse & Qualitätsstandards*, nicht die Sprache.
