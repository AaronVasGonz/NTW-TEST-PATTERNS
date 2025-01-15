using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Strategies.ImageUploader;

public class UploadImageWithFirebase: IUploadImageStrategy
{
    private readonly IFirebaseStorageService _firebaseStorageService;

    public UploadImageWithFirebase(IFirebaseStorageService firebaseStorageService)
    {
        _firebaseStorageService = firebaseStorageService;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
    {
       return await _firebaseStorageService.UploadImageAsync(imageStream, fileName);
    }

    public Task<List<string>> UploadMultipleImageAsync(List<Stream> imagesStreams, string baseFileName)
    {
        return _firebaseStorageService.UploadMultipleImageAsync(imagesStreams, baseFileName);
    }
}
