using BussinessObjects.Utility;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.ImageService
{
    public class ImageService : IImageService
    {
        private readonly FireBaseOptions _fireBaseOptions;
        public ImageService(IOptions<FireBaseOptions> fireBaseOptions)
        {
            _fireBaseOptions = fireBaseOptions.Value;
        }

        public async Task<string> UploadImage(IFormFile File)
        {
            var apiKey = _fireBaseOptions.ApiKey;
            var bucket = _fireBaseOptions.Bucket;
            var authEmail = _fireBaseOptions.AuthEmail;
            var authPassword = _fireBaseOptions.AuthPassword;

            var fileName = Path.GetFileName(File.FileName);
            using var stream = new FileStream(Path.Combine(Path.GetTempPath(), fileName), FileMode.Create);
            await File.CopyToAsync(stream);
            stream.Position = 0;

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(authEmail, authPassword);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("images")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);
            try
            {
                string link = await task;
                return link;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
