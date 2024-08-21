using AtmWebAppTesting.CustomModals;
using AtmWebAppTesting.Helpers;
using AventStack.ExtentReports;
using log4net;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AtmWebAppTesting.PageObjectModels
{
    public class AdminDashboardAllScenariosPOM
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExtentTest test;
        public IWebDriver driver;
        private readonly SoftAssertion softAssertion;

        public AdminDashboardAllScenariosPOM(IWebDriver driver, ExtentTest test)
        {
            this.driver = driver;
            this.test = test;
            PageFactory.InitElements(driver, this);

            // Initialize SoftAssertion after test is assigned
            this.softAssertion = new SoftAssertion(test);
        }

        #region //Clicks on admin login button
        [FindsBy(How = How.CssSelector, Using = "#adminButton")]
        public IWebElement adminBtn;

        public void ClickOnAdminBtn()
        {
            adminBtn.Click();
            Thread.Sleep(3000);
            softAssertion.AssertionForTitle(driver, "Admin - ATMWebApp", "Admin login Page not found");
        }
        #endregion

        #region //Fills Form for admin login and logs in to admin dashboard
        [FindsBy(How = How.XPath, Using = "//input[@id = 'accountN']")]
        public IWebElement inputAdminName;

        [FindsBy(How = How.XPath, Using = "//input[@id = 'password']")]
        public IWebElement inputAdminPassword;

        [FindsBy(How = How.XPath, Using = "//form[@class = 'user-login-form m-3 me-5 ms-5']//input[@class = 'login-button border-1 text-center']")]
        public IWebElement loginBtn;

        public SoftAssertion LogintoAdmin(string adminName, string adminPassword)
        {
            inputAdminName.Clear();

            inputAdminName.SendKeys(adminName);
            inputAdminPassword.SendKeys(adminPassword);
            log.Info("Inserted value to admin login form");
            test.Log(Status.Info, "Inserted value to admin login form");

            loginBtn.Click();
            log.Info("Login Button was clicked");
            test.Log(Status.Info, "Login Button was clicked");

            //Waits for the user to reach dashboard
            //WaitForElementToLoad.WaitUntilElementDisplayed(driver, 5, By.ClassName("table"));
            //User checks if they reached admin dashboard or not
            softAssertion.AssertionForTitle(driver, "AdminDashboard - ATMWebApp", "Login failed");
            return softAssertion;
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

            softAssertion.CheckAlertPresent(driver, 2);

            softAssertion.CheckIfModalIsNotDisplayed(driver, 2, By.Id("addUserModal"));

            //User waits for the modal to dissappear if all information is valid
            //WaitForElementToLoad.WaitUntilElementNotDisplayed(driver, 5, By.Id("addUserModal"));

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

        public bool EditUserDetailPress()
        {
            if(softAssertion.CheckIfElementIsPresent(driver, 2, By.XPath("//a[normalize-space()='Edit User Detail']")))
            {
                editUserDetailBtn.Click();
                // User Verifies weather they are on the correct redirected page
                softAssertion.AssertionForTitle(driver, "Edit User - ATMWebApp", "Edit user page not found");
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region //Deletes a user
        [FindsBy(How = How.XPath, Using = "//a[normalize-space()='Delete User']")]
        public IWebElement deleteUserBtn;

        public void DeleteUser(string accountNumber)
        {
            if (softAssertion.CheckIfElementIsPresent(driver, 2, By.XPath("//a[normalize-space()='Delete User']")))
            {
                deleteUserBtn.Click();
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();

                // User waits for the updated table to load
                WaitForElementToLoad.WaitUntilElementDisplayed(driver, 5, By.ClassName("table"));

                bool isDeleted = CheckIfUserIsDeleted(accountNumber);
                softAssertion.AssertionForTrue(driver ,isDeleted, "User hasnot been deleted");
            }
        }
        #endregion

        #region //To verify Unique Account Number
        public void VerifyUniqueAccountNumber(string username, string accountNumber)
        {
            int count = CheckForUser(accountNumber);

            softAssertion.AssertionForTrue(driver, count == 1, $"The account number {accountNumber} should be unique but was found {count} times.");
            //Name of the new user that has been added is displayed in the console 
            Console.WriteLine($"Successfully added {username} as a customer");
        }

        #endregion

        #region //Displays a data of a user 
        public void DisplaySearchedUserDetails(string accountNumber)
        {
            int count = CheckForUser(accountNumber);
            softAssertion.AssertionForTrue(driver, count > 0, $"No User having account number {accountNumber} Found");
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

        #region //Close Modal
        [FindsBy(How = How.XPath, Using = "//button[@class = 'close border-1']")]
        public IWebElement modalCloseBtn;
        public void CloseModal()
        {
            modalCloseBtn.Click();
        }
        #endregion

        #region // JS Events
        public void JSExecution()
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("alert('Hello');");
            Thread.Sleep(2000);
            softAssertion.CheckAlertPresent(driver, 2);

            IWebElement footer = driver.FindElement(By.TagName("footer"));
            jse.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'end' });", footer);
            Thread.Sleep(2000);

            IWebElement button = driver.FindElement(By.Id("addUserButton"));

            bool isButtonInView = false;
            while (!isButtonInView)
            {
                // Check if the button is in view
                isButtonInView = (bool)jse.ExecuteScript(
                    "var rect = arguments[0].getBoundingClientRect();" +
                    "return (rect.top >= 0 && rect.bottom <= (window.innerHeight || document.documentElement.clientHeight));",
                    button);

                // If not in view, scroll up
                if (!isButtonInView)
                {
                    jse.ExecuteScript("window.scrollBy(0, -900);");  // Adjust the scroll amount as needed                    
                }
            }
            Thread.Sleep(2000);  // Adjust the delay as needed
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

            softAssertion.AssertionForTrue(driver, accountNumberIndex != -1, $"Header '{headerToFind}' was not found in the table.");
            return accountNumberIndex;
        }
    }
}
