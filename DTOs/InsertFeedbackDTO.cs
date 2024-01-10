using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class InsertFeedbackDTO
    {
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string email { get; set; }

        public string name { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format")]
        public string phone { get; set; }

        public string message { get; set; }
    }
}
