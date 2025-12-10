using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using SchoolAccount.Application;
using SchoolAccount.Infrastructure;
using SchoolAccount.Web.Manage;
using SchoolAccount.Web.Manage.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var appConfigEndpoint = builder.Configuration["AppConfigEndpoint"];
var managedIdentityClientId = builder.Configuration["MANAGED_IDENTITY_CLIENT_ID"];
var tenantId = builder.Configuration["TenantId"];

builder.Services.AddApplication().AddInfrastructure(builder.Configuration).AddPresentation();

if (!string.IsNullOrEmpty(appConfigEndpoint))
{
    builder.Configuration.AddAzureConfigurations(appConfigEndpoint, 
        managedIdentityClientId, 
        tenantId);
}

builder.Services.AddAzureAppConfiguration();

builder.Services.AddFeatureManagement()
    .AddFeatureFilter<TimeWindowFilter>()
    .AddFeatureFilter<PercentageFilter>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAzureAppConfiguration();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
