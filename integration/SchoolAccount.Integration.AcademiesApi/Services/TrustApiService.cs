using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Distributed;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DistributedCache.Extensions;

namespace SchoolAccount.Integration.AcademiesApi.Services;

public interface ITrustApiService
{
    Task<AcademyTrust?> GetTrust(string ukPrn);
}

[SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
public class TrustApiService(HttpClient httpClient, IDistributedCache cache) : ITrustApiService
{
    public async Task<AcademyTrust?> GetTrust(string ukPrn)
    {
        return await cache.GetOrCreateAsync(
            $"aca:trust:{ukPrn}",
            async () =>
            {
                var response = await httpClient.GetAsync($"trust/{ukPrn}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"{response.StatusCode}: Could not read organisations");
                }

                return await response.Content.ReadFromJsonAsync<AcademyTrust>() ?? null;
            }
        );
    }
}