using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Bank_Management.Models
{
    public class registeruser
    {
        [Display(Name = "id")]
        public int BankId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //[Display(Name = "Date Of Birth")]

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }
        [Required]

        public string Gender { get; set; }
        [Required]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string Address { get; set; }
        [Required]

        public string State { get; set; }
        [Required]

        public string City { get; set; }
        [Required]

        public string Username { get; set; }
        [Display(Name = "Password")]

        public string Password { get; set; }
        [Display(Name = "Add photo")]
        public byte[] ProfileImage { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        [Display(Name = "Account number")]
        public string AccountNumber { get; set; }



    }
}