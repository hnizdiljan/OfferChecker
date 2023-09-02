public interface ICrawler
{
    Task<Product> CrawlAsync(ProductConfig config);
}