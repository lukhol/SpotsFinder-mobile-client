using System;
using System.Threading.Tasks;
using SpotFinder.Core.Enums;
using SpotFinder.OwnControls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace SpotFinder.Services
{
    public class PhotoProvider : IPhotoProvider
    {
        public async Task<MyImage> GetPhotoAsync(GetPhotoType photoType)
        {
            await CrossMedia.Current.Initialize();

            MediaFile file = null;

            if (photoType == GetPhotoType.Camera)
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return null;
                }

                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "SpotsFindePictures",
                    Name = "SpotsFinder.jpg",
                    PhotoSize = PhotoSize.Small,
                    SaveToAlbum = true
                });
            }
            else if (photoType == GetPhotoType.Gallery)
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    return null;
                }

                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    CompressionQuality = 0
                });
            }

            if (file == null)
                return null;

            var image = new MyImage
            {
                Margin = new Thickness(5, 0, 5, 5),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start
            };

            //Ten stream może być tworzyny w zły sposób. Nie wiem czy jest potrzeba pobierać go aż 2 razy.
            var stream = file.GetStream();
            image.Source = ImageSource.FromStream(() =>
            {
                return stream;
            });

            var streamTwo = file.GetStream();
            file.Dispose();
            var bytes = new byte[streamTwo.Length];
            await streamTwo.ReadAsync(bytes, 0, (int)streamTwo.Length);

            image.Base64Representation = Convert.ToBase64String(bytes);

            return image;
        }
    }
}
