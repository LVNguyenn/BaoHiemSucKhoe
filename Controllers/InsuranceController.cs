using AutoMapper;
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
    public class InsuranceController : ControllerBase
    {   
        private readonly IRepository<Insurance> insuranceRepository;
        private readonly IMapper _mapper;
        
        public InsuranceController(IRepository<Insurance> insuranceRepository, IMapper mapper)
        { 
            this.insuranceRepository = insuranceRepository;
            this._mapper = mapper;
        }
        
        [HttpGet] public IActionResult GetAll()
        {
            return Ok(insuranceRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        { 
            var insuranceEntity = insuranceRepository.GetById(id);
            
            if (insuranceEntity == null)
            { 
                return NotFound();
            }

            var insuranceDTO = _mapper.Map<InsuranceDTO>(insuranceEntity);
            return Ok(insuranceDTO);
        }

        [HttpPost] public async Task<IActionResult> Create([FromForm] InsertInsuranceDTO dto) 
        { 
            string result = await FirebaseService.UploadToFirebase(dto.image);
            var insuranceEntity = _mapper.Map<Insurance>(dto);
            var createdInsurance = insuranceRepository.Create(insuranceEntity);
            var insurance_dto = _mapper.Map<InsuranceDTO>(createdInsurance);

            return CreatedAtAction(nameof(GetById), new { id = insurance_dto.id }, insurance_dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] UpdateInsuranceDTO dto)
        {
            var insuranceEntity = insuranceRepository.GetById(id);

            if (insuranceEntity == null)
            {
                return NotFound();
            }

            string result = await FirebaseService.UploadToFirebase(dto.image);

            insuranceEntity = _mapper.Map<Insurance>(dto);
            insuranceEntity.id = id;
            insuranceEntity.image = result;

            insuranceRepository.Update(id, insuranceEntity);

            var updated_insurance_dto = _mapper.Map<InsuranceDTO>(insuranceEntity);
            return Ok(updated_insurance_dto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var insuranceEntity = insuranceRepository.GetById(id);

            if (insuranceEntity == null)
            {
                return NotFound();
            }

            insuranceRepository.Delete(id);

            var insuranceDTO = _mapper.Map<InsuranceDTO>(insuranceEntity);
            return Ok(insuranceDTO);
        }
    }
}
