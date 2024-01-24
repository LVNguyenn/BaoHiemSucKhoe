using InsuranceManagement.Data;
using InsuranceManagement.Domain;
using InsuranceManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>
    {
        public FeedbackRepository(UserDbContext context) : base(context)
        {

        }
    }
}
