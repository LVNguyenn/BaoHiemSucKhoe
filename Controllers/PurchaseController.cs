using AutoMapper;
using InsuranceManagement.Data;
using InsuranceManagement.Domain;
using InsuranceManagement.DTOs;
using InsuranceManagement.Services;
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
        private readonly IRepository<Purchase> purchaseRepository;
        private readonly IMapper _mapper;
        private readonly UserDbContext userDbContext;

        public PurchaseController(IRepository<Purchase> purchaseRepository, IMapper mapper, UserDbContext userDbContext)
        {
            this.purchaseRepository = purchaseRepository;
            this._mapper = mapper;
            this.userDbContext = userDbContext;
        }

        //[HttpGet("purchase-details")]
        //public IActionResult GetPurchaseDetails()
        //{
        //    return Ok(purchaseRepository.GetAll());
        //}

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
            var purchaseDomain = _mapper.Map<Purchase>(dto);
            purchaseDomain.purchaseDate = DateTime.Now;
            purchaseDomain.status = "Đang chờ xét duyệt";

            var createdPurchase = purchaseRepository.Create(purchaseDomain);
            var purchase_dto = _mapper.Map<PurchaseDTO>(purchaseDomain);

            return Ok(new
            {
                Success = true,
                Data = purchase_dto
            });
        }
        
        [HttpPut("update-purchase")]
        public IActionResult Update(Guid insuranceId, Guid userId)
        {
            var purchaseToUpdate = purchaseRepository.GetById(insuranceId, userId);

            if (purchaseToUpdate == null)
            {
                return NotFound();
            }

            purchaseToUpdate.status = "Đã mua thành công";

            purchaseRepository.Update(insuranceId, purchaseToUpdate, userId);
            return Ok("Tình trạng đã được cập nhật thành công");
        }
    }
}
