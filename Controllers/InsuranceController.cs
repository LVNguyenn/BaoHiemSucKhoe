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
    public class InsuranceController : ControllerBase
    {
        private UserDbContext userDbContext;

        public InsuranceController(UserDbContext userDbContext)
        {
            this.userDbContext = userDbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var insurance = userDbContext.insurances.ToList();
            return Ok(insurance);
        }

        [HttpGet]
        [Route("id:Guid")]
        public IActionResult GetById(Guid id)
        {
            var insurance = userDbContext.insurances.FirstOrDefault(x => x.id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            var insuranceDTO = new InsuranceDTO();
            insuranceDTO.id = insurance.id;
            insuranceDTO.title = insurance.title;
            insuranceDTO.price = insurance.price;
            insuranceDTO.description = insurance.description;
            insuranceDTO.period = insurance.period;
            insuranceDTO.image = insurance.image;

            return Ok(insuranceDTO);
        }

        [HttpPost]
        public IActionResult CreateCourse([FromBody] InsuranceDTO dto)
        {
            var insuranceDomain = new Insurance()
            {
                id = Guid.NewGuid(),
                title = dto.title,
                price = dto.price,
                description = dto.description,
                period = dto.period,
                image = dto.image
            };

            userDbContext.insurances.Add(insuranceDomain);
            userDbContext.SaveChanges();

            var insurance_dto = new InsuranceDTO()
            {
                id = insuranceDomain.id,
                title = insuranceDomain.title,
                price = insuranceDomain.price,
                description = insuranceDomain.description,
                period = insuranceDomain.period,
                image = insuranceDomain.image
            };

            return CreatedAtAction(nameof(GetById), new { id = insurance_dto.id }, insurance_dto);
        }
    }
}
