using Avalonia;
using Avalonia.Controls;
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
            //Ищем в справочнике информацию о радионуклиде
            var sprRad = Spravochniks.SprRadionuclids.FirstOrDefault(rad => rad.latinName == Radionuclid.Name);


            if (PreviousActivity != Radionuclid.Activity
                || Radionuclid.IsLongLivingActivity != sprRad.isLongLiving)
            {
                Radionuclid.IsLongLivingActivity = sprRad.isLongLiving;
                Radionuclid.Characteristic.UpdateLongLivingActivity();
            }

            if (PreviousActivity != Radionuclid.Activity
                || Radionuclid.ActivityType != sprRad.groupCode)
            {
                Radionuclid.ActivityType = sprRad.groupCode;
                //И обновляем информацию о суммарных активностях
                switch (sprRad.groupCode)
                {
                    case GroupCode.Alpha:
                        Radionuclid.Characteristic.UpdateAlphaActivity();
                        break;
                    case GroupCode.BetaGamma:
                        Radionuclid.Characteristic.UpdateBetaGammaActivity();
                        break;
                    case GroupCode.Transuranic:
                        Radionuclid.Characteristic.UpdateTransuraniumActivity();
                        break;
                    case GroupCode.Tritium:
                        Radionuclid.Characteristic.UpdateTritiumActivity();
                        break;


                }
            }

        }

    }
}
