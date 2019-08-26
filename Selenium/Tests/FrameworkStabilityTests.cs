using NUnit.Framework;
using Selenium.PageObjects;
using Selenium.Tests;

namespace Selenium
{
    [TestFixture]
    public class FrameworkStabilityTests : BaseTests
    {
        [Test]
        public void SmokeTest()
        {
            Pages.GooglePage.VirtualKeyboardBtn.JSMouseOver();
            Assert.IsTrue(Pages.GooglePage.VirtualKeyboardToolTip.Displayed);

            Pages.GooglePage.SearchFld.JSSendKeys("search321");
            Assert.IsTrue(Pages.GooglePage.SearchFld.GetAttribute("value") == "search321");

            Pages.GooglePage.SearchFld.JSClear();
            Assert.IsTrue(Pages.GooglePage.SearchFld.GetAttribute("value") == string.Empty);

            Pages.GooglePage.SearchFld.SendKeys("search123");
            Pages.GooglePage.SearchBtn.Click();
            Assert.IsTrue(Pages.GooglePage.SearchFld.GetAttribute("value") == "search123");

            Pages.GooglePage.NavigationTbl.ScrollBy(0, 200);
            Pages.GooglePage.NavigationTbl.ScrollIntoView();
            Assert.IsTrue(Pages.GooglePage.NavigationTbl.Displayed);

            Pages.GooglePage.Page7lbl.JSMouseClick();
            Assert.IsTrue(Pages.GooglePage.Page7SelectedLbl.Displayed);

            WebDriver.CreateAlert("Alert: Hi!");
            Assert.IsTrue(WebDriver.GetAlertText(true) == "Alert: Hi!");
        }

        [Test]
        public void TestToVerifyScreenshot()
        {
            Pages.GooglePage.SearchFld.SendKeys("Test1");

            Assert.IsTrue(false);
        }
    }
}
