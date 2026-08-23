using System.Globalization;
using Spectre.Console;
using PensionComparison;

var birthDate = AnsiConsole.Prompt(
    new TextPrompt<DateOnly>("[cyan]Date of birth[/]:")
        .PromptStyle("cyan")
        .Validate(value => value <= DateOnly.FromDateTime(DateTime.Today)
            ? ValidationResult.Success()
            : ValidationResult.Error("Birth date must be today or earlier.")));

var planA = ReadPlan("Plan A");
var planB = ReadPlan("Plan B");

var finalPayoutAgeYears = AnsiConsole.Prompt(
    new TextPrompt<int>("[green]Final payout age in years[/]:")
        .PromptStyle("green")
        .DefaultValue(85)
        .Validate(value => value >= 0 ? ValidationResult.Success() : ValidationResult.Error("Years must be zero or more.")));

var finalPayoutAgeMonths = AnsiConsole.Prompt(
    new TextPrompt<int>("[green]Final payout age in months[/]:")
        .PromptStyle("green")
        .DefaultValue(0)
        .Validate(value => value is >= 0 and < 12 ? ValidationResult.Success() : ValidationResult.Error("Months must be between 0 and 11.")));

var finalPayoutDate = PensionCalculator.GetAgeDate(birthDate, finalPayoutAgeYears, finalPayoutAgeMonths);

var result = PensionCalculator.Compare(planA, planB, birthDate, finalPayoutDate);

AnsiConsole.Write(new Rule("[bold cyan]Pension Comparison[/]").RuleStyle("cyan"));

var table = new Table();
table.Border(TableBorder.Rounded);
table.AddColumn("Plan");
table.AddColumn("Retirement age");
table.AddColumn("Retirement date");
table.AddColumn("Monthly payout");
table.AddColumn("Total payout to final date");

table.AddRow(planA.Name, FormatAge(planA), result.PlanARetirementDate.ToString("yyyy-MM-dd"), planA.MonthlyPayout.ToString("C", CultureInfo.CurrentCulture), result.PlanATotalPayout.ToString("C", CultureInfo.CurrentCulture));
table.AddRow(planB.Name, FormatAge(planB), result.PlanBRetirementDate.ToString("yyyy-MM-dd"), planB.MonthlyPayout.ToString("C", CultureInfo.CurrentCulture), result.PlanBTotalPayout.ToString("C", CultureInfo.CurrentCulture));

AnsiConsole.Write(table);

AnsiConsole.WriteLine();
var breakEven = result.BreakEvenDate is null
    ? "No break-even date"
    : $"{result.BreakEvenDate:yyyy-MM-dd} (age {result.BreakEvenAgeYears} years {result.BreakEvenAgeMonths} months)";

AnsiConsole.MarkupLine($"[bold]Break-even date:[/] {breakEven}");

static PensionPlan ReadPlan(string name)
{
    var years = AnsiConsole.Prompt(
        new TextPrompt<int>($"[yellow]{name} retirement age in years[/]:")
            .PromptStyle("yellow")
            .Validate(value => value >= 0 ? ValidationResult.Success() : ValidationResult.Error("Years must be zero or more.")));

    var months = AnsiConsole.Prompt(
        new TextPrompt<int>($"[yellow]{name} retirement age in months[/]:")
            .PromptStyle("yellow")
            .DefaultValue(0)
            .Validate(value => value is >= 0 and < 12 ? ValidationResult.Success() : ValidationResult.Error("Months must be between 0 and 11.")));

    var monthlyPayout = AnsiConsole.Prompt(
        new TextPrompt<decimal>($"[yellow]{name} monthly payout[/]:")
            .PromptStyle("yellow")
            .Validate(value => value >= 0m ? ValidationResult.Success() : ValidationResult.Error("Monthly payout must be zero or more.")));

    return new PensionPlan(name, years, months, monthlyPayout);
}

static string FormatAge(PensionPlan plan)
{
    return $"{plan.RetirementAgeYears}y {plan.RetirementAgeMonths}m";
}
