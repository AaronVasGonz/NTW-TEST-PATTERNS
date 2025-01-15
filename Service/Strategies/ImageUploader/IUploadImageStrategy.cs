using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Strategies.ImageUploader;

public interface IUploadImageStrategy
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName);
    Task<List<string>> UploadMultipleImageAsync(List<Stream> imagesStreams, string baseFileName);
}
