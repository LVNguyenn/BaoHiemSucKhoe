using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class PurchaseDetailsDTO
    {
        public Guid id { get; set; }

        public Guid userID { get; set; }

        public string email { get; set; }

        public string insuranceName { get; set; }

        public string name { get; set; }
        
        public string phone { get; set; }

        public DateTime purchaseDate { get; set; }
        
        public string status { get; set; }
        
        public int insurancePrice { get; set; }

        public string note { get; set; }
    }
}
