using InsuranceManagement.Data;
using InsuranceManagement.Domain;
using InsuranceManagement.DTOs;
using InsuranceManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace InsuranceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserDbContext userDbContext;
        private readonly ITokenRepository tokenRepository;
        private readonly IPasswordHasher passwordHasher;

        public UserController(UserDbContext userDbContext, ITokenRepository repository, IPasswordHasher passwordHasher)
        {
            this.userDbContext = userDbContext;
            tokenRepository = repository;
            this.passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var userRMPWList = new List<UserRMPW>();
            var users = userDbContext.users.ToList();
            foreach (var user in users)
            {
                var userRMPW = new UserRMPW
                {
                    userID = user.userID,
                    email = user.email,
                    displayName = user.displayName,
                    phone = user.phone,
                };
                userRMPWList.Add(userRMPW);
            }
            return Ok(userRMPWList);
        }

        //[HttpGet]
        ////[Route("{email:string}/{password:string}")]
        //public IActionResult GetByEmailAndPassword(string email, string password)
        //{
        //    var user = userDbContext.users.FirstOrDefault(x => x.email == email);
        //    if (user == null)
        //    {
        //        return NotFound(new { errorCode = 1, errorMessage = "Tài khoản không tồn tại" });
        //    }
        //    if (user.password != password)
        //    //if (user.password != passwordHasher.HashPassword(password))
        //    {
        //        return BadRequest(new { errorCode = 2, errorMessage = "Mật khẩu không đúng" });
        //    }

        //    var userDTO = new UserDTO();
        //    userDTO.userID = user.userID;
        //    userDTO.email = user.email;
        //    userDTO.displayName = user.displayName;
        //    userDTO.phone = user.phone;

        //    var jwtToken = tokenRepository.CreateJWTToken(userDTO);

        //    /*var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true, // Set the cookie as HTTP-only
        //        SameSite = SameSiteMode.None,
        //    };*/

        //    //Response.Cookies.Append("jwtToken", jwtToken, cookieOptions);
        //    return Ok(new { errorCode = 0, errorMessage = "Đăng nhập thành công", userDTO });
        //    //return Ok(new LoginResponseDto() { token = jwtToken });
        //}

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            var user = userDbContext.users.FirstOrDefault(x => x.email == login.email);
            if (user == null)
            {
                return NotFound(new { errorCode = 1, errorMessage = "Tài khoản không tồn tại" });
            }

            if (user.password != login.password)
            {
                return BadRequest(new { errorCode = 2, errorMessage = "Mật khẩu không đúng" });
            }

            var userDTO = new UserDTO();
            userDTO.userID = user.userID;
            userDTO.email = user.email;
            userDTO.displayName = user.displayName;
            userDTO.phone = user.phone;

            var jwtToken = tokenRepository.CreateJWTToken(userDTO);

            // Phần code xử lý cookie nếu cần thiết

            return Ok(new { errorCode = 0, errorMessage = "Đăng nhập thành công", userDTO });
        }

        [HttpPost("sign-up")]
        public IActionResult CreateUser([FromBody] InsertUserDTO dto)
        {
            if (!ModelState.IsValid)
            { 
                // Trả về lỗi nếu dữ liệu không hợp lệ 
                return BadRequest(ModelState);
            }

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
                userID = Guid.NewGuid(),
                email = dto.email,
                password = dto.password,
                //password = passwordHasher.HashPassword(dto.password),
                displayName = dto.displayName,
                phone = dto.phone,
                role = "Customer"
            };

            userDbContext.users.Add(userDomain);
            userDbContext.SaveChanges();

            var user_dto = new UserDTO()
            {
                userID = userDomain.userID,
                email = userDomain.email,
                displayName = userDomain.displayName,
                phone = userDomain.phone,
                role = userDomain.role,
            };

            return CreatedAtAction(nameof(Login), new { email = user_dto.email },
                new { errorCode = 6, errorMessage = "Tài khoản được tạo thành công", user = user_dto });
        }

        [HttpGet]
        [Route("{userId}")]
        //[Authorize]
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
            userDTO.image = user.image;

            return Ok(userDTO);
        }

        [HttpPut]
        [Route("{userId}")]
        //[Authorize]
        public async Task <IActionResult> Update([FromRoute] string userId, [FromForm] UpdateUserDTO dto)
        {
            var userDomain = userDbContext.users.FirstOrDefault(x => x.userID == Guid.Parse(userId));
            if (userDomain == null)
            {
                return NotFound();
            }

            string result = await FirebaseService.UploadToFirebase(dto.image);

            userDomain.displayName = dto.displayName;
            userDomain.phone = dto.phone;
            userDomain.image = result;

            userDbContext.SaveChanges();

            var user_dto = new UserDTO()
            {
                displayName = userDomain.displayName,
                phone = userDomain.phone,
                image = userDomain.image,
            };

            return Ok(user_dto);
        }

        [HttpGet("{userId}/purchased-insurances")]
        //[Authorize]
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
                    image = p.Insurance.image
                })
                .ToList();

            return Ok(purchasedInsurances);
        }

        [HttpGet("{userId}/payment")]
        //[Authorize]
        public IActionResult GetPayment(string userId)
        {
            // ghi chu, hinh anh, tinh trang
            var payment = userDbContext.payments
                .Where(p => p.userID == Guid.Parse(userId))
                .Select(p => new PaymentDTO
                {
                    note = p.note,
                    image = p.image,
                    status = p.status,
                    reason = p.reason,
                })
                .ToList();

            return Ok(payment);
        }
    }
}
