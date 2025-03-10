using FinanceTracker.Domain.Dtos.Owned;
using FinanceTracker.Domain.Entities.Owned;

namespace FinanceTracker.Application.Mappers;

public static class CategoryMapper
{
    public static CategoryDto? MapToCategoryDto(Category? category)
    {
        if (category == null)
        {
            return null;
        }

        var categoryDto = new CategoryDto
        {
            Name = category.Name,
        };
        
        return categoryDto;
    }
}