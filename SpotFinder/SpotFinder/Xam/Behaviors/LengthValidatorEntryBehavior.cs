using Xamarin.Forms;

namespace SpotFinder.Xam.Behaviors
{
    public class LengthValidatorEntryBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty MinLengthProperty
            = BindableProperty.Create("MinLength", typeof(int), typeof(LengthValidatorEntryBehavior), 4);

        public static readonly BindableProperty MaxLengthProperty
            = BindableProperty.Create("MaxLength", typeof(int), typeof(LengthValidatorEntryBehavior), 255);

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public int MinLength
        {
            get => (int)GetValue(MinLengthProperty);
            set => SetValue(MinLengthProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += TextChangedAcitivty;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= TextChangedAcitivty;
        }

        private void TextChangedAcitivty(object sender, TextChangedEventArgs ea)
        {
            var entry = sender as Entry;

            if (entry == null)
                return;

            var newTextLength = ea.NewTextValue.Length;

            SetTextColor(entry, newTextLength);
            SetTextLength(entry, newTextLength);
        }

        private void SetTextColor(Entry entry, int newTextLength)
        {
            if (newTextLength < MinLength || newTextLength > MaxLength)
                entry.TextColor = Color.Red;
            else
                entry.TextColor = Color.Black;
        }

        private void SetTextLength(Entry entry, int newTextLength)
        {
            if (newTextLength > MaxLength)
                entry.Text = entry.Text.Substring(0, MaxLength);
        }
    }
}
