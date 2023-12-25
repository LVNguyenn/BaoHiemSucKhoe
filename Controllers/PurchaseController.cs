using InsuranceManagement.Data;
using InsuranceManagement.Domain;
using InsuranceManagement.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private UserDbContext userDbContext;

        public PurchaseController(UserDbContext userDbContext)
        {
            this.userDbContext = userDbContext;
        }

        [HttpGet("purchase-details")]
        public IActionResult GetPurchaseDetails()
        {
            var purchaseDetails = userDbContext.purchases
                .Join(userDbContext.users, p => p.userID, u => u.userID, (p, u) => new { Purchase = p, User = u })
                .Join(userDbContext.insurances, pu => pu.Purchase.id, i => i.id, (pu, i) => new { PurchaseUser = pu, Insurance = i })
                .Select(result => new
                {
                    Id = result.Insurance.id,
                    UserId = result.PurchaseUser.User.userID,
                    Email = result.PurchaseUser.User.email,
                    InsuranceName = result.Insurance.name,
                    Name = result.PurchaseUser.User.displayName,
                    Phone = result.PurchaseUser.User.phone,
                    PurchaseDate = result.PurchaseUser.Purchase.purchaseDate,
                    Status = result.PurchaseUser.Purchase.status,
                    InsurancePrice = result.Insurance.price
                })
                .ToList();

            return Ok(purchaseDetails);
        }

        [HttpPost]
        public IActionResult Create([FromBody] PurchaseDTO dto)
        {
            var purchaseDomain = new Purchase()
            {
                id = dto.id,
                userID = dto.userID,
                purchaseDate = DateTime.Now,
                status = "Đang chờ xét duyệt",
                note = dto.note,
            };

            userDbContext.purchases.Add(purchaseDomain);
            userDbContext.SaveChanges();

            var purchase_dto = new PurchaseDTO()
            {
                id = purchaseDomain.id,
                userID = purchaseDomain.userID,
                purchaseDate = purchaseDomain.purchaseDate,
                status = purchaseDomain.status,
                note = purchaseDomain.note,
            };

            return Ok(new
            {
                Success = true,
                Data = purchase_dto
            });
        }

        [HttpPut("update-purchase")]
        public IActionResult UpdatePurchase(Guid insuranceId, Guid userId)
        {
            var purchaseToUpdate = userDbContext.purchases.FirstOrDefault(p => p.id == insuranceId && p.userID == userId);

            if (purchaseToUpdate == null)
            {
                return NotFound();
            }

            purchaseToUpdate.status = "Đã mua thành công";

            userDbContext.SaveChanges();

            return Ok("Purchase updated successfully");
        }
    }
}
