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
    public class FeedbackController : ControllerBase
    {
        private UserDbContext userDbContext;

        public FeedbackController(UserDbContext userDbContext)
        {
            this.userDbContext = userDbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var feedback = userDbContext.feedbacks.ToList();
            return Ok(feedback);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(string id)
        {
            var feedback = userDbContext.feedbacks.FirstOrDefault(x => x.id == Guid.Parse(id));
            if (feedback == null)
            {
                return NotFound();
            }

            var feedbackDTO = new Feedback();
            
            feedbackDTO.id = feedback.id;
            feedbackDTO.email = feedback.email;
            feedbackDTO.name = feedback.name;
            feedbackDTO.phone = feedback.phone;
            feedbackDTO.message = feedback.message;
            feedbackDTO.status = feedback.status;

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

            var feedbackDomain = new Feedback()
            {
                email = dto.email,
                name = dto.name,
                phone = dto.phone,
                message = dto.message,
                status = "Đang chờ phản hồi",
            };

            userDbContext.feedbacks.Add(feedbackDomain);
            userDbContext.SaveChanges();

            var feedback_dto = new Feedback()
            {
                id = feedbackDomain.id,
                email = feedbackDomain.email,
                name = feedbackDomain.name,
                phone = feedbackDomain.phone,
                message = feedbackDomain.message,
                status = feedbackDomain.status,
            };

            return Ok(new
            {
                Success = true,
                Data = feedback_dto,
            });
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdatePurchase(string id)
        {
            var feedbackToUpdate = userDbContext.feedbacks.FirstOrDefault(p => p.id == Guid.Parse(id));

            if (feedbackToUpdate == null)
            {
                return NotFound();
            }

            feedbackToUpdate.status = "Đã phản hồi";

            userDbContext.SaveChanges();

            return Ok("Phản hồi đã được cập nhật thành công");
        }
    }
}
