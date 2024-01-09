using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Configuration;

namespace InsuranceManagement.Services
{
    public class FirebaseService
    {
        private static string ApiKey;
        private static string Bucket;
        private static string AuthEmail;
        private static string AuthPassword;

        private static FirebaseAuthLink authLink;

        public static void Initialize(IConfiguration configuration)
        {
            ApiKey = configuration.GetValue<string>("Firebase:ApiKey");
            Bucket = configuration.GetValue<string>("Firebase:Bucket");
            AuthEmail = configuration.GetValue<string>("Firebase:AuthEmail");
            AuthPassword = configuration.GetValue<string>("Firebase:AuthPassword");
        }

        public static async Task<String> Upload(Stream stream, string fileName)
        {
            if (authLink == null || authLink.IsExpired())
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                //var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                authLink = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            }
            
            // you can use CancellationTokenSource to cancel the upload midway
            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken),
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
                var stream = file.OpenReadStream();
                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var result = await Task.Run(() => Upload(stream, fileName));
                return result;
            }
            return null;
        }
    }
}
