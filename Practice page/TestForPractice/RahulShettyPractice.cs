using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using SeleniumTestLearning;
using System.Threading;

namespace AtmWebAppTesting.Tests.Practice_page
{
    internal class RahulShettyPractice
    {

        private readonly IWebDriver driver = new ChromeDriver();


        [SetUp]
        public void Initialize()
        {
            driver.Navigate().GoToUrl("https://rahulshettyacademy.com/AutomationPractice/");
            Console.WriteLine("Wrote for URL");
        }

        [Test]
        public void TestPractice()
        {
            IWebElement element = driver.FindElement(By.Id("autocomplete"));
            element.SendKeys("Nepal");
            Console.WriteLine("Opened for testing");

            POMClass pOMClass = new POMClass(driver);
            pOMClass.SelectRadioButtonByValue("//div[@id='radio-btn-example']//fieldset//label", "radio3");
            pOMClass.AssertRadioButtonSelectedByValue("//div[@id='radio-btn-example']//fieldset//label", "radio3");

            pOMClass.OpenNewWindow();

            pOMClass.OpenNewTab();
            Thread.Sleep(5000);
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Close();
        }
    }
}
