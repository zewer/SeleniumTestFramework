using System;
using System.Threading;
using NUnit.Framework;
using Selenium.PageObjects;

namespace Selenium
{
    [TestFixture]
    public class FrameworkStabilityTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            WebDriver.OpenBrowser("https://google.com");
        }

        [Test]
        public void SmokeTest()
        {
            Application.GooglePage.VirtualKeyboardBtn.JSMouseOver();
            Assert.IsTrue(Application.GooglePage.VirtualKeyboardToolTip.Displayed);

            Application.GooglePage.SearchFld.SendKeys("search123");
            Application.GooglePage.SearchBtn.Click();
            Assert.IsTrue(Application.GooglePage.SearchFld.GetAttribute("value") == "search123");

            Application.GooglePage.NavigationTbl.ScrollBy(0, 200);
            Application.GooglePage.NavigationTbl.ScrollIntoView();
            Assert.IsTrue(Application.GooglePage.NavigationTbl.Displayed);

            Application.GooglePage.Page7lbl.JSMouseClick();
            Assert.IsTrue(Application.GooglePage.Page7SelectedLbl.Displayed);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            WebDriver.Quit();
        }
    }
}
