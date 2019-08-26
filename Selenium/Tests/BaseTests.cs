using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Selenium.PageObjects;
using Selenium.Utils;

namespace Selenium.Tests
{
    [TestFixture]
    public abstract class BaseTests
    {
        protected ExtentReports extent;
        protected ExtentTest test;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            extent = new ExtentReports();

            var dir = DesktopUtils.GetDebugFolderPath().Replace("\\bin\\Debug", "");

            var htmlReporter = new ExtentHtmlReporter(dir + "\\TestExecutionResults\\Test.html");
            htmlReporter.Configuration().Theme = Theme.Dark;

            extent.AddSystemInfo("Environment", "Local computer");
            extent.AddSystemInfo("Username", "Zewer");
            extent.AttachReporter(htmlReporter);

            WebDriver.OpenBrowser("https://google.com");
        }

        [SetUp]
        public virtual void SetUp()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public virtual void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
            {
                test.Log(Status.Fail, status + stackTrace + errorMessage);
                (Pages.GooglePage.Document as ITakesScreenshot).GetScreenshot().SaveAsFile(DesktopUtils.GetScreenshotFolderPath() + $"\\{TestContext.CurrentContext.Test.Name}.jpeg");
                test.AddScreenCaptureFromPath(DesktopUtils.GetScreenshotFolderPath() + $"\\{TestContext.CurrentContext.Test.Name}.jpeg");
            }
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            extent.Flush();
            WebDriver.Quit();
        }
    }
}
