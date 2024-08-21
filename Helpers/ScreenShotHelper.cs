using OpenQA.Selenium;
using System;
using System.IO;

namespace AtmWebAppTesting.Helpers
{
    public class ScreenShotHelper
    {
        public static string TakeScreenshot(IWebDriver driver, string screenshotName)
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                Directory.CreateDirectory(screenshotDirectory);
                string filePath = Path.Combine(screenshotDirectory, $"{screenshotName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(filePath);
                Console.WriteLine($"Screenshot saved to: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error taking screenshot: {ex.Message}");
                return ex.ToString();
            }
        }
    }
}
