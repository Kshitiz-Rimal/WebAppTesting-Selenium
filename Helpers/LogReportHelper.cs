using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AtmWebAppTesting.Helpers
{
    public class LogReportHelper
    {
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }

        // Static method to parse log file
        public static List<LogReportHelper> ParseLogFile(string logFilePath)
        {
            string format;
            var logEntries = new List<LogReportHelper>();
            string[] lines = File.ReadAllLines(logFilePath);

            foreach (string line in lines)
            {
                // Simple parser assuming a specific log pattern
                try
                {
                    string[] parts = line.Split(new[] { ' ' }, 5, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length < 5) continue;
                    format = "yyyy-MM-dd HH:mm:ss,fff";
                    DateTime date = DateTime.ParseExact(parts[0] + " " + parts[1], format, null);
                    string thread = parts[2].Trim(new char[] { '[', ']' });
                    string level = parts[3];
                    string logger = parts[4].Split('-')[0].Trim();
                    string message = parts[4].Split('-')[1].Trim();

                    logEntries.Add(new LogReportHelper
                    {
                        Date = date,
                        Thread = thread,
                        Level = level,
                        Logger = logger,
                        Message = message
                    });
                }
                catch
                {
                    // Handle parsing errors
                    Console.WriteLine("Error parsing line: " + line);
                }
            }

            return logEntries;
        }

        // Static method to generate HTML report
        public static string GenerateHtmlReport(List<LogReportHelper> logEntries)
        {
            var report = new StringBuilder();

            report.AppendLine("<html>");
            report.AppendLine("<head><title>Log Report</title></head>");
            report.AppendLine("<body>");
            report.AppendLine("<h1>Log Report</h1>");
            report.AppendLine("<table border='1'>");
            report.AppendLine("<tr><th>Date</th><th>Thread</th><th>Level</th><th>Logger</th><th>Message</th></tr>");

            foreach (var entry in logEntries)
            {
                report.AppendLine("<tr>");
                report.AppendLine($"<td>{entry.Date}</td>");
                report.AppendLine($"<td>{entry.Thread}</td>");
                report.AppendLine($"<td>{entry.Level}</td>");
                report.AppendLine($"<td>{entry.Logger}</td>");
                report.AppendLine($"<td>{entry.Message}</td>");
                report.AppendLine("</tr>");
            }

            report.AppendLine("</table>");
            report.AppendLine("</body>");
            report.AppendLine("</html>");

            return report.ToString();
        }

        // Static method to save report to a file
        public static void SaveReportToFile(string report, string filePath)
        {
            try
            {
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Write the report to the specified file
                File.WriteAllText(filePath, report);
                Console.WriteLine($"Report saved successfully to {filePath}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
