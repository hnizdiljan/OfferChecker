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

    public string ConvertToApiUrl(string originalUrl)
    {
        // Parsujeme vstupní URL
        Uri uri = new Uri(originalUrl);

        // Získáme části URL
        string path = uri.AbsolutePath;
        string query = uri.Query;

        // Získáme produkt a variantu z původního URL
        string product = path.Split("/")[2];
        string variant = query.Split("=")[1];

        // Vytvoříme nový URL
        UriBuilder apiUriBuilder = new UriBuilder
        {
            Scheme = "https",
            Host = "www.zbozi.cz",
            Path = $"api/v3/product/{product}/",
            Query = $"productVariant={variant}&limitTopOffers=3&limitCheapOffers=20&filterFields=offersData"
        };

        return apiUriBuilder.ToString();
    }

    // Fetches response data and handles exceptions
    private async Task<HttpResponseMessage> GetHttpResponseAsync(string url)
    {
        try
        {
            return await _httpClient.GetAsync(ConvertToApiUrl(url));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return default;
        }
    }

    // Parses response content to a model
    private async Task<APIResponse> ParseApiResponseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<APIResponse>(content);
    }

    // Finds the cheapest offer based on the configuration
    private Offer FindCheapestOffer(APIResponse apiResponse, ProductConfig config)
    {

        var products = apiResponse.product.cheapestOffers.offers.Where(x => x.shop.recensionCount >= config.RecensionCountLimit && 
                                                                            x.shop.rating >= config.RatingLimit);

        return (config.OnlyCertified)
            ? products.OrderBy(x => x.price).FirstOrDefault(x => x.isAuthorizedDealer == true)
            : products.OrderBy(x => x.price).FirstOrDefault();
    }

    // Main method for crawling product information
    public async Task<Product> CrawlAsync(ProductConfig config)
    {
        var response = await GetHttpResponseAsync(config.URL);

        if (!response?.IsSuccessStatusCode ?? true) return null; // Handle null or unsuccessful responses

        var apiResponse = await ParseApiResponseAsync(response);

        var cheapestOffers = FindCheapestOffer(apiResponse, config);

        if (cheapestOffers == null) return null; // Handle null offers

        return new Product
        {
            ProductName = config.Name,
            Price = cheapestOffers.price / 100,
            Url = $"https://www.zbozi.cz{cheapestOffers.click}",
            ShopName = cheapestOffers.shop.displayName,
            OfferName = cheapestOffers.displayName
        };
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
        public int rating { get; set; }
        public int recensionCount { get; set; }
    }
}