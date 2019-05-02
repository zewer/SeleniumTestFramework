using OpenQA.Selenium.Remote;

namespace Selenium.PageObjects
{
    /// <summary>
    /// Parent class for all Pages
    /// </summary>
    public abstract class WebPage
    {
        public RemoteWebDriver Document { get; set; } = null;

        public virtual int? Timeout { get; set; }
    }
}
