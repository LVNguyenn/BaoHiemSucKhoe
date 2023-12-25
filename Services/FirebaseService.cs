using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.CompilerServices;

namespace InsuranceManagement.Services
{
    public class FirebaseService
    {
        private static string ApiKey = "AIzaSyDkt_q4U9FdTRai-q8RtAKiP7aW93uxgms";
        private static string Bucket = "insurance-project-file-storage.appspot.com";
        private static string AuthEmail = "truongcuong@gmail.com";
        private static string AuthPassword = "ungdungphantan123";

        public static async Task<String> Upload(FileStream stream, string fileName)
        {
            // FirebaseStorage.Put method accepts any type of stream.
            //var stream = new MemoryStream(Encoding.ASCII.GetBytes("Hello world!"));
            //var streamd = File.Open(@"C:\someFile.png", FileMode.Open);

            // of course you can login using other method, not just email+password
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            // you can use CancellationTokenSource to cancel the upload midway
            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
                .Child("images")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);

            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // cancel the upload
            // cancellation.Cancel();

            try
            {
                // error during upload will be thrown when you await the task
                return await task;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception was thrown: {0}", ex);
                return ex.Message;
            }
        }

        public static async Task<String> UploadToFirebase(IFormFile file)
        {
            if (file.Length > 0)
            {
                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string virtualPath = "Content/Images";
                string physicalPath = Path.Combine(webRootPath, virtualPath, file.FileName);
                using (FileStream fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Close();
                    var fs = new FileStream(physicalPath, FileMode.Open);

                    //Upload to firebase and get URL.
                    var result = await Task.Run(() => FirebaseService.Upload(fs, file.FileName));
                    fs.Close();

                    return result;
                }
            }
            return null;
        }
    }
}
