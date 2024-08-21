using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace AtmWebAppTesting.Helpers
{
    internal class WaitForElementToLoad
    {
        public static void WaitUntilElementDisplayed(IWebDriver driver,int seconds,By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public static void WaitUntilElementNotDisplayed(IWebDriver driver,int seconds,By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        public static void WaitUntilAlertDisplayed(IWebDriver driver,int seconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            wait.Until(ExpectedConditions.AlertIsPresent());
        }
    }}
