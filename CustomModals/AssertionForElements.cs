using NUnit.Framework;
using OpenQA.Selenium;

namespace AtmWebAppTesting.Helpers
{
    internal class AssertionForElements
    {
        public static void AssertionForTitle(IWebDriver driver, string pageTitle, string message)
        {
            string expectedTitle = pageTitle;
            string actualTitle = driver.Title;
            Assert.AreEqual(expectedTitle, actualTitle, message);
        }
        public static void AssertionForTrue(bool condition, string message)
        {
            Assert.IsTrue(condition, message);
        }
    }
}
