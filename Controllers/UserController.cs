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
    public class UserController : ControllerBase
    {
        private UserDbContext userDbContext;

        public UserController(UserDbContext userDbContext)
        {
            this.userDbContext = userDbContext;
        }

        [HttpGet]
        //[Route("{email:string}/{password:string}")]
        public IActionResult GetByEmailAndPassword (string email, string password)
        {
            var user = userDbContext.users.FirstOrDefault(x => x.email == email);
            if (user == null)
            {
                return NotFound(new { errorCode = 1, errorMessage = "Tài khoản không tồn tại" });
            }

            if (user.password != password)
            {
                return BadRequest(new { errorCode = 2, errorMessage = "Mật khẩu không đúng" });
            }

            var userDTO = new UserDTO();
            userDTO.email = user.email;
            userDTO.displayName = user.displayName;
            userDTO.phone = user.phone;
          
            return Ok(new { errorCode = 0, errorMessage = "Đăng nhập thành công", userDTO });
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO dto)
        {
            // Kiểm tra xem có thiếu thông tin không
            if (string.IsNullOrWhiteSpace(dto.email) || string.IsNullOrWhiteSpace(dto.password) ||
                string.IsNullOrWhiteSpace(dto.displayName) || string.IsNullOrWhiteSpace(dto.phone))
            {
                return BadRequest(new { errorCode = 3, errorMessage = "Bạn cần nhập đầy đủ thông tin để đăng ký" });
            }

            // Kiểm tra xem email đã tồn tại hay chưa
            var existingUser = userDbContext.users.FirstOrDefault(x => x.email == dto.email);
            if (existingUser != null)
            {
                return BadRequest(new { errorCode = 4, errorMessage = "Tài khoản này đã tồn tại" });
            }

            // Kiểm tra xem password và retypePassword có giống nhau không
            if (dto.password != dto.retypePassword)
            {
                return BadRequest(new { errorCode = 5, errorMessage = "Mật khẩu và Nhập lại mật khẩu không giống nhau. Xin kiểm tra lại!" });
            }

            var userDomain = new User()
            {
                email = dto.email,
                password = dto.password,
                displayName = dto.displayName,
                phone = dto.phone
            };

            userDbContext.users.Add(userDomain);
            userDbContext.SaveChanges();

            var user_dto = new UserDTO()
            {
                email = userDomain.email,
                displayName = userDomain.displayName,
                phone = userDomain.phone
            };

            return CreatedAtAction(nameof(GetByEmailAndPassword), new { email = user_dto.email },
                new { errorCode = 6, errorMessage = "Tài khoản được tạo thành công", user = user_dto });
        }

    }
}
