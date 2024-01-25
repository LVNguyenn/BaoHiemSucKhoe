using Firebase.Auth;
using InsuranceManagement.Data;
using InsuranceManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly UserDbContext context;
        public UserRepository(UserDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
