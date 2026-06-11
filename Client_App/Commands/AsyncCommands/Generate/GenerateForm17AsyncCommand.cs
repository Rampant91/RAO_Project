using Avalonia.Controls;
using Client_App.ViewModels.Forms;
using Client_App.Views;
using Models.Collections;
using Models.Forms.Form1;
using Models.Passports;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Client_App.Commands.AsyncCommands.Generate
{
    public class GenerateForm17AsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        Report Report => formVM.Report;

        public override async Task AsyncExecute(object? parameter)
        {

            //Сообщение запрашивает коллекцию паспортов
            var mainWindow = Desktop.MainWindow as MainWindow;
            var passportCollection = await ShowAskReportMessage(mainWindow);
            if (passportCollection == null ||
                passportCollection.Count <= 0) return;

            int index = 0;
            if (Report.Rows17.Count > 0)
                index = Report.Rows17.Max(form => form.NumberInOrder_DB);

            foreach (var passport in passportCollection)
            {
                var characteristic = passport.ContentCharacteristics[0];
                characteristic.UpdateRadionuclidsActivityType();

                var form17 = new Form17();

                index++;
                form17.NumberInOrder_DB = index;

                Report.Rows17.Add(form17);

                form17.PackName_DB = "контейнер";

                form17.PackType_DB = passport.PackageType;


                //if (characteristic.PackageIdNum.Split('/').Length == 3)
                //    form17.PackFactoryNumber_DB = characteristic.PackageIdNum.Split('/')[1];


                form17.PackFactoryNumber_DB = passport.ContainerFactoryNum;
                form17.PackNumber_DB = passport.PackageIdCode;
                form17.FormingDate_DB = passport.ManufactureDate.ToString(new CultureInfo("ru-RU"));
                form17.PassportNumber_DB = passport.PassportNum;
                form17.Volume_DB = passport.PackageVolume.ToString($"e{passport.PackageVolume.ToString().Length - 1}");
                form17.Mass_DB = (passport.PackageMass / 1000).ToString($"e{passport.PackageMass.ToString().Length - 1}");
                form17.CodeRAO_DB = characteristic.CodeRao;
                form17.StatusRAO_DB = passport.StatusRaoCode;
                form17.VolumeOutOfPack_DB = passport.RaoVolume.ToString($"e{passport.RaoVolume.ToString().Length - 1}");
                form17.MassOutOfPack_DB = (passport.RaoMass / 1000).ToString($"e{passport.RaoMass.ToString().Length - 1}");

                form17.TritiumActivity_DB = characteristic.TritiumActivity.ToString($"e{characteristic.TritiumActivity.ToString().Length - 1}");
                form17.BetaGammaActivity_DB = characteristic.BetaGammaActivity.ToString($"e{characteristic.BetaGammaActivity.ToString().Length - 1}");
                form17.AlphaActivity_DB = characteristic.AlphaActivity.ToString($"e{characteristic.AlphaActivity.ToString().Length - 1}");
                form17.TransuraniumActivity_DB = characteristic.TransuraniumActivity.ToString($"e{characteristic.TransuraniumActivity.ToString().Length - 1}");

                if (characteristic.RadionuclidsList.Count > 0)
                {
                    var radionuclidName = characteristic.RadionuclidsList[0].Name;
                    if (Spravochniks.SprRadionuclids.Any(rad => rad.latinName == characteristic.RadionuclidsList[0].Name))
                        radionuclidName = Spravochniks.SprRadionuclids.FirstOrDefault(rad => rad.latinName == characteristic.RadionuclidsList[0].Name).rusName;

                    form17.Radionuclids_DB = radionuclidName;
                    form17.SpecificActivity_DB = characteristic.RadionuclidsList[0].Activity.ToString("e5");
                }
                for (int i = 1; i< characteristic.RadionuclidsList.Count; i++)
                {
                    var radionuclidOnlyForm17 = new Form17();

                    var radionuclidName = characteristic.RadionuclidsList[i].Name;
                    if (Spravochniks.SprRadionuclids.Any(rad => rad.latinName == characteristic.RadionuclidsList[i].Name))
                        radionuclidName = Spravochniks.SprRadionuclids.FirstOrDefault(rad => rad.latinName == characteristic.RadionuclidsList[i].Name).rusName;

                    radionuclidOnlyForm17.Radionuclids_DB = radionuclidName;
                    radionuclidOnlyForm17.SpecificActivity_DB = characteristic.RadionuclidsList[i].Activity.ToString("e5");
                    Report.Rows17.Add(radionuclidOnlyForm17);

                    index++;
                    radionuclidOnlyForm17.NumberInOrder_DB = index;
                }
            }

            //for (int i=0; i<Report.Rows17.Count; i++)
            //{
            //    Report.Rows17[i].NumberInOrder_DB = i + 1;
            //}
            formVM.UpdateFormList();
            formVM.UpdatePageInfo();
        }
        private async Task<ObservableCollection<PackagePassport>?> ShowAskReportMessage(Window owner)
        {
            var dialog = new AskPackagePassportMessage();

            var report = await dialog.ShowDialog<ObservableCollection<PackagePassport>?>(owner);
            return report;
        }
    }
}
