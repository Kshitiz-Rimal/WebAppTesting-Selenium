using AtmWebAppTesting.Helpers;
using AtmWebAppTesting.PageObjectModels;
using AventStack.ExtentReports;
using log4net;
using log4net.Appender;
using log4net.Repository;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AtmWebAppTesting.CustomModals
{
    public class SoftAssertion
    {

        private readonly List<string> exceptionsList = new List<string>();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly ILoggerRepository repository = LogManager.GetRepository();
        private readonly ExtentTest test;

        public SoftAssertion(ExtentTest test)
        {
            this.test = test;
        }

        public void AssertionForTitle(IWebDriver driver, string pageTitle, string message)
        {
            string expectedTitle = pageTitle;
            string actualTitle = driver.Title;
            try
            {
                Assert.AreEqual(expectedTitle, actualTitle, message);
                log.Info("Action was successful"); 
                test.Log(Status.Info, "Action was successful");

            }
            catch (AssertionException)
            {
                log.Error(message);
                test.Log(Status.Error, message);
                string screenShotPath = ScreenShotHelper.TakeScreenshot(driver,message);
                log.Info($"Screenshot saved at {screenShotPath}");
                test.AddScreenCaptureFromPath(screenShotPath);

                exceptionsList.Add(message);
            }
        }
        public void AssertionForTrue(IWebDriver driver, bool condition, string message)
        {
            try
            {
                Assert.IsTrue(condition, message);
            }
            catch (AssertionException)
            {
                log.Error(message);
                test.Log(Status.Error, message);
                string screenShotPath = ScreenShotHelper.TakeScreenshot(driver, message);
                log.Info($"Screenshot saved at {screenShotPath}");
                test.AddScreenCaptureFromPath(screenShotPath);
                exceptionsList.Add(message);
            }
        }

        public void CheckAlertPresent(IWebDriver driver, int timeoutInSeconds)
        {
            bool isAlertPresent;

            try
            {
                WaitForElementToLoad.WaitUntilAlertDisplayed(driver, timeoutInSeconds);
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();
                isAlertPresent = true;
            }
            catch (WebDriverTimeoutException)
            {
                log.Error("Alert Was Not Displayed");
                test.Log(Status.Error, "Alert Was Not Displayed");
                string screenShotPath = ScreenShotHelper.TakeScreenshot(driver, "Alert Was Not Displayed");
                test.AddScreenCaptureFromPath(screenShotPath);
                isAlertPresent = false;
            }

            try
            {
                Assert.IsTrue(isAlertPresent, "Adding User Failed");
            }
            catch (AssertionException)
            {
                exceptionsList.Add("Alert is not present");
            }

        }

        public void CheckIfModalIsNotDisplayed(IWebDriver driver, int seconds, By locator)
        {
            bool isModalPresent;
            try
            {
                WaitForElementToLoad.WaitUntilElementNotDisplayed(driver, seconds, locator);
                isModalPresent = false;
            }
            catch (WebDriverTimeoutException)
            {
                isModalPresent = true;
                AdminDashboardAllScenariosPOM adminDashboardAllScenariosPOM = new AdminDashboardAllScenariosPOM(driver, test);
                adminDashboardAllScenariosPOM.CloseModal();
            }

            try
            {
                Assert.IsFalse(isModalPresent, "Invalid Data to add user");
            }
            catch (AssertionException)
            {
                log.Error("Modal Still in view");
                test.Log(Status.Error, "Modal Still in view");

                string screenShotPath = ScreenShotHelper.TakeScreenshot(driver, "Modal Still in view");
                log.Info($"Screenshot saved at {screenShotPath}");
                test.AddScreenCaptureFromPath(screenShotPath);

                exceptionsList.Add("Modal Still in view");
            }
        }

        public bool CheckIfElementIsPresent(IWebDriver driver, int seconds, By locator)
        {
            bool isElementPresent;
            try
            {
                WaitForElementToLoad.WaitUntilElementDisplayed(driver, seconds, locator);
                isElementPresent = true;
            }
            catch (WebDriverTimeoutException)
            {
                isElementPresent = false;
            }

            try
            {
                Assert.IsTrue(isElementPresent, "User cannot be edited");
                return true;
            }
            catch (AssertionException)
            {
                log.Error("Element is Present");
                test.Log(Status.Error, "Element is Present");

                string screenShotPath = ScreenShotHelper.TakeScreenshot(driver, "Element is Present");
                log.Info($"Screenshot saved at {screenShotPath}");
                test.AddScreenCaptureFromPath(screenShotPath);

                exceptionsList.Add("Element is Present");
                return false;
            }
        }

        public void AssertAll()
        {

            // Iterate through each appender in the repository
            foreach (IAppender appender in repository.GetAppenders())
            {
                // Check if the appender is a file appender
                if (appender is FileAppender fileAppender)
                {
                    fileAppender.Close(); // Close the appender
                }
                else if (appender is RollingFileAppender rollingFileAppender)
                {
                    rollingFileAppender.Close(); // Close the appender
                }
            }
        }
    }
}
