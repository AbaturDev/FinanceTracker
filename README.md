# Finance Tracker
**Finance Tracker** is a personal financial management application built as project for a university .NET course. Its purpose is to help users plan expenses, save money, and understand their financial habits through budgeting and transaction tracking. The app combines practical tools for managing daily finances with an intuitive and structured approach to personal budgeting.

## Features
- **Expenses Planners** - Users can create detailed expense planners by defining currencies, spending categories, and expected expenses. Funds can be deposited into planners using transactions. Each planner maintains a transaction history, allowing users to track their spending over time.
- **Saving Goals** - Users can set specific financial goals by choosing a target amount and a category. Money can be deposited toward these goals via transactions. The feature enables users to monitor their progress and stay focused on saving for what matters most.
- **Incomes** - Users can record and manage their sources of income. These entries are used to automatically calculate a monthly budget, helping users understand their financial capacity and plan spending accordingly.
- **Budgets** - Monthly budgets are created automatically on the first day of each month. Users can allocate funds from the budget to both expense planners and saving goals. Each budget's history is preserved, allowing users to view financial trends across past months.
- **Transactions** - Transactions are used to move money between the user’s budget and their planners or saving goals. The app integrates with the `NBP API` to automatically convert currencies, ensuring that all financial operations match the currency defined in the planner or saving goal.

## Technology stack
### Backend
- C#
- .NET 9
- ASP.NET Core Minimal API
- Entity Framework Core

## Frontend
- C#
- .NET 7
- Blazor WASM
- Mud Blazor

## Database
- MS SQL Server

### Libraries, Tools and Frameworks
- FluentValidation
- FluentResults
- HangFire
- JwtBearer
- Seeders using Bogus
- OpenAPI documentation
- ASP.NET Identity
- Refit
- Polly
- Cache

## Installation & Setup
1. **.NET SDK**
* Download and install .NET 9 SDK  from [here](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* Downolad and install .NET 7 SDK from [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
* Ensure you installed it by typing `dotnet --info` in terminal
2. **Clone the repository**
```
git clone https://github.com/AbaturDev/FinanceTracker.git
```
3. **Configure the database**
* Ensure the MS SQL Server is installed and running
* Modify the connection string in `appsettings.json` if necessary.
4. **Jwt Secret**
* Generate JWT secret and modify in `appsettings.json`
5. **Apply migrations**
```
ef database update --project src\FinanceTracker.Infrastructure\FinanceTracker.Infrastructure.csproj --startup-project src\FinanceTracker.API\FinanceTracker.API.csproj --context FinanceTracker.Infrastructure.Context.FinanceTrackerDbContext --configuration Debug <migration> --connection "<connection_string>"
```
6. **Run backend**
```
cd src
dotnet run --project FinanceTracker.API
```
7. **Run frontend**
```
dotnet run --project FinanceTracker.Client
```

## How to start
Once the backend and frontend are running, you can create your own account by registering, then log in and start managing your finances.

## API Documentation
The API is documented using OpenAPI.
Once the backend is running, visit:

for **HTTPS**
```{bash}
https://localhost:5001/swagger
```
or for **HTTP**
```{bash}
https://localhost:500/swagger
```

## Authors
* **AbaturDev** - Project Manager, Backend developer, Frontend developer
* **Serpenteno** - Backend developer, Frontend developer
