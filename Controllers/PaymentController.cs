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
    public class PaymentController : ControllerBase
    {
        private UserDbContext userDbContext;

        public PaymentController(UserDbContext userDbContext)
        {
            this.userDbContext = userDbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var feedback = userDbContext.payments.ToList();
            return Ok(feedback);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(string id)
        {
            var payment = userDbContext.payments.FirstOrDefault(x => x.id == Guid.Parse(id));
            if (payment == null)
            {
                return NotFound();
            }

            var paymentDTO = new Payment();

            paymentDTO.id = payment.id;
            paymentDTO.email = payment.email;
            paymentDTO.name = payment.name;
            paymentDTO.phone = payment.phone;
            paymentDTO.image = payment.image;
            paymentDTO.bankAccount = payment.bankAccount;
            paymentDTO.bankName = payment.bankName;
            paymentDTO.note = payment.note;
            paymentDTO.status = payment.status;
            paymentDTO.reason = payment.reason;

            return Ok(paymentDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] InsertPaymentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Trả về lỗi nếu dữ liệu không hợp lệ 
                return BadRequest(ModelState);
            }

            string result = await FirebaseService.UploadToFirebase(dto.image);

            var paymentDomain = new Payment()
            {
                email = dto.email,
                name = dto.name,
                phone = dto.phone,
                image = result,
                bankAccount = dto.bankAccount,
                bankName = dto.bankName,
                note = dto.note,
                status = "Đang chờ phản hồi",
                reason = "",
                userID = dto.userID,
            };

            userDbContext.payments.Add(paymentDomain);
            userDbContext.SaveChanges();

            var payment_dto = new Payment()
            {
                id = paymentDomain.id,
                email = paymentDomain.email,
                name = paymentDomain.name,
                phone = paymentDomain.phone,
                image = paymentDomain.image,
                bankAccount = paymentDomain.bankAccount,
                bankName = paymentDomain.bankName,
                note = paymentDomain.note,
                status = paymentDomain.status,
                reason = paymentDomain.reason,
                userID = paymentDomain.userID,
            };

            return Ok(new
            {
                Success = true,
                Data = payment_dto,
            });
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] string id, [FromForm] UpdatePaymentDTO dto)
        {
            var paymentDomain = userDbContext.payments.FirstOrDefault(x => x.id == Guid.Parse(id));
            if (paymentDomain == null)
            {
                return NotFound();
            }

            paymentDomain.status = dto.status;
            paymentDomain.reason = dto.reason;

            userDbContext.SaveChanges();

            var updated_payment_dto = new Payment()
            {
                id = paymentDomain.id,
                email = paymentDomain.email,
                name = paymentDomain.name,
                phone = paymentDomain.phone,
                image = paymentDomain.image,
                bankAccount = paymentDomain.bankAccount,
                bankName = paymentDomain.bankName,
                note = paymentDomain.note,
                status = paymentDomain.status,
                reason = paymentDomain.reason,
            };
            return Ok(updated_payment_dto);
        }
    }
}
