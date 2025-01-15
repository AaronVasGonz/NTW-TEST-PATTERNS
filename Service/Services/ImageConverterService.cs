using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IImageConverterService
{
    Task<List<Stream>> ConvertImagesToStreamsAsync(IEnumerable<IFormFile> images);
}

public class ImageConverterService : IImageConverterService
{
    public async Task<List<Stream>> ConvertImagesToStreamsAsync(IEnumerable<IFormFile> images)
    {
        var streams = new List<Stream>();

        foreach (var image in images)
        {
            var stream = image.OpenReadStream();
            streams.Add(stream);
        }
        return streams;
    }
}
