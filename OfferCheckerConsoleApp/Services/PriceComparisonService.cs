using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

public class PriceComparisonService : IPriceComparisonService
{
    private readonly ICrawler _crawler;
    private readonly ITelegramNotifier _notifier;
    private const string MemoryFilePath = "memory.json";

    public PriceComparisonService(ICrawler crawler, ITelegramNotifier notifier)
    {
        _crawler = crawler;
        _notifier = notifier;
    }

    public async Task CompareAndNotifyAsync(List<ProductConfig> products)
    {
        List<Product> previousLowestPrices = LoadMemory();

        foreach (var productConfig in products)
        {
            var product = await _crawler.CrawlAsync(productConfig.URL, productConfig.Name);
            var previousProduct = previousLowestPrices.FirstOrDefault(p => p.Name == productConfig.Name);
            var isModified = false;

            // Pro nejnižší cenu
            if (previousProduct != null)
            {
                if (product.LowestPrice.Price < previousProduct.LowestPrice.Price)
                {
                    _notifier.Notify($"*Price Dropped!* 📉\n\n*Product:* `{productConfig.Name}`\n*New Price:* `{product.LowestPrice.Price}`\n*Shop:* [Link]({product.LowestPrice.Url})");
                    isModified = true;
                }
                else if (product.LowestPrice.Price > previousProduct.LowestPrice.Price)
                {
                    _notifier.Notify($"*Price Increased!* 📈\n\n*Product:* `{productConfig.Name}`\n*New Price:* `{product.LowestPrice.Price}`\n*Shop:* [Link]({product.LowestPrice.Url})");
                    isModified = true;
                }
            }
            else
            {
                _notifier.Notify($"*New Lowest Price!* 🌟\n\n*Product:* `{productConfig.Name}`\n*Price:* `{product.LowestPrice.Price}`\n*Shop:* [Link]({product.LowestPrice.Url})");
                isModified = true;
            }

            // Pro nejnižší certifikovanou cenu
            if (previousProduct != null)
            {
                if (product.LowestCertifiedPrice.Price < previousProduct.LowestCertifiedPrice.Price)
                {
                    _notifier.Notify($"*Certified Price Dropped!* 📉\n\n*Product:* `{productConfig.Name}`\n*New Certified Price:* `{product.LowestCertifiedPrice.Price}`\n*Shop:* [Link]({product.LowestCertifiedPrice.Url})");
                    isModified = true;
                }
                else if (product.LowestCertifiedPrice.Price > previousProduct.LowestCertifiedPrice.Price)
                {
                    _notifier.Notify($"*Certified Price Increased!* 📈\n\n*Product:* `{productConfig.Name}`\n*New Certified Price:* `{product.LowestCertifiedPrice.Price}`\n*Shop:* [Link]({product.LowestCertifiedPrice.Url})");
                    isModified = true;
                }
            }
            else
            {
                _notifier.Notify($"*New Lowest Certified Price!* 🌟\n\n*Product:* `{productConfig.Name}`\n*Certified Price:* `{product.LowestCertifiedPrice.Price}`\n*Shop:* [Link]({product.LowestCertifiedPrice.Url})");
                isModified = true;
            }

            if (isModified)
            {
                UpdateMemory(product, previousLowestPrices);
            }
        }
    }

    private List<Product> LoadMemory()
    {
        if (!File.Exists(MemoryFilePath))
        {
            return new List<Product>();
        }

        string json = File.ReadAllText(MemoryFilePath);
        return JsonSerializer.Deserialize<List<Product>>(json);
    }

    private void UpdateMemory(Product updatedProduct, List<Product> currentMemory)
    {
        var existingProduct = currentMemory.FirstOrDefault(p => p.Name == updatedProduct.Name);

        if (existingProduct == null)
        {
            currentMemory.Add(updatedProduct);
        }
        else
        {
            existingProduct.LowestPrice = updatedProduct.LowestPrice;
            existingProduct.LowestCertifiedPrice = updatedProduct.LowestCertifiedPrice;
        }

        string json = JsonSerializer.Serialize(currentMemory);
        File.WriteAllText(MemoryFilePath, json);
    }
}