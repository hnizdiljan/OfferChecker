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
}