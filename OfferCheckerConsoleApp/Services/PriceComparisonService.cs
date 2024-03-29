﻿using System.Collections.Generic;
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
            var currentProduct = await _crawler.CrawlAsync(productConfig);
            var previousProduct = previousLowestPrices.FirstOrDefault(p => p.ProductName == productConfig.Name);

            NotifyIfPriceChanged(previousProduct, currentProduct, productConfig);
        }
    }

    private void NotifyIfPriceChanged(Product previousProduct, Product currentProduct, ProductConfig productConfig)
    {
        bool isModified = false;

        isModified |= CheckAndNotifyPriceChange(previousProduct, currentProduct, productConfig, (productConfig.OnlyCertified ? "Certified " : "") + "Price", "📉", "📈");


        if (isModified)
        {
            UpdateMemory(currentProduct);
        }
    }

    private bool CheckAndNotifyPriceChange(Product previousPriceInfo, Product currentPriceInfo, ProductConfig productConfig, string priceLabel, string dropEmoji, string riseEmoji)
    {
        if (previousPriceInfo == null)
        {
            NotifyNewPrice(currentPriceInfo, productConfig, priceLabel, "🌟");
            return true;
        }

        decimal percentageChange = CalculatePercentageChange(previousPriceInfo.Price, currentPriceInfo.Price);

        if (currentPriceInfo.Price < previousPriceInfo.Price)
        {
            NotifyPriceChange(currentPriceInfo, previousPriceInfo, productConfig, priceLabel, percentageChange, dropEmoji);
            return true;
        }
        else if (currentPriceInfo.Price > previousPriceInfo.Price)
        {
            NotifyPriceChange(currentPriceInfo, previousPriceInfo, productConfig, priceLabel, percentageChange, riseEmoji);
            return true;
        }

        return false;
    }

    private decimal CalculatePercentageChange(decimal oldPrice, decimal newPrice)
    {
        return Math.Round((newPrice - oldPrice) / oldPrice * 100, 1);
    }

    private void NotifyPriceChange(Product newPriceInfo, Product oldPriceInfo, ProductConfig productConfig, string priceLabel, decimal percentageChange, string emoji)
    {
        string priceUnit = "CZK";
        string direction = percentageChange > 0 ? "+" : "-";

        string message = $"*{priceLabel} Changed!* {emoji}\n\n*Product:* `{productConfig.Name}`\n*Old {priceLabel}:* `{oldPriceInfo.Price} {priceUnit}`\n*New {priceLabel}:* `{newPriceInfo.Price} {priceUnit}`\n*Change:* `{direction}{Math.Abs(percentageChange)}%`\n*Shop:* [Link]({newPriceInfo.Url})";

        _notifier.Notify(message);
    }

    private void NotifyNewPrice(Product priceInfo, ProductConfig productConfig, string priceLabel, string emoji)
    {
        string priceUnit = "CZK";

        string message = $"*New Lowest {priceLabel}!* {emoji}\n\n*Product:* `{productConfig.Name}`\n*{priceLabel}:* `{priceInfo.Price} {priceUnit}`\n*Shop:* [Link]({priceInfo.Url})";

        _notifier.Notify(message);
    }

    private List<Product> LoadMemory()
    {
        if (!File.Exists(MemoryFilePath))
        {
            return new List<Product>();
        }

        string json = File.ReadAllText(MemoryFilePath);
        return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
    }

    private void UpdateMemory(Product updatedProduct)
    {

        var memory = LoadMemory();

        var existingProduct = memory.FirstOrDefault(p => p.ProductName == updatedProduct.ProductName);

        if (existingProduct == null)
        {
            memory.Add(updatedProduct);
        }
        else
        {
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Url = updatedProduct.Url;
            existingProduct.ShopName = updatedProduct.ShopName;
            existingProduct.OfferName = updatedProduct.OfferName;
        }

        string json = JsonSerializer.Serialize(memory);
        File.WriteAllText(MemoryFilePath, json);
    }
}