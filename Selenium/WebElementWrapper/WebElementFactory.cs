using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Selenium.WebElementWrapper
{
    public static class WebElementFactory
    {
        public static WebElement GetElementByXPath(this RemoteWebDriver driver, string xpath)
        {
            return GetElement(driver, By.XPath(xpath));
        }

        public static WebElement GetElementByText(this RemoteWebDriver driver, string text)
        {
            return GetElement(driver, By.LinkText(text));
        }

        public static WebElement GetElement(this RemoteWebDriver driver, By by)
        {
            return new WebElement(driver, by);
        }
    }
}
