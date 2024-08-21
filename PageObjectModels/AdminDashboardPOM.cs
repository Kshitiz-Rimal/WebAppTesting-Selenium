using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System.Threading;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using System.Linq;
using AtmWebAppTesting.Helpers;
namespace AtmWebAppTesting
{
    public class AdminDashboardPOM
    {
        public IWebDriver driver;
        public AdminDashboardPOM(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        # region //Clicks on admin login button
        [FindsBy(How = How.CssSelector, Using = "#adminButton")]
        public IWebElement adminBtn;

        public void ClickOnAdminBtn()
        {
            adminBtn.Click();
            Thread.Sleep(3000);
            AssertionForElements.AssertionForTitle(driver, "Admin - ATMWebApp", "Admin login Page not found");
        }
        #endregion

        #region //Fills Form for admin login and logs in to admin dashboard
        [FindsBy(How = How.XPath, Using = "//input[@id = 'accountN']")]
        public IWebElement inputAdminName;

        [FindsBy(How = How.XPath, Using = "//input[@id = 'password']")]
        public IWebElement inputAdminPassword;

        [FindsBy(How = How.XPath, Using = "//form[@class = 'user-login-form m-3 me-5 ms-5']//input[@class = 'login-button border-1 text-center']")]
        public IWebElement loginBtn;

        public void LogintoAdmin(string adminName, string adminPassword)
        {
            inputAdminName.Clear();

            inputAdminName.SendKeys(adminName);
            inputAdminPassword.SendKeys(adminPassword);
            loginBtn.Click();

            //Waits for the user to reach dashboard
            //WaitForElementToLoad.WaitUntilElementDisplayed(driver, 5, By.ClassName("table"));
            //User checks if they reached admin dashboard or not
            AssertionForElements.AssertionForTitle(driver, "AdminDashboard - ATMWebApp", "Login failed");

        }
        #endregion

        #region // Adding a new customer to app
        [FindsBy(How = How.Id, Using = "addUserButton")]
        public IWebElement adminAddUserBtn;

        public void ClickAddUserBtn()
        {
            adminAddUserBtn.Click();
            // The scripts waits untill the modal appears
            WaitForElementToLoad.WaitUntilElementDisplayed(driver, 5, By.Id("addUserModal"));
        }

        [FindsBy(How = How.Id, Using = "customerName")]
        public IWebElement inputUserName;

        [FindsBy(How = How.Id, Using = "accountNumber")]
        public IWebElement inputAccountNumber;

        [FindsBy(How = How.Id, Using = "balance")]
        public IWebElement inputBalance;

        [FindsBy(How = How.XPath, Using = "//form[@id = 'addUserForm']//button[@class = 'btn btn-primary mt-3']")]
        public IWebElement addUserBtn;

        public void AddNewUser(string username, string accountNumber, string balance)
        {
            inputUserName.Clear();
            inputAccountNumber.Clear();
            inputBalance.Clear();

            inputUserName.SendKeys(username);
            inputAccountNumber.SendKeys(accountNumber);
            inputBalance.SendKeys(balance);
            addUserBtn.Click();

            WaitForElementToLoad.WaitUntilAlertDisplayed(driver, 5);
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
            //User waits for the modal to dissappear if all information is valid
            WaitForElementToLoad.WaitUntilElementNotDisplayed(driver, 5, By.Id("addUserModal"));

            WaitForElementToLoad.WaitUntilElementDisplayed(driver, 5, By.ClassName("table"));
        }
        #endregion

        #region //Searching for a user
        [FindsBy(How = How.Id, Using = "searchingUser")]
        public IWebElement searchBoxVal;

        [FindsBy(How = How.Id, Using = "submitSearchButton")]
        public IWebElement searchBoxSubmitBtn;

        public void SearchForCustomer(string searchVal)
        {
            //User Waits until the dashboard has table in it
            WaitForElementToLoad.WaitUntilElementDisplayed(driver, 5, By.ClassName("table"));

            searchBoxVal.SendKeys(searchVal);
            searchBoxSubmitBtn.Click();
            //Console.WriteLine(driver.FindElement(By.ClassName("table")).Text);

            Thread.Sleep(3000);
        }
        #endregion

        #region //Presses a button for edit user details
        [FindsBy(How = How.XPath, Using = "//a[normalize-space()='Edit User Detail']")]
        public IWebElement editUserDetailBtn;

        public void EditUserDetailPress()
        {
            editUserDetailBtn.Click();

            // User Verifies weather they are on the correct redirected page
            AssertionForElements.AssertionForTitle(driver, "Edit User - ATMWebApp", "Edit user page not found");
        }
        #endregion

        #region //Deletes a user
        [FindsBy(How = How.XPath, Using = "//a[normalize-space()='Delete User']")]
        public IWebElement deleteUserBtn;

        public void DeleteUser(string accountNumber)
        {
            deleteUserBtn.Click();
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();

            // User waits for the updated table to load
            WaitForElementToLoad.WaitUntilElementDisplayed(driver, 5, By.ClassName("table"));

            bool isDeleted = CheckIfUserIsDeleted(accountNumber);
            Assert.IsTrue(isDeleted, "User hasnot been deleted");
        }
        #endregion

        #region //To verify Unique Account Number
        public void VerifyUniqueAccountNumber(string username, string accountNumber)
        {
            int count = CheckForUser(accountNumber);
            
            Assert.AreEqual(1, count, $"The account number {accountNumber} should be unique but was found {count} times.");
            //Name of the new user that has been added is displayed in the console 
            Console.WriteLine($"Successfully added {username} as a customer");
        }

        #endregion

        #region //Displays a data of a user 
        public void DisplaySearchedUserDetails(string accountNumber)
        {
            int count = CheckForUser(accountNumber);
            AssertionForElements.AssertionForTrue(count > 0, $"No User having account number {accountNumber} Found");
        }
        #endregion

        #region //Confirm User is Deleted
        private bool CheckIfUserIsDeleted(string accountNumber)
        {
            int count = CheckForUser(accountNumber);
            if (count != 0)
            {
                return false;
            }
            else
            {
                Console.WriteLine("User Has been deleted");
                return true;
            }
        }
        #endregion

        private int CheckForUser(string accountNumber)
        {
            int accountNumberCellIndex = CellIndexForAccountNumber();
            int count = 0;
            // Locate the table
            IWebElement table = driver.FindElement(By.ClassName("table"));

            // Get all the rows of the table
            IList<IWebElement> rows = table.FindElements(By.CssSelector("tbody tr"));

            // Iterate through each row to collect account numbers using the header index
            foreach (var row in rows)
            {
                IList<IWebElement> cells = row.FindElements(By.TagName("td"));

                // Check if any cell contains the account number
                if (cells.Count > 1 && cells[accountNumberCellIndex - 1].Text == accountNumber)
                {
                    Console.WriteLine($"\n{row.Text}");
                    count++;
                }
            }
            return count;
        }
        private int CellIndexForAccountNumber()
        {
            IWebElement headerRow = driver.FindElement(By.CssSelector(".table thead tr"));
            IList<IWebElement> headerCells = headerRow.FindElements(By.TagName("th"));

            string headerToFind = "Account Number";

            int accountNumberIndex = -1;

            // Iterate through each header cell to find the specific header
            for (int i = 0; i < headerCells.Count; i++)
            {
                if (headerCells[i].Text.Equals(headerToFind, StringComparison.OrdinalIgnoreCase))
                {
                    accountNumberIndex = i;
                    break;
                }
            }

            Assert.IsTrue(accountNumberIndex != -1, $"Header '{headerToFind}' was not found in the table.");
            return accountNumberIndex;
        }
    }
}
