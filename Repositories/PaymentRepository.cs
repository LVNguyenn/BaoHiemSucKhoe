using InsuranceManagement.Data;
using InsuranceManagement.Domain;
using InsuranceManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository(UserDbContext context) : base(context)
        {

        }
    }
}
