using System;
using System.Collections.Generic;

namespace AtmWebAppTesting.Helpers
{
    internal class RandomNameAndAccounNumber
    {
        public static string RandomNameGenerator()
        {
            var firstNames = new List<string> { "John", "Jane", "Alex", "Chris", "Katie", "Tom", "Sara", "James", "Laura", "Michael" };
            var lastNames = new List<string> { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin" };

            var random = new Random();
            var randomFirstName = firstNames[random.Next(firstNames.Count)];
            var randomLastName = lastNames[random.Next(lastNames.Count)];

            var randomFullName = $"{randomFirstName} {randomLastName}";

            return randomFullName;
        }

        public static string RandomAccountNumberGenerator()
        {
            Random random = new Random();
            char[] digits = new char[10];

            // Ensure the first digit is non-zero to maintain 10 digits
            digits[0] = (char)('1' + random.Next(9));

            // Generate the remaining 9 digits
            for (int i = 1; i < 10; i++)
            {
                digits[i] = (char)('0' + random.Next(10));
            }

            return new string(digits);
        }
    }
}
