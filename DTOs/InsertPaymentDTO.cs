using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class InsertPaymentDTO
    {
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string email { get; set; }

        public string name { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có 10 chữ số")]
        public string phone { get; set; }

        public IFormFile image { get; set; }

        [StringLength(14, MinimumLength = 9, ErrorMessage = "Vui lòng nhập số tài khoản hợp lệ (9-14 số)")]
        public string bankAccount { get; set; }

        public string bankName { get; set; }

        public string note { get; set; }

        public Guid userID { get; set; }
    }
}
