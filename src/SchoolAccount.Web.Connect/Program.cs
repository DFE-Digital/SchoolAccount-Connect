using GovUk.Frontend.AspNetCore;
using SchoolAccount.Application;
using SchoolAccount.Infrastructure;
using SchoolAccount.Web.Connect;
using SchoolAccount.Web.Connect.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationSights(builder.Configuration);

builder.Services.AddAzureAppConfiguration();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseGovUkFrontend();
app.UseStaticFiles();

app.UseAzureAppConfiguration();

app.UseHttpsRedirection();

app.UseSession();
app.UseRouting();

app.ExceptionHandlers();

app.UseAuthentication();
app.AddMiddleware();
app.UseAuthorization();

app.ConfigureAreas();
app.StripHeaders();

app.Run();

namespace SchoolAccount.Web.Connect
{
    public partial class Program
    {
        // This partial class is used to allow the Program class to be extended in other files.
    }
}

