﻿using InsuranceManagement.Data;
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
        [Route("{id}")]
        public IActionResult GetById(string id)
        {
            var insurance = userDbContext.insurances.FirstOrDefault(x => x.id == Guid.Parse(id));
            if (insurance == null)
            {
                return NotFound();
            }

            var insuranceDTO = new InsuranceDTO();
            insuranceDTO.id = insurance.id;
            insurance.name = insurance.name;
            insuranceDTO.title = insurance.title;
            insuranceDTO.price = insurance.price;
            insuranceDTO.description = insurance.description;
            insuranceDTO.period = insurance.period;
            insuranceDTO.image = insurance.image;

            return Ok(insuranceDTO);
        }

        [HttpPost]
        public IActionResult CreateInsurance([FromBody] InsertInsuranceDTO dto)
        {
            var insuranceDomain = new Insurance()
            {
                id = Guid.NewGuid(),
                name = dto.name,
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
                name = insuranceDomain.name,
                title = insuranceDomain.title,
                price = insuranceDomain.price,
                description = insuranceDomain.description,
                period = insuranceDomain.period,
                image = insuranceDomain.image
            };

            return CreatedAtAction(nameof(GetById), new { id = insurance_dto.id }, insurance_dto);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] string id, [FromBody] UpdateInsuranceDTO dto)
        {
            var insuranceDomain = userDbContext.insurances.FirstOrDefault(x => x.id == Guid.Parse(id));
            if (insuranceDomain == null)
            {
                return NotFound();
            }

            insuranceDomain.name = dto.name;
            insuranceDomain.title = dto.title;
            insuranceDomain.price = dto.price;
            insuranceDomain.description = dto.description;
            insuranceDomain.period = dto.period;
            insuranceDomain.image = dto.image;

            userDbContext.SaveChanges();
            var updated_insurance_dto = new UpdateInsuranceDTO()
            {
                name = insuranceDomain.name,
                title = insuranceDomain.title,
                price = insuranceDomain.price,
                description = insuranceDomain.description,
                period = insuranceDomain.period,
                image = insuranceDomain.image
            };
            return Ok(updated_insurance_dto);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            var insuranceDomain = userDbContext.insurances.FirstOrDefault(x => x.id == Guid.Parse(id));
            if (insuranceDomain == null)
            {
                return NotFound();
            }

            userDbContext.insurances.Remove(insuranceDomain);
            userDbContext.SaveChanges();

            var insuranceDTO = new InsuranceDTO()
            {
                id = insuranceDomain.id,
                name = insuranceDomain.name,
                title = insuranceDomain.title,
                price = insuranceDomain.price,
                description = insuranceDomain.description,
                period = insuranceDomain.period,
                image = insuranceDomain.image
            };

            return Ok(insuranceDTO);
        }
    }
}
