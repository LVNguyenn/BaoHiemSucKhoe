using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Domain
{
    public class User
    {
        [Key]
        public Guid userID { get; set; }

        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string email { get; set; }

        public string password { get; set; }

        public string displayName { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        public string phone { get; set; }

        public string image { get; set; }

        //public string role { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public User()
        {
            Purchases = new List<Purchase>();
        }
    }
}
