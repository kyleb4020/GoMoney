using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class BankType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    //This will be for things like:
    //-Credit Card
    //-Savings Account
    //-Checking Account
    //-Retirement Account
    //-Loan
}