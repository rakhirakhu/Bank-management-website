using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank_Management.Models
{
    public class BalanceEnquiryModel
    {
        public int UserId { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public decimal Balance { get; set; }

    }
}