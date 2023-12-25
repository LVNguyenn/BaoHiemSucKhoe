using InsuranceManagement.Data;
using InsuranceManagement.Domain;
using InsuranceManagement.DTOs;
using InsuranceManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var userRMPWList = new List<UserRMPW>();
        //    var users = userDbContext.users.ToList();
        //    foreach (var user in users)
        //    {
        //        var userRMPW = new UserRMPW
        //        {
        //            userID = user.userID,
        //            email = user.email,
        //            displayName = user.displayName,
        //            phone = user.phone,
        //        };
        //        userRMPWList.Add(userRMPW);
        //    }
        //    return Ok(userRMPWList);
        //}

        [HttpGet]
        //[Route("{email:string}/{password:string}")]
        public IActionResult GetByEmailAndPassword(string email, string password)
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
            userDTO.userID = user.userID;
            userDTO.email = user.email;
            userDTO.displayName = user.displayName;
            userDTO.phone = user.phone;
   
            return Ok(new { errorCode = 0, errorMessage = "Đăng nhập thành công", userDTO });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] InsertUserDTO dto)
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

            string result = await FirebaseService.UploadToFirebase(dto.image);

            var userDomain = new User()
            {
                userID = Guid.NewGuid(),
                email = dto.email,
                password = dto.password,
                displayName = dto.displayName,
                phone = dto.phone,
                image = result,
            };

            userDbContext.users.Add(userDomain);
            userDbContext.SaveChanges();

            var user_dto = new UserDTO()
            {
                userID = userDomain.userID,
                email = userDomain.email,
                displayName = userDomain.displayName,
                phone = userDomain.phone,
                image = userDomain.image
            };

            return CreatedAtAction(nameof(GetByEmailAndPassword), new { email = user_dto.email },
                new { errorCode = 6, errorMessage = "Tài khoản được tạo thành công", user = user_dto });
        }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetById(string userId)
        {
            var user = userDbContext.users.FirstOrDefault(x => x.userID == Guid.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            var userDTO = new UserDTO();
            userDTO.userID = user.userID;
            userDTO.email = user.email;
            userDTO.password = user.password;
            userDTO.displayName = user.displayName;
            userDTO.phone = user.phone;

            return Ok(userDTO);
        }

        [HttpPut]
        [Route("{userId}")]
        public IActionResult Update([FromRoute] string userId, [FromBody] UpdateUserDTO dto)
        {
            var userDomain = userDbContext.users.FirstOrDefault(x => x.userID == Guid.Parse(userId));
            if (userDomain == null)
            {
                return NotFound();
            }

            //if (dto.password != dto.retypePassword)
            //{
            //    return BadRequest(new { errorCode = 5, errorMessage = "Mật khẩu và Nhập lại mật khẩu không giống nhau. Xin kiểm tra lại!" });
            //}

            //userDomain.password = dto.password;
            userDomain.displayName = dto.displayName;
            userDomain.phone = dto.phone;

            userDbContext.SaveChanges();
            var updated_user_dto = new UpdateUserDTO()
            {
                //password = userDomain.password,
                //retypePassword = dto.retypePassword,
                displayName = userDomain.displayName,
                phone = userDomain.phone,
            };

            return Ok(updated_user_dto);
        }

        [HttpGet("{userId}/purchased-insurances")]
        public IActionResult GetPurchasedInsurances(string userId)
        {
            var purchasedInsurances = userDbContext.purchases
                .Where(p => p.userID == Guid.Parse(userId))
                .Select(p => new PurchasedInsuranceDTO
                {
                    id = p.Insurance.id.ToString(),
                    name = p.Insurance.name,
                    title = p.Insurance.title,
                    price = p.Insurance.price,
                    description = p.Insurance.description,
                    period = p.Insurance.period,
                    PurchaseDate = p.purchaseDate,
                    status = p.status,
                })
                .ToList();

            return Ok(purchasedInsurances);
        }
    }
}
