using InsuranceManagement.Data;
using InsuranceManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Services
{
    public class InsuranceRepository : GenericRepository<Insurance>
    {
        public InsuranceRepository(UserDbContext context) : base(context)
        {

        }
    }
}
