using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.Core
{
    static class Utils
    {
        public static ImageSource Base64ImageToImageSource(string base64Image)
        {
            var imageBytes = Convert.FromBase64String(base64Image);
            return ImageSource.FromStream(() => { return new MemoryStream(imageBytes); });
        }
    }
}
