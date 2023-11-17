using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank_Management.Models
{
    public class LoanRequestModel
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string PANCardNumber { get; set; }
        public byte[] PANCardImage { get; set; }
        public decimal RequestedAmount { get; set; }
        public string Status { get; set; }
        public string AdminComment { get; set; }
        public DateTime RequestDate { get; set; }
    }
}