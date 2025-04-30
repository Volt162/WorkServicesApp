using System;
using System.Threading.Tasks;

namespace MopsterTeams.Services.ResizeImage
{
    public interface IResizeImageService
    {
        Task<byte[]> ResizeImageAsync(byte[] imageData, float width, float height);
        Task<byte[]> FixOrientationAsync(byte[] imageData);
    }
}
