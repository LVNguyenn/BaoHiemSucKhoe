using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using InsuranceManagement.Services;
using Firebase.Storage;
using Microsoft.Extensions.Hosting.Internal;
using System.Web;
using Microsoft.AspNetCore.Hosting;

namespace InsuranceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileUploadController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileDemo(IFormFile httpPostedFile)
        {
            string result = await FirebaseService.UploadToFirebase(httpPostedFile);
            return Ok(result);
        }
    }
}

