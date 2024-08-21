using AtmWebAppTesting.Helpers;
using AtmWebAppTesting.PageObjectModels;
using NUnit.Framework;
using System;
using System.Data.SQLite;

namespace AtmWebAppTesting
{
    [Parallelizable(ParallelScope.Children)]
    public class AdminTasks : DriverSetup
    {
        private readonly string newUserName;
        private readonly string accountNumber;

        public AdminTasks()
        {
            newUserName = RandomNameAndAccounNumber.RandomNameGenerator();
            accountNumber = RandomNameAndAccounNumber.RandomAccountNumberGenerator();
        }

        [Test, Order(1)]
        [Parallelizable(ParallelScope.Self)]
        public void LoginAsAdmin()
        {
            AdminDashboardPOM adminDashboardPOM = new AdminDashboardPOM(driver);

            //User Clicks on admin login button
            adminDashboardPOM.ClickOnAdminBtn();

            Assert.Multiple(() =>
            {
                SQLiteConnection connection;
                connection = databaseHelper.GetConnection();

                SQLiteDataReader reader = databaseHelper.ReadDataFromTable(connection, "AdminDetail");
                while (reader.Read())
                {
                    //User enters the admin name and admin password in the respective field and click on submit button
                    adminDashboardPOM.LogintoAdmin($"{reader["AdminName"]}", $"{reader["AdminPassword"]}");
                }
                databaseHelper.CloseConnection(connection);
            }); 
        }

        [Test, Order(2)]
        [Parallelizable(ParallelScope.Self)]
        public void AddNewUser()

        {
            AdminDashboardPOM adminDashboardPOM = new AdminDashboardPOM(driver);

            Assert.Multiple(() =>
            {
                LoginAsAdmin();


                foreach (var userData in userDatas)
                {
                    //As the user reaches admin dashboard the user clicks on add user button
                    adminDashboardPOM.ClickAddUserBtn();

                    //User enters the inforamtion of new Customer
                    adminDashboardPOM.AddNewUser(userData.UserName, userData.AccountNumber, userData.Balance.ToString());

                    adminDashboardPOM.VerifyUniqueAccountNumber(userData.UserName, userData.AccountNumber);
                }
            });

        }

        [Test, Order(3)]
        [Parallelizable(ParallelScope.Self)]
        public void SearchForUser()
        {
            AdminDashboardPOM adminDashboardPOM = new AdminDashboardPOM(driver);

            // User Logs in as admin
            LoginAsAdmin();
            foreach (var userData in userDatas)
            {
                //User searches for a customer and sees the result
                adminDashboardPOM.SearchForCustomer(userData.AccountNumber);

                adminDashboardPOM.DisplaySearchedUserDetails(userData.AccountNumber);
            }

        }

        [Test, Order(4)]
        [Parallelizable(ParallelScope.Self)]
        public void EditAUser()
        {
            AdminDashboardPOM adminDashboardPOM = new AdminDashboardPOM(driver);

            // User Logins
            LoginAsAdmin();

            foreach (var userData in userDatas)
            {
                //User searches for a customer and sees the result
                adminDashboardPOM.SearchForCustomer(userData.AccountNumber);
                adminDashboardPOM.DisplaySearchedUserDetails(userData.AccountNumber);

                // User Clicks on the "Edit User Details" link where they will be redirected to  a new page to edit the Customer Information
                adminDashboardPOM.EditUserDetailPress();

                // User Enters all the new values in the form on the page
                EditUserPOM edit = new EditUserPOM(driver);
                edit.EditUserDetails(userData.UserName, CustomVariables.newPin, CustomVariables.AccountStatus.Deactivate, CustomVariables.addBalance);
                Console.WriteLine("\nData after Editing");
                adminDashboardPOM.DisplaySearchedUserDetails(userData.AccountNumber);
            }

        }

        [Test, Order(5)]
        [Parallelizable(ParallelScope.Self)]
        public void DeleteAUser()
        {
            AdminDashboardPOM adminDashboardPOM = new AdminDashboardPOM(driver);

            // User Login and searches for a customer
            LoginAsAdmin();
            foreach (var userData in userDatas)
            {
                //User searches for a customer and sees the result
                adminDashboardPOM.SearchForCustomer(userData.AccountNumber);
                adminDashboardPOM.DisplaySearchedUserDetails(userData.AccountNumber);

                // User Taps on the "Delete User" link and clicks on "Ok" button on alert
                adminDashboardPOM.DeleteUser(userData.AccountNumber);
            }

        }
    }
}
