# StudyCards

![CI Workflow](https://github.com/berndtmark/studycards/actions/workflows/ci.yml/badge.svg)
![Azure Deploy](https://github.com/berndtmark/studycards/actions/workflows/azure-container-webapp.yml/badge.svg)

StudyCards is a flashcard and study management application. It features a modern Angular frontend and a robust .NET backend following Clean Architecture principles, orchestrated using .NET Aspire.

## 🏗️ Architecture

### Backend
- **Framework**: .NET
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, API)
- **Database**: Azure Cosmos DB (can be emulated locally using Docker)
- **Orchestration**: .NET Aspire (`StudyCards.AppHost`)

### Frontend
- **Framework**: Angular
- **UI Components**: Angular Material
- **State Management**: NgRx Signals (`@ngrx/signals`)
- **API Client**: Auto-generated via `ng-openapi-gen`

## 📁 Project Structure

- `Aspire/` - .NET Aspire orchestration and service defaults.
- `StudyCards.Api/` - REST API endpoints and web application configuration.
- `StudyCards.Application/` - Core business logic, CQRS handlers, and interfaces.
- `StudyCards.Domain/` - Core domain entities and rules.
- `StudyCards.Infrastructure.*` - External concerns like Database and Secrets.
- `StudyCards.UI/studycardsui/` - The Angular frontend application.
- `Tests/` - Unit tests and Architecture tests.

## 🚀 Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) & [npm](https://www.npmjs.com/)
- [Docker](https://www.docker.com/) (required for local Cosmos DB emulation)

### Backend Setup
You can either run the .NET Aspire AppHost to orchestrate all services or run the API directly.

To run the API directly:
```bash
dotnet run --project StudyCards.Api/StudyCards.Api.csproj --launch-profile https
```

### Frontend Setup
1. Navigate to the UI project:
   ```bash
   cd StudyCards.UI/studycardsui
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Run the development server:
   ```bash
   ng serve
   ```

## 🔐 Authentication (Local Development)
To run the app locally, you will need the appropriate authentication cookies. 
Get the cookies needed to run the app by navigating to:
👉 [https://localhost:5102/api/auth/login](https://localhost:5102/api/auth/login?ReturnUrl=%2Fapi%2Fauth%2Fme)

## 🛠️ UI Development Scripts

In the `StudyCards.UI/studycardsui` folder, you have access to several handy scripts:
- `npm run generate-api`: Regenerates the Angular API client from the OpenAPI spec (`servercardsapi.json`).
- `npm run build:prod`: Builds the UI for production, generating the API client first.