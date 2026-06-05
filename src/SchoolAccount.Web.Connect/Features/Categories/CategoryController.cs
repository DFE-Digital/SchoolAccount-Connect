using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;

namespace SchoolAccount.Web.Connect.Features.Categories;

public partial class CategoryController(IQueryHandler<GetCategoryHubQuery, GetCategoryHubResponse> categoryHubHandler)
    : Controller;
