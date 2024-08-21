using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace SeleniumTestLearning
{
    public class POMClass
    {
        public IWebDriver driver;
        public POMClass(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        #region //ForRadio Buttons
        // Method to select a radio button by value
        public void SelectRadioButtonByValue(string groupName, string value)
        {
            var lablesInRadioButtons = driver.FindElements(By.XPath(groupName));
            foreach (var radioButton in lablesInRadioButtons)
            {
                if (radioButton.GetAttribute("for") == value)
                {
                    radioButton.FindElement(By.TagName("input")).Click();
                    break;
                }
            }
        }
        public void AssertRadioButtonSelectedByValue(string groupName, string value)
        {
            var lablesInRadioButtons = driver.FindElements(By.XPath(groupName));
            bool isSelected = false;
            foreach (var radioButton in lablesInRadioButtons)
            {
                if (radioButton.GetAttribute("for") == value)
                {
                    isSelected = radioButton.FindElement(By.TagName("input")).Selected;
                    break;
                }
            }
            Assert.IsTrue(isSelected, $"Radio button with value '{value}' should be selected.");
        }

        #endregion

        #region // For New Window
        public void OpenNewWindow()
        {
            string originalWindowHandle = driver.CurrentWindowHandle;
            driver.FindElement(By.Id("openwindow")).Click();

            SwitchToNewWindow();

            AssertNewWindowTitle("QAClick Academy - A Testing Academy to Learn, Earn and Shine");

            CloseCurrentWindow();

            SwitchToOriginalWindow(originalWindowHandle);
        }

        private void SwitchToNewWindow()
        {
            var windowHandles = driver.WindowHandles;
            driver.SwitchTo().Window(windowHandles[windowHandles.Count - 1]);
        }

        private void AssertNewWindowTitle(string expectedTitle)
        {
            Assert.AreEqual(expectedTitle, driver.Title, $"The title of the new window should be '{expectedTitle}'.");
        }

        private void SwitchToOriginalWindow(string originalWindowHandle)
        {
            driver.SwitchTo().Window(originalWindowHandle);
        }

        private void CloseCurrentWindow()
        {
            driver.Close();
        }

        #endregion

        #region //For New Tab

        // Same as new window
        public void OpenNewTab()
        {
            string originalWindowHandle = driver.CurrentWindowHandle;
            driver.FindElement(By.Id("opentab")).Click();

            SwitchToNewWindow();

            AssertNewWindowTitle("QAClick Academy - A Testing Academy to Learn, Earn and Shine");

            CloseCurrentWindow();

            SwitchToOriginalWindow(originalWindowHandle);
        }
        #endregion
    }
}
