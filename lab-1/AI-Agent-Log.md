# AI Agent Log - Lab 1

## Purpose

This file tracks how the AI agent was used during Lab 1 development. It should be updated continuously as the project evolves.

## Session Log

### 2026-04-02

#### Task

Implemented the initial Lab 1 object model and demonstration endpoints for the RestoManager project.

#### Work Completed

- Read and analyzed the requirements from `.projectDefinition/Lab-1.md`
- Reviewed the existing ASP.NET Core project structure
- Created the initial object model for the restaurant domain
- Added enums required for the model
- Seeded in-memory demo data for restaurants, locations, staff, meals, ingredients, recipes, and orders
- Implemented LINQ queries that match realistic restaurant-management use cases
- Added an async/await example through an API endpoint and startup summary output
- Added a controller exposing Lab 1 data through HTTP endpoints
- Built the project successfully to verify compilation

#### Files Added

- `../RestoManager.Model/Enums/StaffRole.cs`
- `../RestoManager.Model/Enums/OrderStatus.cs`
- `../RestoManager.Model/Enums/IngredientUnit.cs`
- `../RestoManager.Model/Restaurant.cs`
- `../RestoManager.Model/Location.cs`
- `../RestoManager.Model/StaffMember.cs`
- `../RestoManager.Model/Meal.cs`
- `../RestoManager.Model/Ingredient.cs`
- `../RestoManager.Model/RecipeIngredient.cs`
- `../RestoManager.Model/Order.cs`
- `../RestoManager.Model/OrderItem.cs`
- `Db/Lab1DataStore.cs`
- `Controllers/Lab1Controller.cs`
- `lab-1/AI-Agent-Log.md`

#### Files Updated

- `Program.cs`
- `RestoManager.Api.csproj`
- `../RestoManager.Model/RestoManager.Model.csproj`

#### Notes

- The implementation currently uses in-memory seeded data to satisfy Lab 1 requirements quickly and clearly.
- The `restaurants` endpoint returns a summary projection instead of the full object graph to avoid JSON circular reference issues.
- This log should be expanded after every meaningful implementation step.

#### Architecture Update

- Refactored namespaces from the original generated project namespace to `RestoManager`
- Moved all domain model classes out of the API project into the `RestoManager.Model` class library
- Added an API-to-model project reference
- Moved the seeded in-memory data store from `Services` to `Db` to better reflect its responsibility
- Updated the API code to use the new namespace and project structure
- Reduced `Lab1DataStore` so it is responsible only for seeding and holding data
- Moved overview, LINQ, and async summary logic into `Program.cs`
- Replaced loose `object` return values with strongly typed DTO classes under `Contracts/Lab1`
- Removed the controller and kept the Lab 1 output logic directly in `Program.cs`
- Moved the DTO classes into the `Db` folder to keep the current lab structure simpler
- Renamed the aggregate LINQ response model to `RestaurantStatisticsDto`
- Split the statistics queries into separate descriptive methods to reduce cognitive complexity in `Program.cs`
- Adjusted seeded meal prices so the premium-meal statistics query returns meaningful data
- Moved the `RestoManager.Model` class library into the repository so Git can track it
- Updated the solution and API project references to point to the in-repo model project
- Removed unused leftover folders from earlier refactors to clean up the project structure
- Moved the API project into a dedicated `RestoManager.Api` subfolder so both `.csproj` files now live inside project folders
- Cleaned the repository root so it contains only the solution file and shared root-level folders
- Reorganized the model project by entity/domain folders so related enums now live next to their corresponding models
