﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Domain
{
    public class Insurance
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public int price { get; set; }

        public string description { get; set; }

        public string period { get; set; }

        public string image { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public Insurance()
        {
            Purchases = new List<Purchase>();
        }
    }
}
