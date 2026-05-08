using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using SchoolAccount.Integration.AcademiesApi.Models;

namespace SchoolAccount.Integration.AcademiesApi.Services;

public interface IOrganisationApiService
{
    Task<AcademyOrganisation?> GetEstablishment(string ukPrn);
}

[SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
public class OrganisationApiService(HttpClient httpClient) : IOrganisationApiService
{
    public async Task<AcademyOrganisation?> GetEstablishment(string ukPrn)
    {

        var response = await httpClient.GetAsync($"establishment/{ukPrn}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"{response.StatusCode}: Could not read organisations");
        }

        return await response.Content.ReadFromJsonAsync<AcademyOrganisation>() ?? null;
    }   
}