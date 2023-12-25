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

        public string email { get; set; }

        public string password { get; set; }

        public string displayName { get; set; }

        public string phone { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public User()
        {
            Purchases = new List<Purchase>();
        }
    }
}
