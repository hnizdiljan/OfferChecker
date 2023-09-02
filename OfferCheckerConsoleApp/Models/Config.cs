using System.Collections.Generic;

public class Config
{
    public List<ProductConfig> Products { get; set; }
    public string OutputFile { get; set; }
}

public class ProductConfig
{
    public string Name { get; set; }
    public string URL { get; set; }

    public bool OnlyCertified { get; set; }
    public int RecensionCountLimit { get; set; } = 0;
    public int RatingLimit { get; set; } = 0;
}