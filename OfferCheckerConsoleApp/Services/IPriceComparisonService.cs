public interface IPriceComparisonService
{
    Task CompareAndNotifyAsync(List<ProductConfig> products);
}