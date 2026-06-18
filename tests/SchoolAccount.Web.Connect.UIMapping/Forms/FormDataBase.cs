using Bogus;
using PlaywrightTests.GlobalConstants.Global;

namespace PlaywrightTests.DfE.UIMapping.Forms;

/// <summary>
/// This allows all form data generators to use the same Faker seed
/// </summary>

public abstract class FormDataBase
{
    public static string testEmail = Global.TestEmailAddress;

    protected FormDataBase() { }

    // The seed used for deterministic data generation
    // Seed uses day of year, hour and minute to allow for each run to have different data, but be recreatable
    private static readonly int Seed = int.Parse(
        $"{DateTime.UtcNow.DayOfYear}{DateTime.UtcNow.Hour:D2}{DateTime.UtcNow.Minute:D2}"
    );
    protected static readonly Faker _faker = new Faker("en_GB") { Random = new Randomizer(Seed) };

    // Log the seed to the console for recording/debugging purposes
    static FormDataBase()
    {
        Console.WriteLine($"Faker Randomizer Seed: {Seed}");
    }
}