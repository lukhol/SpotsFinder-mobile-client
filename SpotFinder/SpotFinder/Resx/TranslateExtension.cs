using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpotFinder.Resx
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "SpotFinder.Resx.AppResources";
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;

            var resourceManager = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

            var text = resourceManager.GetString(Text, CultureInfo.CurrentCulture);

            if (string.IsNullOrEmpty(text))
                return "String not found in AppResources!";

            return text;

        }
    }
}
