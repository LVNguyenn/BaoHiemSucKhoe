using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Domain
{
    public class Feedback
    {
        public Guid id { get; set; }

        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format")]
        public string email { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string message { get; set; }

        public string status { get; set; }
    }
}
