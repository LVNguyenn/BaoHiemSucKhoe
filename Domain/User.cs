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

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string email { get; set; }

        public string password { get; set; }

        public string displayName { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có 10 chữ số")]
        public string phone { get; set; }

        public string image { get; set; }

        public string role { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public ICollection<Payment> Payments { get; set; }


        public User()
        {
            Purchases = new List<Purchase>();
            Payments = new List<Payment>();
        }
    }
}
