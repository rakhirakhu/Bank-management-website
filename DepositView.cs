using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_Management.Models
{
    public class DepositView
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string Password { get; set; }
    }
}