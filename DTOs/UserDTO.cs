using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class UserDTO
    {
        [Key]
        public string email { get; set; }

        public string password { get; set; }

        public string retypePassword { get; set; }

        public string displayName { get; set; }

        public string phone { get; set; }
    }
}
