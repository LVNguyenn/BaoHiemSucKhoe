using InsuranceManagement.Data;
using InsuranceManagement.Domain;
using InsuranceManagement.DTOs;
using InsuranceManagement.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Repositories
{
    public class PurchaseRepository : GenericRepository<Purchase>
    {
        private readonly UserDbContext context;
        public PurchaseRepository(UserDbContext context) : base(context)
        {
            this.context = context;
        }

        public override Purchase GetById(Guid insuranceId, Guid? userId)
        {
            var purchaseToUpdate = context.purchases.FirstOrDefault(p => p.id == insuranceId && p.userID == userId);

            if (purchaseToUpdate == null)
            {
                return null;
            }

            return purchaseToUpdate;
        }

        public override void Update(Guid insuranceId, Purchase entity, Guid? userId)
        {
            var existingEntity = GetById(insuranceId, userId);

            if (existingEntity != null)
            {
                // Update relevant properties of the existingEntity with entity's properties
                context.Entry(existingEntity).CurrentValues.SetValues(entity);

                context.SaveChanges();
            }
        }

        //public override List<Purchase> GetAll()
        //{
        //    var purchaseDetails = context.purchases
        //        .Join(context.users, p => p.userID, u => u.userID, (p, u) => new { Purchase = p, User = u })
        //        .Join(context.insurances, pu => pu.Purchase.id, i => i.id, (pu, i) => new { PurchaseUser = pu, Insurance = i })
        //        .Select(result => new PurchaseDetailsDTO
        //        {
        //            id = result.Insurance.id,
        //            userID = result.PurchaseUser.User.userID,
        //            email = result.PurchaseUser.User.email,
        //            insuranceName = result.Insurance.name,
        //            name = result.PurchaseUser.User.displayName,
        //            phone = result.PurchaseUser.User.phone,
        //            purchaseDate = result.PurchaseUser.Purchase.purchaseDate,
        //            status = result.PurchaseUser.Purchase.status,
        //            insurancePrice = result.Insurance.price
        //        });
        //        //.ToList();

        //    return purchaseDetails.Cast<Purchase>().ToList();
        //}

        
    }
}
