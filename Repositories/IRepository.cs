using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Services
{
    public interface IRepository<T>
    {
        List<T> GetAll();

        T GetById(Guid id, Guid? test = null);

        T Create(T entity);

        void Update(Guid id, T entity, Guid? test = null);

        void Delete(Guid id);
    }
}
