public interface ICrawler
{
    Task<Product> CrawlAsync(string url, string name);
}