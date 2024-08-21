using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmWebAppTesting.CustomModals
{
    public class UserDataToAdd
    {
        [Name("User Name")]
        public string UserName { get; set; }

        [Name("Account Number")]
        public string AccountNumber { get; set; }

        [Name("Balance")]
        public double Balance { get; set; }
    }
}
