using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Enums;
using SchoolAccount.Application.Features.Shared;
using SchoolAccount.Application.Features.SubTask;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Authentication;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Builders.Categories;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Builders;

public class DashboardViewBuilder(
    CalendarOfItemsViewBuilder calendarOfItemsViewBuilder,
    CategoryListViewBuilder categoryListViewBuilder,
    IOrganisationContext organisationContext
)
{
    public DashboardViewModel Build(
        GetSubTasksForCardsResponse cardsResponse,
        CalendarOfItemsPagedResult calendarOfItemsPagedResult,
        CategoryPagedResult categoryPagedResult,
        Uri currentUri
    )
    {
        var dashboardViewItems = new Collection<DashboardViewItem>();

        dashboardViewItems.Add(
            new DashboardViewItem(
                ViewAddressConstraints.CalendarOfItems.Tab,
                calendarOfItemsViewBuilder.BuildForDashboard(calendarOfItemsPagedResult, currentUri)
            )
        );

        dashboardViewItems.Add(
            new DashboardViewItem(
                ViewAddressConstraints.Categories.List,
                categoryListViewBuilder.BuildForDashboard(
                    categoryPagedResult,
                    CategoryListViewModes.Dashboard,
                    currentUri
                )
            )
        );
        
        return new DashboardViewModel(Result.Success(), dashboardViewItems)
        {
            Slides = new SliderCollection(cardsResponse.SubTasks
                .Select(x => new SliderComponent()
                {
                    Title = x.Name,
                    Description = x.Description,
                    Status = new NodeComponent
                    {
                        Value = string.Join(" ", x.Types.Select(t => t.DisplayValue)),
                    },
                    Metadata = x.Tags
                        .Select(t => new NodeComponent
                        {
                            Value = t.DisplayValue,
                            Colour = t.Colour,
                            Group = t.Group
                        })
                        .ToCollection(),
                    Conditions = organisationContext.Organisation switch
                    {
                        TrustOrganisation trust => trust.Establishments
                            .SelectMany(t => x.Condition
                            .Select(c => new NodeComponent
                            {
                                Group = t.Name,
                                Value = $"{c.Identifier} {c.ComparitorType.Comparitor()} {c.Value}",
                                Colour = c.DetermineColour(t)
                            }))
                            .ToCollection(),
                        EstablishmentOrganisation establishment => x.Condition
                            .Select(c => new NodeComponent
                            {
                                Value = $"{c.Identifier} {c.ComparitorType} {c.Value}",
                                Colour = c.DetermineColour(establishment)
                            })
                            .ToCollection(),
                        _ => []
                    } ,
                    Action = new NodeComponent { Value = "Read the guidance", Url = "#" },
                })
            )
        };
    }
}
