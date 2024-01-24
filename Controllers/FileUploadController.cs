using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InsuranceManagement.Services;
using Microsoft.AspNetCore.Hosting;

namespace InsuranceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPasswordHasher _passwordHasher;

        public FileUploadController(IWebHostEnvironment hostingEnvironment, IPasswordHasher passwordHasher)
        {
            _hostingEnvironment = hostingEnvironment;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileDemo(IFormFile httpPostedFile)
        {
            string result = await FirebaseService.UploadToFirebase(httpPostedFile);
            return Ok(result);
        }
    }
}

