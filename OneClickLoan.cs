using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank_Management.Models
{
    public class OneClickLoan
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string PANCardNumber { get; set; }
        public DateTime LoanDate { get; set; }
        public string Password { get; set; }

    }
}