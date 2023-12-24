using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class UserRMPW
    {
        public Guid userID { get; set; }

        public string email { get; set; }

        public string displayName { get; set; }

        public string phone { get; set; }
    }
}
