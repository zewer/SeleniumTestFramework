using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;

namespace Selenium.WebElementWrapper
{
    public class WebElement
    {
        private RemoteWebDriver driver;

        private By by;
        private int timeout = WebDriver.DefaultTimeout;
        private Exception lastException;

        /// <summary>
        /// Get element for any unwrapped operations, like Actions, etc.
        /// </summary>
        public IWebElement Element { get; private set; }

        public bool Displayed
        {
            get
            {
                var endDate = DateTime.Now.AddSeconds(timeout);
                while (endDate > DateTime.Now)
                {
                    try
                    {
                        return Element.Displayed;
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

        public bool Enabled
        {
            get
            {
                var endDate = DateTime.Now.AddSeconds(timeout);
                while (endDate > DateTime.Now)
                {
                    try
                    {
                        return Element.Enabled;
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

        public string Text
        {
            get
            {
                var endDate = DateTime.Now.AddSeconds(timeout);
                while (endDate > DateTime.Now)
                {
                    try
                    {
                        return Element.Text;
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
                    Element = driver.FindElement(by);
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

        #region Selenium operations
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
                    Element.Click();
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
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
            throw lastException;
        }

        /// <summary>
        /// Sendkeys to an element.
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
                        Element.SendKeys(Keys.Control + "a");
                    }
                    Element.SendKeys(keysToBeSend);
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
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
            throw lastException;
        }

        /// <summary>
        /// Delete all information from text.
        /// </summary>
        public void Clear()
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    Element.Clear();
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
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
            throw lastException;
        }

        /// <summary>
        /// Get element's attribute 
        /// </summary>
        /// <param name="attributeName">Attribute name</param>
        /// <returns></returns>
        public string GetAttribute(string attributeName)
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    return Element.GetAttribute(attributeName);
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
        /// Get element's Css attribute .
        /// </summary>
        /// <param name="attributeName">Css attribute name</param>
        /// <returns></returns>
        public string GetCssValue(string attributeName)
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    return Element.GetCssValue(attributeName);
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
        /// Execute Submit operation to an element.
        /// </summary>
        public void Submit()
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    Element.Submit();
                    return;
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
        /// Find new element from existing
        /// </summary>
        /// <param name="by">Locator to be searched for</param>
        /// <returns>new WebElement</returns>
        public WebElement FindElement(By by)
        {
            return new WebElement(driver, by) { Element = Element.FindElement(by) };
        }
        #endregion

        #region JS Operations
        /// <summary>
        /// Scroll to element.
        /// </summary>
        /// <param name="scrollIntoView">true/false</param>
        public void ScrollIntoView(bool scrollIntoView = true)
        {
            var until = scrollIntoView.ToString().ToLower();
            string commandToExecute = $"arguments[0].scrollIntoView({until});";
            ExecuteJS(commandToExecute);
        }

        /// <summary>
        /// Scroll page by x and y offset
        /// </summary>
        /// <param name="x">X offset</param>
        /// <param name="y">Y offset</param>
        public void ScrollBy(int x, int y)
        {
            ExecuteJS($"window.scrollBy({x},{y});");
        }

        /// <summary>
        /// Execute JS click.
        /// </summary>
        public void JSClick()
        {
            ExecuteJS("arguments[0].click();");
        }

        /// <summary>
        /// Set text to element via JS
        /// </summary>
        /// <param name="testToBeSend"></param>
        public void JSSendKeys(string testToBeSend)
        {
            ExecuteJS($"arguments[0].value = '{testToBeSend}'");
        }

        /// <summary>
        /// Delete all text from element
        /// </summary>
        /// <param name="testToBeSend"></param>
        public void JSClear()
        {
            ExecuteJS($"arguments[0].value = ''");
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
            string commandToExecute = "var event = document.createEvent('MouseEvents');" + 
            $"event.initMouseEvent('{mouseEvent.ToString()}', true, true, window, 0, 0, 0, 80, 20, false, false, false, false, 0, null);" +
            "element.dispatchEvent(event);";

            ExecuteJS(commandToExecute);
        }

        /// <summary>
        /// Execute JS to an element.
        /// </summary>
        /// <param name="commandToExecute">Command to be executed</param>
        private void ExecuteJS(string commandToExecute)
        {
            var endDate = DateTime.Now.AddSeconds(timeout);
            while (endDate > DateTime.Now)
            {
                try
                {
                    var executor = driver as IJavaScriptExecutor;
                    executor.ExecuteScript($"{commandToExecute}", Element);
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
        #endregion
    }
}
