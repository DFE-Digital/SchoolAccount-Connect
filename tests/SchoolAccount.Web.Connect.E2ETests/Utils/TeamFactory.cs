using PlaywrightTests.DfE.Infrastructure;
using PlaywrightTests.DfE.UIMapping.Forms;
using PlaywrightTests.GlobalConstants.ConfigTableNames;

namespace PlaywrightTests.DfE.Tests.Utils;

public static class TeamFactory
{
    public static async Task<(int, string)> GenerateAndInsertTeam(Database database, RandomGeneratorOptions options = RandomGeneratorOptions.None)
    {
        var teamFormData = await GenerateTeam(options);
        teamFormData.TeamName = string.Concat("Team-", Guid.NewGuid().ToString().AsSpan(0, 8));

        var insertSql = SQLHelper.GenerateInsertScript(teamFormData, ConfigTableNames.TeamTable);
        Console.WriteLine($"Attempting to insert: {insertSql}");
        _ = await database.InsertAsync(insertSql, teamFormData);

        var selectSql = SQLHelper.GenerateSelectScript<TeamFormData>(ConfigTableNames.TeamTable, $"TeamName = '{teamFormData.TeamName}'");
        var results = await database.SelectAsync(selectSql);

        var teamId = (long)results[0]["Id"];
        Console.WriteLine($"Inserted new Team with ID: {teamId} and name {teamFormData.TeamName}");
        return ((int)teamId, teamFormData.TeamName);
    }
    
    public static async Task<TeamFormData> GenerateTeam(RandomGeneratorOptions options = RandomGeneratorOptions.None)
    {
        var teamFormData = TeamFormData.GenerateRandomData(options);
        Console.WriteLine($"Generated team with name: {teamFormData.TeamName}");
        return teamFormData;
    }
}
