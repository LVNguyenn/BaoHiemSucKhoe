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
using InsuranceManagement.Services;

namespace InsuranceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : Controller
    {
        private static string ApiKey = "AIzaSyDkt_q4U9FdTRai-q8RtAKiP7aW93uxgms";
        private static string Bucket = "insurance-project-file-storage.appspot.com";
        private static string AuthEmail = "truongcuong@gmail.com";
        private static string AuthPassword = "ungdungphantan123";

        private readonly IHostingEnvironment _hostingEnvironment;

        public FileUploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> UploadFileDemo(IFormFile httpPostedFile)
        {
            FileStream stream;
            if (httpPostedFile.Length > 0)
            {
                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string virtualPath = "Content/Images";
                //string physicalPath = Path.Combine(_hostingEnvironment.WebRootPath, virtualPath);
                string physicalPath = Path.Combine(webRootPath, virtualPath, httpPostedFile.FileName);
                using (FileStream fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    await httpPostedFile.CopyToAsync(fileStream);
                    fileStream.Close();
                    var fs = new FileStream(physicalPath, FileMode.Open);

                    //Upload to firebase and get URL.
                    var result = await Task.Run(() => FirebaseService.Upload(fs, httpPostedFile.FileName));
                    return Ok();
                }
            }
            else return BadRequest();
        }
    }
}

