namespace Demo.Models;

public class AppSettingsConfiguration
{
    public const string Key = "AppSettings";

    public string Default { get; set; }

    public string Secret { get; set; }
}

