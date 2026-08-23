using System.Text.Json;

namespace PensionComparison;

public sealed record PensionPlanDefaults(int RetirementAgeYears, int RetirementAgeMonths, decimal MonthlyPayout);

public sealed record ComparisonDefaults(
    DateOnly BirthDate,
    PensionPlanDefaults PlanA,
    PensionPlanDefaults PlanB,
    int FinalPayoutAgeYears,
    int FinalPayoutAgeMonths)
{
    public static ComparisonDefaults CreateDefault()
    {
        return new ComparisonDefaults(
            DateOnly.FromDateTime(DateTime.Today),
            new PensionPlanDefaults(67, 0, 0m),
            new PensionPlanDefaults(0, 0, 0m),
            85,
            0);
    }

    public static ComparisonDefaults Load(string path)
    {
        if (!File.Exists(path))
            return CreateDefault();

        try
        {
            var json = File.ReadAllText(path);
            var defaults = JsonSerializer.Deserialize<ComparisonDefaults>(json);
            return defaults ?? CreateDefault();
        }
        catch
        {
            return CreateDefault();
        }
    }

    public static void Save(string path, ComparisonDefaults defaults)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        var json = JsonSerializer.Serialize(defaults, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
}
