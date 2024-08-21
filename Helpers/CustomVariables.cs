using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmWebAppTesting.Helpers
{
    public enum MyEnum
    {
        AdminName,
        AdminPassword,
    }
    public static class CustomVariables
    {

        private static readonly Dictionary<MyEnum, string> _settings = new Dictionary<MyEnum, string>
        {
            { MyEnum.AdminName, "Kshitiz" },
            { MyEnum.AdminPassword, "admin123" },
        };

        public static string GetSetting(MyEnum key)
        {
            if (_settings.TryGetValue(key, out var value))
            {
                return value;
            }
            throw new ArgumentException($"No setting found for key {key}");
        }

        #region //Admin Login Credentials
        public static string adminName = "Kshitiz";
        public static string adminPassword = "admin123";
        #endregion

        #region //Adding User(Name and account number are auto generated)
        public static string defaultInitialBalance = "1000000";
        #endregion

        #region //Edit User Variables (Account status, Pin, balance)
        public enum AccountStatus
        {
            Activate,
            Deactivate,
        }
        public static string activateUser = "Activate";
        public static string deactivateUser = "Deactivate";

        public static string newPin = "1111";
        public static string addBalance = "20000";
        #endregion
    }
}
