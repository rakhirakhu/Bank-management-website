﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank_Management.Models
{
    public class ChangePasswordModel
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
      
    }
}