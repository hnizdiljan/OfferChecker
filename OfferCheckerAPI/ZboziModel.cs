namespace OfferCheckerAPI
{
    //public class AggAvailability
    //{
    //    public string displayName { get; set; }
    //    public string urlName { get; set; }
    //    public int documents { get; set; }
    //}

    //public class AggDistrict
    //{
    //    public string displayName { get; set; }
    //    public string urlName { get; set; }
    //    public int documents { get; set; }
    //    public List<Region> regions { get; set; }
    //}

    //public class Analytics
    //{
    //    public Category category { get; set; }
    //    public Vendor vendor { get; set; }
    //}

    //public class Category
    //{
    //    public int id { get; set; }
    //    public string displayName { get; set; }
    //}

    public class CheapestOffers
    {
        public int count { get; set; }
        public List<ZboziOffer> offers { get; set; }
    }

    //public class Delivery
    //{
    //    public int minPrice { get; set; }
    //    public int count { get; set; }
    //    public bool hasMore { get; set; }
    //}

    public class ZboziOffer
    {
        //public string id { get; set; }
        public string displayName { get; set; }
        //public string url { get; set; }
        public int price { get; set; }
        public string availability { get; set; }
        //public int categoryId { get; set; }
        //public string image { get; set; }
        //public string oldId { get; set; }
        //public int productId { get; set; }
        public Shop shop { get; set; }
        //public Delivery delivery { get; set; }
        //public string trackingUUID { get; set; }
        //public string impression { get; set; }
        public string click { get; set; }
        //public string wa { get; set; }
        //public Pickup pickup { get; set; }
    }

    //public class Pickup
    //{
    //    public int minPrice { get; set; }
    //    public int count { get; set; }
    //    public int min2ndPrice { get; set; }
    //    public int count2nd { get; set; }
    //    public bool hasMore { get; set; }
    //    public int placesCount { get; set; }
    //}

    public class Product
    {
        //public int id { get; set; }
        //public string displayName { get; set; }
        //public int minPrice { get; set; }
        //public int maxPrice { get; set; }
        //public int shopCount { get; set; }
        //public string trackingUUID { get; set; }
        //public string impression { get; set; }
        public CheapestOffers cheapestOffers { get; set; }
        //public Analytics analytics { get; set; }
    }

    //public class Region
    //{
    //    public string displayName { get; set; }
    //    public int documents { get; set; }
    //    public string urlName { get; set; }
    //}

    public class ZboziAPI
    {
        //public int status { get; set; }
        //public string statusMessage { get; set; }
        //public int version { get; set; }
        //public string trackerId { get; set; }
        //public string impressionControl { get; set; }
        public Product product { get; set; }
        //public List<AggAvailability> aggAvailabilities { get; set; }
        //public List<AggDistrict> aggDistricts { get; set; }
        //public string searchQueryResponseType { get; set; }
    }

    public class Shop
    {
        public string displayName { get; set; }
        //public int rating { get; set; }
        //public int recensionCount { get; set; }
        //public string recensionUrl { get; set; }
        //public int id { get; set; }
        //public string logo { get; set; }
    }

    //public class Vendor
    //{
    //    public string id { get; set; }
    //    public string displayName { get; set; }
    //}
}
