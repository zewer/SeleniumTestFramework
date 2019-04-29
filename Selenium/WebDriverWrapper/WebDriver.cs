using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Selenium.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                throw new Exception("Browser is not opened. Use OpenBrowser() method first");
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
            var executor = driver as IJavaScriptExecutor;
            return wait.Until(d => executor.ExecuteScript("return document.readyState") as string == "complete");
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
            BrowserType = BrowserTypes.Chrome;
            // TO DO - get expected browser
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
        /// Wait until Ajax page is loading
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        private static bool WaitForAjax(this IWebDriver driver)
        {
            var ajaxIsComplete = false;
            try
            {
                while (!ajaxIsComplete)
                {
                    ajaxIsComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                }
            }
            catch (TimeoutException e)
            {
                Console.Error.WriteLine(e.Message + "Error waiting for ajax");
            }

            return true;
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
        /// Quit from Browser
        /// </summary>
        public static void Quit()
        {
            driver.Quit();
            driver = null;
        }
    }
}
