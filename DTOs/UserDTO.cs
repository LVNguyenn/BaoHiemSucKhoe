﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class UserDTO
    {
        [Key]
        public Guid userID { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string displayName { get; set; }

        public string phone { get; set; }

        public string image { get; set; }
    }
}
