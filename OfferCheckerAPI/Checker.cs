using OfferCheckerModel.InputConfig;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OfferCheckerAPI
{
    public static class Checker
    {

        public static async Task<Offer> CheckPrices(string productName)
        {
            using var client = new HttpClient();
            var baseURL = $"https://www.zbozi.cz/api/v3/product/{productName}/?limitTopOffers=0&limitCheapOffers=20&filterFields=offersData";

            var content = await client.GetStringAsync(baseURL);

            var cheapestOffer = JsonSerializer.Deserialize<ZboziAPI>(content).product.cheapestOffers.offers.FirstOrDefault(x => x.availability == "in_stock");

            return new Offer()
            {
                Name = cheapestOffer.displayName,
                Price = cheapestOffer.price,
                ShopName = cheapestOffer.shop.displayName,
                URL = $"https://www.zbozi.cz/{cheapestOffer.click}"
            };
        }
    }
}
