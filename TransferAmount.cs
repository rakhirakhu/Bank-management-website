using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_Management.Models
{
    public class TransferAmount
    {


        public int UserId { get; set; }

        public int ToUserId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        public string AccountNumber { get; set; }

        public string  IFSCCode { get; set; }

        public string ToAccountNumber { get; set; }

        public string ToIFSCCode { get; set; }
        public string Password { get; set; }

    }
}