using AtmWebAppTesting.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System.Threading;

namespace AtmWebAppTesting.PageObjectModels
{
    internal class EditUserPOM
    {
        public IWebDriver driver;
        public EditUserPOM(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        #region //Inputs new value to edit user form and submits it
        [FindsBy(How = How.Id, Using = "userN")]
        public IWebElement newCustomerName;

        [FindsBy(How = How.Id, Using = "pin")]
        public IWebElement newPin;

        [FindsBy(How = How.Id, Using = "AccountStatus")]
        public IWebElement newAccountStatus;

        [FindsBy(How = How.Id, Using = "Balance")]
        public IWebElement addBalance;

        [FindsBy(How = How.Id, Using = "submitToLogin")]
        public IWebElement submitBtn;

        public void EditUserDetails(string customerName, string pin, CustomVariables.AccountStatus accountStatus, string balance)
        {
            newCustomerName.Clear();
            newCustomerName.SendKeys(customerName);
            newPin.Clear();
            newPin.SendKeys(pin);   
            if (accountStatus == CustomVariables.AccountStatus.Activate)
            {
                new SelectElement(newAccountStatus).SelectByText(CustomVariables.activateUser);
            }
            if (accountStatus == CustomVariables.AccountStatus.Deactivate)
            {
                new SelectElement(newAccountStatus).SelectByText(CustomVariables.deactivateUser);
            }
            addBalance.Clear();
            addBalance.SendKeys(balance);

            submitBtn.Click();

            // User Waits until the form is completed and is redirected back to admin dashboard
            WaitForElementToLoad.WaitUntilElementDisplayed(driver, 5, By.Id("displayAdminName"));

            // User verifies that they are on admin dashboard
            AssertionForElements.AssertionForTitle(driver, "AdminDashboard - ATMWebApp", "Edit user was failed");
            Thread.Sleep(3000);
        }
        #endregion
    }
}
