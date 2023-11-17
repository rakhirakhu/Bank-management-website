using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Bank_Management.Models
{
    public class BankAccount
    {

        public int AccountId { get; set; }
        [Display(Name = "  User id")]

        public int UserId { get; set; }
        [Display(Name ="Account holder name")]
        public string AccountHolderName { get; set; }
        public string BranchName { get; set; }

        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public DateTime  OpenDate { get; set; }
        public decimal Balance { get; set; }
        public decimal Amount { get; set; }

    }
} 