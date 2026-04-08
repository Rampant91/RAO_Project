using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using Models.Passports;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Behaviors
{
    public class TextBoxCalculateActivitiesOnLostFocus : Behavior<Control>
    {
        public static readonly AttachedProperty<Radionuclid?> RadionuclidProperty =
        AvaloniaProperty.RegisterAttached<TextBoxCalculateActivitiesOnLostFocus, Control, Radionuclid?>("Radionuclid");

        public Radionuclid? Radionuclid
        {
            get => GetValue(RadionuclidProperty);
            set => SetValue(RadionuclidProperty, value);
        }

        public string PreviousName
        {
            get;set;
        }
        public double PreviousActivity
        {
            get; set;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                AssociatedObject.GotFocus += AssociatedObject_GotFocus;
                AssociatedObject.LostFocus += TextBox_LostFocus;
            }
        }

        private void AssociatedObject_GotFocus(object? sender, Avalonia.Input.GotFocusEventArgs e)
        {
            PreviousName = Radionuclid.Name;
            PreviousActivity = Radionuclid.Activity;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {

                AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
                AssociatedObject.LostFocus -= TextBox_LostFocus;
            }

            base.OnDetaching();
        }

        private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (Radionuclid == null) return;

            if (PreviousName == Radionuclid.Name
                && PreviousActivity == Radionuclid.Activity) return;

            if (PreviousName != Radionuclid.Name
                && PreviousActivity == Radionuclid.Activity
                && Radionuclid.Activity == 0) return;


            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Radionuclid.GetGroupCode();
                Radionuclid.GetIsLongLivingActivity();
                Radionuclid.Characteristic.UpdateAlphaActivity();
                Radionuclid.Characteristic.UpdateBetaGammaActivity();
                Radionuclid.Characteristic.UpdateLongLivingActivity();
                Radionuclid.Characteristic.UpdateTransuraniumActivity();
                Radionuclid.Characteristic.UpdateTritiumActivity();
            });

        }

    }
}
