using GovUk.Frontend.AspNetCore;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using SchoolAccount.Application;
using SchoolAccount.Infrastructure;
using SchoolAccount.Web.Manage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication().AddInfrastructure(builder.Configuration).AddPresentation(builder.Configuration);

builder.Services.AddAzureAppConfiguration();

builder.Services.AddFeatureManagement()
    .AddFeatureFilter<TimeWindowFilter>()
    .AddFeatureFilter<PercentageFilter>();

builder.Configure();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseGovUkFrontend();
app.UseStaticFiles();

app.UseAzureAppConfiguration();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureAreas();
app.ExceptionHandlers();

app.Run();
