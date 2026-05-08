using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolAccount.Integration.DfESignIn.Configuration;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Services
{
    public interface IDsiApiService
    {
        Task<List<OrganisationClaim>> GetUserOrganisations(string userId);
    }

    [SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
    public class DsiApiService(HttpClient httpClient, IOptions<DsiApiConfig> options) : IDsiApiService
    {
        private readonly DsiApiConfig _apiConfig = options.Value;

        private string CreateBearerToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_apiConfig.ApiSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _apiConfig.ServiceAudience,
                Issuer = _apiConfig.Issuer,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<List<OrganisationClaim>> GetUserOrganisations(string userId)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CreateBearerToken());

            
            var response = await httpClient.GetAsync($"users/{userId}/v2/organisations");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<OrganisationClaim>();
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"{response.StatusCode}: Could not read organisations");
            }

            return await response.Content.ReadFromJsonAsync<List<OrganisationClaim>>() ?? [];
        }
    }
}
