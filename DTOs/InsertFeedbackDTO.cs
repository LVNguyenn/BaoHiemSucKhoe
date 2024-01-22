using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class InsertFeedbackDTO
    {
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string email { get; set; }

        public string name { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có 10 chữ số")]
        public string phone { get; set; }

        public string message { get; set; }
    }
}
