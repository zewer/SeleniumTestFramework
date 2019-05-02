using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Selenium.PageObjects;
using System;

namespace Selenium
{
    public static class WebDriver
    {
        /// <summary>
        /// Represents Browser types
        /// </summary>
        public enum BrowserTypes
        {
            IE,
            Chrome,
            Edge,
            Firefox
        }

        /// <summary>
        /// WebDriver instance.
        /// </summary>
        private static RemoteWebDriver driver = null;

        /// <summary>
        /// Default WebDriver timeout for all operations.
        /// </summary>
        public static int DefaultTimeout { get; set; } = 120;

        /// <summary>
        /// Wait instance for 'wait' operations
        /// </summary>
        private static WebDriverWait wait;

        /// <summary>
        /// Get URL of existing page.
        /// </summary>
        public static string Url => driver.Url;

        /// <summary>
        /// Get Title of existing page.
        /// </summary>
        public static string Title => driver.Title;

        /// <summary>
        /// Get current Browser type
        /// </summary>
        public static BrowserTypes BrowserType { get; private set; }

        /// <summary>
        /// This method performs page loading.
        /// </summary>
        /// <typeparam name="T">Page which is delivered from WebPage class</typeparam>
        /// <returns></returns>
        public static T AttachToPage<T>() where T : WebPage, new()
        {
            if (driver == null)
            {
                throw new NullReferenceException("Browser is not opened. Use OpenBrowser() method first");
            }

            T page = new T();
            if (page.Timeout.HasValue)
            {
                DefaultTimeout = page.Timeout.Value;
            }
            wait = new WebDriverWait(driver, default);
            TryLoadPage();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(DefaultTimeout);
            driver.Manage().Window.Maximize();
            (driver as IJavaScriptExecutor).ExecuteScript("window.focus();");
            page.Document = driver;

            return page;
        }

        /// <summary>
        /// Trying to load page with default timeout
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns></returns>
        private static bool TryLoadPage()
        {
            var endDate = DateTime.Now.AddSeconds(DefaultTimeout);

            while (endDate > DateTime.Now)
            {
                try
                {
                    if (LoadPage())
                    {
                        return true;
                    }
                }
                catch
                {
                    continue;
                }
            }

            throw new Exception($"Unable to load page after {DefaultTimeout} seconds");
        }

        /// <summary>
        /// Wait until page loading
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private static bool LoadPage()
        {
            return wait.Until(d => (d as IJavaScriptExecutor).ExecuteScript("return document.readyState") as string == "complete");
        }

        /// <summary>
        /// Open expected browser and navigate by URL
        /// </summary>
        public static void OpenBrowser(string url)
        {
            if (driver != null)
            {
                Quit();
            }

            // TO DO - extract expected browser type.
            BrowserType = GetBrowser();

            if (BrowserType == BrowserTypes.Chrome)
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--disable-extensions");

                driver = new ChromeDriver(@"C:\Users\Zewer\Desktop\Selenium\packages", options);
            }
            NavigateTo(url);
            driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Get browser type.
        /// </summary>
        /// <returns></returns>
        private static BrowserTypes GetBrowser()
        {
            return BrowserTypes.Chrome;
        }

        /// <summary>
        /// Wait until Ajax page is loading
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        private static bool LoadAjax()
        {
            return ExecuteCondition(d => (d as IJavaScriptExecutor).ExecuteScript("return jQuery.active") as string == "0");
        }

        /// <summary>
        /// Execute condition
        /// </summary>
        /// <param name="conditionToExecute">Condition to be executed</param>
        /// <returns></returns>
        private static bool ExecuteCondition(Func<IWebDriver, bool> conditionToExecute)
        {
            return wait.Until(conditionToExecute);
        }

        /// <summary>
        /// Performs page refresh
        /// </summary>
        public static void RefreshPage()
        {
            driver.Navigate().Refresh();
        }

        /// <summary>
        /// Performs page refresh
        /// </summary>
        public static void NavigateTo(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Test method to generate Alert window in browser.
        /// </summary>
        /// <param name="text">Text to be added to the Alert</param>
        public static void CreateAlert(string text)
        {
            (driver as IJavaScriptExecutor).ExecuteScript($"alert('{text}');");
        }

        /// <summary>
        /// Switch to Alert and extract it's text
        /// </summary>
        /// <param name="accept"></param>
        /// <returns></returns>
        public static string GetAlertText(bool accept = true)
        {
            return HandleAlert(accept);
        }

        /// <summary>
        /// Switch to Alert and accept it
        /// </summary>
        public static void AcceptAlert()
        {
            HandleAlert(true);
        }

        /// <summary>
        /// Switch to Alert and close it
        /// </summary>
        public static void DismissAlert()
        {
            HandleAlert(false);
        }

        /// <summary>
        /// Base method to handle Alert from browser.
        /// </summary>
        /// <param name="accept">Accept/Decline alert</param>
        /// <returns></returns>
        private static string HandleAlert(bool accept)
        {
            string alertText = string.Empty;
            var endDate = DateTime.Now.AddSeconds(DefaultTimeout);

            while (endDate > DateTime.Now)
            {
                try
                {
                    var alert = driver.SwitchTo().Alert();
                    alertText = alert.Text;
                    if (accept)
                    {
                        alert.Accept();
                    }
                    else
                    {
                        alert.Dismiss();
                    }
                }
                catch (NoAlertPresentException)
                {
                    continue;
                }
                break;
            }
            return alertText;
        }

        /// <summary>
        /// Quit from Browser
        /// </summary>
        public static void Quit()
        {
            driver.Quit();
            driver = null;
        }
    }
}
