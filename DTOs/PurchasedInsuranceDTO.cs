﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.DTOs
{
    public class PurchasedInsuranceDTO
    {
        public string name { get; set; }

        public string title { get; set; }

        public int price { get; set; }

        public string description { get; set; }

        public string period { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}
