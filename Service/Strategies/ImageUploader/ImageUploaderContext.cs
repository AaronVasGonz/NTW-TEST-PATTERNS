using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service.Strategies.ImageUploader;

public interface IImageUploaderContext
{
    void SetUploadImageStrategy(IUploadImageStrategy uploadImageStrategy);
    Task<string> UploadImageAsync(Stream imageStream, string fileName);
    Task<List<string>> UploadMultipleImageAsync(List<Stream> imagesStreams, string baseFileName);
}

public class ImageUploaderContext : IImageUploaderContext
{
    private IUploadImageStrategy _uploadImageStrategy;
    private readonly IFirebaseStorageService _firebaseStorageService;

    public ImageUploaderContext(IFirebaseStorageService firebaseStorageService, IUploadImageStrategy uploadImageStrategy)
    {
        _firebaseStorageService = firebaseStorageService;
        _uploadImageStrategy = uploadImageStrategy ?? new UploadImageWithFirebase(firebaseStorageService);
    }

    public void SetUploadImageStrategy(IUploadImageStrategy uploadImageStrategy)
    {
        _uploadImageStrategy = uploadImageStrategy;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
    {
        return await _uploadImageStrategy.UploadImageAsync(imageStream, fileName);
    }

    public Task<List<string>> UploadMultipleImageAsync(List<Stream> imagesStreams, string baseFileName)
    {
        return _uploadImageStrategy.UploadMultipleImageAsync(imagesStreams, baseFileName);
    }
}
