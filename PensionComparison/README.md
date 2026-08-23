# PensionComparison

A small .NET 8 console application that compares two pension plans and calculates:

- the retirement date for each plan
- the total lifetime payout up to a final payout date
- the break-even date between the two plans
- the retiree's age when the break-even occurs

It uses Spectre.Console for a polished terminal UI.

## Prerequisites

- .NET 8 SDK or later
- A terminal or command prompt

## Project structure

- `PensionComparison/` - console app
- `PensionComparison.Tests/` - xUnit tests
- `PensionComparison.sln` - solution file
- `NuGet.Config` - public NuGet-only configuration used to avoid the private package feed in this environment

## Run from the root

```bash
dotnet restore --configfile NuGet.Config PensionComparison.sln
dotnet build PensionComparison.sln --no-restore
dotnet run --project PensionComparison/PensionComparison.csproj
```

## Example input flow

When the app starts, it will prompt for:

1. Date of birth
2. Plan A retirement age in years
3. Plan A retirement age in months
4. Plan A monthly payout
5. Plan B retirement age in years
6. Plan B retirement age in months
7. Plan B monthly payout
8. Final payout date

Example values:

```text
Date of birth: 1962-08-15
Plan A retirement age in years: 63
Plan A retirement age in months: 9
Plan A monthly payout: 3084
Plan B retirement age in years: 67
Plan B retirement age in months: 0
Plan B monthly payout: 3963
Final payout date: 2045-12-31
```

## Example output

The app displays a summary table similar to:

```text
Pension Comparison
Plan                Retirement age     Retirement date   Monthly payout   Total payout to final date
Plan A              63y 9m             2026-05-15       $3,084.00        $X
Plan B              67y 0m             2029-08-15       $3,963.00        $Y
Break-even date: 2032-11-15 (age 69 years 10 months)
```

The exact values depend on the data entered.

## Run tests

```bash
dotnet test PensionComparison.Tests/PensionComparison.Tests.csproj --no-restore
```

## Notes

- This is a simple CLI project intended for quick comparisons, not for actuarial or financial advice.
- The calculation logic is kept separate from the console UI so it is easy to expand or test.
