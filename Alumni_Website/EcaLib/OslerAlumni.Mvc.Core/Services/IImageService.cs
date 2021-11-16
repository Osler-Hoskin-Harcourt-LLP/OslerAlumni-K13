using System.IO;
using ECA.Core.Services;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IImageService
        : IService
    {
        byte[] ImageCrop(byte [] file, int x, int y, int w, int h);

    }
}
