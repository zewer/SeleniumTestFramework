using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Selenium.WebElementWrapper
{
    public class WebElement
    {
        private RemoteWebDriver driver;

        private By by;
        private int timeout = WebDriver.DefaultTimeout;
        private IWebElement element;
        private Exception lastException;

        public bool Displayed
        {
            get
            {
                var endDate = DateTime.Now.AddSeconds(timeout);
                while (endDate > DateTime.Now)
                {
                    try
                    {
                        return element.Displayed;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        continue;
                    }
                }
                throw lastException;
            }
        }

        public bool Enabled
        {
            get
            {
                var endDate = DateTime.Now.AddSeconds(timeout);
                while (endDate > DateTime.Now)
                {
                    try
                    {
                        return element.Enabled;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        continue;
                    }
                }
                throw lastException;
            }
        }

        public string Text
        {
            get
            {
                var endDate = DateTime.Now.AddSeconds(timeout);
                while (endDate > DateTime.Now)
                {
                    try
                    {
                        return element.Text;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        continue;
                    }
                }
                throw lastException;
            }
        }

        public WebElement(RemoteWebDriver driver, By by)
        {
            this.driver = driver;
            this.by = by;
            InitElement();
        }

        /// <summary>
        /// Initialize element.
        /// </summary>
        private void InitElement()
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    element = driver.FindElement(by);
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    continue;
                }
            }
            throw lastException;
        }

        /// <summary>
        /// Execute click to an element.
        /// </summary>
        public void Click()
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    element.Click();
                    return;
                }
                catch (Exception ex)
                {
                    if (ex is StaleElementReferenceException || 
                        ex is ElementNotVisibleException ||
                        ex is ElementClickInterceptedException)
                    {
                        InitElement();
                        continue;
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// Sendkeys to an element
        /// </summary>
        /// <param name="keysToBeSend">Text to be send</param>
        /// <param name="deletePrevText">Delete previous text - true/false</param>
        public void SendKeys(string keysToBeSend, bool deletePrevText = true)
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    if (deletePrevText)
                    {
                        element.SendKeys(Keys.Control + "a");
                    }
                    element.SendKeys(keysToBeSend);
                    return;
                }
                catch (Exception ex)
                {
                    if (ex is StaleElementReferenceException ||
                        ex is ElementNotVisibleException ||
                        ex is InvalidElementStateException)
                    {
                        InitElement();
                        continue;
                    }
                    throw;
                }
            }
        }

        public string GetAttribute(string attributeName)
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    return element.GetAttribute(attributeName);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    if (ex is StaleElementReferenceException)
                    {
                        InitElement();
                        continue;
                    }
                    throw;
                }
            }
            throw lastException;
        }

        /// <summary>
        /// Scroll to element
        /// </summary>
        /// <param name="scrollIntoView">true/false</param>
        public void ScrollIntoView(bool scrollIntoView = true)
        {
            var until = scrollIntoView.ToString().ToLower();
            string commandToExecute = $"arguments[0].scrollIntoView({until});";
            ExecuteJS(commandToExecute);
        }

        /// <summary>
        /// Execute JS click.
        /// </summary>
        public void JSClick()
        {
            ExecuteJS("arguments[0].click();");
        }

        /// <summary>
        /// Execute JS mouse click.
        /// </summary>
        public void JSMouseClick()
        {
            ExecuteMouseEvent("click");
        }

        /// <summary>
        /// Execute mouseover event to an element.
        /// Use it when you need call a tooltip or expand JS dropdown, etc.
        /// </summary>
        public void JSMouseOver()
        {
            ExecuteMouseEvent("mouseover");
        }

        /// <summary>
        /// Execute selected mouse event
        /// </summary>
        /// <param name="mouseEvent"></param>
        private void ExecuteMouseEvent(string mouseEvent)
        {
            string commandToExecute =
                $"var event = new MouseEvent('{mouseEvent}');" +
                "arguments[0].dispatchEvent(event);";
            ExecuteJS(commandToExecute);
        }

        /// <summary>
        /// Execute mouseover event to an element.
        /// Use it when you need call a tooltip or expand JS dropdown.
        /// </summary>
        /// <param name="mouseEvent"></param>
        [Obsolete("Depricated. Use 'ExecuteMouseEvent' instead")]
        private void InitMouseEvent(string mouseEvent)
        {
            string commandToExecute = "var evt = document.createEvent('MouseEvents');" + 
            $"evt.initMouseEvent('{mouseEvent.ToString()}', true, true, window, 0, 0, 0, 80, 20, false, false, false, false, 0, null);" +
            "element.dispatchEvent(evt);";

            ExecuteJS(commandToExecute);
        }

        /// <summary>
        /// Execute JS to an element.
        /// </summary>
        /// <param name="commandToExecute"></param>
        public void ExecuteJS(string commandToExecute)
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    var executor = driver as IJavaScriptExecutor;
                    executor.ExecuteScript($"{commandToExecute}", element);
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    InitElement();
                    continue;
                }
            }
            throw lastException;
        }
    }
}
