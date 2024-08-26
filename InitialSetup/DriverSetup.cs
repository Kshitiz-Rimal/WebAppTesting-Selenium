using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using AtmWebAppTesting.CustomModals;
using AtmWebAppTesting.Database;
using AtmWebAppTesting.Helpers;
using AventStack.ExtentReports;
using log4net.Config;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using AventStack.ExtentReports.Reporter;

namespace AtmWebAppTesting
{
    public class DriverSetup
    {
        public IWebDriver driver;
        public DatabaseHelper databaseHelper;
        public List<UserDataToAdd> userDatas = CSVHelper.GetUserData();
        protected ExtentReports extent;
        protected ExtentTest test;

        [SetUp]
        public void Setup() {
            #region // For Driver setup
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            //options.AddArgument("--headless");  // Enable headless mode
            options.AddArgument("--window-size=1920,1080");
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            #endregion

            #region // Setup for log4net and extends report
            XmlConfigurator.Configure(new FileInfo("log4net.config"));

            // Initialize ExtentReports
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logging");
            Directory.CreateDirectory(logDirectory);
            string reportFilePath = Path.Combine(logDirectory, $"LogReport_ExtentReport{DateTime.Now:yyyyMMdd_HHmmss}.html");
            var htmlReporter = new ExtentHtmlReporter(reportFilePath);
            // Optionally configure the reporter (e.g., set the report name or document title)
            htmlReporter.Config.ReportName = "Test Execution Report";
            htmlReporter.Config.DocumentTitle = "Extent Reports";
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            // Add some system info to the report
            extent.AddSystemInfo("OS", "Windows");
            extent.AddSystemInfo("Browser", "Chrome");
            #endregion

            #region // SQlite connection
            string databaseFilePath = DatabaseHelper.GetDatabasePath("AtmWebAppTest");
            databaseHelper = new DatabaseHelper(databaseFilePath);
            SQLiteConnection connection = databaseHelper.GetConnection();
            #endregion

            #region //Setup Url from SQlite to driver
            SQLiteDataReader reader = databaseHelper.ReadDataFromTable(connection, "NecessaryURLs");
            while (reader.Read())
            {
                if ($"{reader["UrlName"]}" == "BaseUrl")
                {
                    driver.Url = $"{reader["Address"]}";
                }
            }
            databaseHelper.CloseConnection(connection);
            #endregion
        }

        [TearDown]
        public void Teardown() {
            driver.Quit();
            extent.Flush();
        }
    }
}
