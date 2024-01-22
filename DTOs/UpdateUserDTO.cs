using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class UpdateUserDTO
    {
        //public string password { get; set; }

        //public string retypePassword { get; set; }

        public string displayName { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có 10 chữ số")]
        public string phone { get; set; }

        public IFormFile image { get; set; }
    }
}
