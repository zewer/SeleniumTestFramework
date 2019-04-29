using OpenQA.Selenium;
using Selenium.WebElementWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.PageObjects
{
    public class GooglePage : WebPage
    {
        public override int? Timeout { get; set; } = 120;

        public WebElement SearchFld => Document.GetElementByXPath("//*[@title='Пошук']");

        public WebElement GoogleLogo => Document.GetElementByXPath("//img[@alt='Google']");

        public WebElement SearchBtn => Document.GetElementByXPath("//input[@value='Пошук Google']");

        public WebElement NavigationTbl => Document.GetElementByXPath("//*[@id='nav']");

        public WebElement Page7lbl => Document.GetElementByXPath("//a[@aria-label='Page 7']");

        public WebElement Page7SelectedLbl => Document.GetElementByXPath("//td[@class='cur'][text()='7']");

        public WebElement VirtualKeyboardBtn => Document.GetElementByXPath("//div[@aria-label='Транслітерація']");

        public WebElement VirtualKeyboardToolTip => Document.GetElementByXPath("//div[text()='Транслітерація']");
    }
}
