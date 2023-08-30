using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class DefaultCrawler : ICrawler
{
    private readonly HttpClient _httpClient;

    public DefaultCrawler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Product> CrawlAsync(string url, string name)
    {
        HttpResponseMessage response = default;
        try
        {
            response = await _httpClient.GetAsync(url);
        }catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }


        if (!response.IsSuccessStatusCode)
        {
            // Zde můžete logovat nebo zpracovat chybový stav
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<APIResponse>(content);

        var cheapestOffers = apiResponse.product.cheapestOffers.offers.First();
        var cheapestCertifiedOffers = apiResponse.product.cheapestOffers.offers.First(x => x.isAuthorizedDealer == true);

        var product = new Product
        {
            Name = name,
            LowestPrice = new LowestPriceInfo
            {
                Price = cheapestOffers.price / 100,
                Url = $"https://www.zbozi.cz{cheapestOffers.click}",
                ShopName = cheapestOffers.shop.displayName,
                OfferName = cheapestOffers.displayName
            },
            LowestCertifiedPrice = new LowestPriceInfo
            {
                Price = cheapestCertifiedOffers.price / 100,
                Url = $"https://www.zbozi.cz{cheapestCertifiedOffers.click}",
                ShopName = cheapestCertifiedOffers.shop.displayName,
                OfferName = cheapestCertifiedOffers.displayName
            }
        };

        return product;
    }

    // Třída pro deserializaci API odpovědi

    public class CheapestOffers
    {
        public int count { get; set; }
        public List<Offer> offers { get; set; }
    }

    public class Offer
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public int price { get; set; }
        public Shop shop { get; set; }
        public string click { get; set; }
        public bool? isAuthorizedDealer { get; set; }
    }


    public class ProductAPI
    {
        public CheapestOffers cheapestOffers { get; set; }
    }


    public class APIResponse
    {
        public int status { get; set; }
        public string statusMessage { get; set; }

        public ProductAPI product { get; set; }

    }

    public class Shop
    {
        public string displayName { get; set; }
    }
}