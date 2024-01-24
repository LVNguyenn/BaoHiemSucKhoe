using AutoMapper;
using InsuranceManagement.Data;
using InsuranceManagement.Domain;
using InsuranceManagement.DTOs;
using InsuranceManagement.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IRepository<Payment> paymentRepository;
        private readonly IMapper _mapper;

        public PaymentController(IRepository<Payment> paymentRepository, IMapper mapper)
        {
            this.paymentRepository = paymentRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok(paymentRepository.GetAll());
        }
        
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public IActionResult GetById(Guid id)
        {
            var payment = paymentRepository.GetById(id);

            if (payment == null)
            {
                return NotFound();
            }

            var paymentDTO = _mapper.Map<Payment>(payment);
            return Ok(paymentDTO);
        }

        
        [HttpPost]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromForm] InsertPaymentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Trả về lỗi nếu dữ liệu không hợp lệ 
                return BadRequest(ModelState);
            }

            string result = await FirebaseService.UploadToFirebase(dto.image);

            var paymentDomain = _mapper.Map<Payment>(dto);
            paymentDomain.status = "Đang chờ phản hồi";
            paymentDomain.reason = "";
            paymentDomain.image = result;

            var createdInsurance = paymentRepository.Create(paymentDomain);
            var payment_dto = _mapper.Map<Payment>(paymentDomain);

            return Ok(new
            {
                Success = true,
                Data = payment_dto,
            });
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public IActionResult Update([FromRoute] Guid id, [FromForm] UpdatePaymentDTO dto)
        {
            var paymentDomain = paymentRepository.GetById(id);

            if (paymentDomain == null)
            {
                return NotFound();
            }

            paymentDomain.status = dto.status;
            paymentDomain.reason = dto.reason;

            paymentRepository.Update(id, paymentDomain);

            var updated_payment_dto = _mapper.Map<Payment>(paymentDomain);

            return Ok(updated_payment_dto);
        }
        
    }
}
