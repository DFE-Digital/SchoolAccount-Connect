using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Category.Models;

namespace SchoolAccount.Application.Features.Category.Query;

public record GetCategoryByIdQuery(int Id) : IQuery<CategoryType>;