using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.PageObjects
{
    public abstract class WebPage
    {
        public RemoteWebDriver Document { get; set; } = null;

        public virtual int? Timeout { get; set; }
    }
}
