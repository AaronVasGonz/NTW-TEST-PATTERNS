using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arquitecture.Initializers;
namespace Service.Services;

public interface IFirebaseStorageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName);
    Task<List<string>> UploadMultipleImageAsync(List<Stream> imagesStreams, string baseFileName);
}

public class FirebaseStorageService : IFirebaseStorageService
{
    private readonly string _fireBaseBucket;

    public FirebaseStorageService(string fireBaseBucket)
    {
        _fireBaseBucket = fireBaseBucket;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
    {
        try
        {
            var firebaseStorageInitializer = new FirebaseInitializer(_fireBaseBucket);
            var firebaseStorage = firebaseStorageInitializer.GetFirebaseStorage();


            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

            var imageUrl = await firebaseStorage
                .Child("images")
                .Child(uniqueFileName)
                .PutAsync(imageStream);

            return imageUrl;
        }
        catch (Exception ex)
        {
            throw new Exception("Error while uploading the image", ex);
        }
    }

    public async Task<List<string>> UploadMultipleImageAsync(List<Stream> imagesStreams, string baseFileName)
    {
        try
        {
            var tasks = imagesStreams.Select(async (imageStream) =>
            {
                return await UploadImageAsync(imageStream, baseFileName);
            });

            var imageUrls = await Task.WhenAll(tasks);
            return imageUrls.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error while uploading multiple images", ex);
        }
    }
}

