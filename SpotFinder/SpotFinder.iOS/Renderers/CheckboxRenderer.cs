using System;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsCheckbox = SpotFinder.OwnControls.Checkbox;
using FormsCheckboxCheckedChangedEventArgs = SpotFinder.OwnControls.CheckedChangedEventArgs;
using SpotFinder.iOS.Renderers;
using SpotFinder.iOS.Native.Controls;

[assembly: ExportRenderer(typeof(FormsCheckbox), typeof(CheckboxRenderer))]
namespace SpotFinder.iOS.Renderers
{
        public class CheckboxRenderer : ViewRenderer<FormsCheckbox, Checkbox>
        {
            /// <summary>
            ///     Used for registration with dependency service
            /// </summary>
            public new static void Init()
            {
                var temp = DateTime.Now;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    Control.ValueChanged -= ControlValueChanged;

                base.Dispose(disposing);
            }

            protected override void OnElementChanged(ElementChangedEventArgs<FormsCheckbox> e)
            {
                base.OnElementChanged(e);
                if (e.OldElement != null)
                    e.OldElement.CheckedChanged -= OnElementCheckedChanged;

                if (e.NewElement != null)
                {
                    if (Control == null)
                    {
                        // Instantiate the native control and assign it to the Control property
                        var width = 140d;
                        if (Element.WidthRequest > 0)
                            width = Element.WidthRequest;
                        var checkBox = new Checkbox(new CGRect(0, 0, width, width));
                        if (e.NewElement.CheckboxBackgroundColor != Color.Default)
                            checkBox.CheckboxBackgroundColor = e.NewElement.CheckboxBackgroundColor.ToUIColor();

                        if (e.NewElement.TickColor != Color.Default)
                            checkBox.CheckColor = e.NewElement.TickColor.ToUIColor();

                        SetNativeControl(checkBox);
                        Control.ValueChanged += ControlValueChanged;
                    }
                    Control.Checked = Element.Checked;
                    e.NewElement.CheckedChanged += OnElementCheckedChanged;
                }
            }

            private void OnElementCheckedChanged(object sender, FormsCheckboxCheckedChangedEventArgs e)
            {
                Control.Checked = Element.Checked;
            }

            private void ControlValueChanged(object sender, EventArgs e)
            {
                ((IElementController)Element).SetValueFromRenderer(FormsCheckbox.CheckedProperty, Control.Checked);
            }
        }
}