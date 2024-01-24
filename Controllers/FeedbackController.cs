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
    public class FeedbackController : ControllerBase
    {
        private readonly IRepository<Feedback> feedbackRepository;
        private readonly IMapper _mapper;

        public FeedbackController(IRepository<Feedback> feedbackRepository, IMapper mapper)
        {
            this.feedbackRepository = feedbackRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(feedbackRepository.GetAll());
        }

        
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public IActionResult GetById(Guid id)
        {
            var feedback = feedbackRepository.GetById(id);

            if (feedback == null)
            {
                return NotFound();
            }

            var feedbackDTO = _mapper.Map<Feedback>(feedback);
            return Ok(feedbackDTO);
        }

        [HttpPost]
        public IActionResult Create([FromBody] InsertFeedbackDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Trả về lỗi nếu dữ liệu không hợp lệ 
                return BadRequest(ModelState);
            }

            var feedbackDomain = _mapper.Map<Feedback>(dto);
            feedbackDomain.status = "Đang chờ phản hồi";

            var createdFeedback = feedbackRepository.Create(feedbackDomain);
            var feedback_dto = _mapper.Map<Feedback>(feedbackDomain);

            return Ok(new
            {
                Success = true,
                Data = feedback_dto,
            });
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public IActionResult UpdatePurchase(Guid id)
        {
            var feedbackToUpdate = feedbackRepository.GetById(id);

            if (feedbackToUpdate == null)
            {
                return NotFound();
            }

            feedbackToUpdate.status = "Đã phản hồi";
            feedbackRepository.Update(id, feedbackToUpdate);

            return Ok("Phản hồi đã được cập nhật thành công");
        }
    }
}
