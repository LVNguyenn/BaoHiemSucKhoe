using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Domain
{
    public class Purchase
    {
        public Guid id { get; set; }

        public Guid userID { get; set; }

        public DateTime purchaseDate { get; set; }

        public string status { get; set; }

        public string note { get; set; }

        // relationship

        public User User { get; set; }

        public Insurance Insurance { get; set; }
    }
}
