using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.Json;

public class ProductController
{
    private readonly IConfiguration _configuration;
    private readonly IPriceComparisonService _priceComparisonService;

    public ProductController(IConfiguration configuration, IPriceComparisonService priceComparisonService)
    {
        _configuration = configuration;
        _priceComparisonService = priceComparisonService;
    }

    public async Task Run(string configPath)
    {
        // Načíst konfigurační soubor produktů
        var configJson = System.IO.File.ReadAllText(configPath);
        var productConfigs = JsonSerializer.Deserialize<List<ProductConfig>>(configJson);

        // Porovnat a oznámit ceny
        await _priceComparisonService.CompareAndNotifyAsync(productConfigs);
    }
}