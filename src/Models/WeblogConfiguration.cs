namespace Demo.Models;

public class WeblogConfiguration
{
    public const string Key = "Weblog";

    public string ApplicationName { get; set; }
    public string ApplicationBasePath { get; set; } = "/";
    public int PostPageSize { get; set; } = 10000;
    public int HomePagePostCount { get; set; } = 30;
    public string PayPalEmail { get; set; }
    public EmailConfiguration Email { get; set; } = new EmailConfiguration();
}