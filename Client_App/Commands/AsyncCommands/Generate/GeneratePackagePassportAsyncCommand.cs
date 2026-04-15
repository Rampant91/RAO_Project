using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.ViewModels.Forms;
using Client_App.Views;
using DynamicData;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Passports;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Models.Collections.Report;

namespace Client_App.Commands.AsyncCommands.Generate
{
    public class GeneratePackagePassportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        Report Report => formVM.Report;
        List<Form17> Rows17
        {
            get
            {
                return Report.Rows17.OrderBy(form17 => form17.NumberInOrder_DB).ToList();
            }
        }
        Window owner => (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                    .FirstOrDefault(w => w.Name == "1.7");

        //На вход поступает строки формы 1.7
        public override async Task AsyncExecute(object? parameter)
        {
            if (parameter is not IEnumerable<Form> forms17Collection
                || forms17Collection.Count()<=0
                || forms17Collection.Any(f => f is not Form17)) return;

            var codeOperationRegex = new Regex("^\\d{2}$");

            var selectedForm17List = forms17Collection.Cast<Form17>().ToList();


            var first = selectedForm17List.First();
            var firstIndex = Rows17.IndexOf(first);

            while (firstIndex > 0
                && !codeOperationRegex.IsMatch(first.OperationCode_DB))
            {
                selectedForm17List.Insert(0, Rows17[firstIndex - 1]);
                first = Rows17[firstIndex - 1];
                firstIndex--;
            }


            var last = selectedForm17List.Last();
            var lastIndex = Rows17.IndexOf(last);
            while (lastIndex < Rows17.Count-1
                && !codeOperationRegex.IsMatch(Rows17[lastIndex + 1].OperationCode_DB))
            {
                selectedForm17List.Add(Rows17[lastIndex + 1]); 
                last = Rows17[lastIndex + 1];
                lastIndex++;
            }


            //ссылка на нужный список радионуклидов
            ObservableCollection<Radionuclid>? radionuclidList = null;

            // Список строк, для которых не будет создан паспорт, т.к. их код операции не предполагает создания паспорта
            List<int> rowNumbers = new();

            foreach (var form in selectedForm17List)
            {
                if (form is not Form17 form17) continue;

                //В формах 1.7 Одна зпись может состоять из нескольких строк
                //после одной полностью заполненой строки могут идти несколько других строк,
                //в которых заполнено только информация о радионуклиде

                //Проверка строки новая ли это запись или продолжение старой 
                //Если новая то создаем новый паспорт и добавляем радионуклид
                //Если продолжение старой то только добавляем радионуклид 
                if (codeOperationRegex.IsMatch(form17.OperationCode_DB)) 
                {
                    var passportMatch = FindPassportMatch(form17);

                    if (form17.OperationCode_DB != "01"
                        && form17.OperationCode_DB != "11"
                        && form17.OperationCode_DB != "12"
                        && form17.OperationCode_DB != "14"
                        && form17.OperationCode_DB != "16"
                        && form17.OperationCode_DB != "18"
                        && form17.OperationCode_DB != "55")
                    {
                        rowNumbers.Add(form17.NumberInOrder_DB);
                        radionuclidList = null;
                        continue;
                    }


                    var passport = new PackagePassport();
                    passport.ContentCharacteristics.Add(new CharacteristicPrimaryPackage(passport));
                    var characteristic = passport.ContentCharacteristics[0];


                    if (passportMatch == null && form17.OperationCode_DB == "18")
                        passport.CorrectionNumber = 1;
                    else if (passportMatch != null && form17.OperationCode_DB == "18")
                        passport.CorrectionNumber = (byte)(passportMatch.CorrectionNumber + 1);
                    else if (passportMatch == null && form17.OperationCode_DB != "18")
                        passport.CorrectionNumber = 0;
                    else if (passportMatch != null && form17.OperationCode_DB != "18")
                    {
                        radionuclidList = null;
                        continue;
                    }

                    // записываем ссылку на новый список радионуклидов
                    radionuclidList = passport.ContentCharacteristics[0].RadionuclidsList;

                    passport.PackageType = form17.PackType_DB;

                    //characteristic.PackageIdNum = form17.PackNumber_DB;
                    passport.ManufactureDate = DateOnly.TryParse(form17.FormingDate_DB, out var date) ? date : DateOnly.MinValue;
                    passport.PassportNum = form17.PassportNumber_DB;
                    passport.PackageVolume = double.TryParse(form17.Volume_DB, out var value) ? value : 0;
                    passport.PackageMass = double.TryParse(form17.Mass_DB, out value) ? value * 1000 : 0;
                    passport.StatusRaoCode = form17.CodeRAO_DB;
                    characteristic.CodeRao = form17.StatusRAO_DB;
                    passport.RaoVolume = double.TryParse(form17.VolumeOutOfPack_DB, out value) ? value : 0;
                    passport.RaoMass = double.TryParse(form17.MassOutOfPack_DB, out value) ? value : 0;
                    //characteristic.TritiumActivity = double.TryParse(form17.TritiumActivity_DB, out value) ? value : 0;
                    //characteristic.BetaGammaActivity = double.TryParse(form17.BetaGammaActivity_DB, out value) ? value : 0;
                    //characteristic.AlphaActivity = double.TryParse(form17.AlphaActivity_DB, out value) ? value : 0;
                    //characteristic.TransuraniumActivity = double.TryParse(form17.TransuraniumActivity_DB, out value) ? value : 0;

                    StaticConfiguration.DBModel.package_passport.Add(passport);
                }

                //Добавляем радионуклид в текущий список
                if (radionuclidList is not null
                    && !string.IsNullOrWhiteSpace(form17.Radionuclids_DB))
                {
                    var radName = form17.Radionuclids_DB;

                    //Ищем в справочнике латинское наименование радионуклида
                    if (Spravochniks.SprRadionuclids.Any(rad => rad.rusName == form17.Radionuclids_DB))
                        radName = Spravochniks.SprRadionuclids.FirstOrDefault(rad => rad.rusName == form17.Radionuclids_DB).latinName;


                    radionuclidList.Add(new Radionuclid()
                    {
                        Name = radName,
                        Activity = double.TryParse(form17.SpecificActivity_DB, out var value) ? value : 0
                    });
                }

            }
            if (rowNumbers.Count > 0)
            {
                var msg = $"Не удалось создать паспорт на основе данных строк:\n" +
                    $"№";
                for(int i =0; i < rowNumbers.Count; i++)
                {
                    if (i != rowNumbers.Count - 1)
                        msg += $" {rowNumbers[i]}, ";
                    else
                        msg += $" {rowNumbers[i]}\n";
                }
                msg += "Так как их код операции не равен 01, 11, 12, 14, 16, 18, 55";

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams()
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок" },
                    ],
                    CanResize = true,
                    ContentTitle = "Формирование паспорта на упаковку",
                    ContentMessage = msg,
                    MinWidth = 300,
                    MinHeight = 125,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(owner));
            }

            #region DBModel.SaveChanges
            try
            {
                StaticConfiguration.DBModel.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок" },
                    ],
                    CanResize = true,
                    ContentTitle = "Формирование паспорта на упаковку",
                    ContentMessage = "Во время формирования произошла ошибка\n" +
                    "Описание:\n" +
                    $"{ex.Message}",
                    MinWidth = 300,
                    MinHeight = 125,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(owner));

                return;
            }
            #endregion

            #region CommandCompletedMessage
            Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ок" },
                ],
                CanResize = true,
                ContentTitle = "Формирование паспорта на упаковку",
                ContentMessage = "Формирование завершено\n",
                MinWidth = 300,
                MinHeight = 125,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(owner));
            #endregion
        }
        private PackagePassport? FindPassportMatch(Form17 form17)
        {
            return StaticConfiguration.DBModel.package_passport.Where(passport =>
            passport.PassportNum == form17.PassportNumber_DB
            && passport.PackageType == form17.PackType_DB)
                .AsEnumerable()
                .MaxBy(passport => passport.CorrectionNumber);
        }
    }
}
