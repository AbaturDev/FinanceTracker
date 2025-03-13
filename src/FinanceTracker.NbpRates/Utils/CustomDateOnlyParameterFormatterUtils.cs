using System.Reflection;
using Refit;

namespace FinanceTracker.NbpRates.Utils;

public class CustomDateOnlyParameterFormatterUtils : IUrlParameterFormatter
{
    public string? Format(object? value, ICustomAttributeProvider attributeProvider, Type type)
    {
        if (value is DateOnly dateOnly)
        {
            return dateOnly.ToString("yyyy-MM-dd");
        }
        
        return value?.ToString();
    }
}