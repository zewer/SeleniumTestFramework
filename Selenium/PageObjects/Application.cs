namespace Selenium.PageObjects
{
    public static class Application
    {
        public static GooglePage GooglePage => WebDriver.AttachToPage<GooglePage>();
    }
}
