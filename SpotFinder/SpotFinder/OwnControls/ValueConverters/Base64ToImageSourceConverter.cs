using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace SpotFinder.OwnControls.ValueConverters
{
    public class Base64ToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if(value is string)
                return Base64ToImageSource(value as string);
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private ImageSource Base64ToImageSource(string base64Image)
        {
            var imageBytes = System.Convert.FromBase64String(base64Image);
            return ImageSource.FromStream(() => { return new MemoryStream(imageBytes); });
        }
    }
}
