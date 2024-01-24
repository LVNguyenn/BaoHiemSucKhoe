using InsuranceManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Services
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly UserDbContext context;

        public GenericRepository(UserDbContext context)
        {
            this.context = context;
        }

        public virtual T Create(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public virtual void Delete(Guid id)
        {
            var entityToDelete = context.Set<T>().Find(id);
            if (entityToDelete != null)
            {
                context.Set<T>().Remove(entityToDelete);
                context.SaveChanges();
            }
        }

        public virtual List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public virtual T GetById(Guid id, Guid? test)
        {
            return context.Set<T>().Find(id);
        }

        public virtual void Update(Guid id, T entity, Guid? test)
        {
            var existingEntity = context.Set<T>().Find(id);

            if (existingEntity != null)
            {
                // Update relevant properties of the existingEntity with entity's properties
                context.Entry(existingEntity).CurrentValues.SetValues(entity);

                context.SaveChanges();
            }
        }
    }
}
