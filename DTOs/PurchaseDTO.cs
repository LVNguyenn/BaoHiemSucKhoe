using InsuranceManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class PurchaseDTO
    {
        public Guid id { get; set; }

        public Guid userID { get; set; }

        public DateTime purchaseDate { get; set; }

        public string status { get; set; }

        public string note { get; set; }
    }
}
