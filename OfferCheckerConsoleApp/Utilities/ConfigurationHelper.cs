using System;
using System.IO;
using System.Text.Json;

public static class ConfigurationHelper
{
    public static Config ParseArgs(string[] args)
    {
        Config config = new Config();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-c" || args[i] == "--config")
            {
                if (i + 1 < args.Length)
                {
                    string configFile = args[i + 1];
                    if (File.Exists(configFile))
                    {
                        string json = File.ReadAllText(configFile);
                        config = JsonSerializer.Deserialize<Config>(json);
                    }
                    else
                    {
                        throw new FileNotFoundException("Config file not found");
                    }
                }
            }
            else if (args[i] == "-o" || args[i] == "--output")
            {
                if (i + 1 < args.Length)
                {
                    config.OutputFile = args[i + 1];
                }
            }
        }

        return config;
    }
}