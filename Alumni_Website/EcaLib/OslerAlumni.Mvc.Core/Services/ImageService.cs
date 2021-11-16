using System.IO;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using System.Drawing;
using OslerAlumni.Mvc.Core.Extensions;

namespace OslerAlumni.Mvc.Core.Services
{
    public class ImageService : ServiceBase, IImageService
    {
        private readonly IEventLogRepository _eventLogRepository;
        private readonly ContextConfig _context;

        public ImageService(IEventLogRepository eventLogRepository, ContextConfig context)
        {
            _eventLogRepository = eventLogRepository;
            _context = context;
        }

        public byte[] ImageCrop(byte[] file, int x, int y, int w, int h)
        {
            using (MemoryStream ms = new MemoryStream(file))
            {
                Bitmap img = Image.FromStream(ms) as Bitmap;

                var croppped = img.CropAtRect(new Rectangle(x, y, w, h));

                return ImageToByte(croppped);
            }
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }



    }
}
