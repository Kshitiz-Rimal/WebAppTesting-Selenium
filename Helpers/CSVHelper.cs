using AtmWebAppTesting.CustomModals;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AtmWebAppTesting.Helpers
{
    public class CSVHelper
    {
        public static List<UserDataToAdd> GetUserData()
        {
            string filePath = AppContext.BaseDirectory + "..\\..\\AddUserDetails.csv";
            var reader = new StreamReader(filePath);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<UserDataToAdd>().ToList();
            return records;
        }
    }
}