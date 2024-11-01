using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Models.Collections;
using Models.Forms.Form1;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;

namespace Client_App.Commands.AsyncCommands.PassportFill;

public abstract class PassportFillBaseCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    protected Report Storage => changeOrCreateViewModel.Storage;
    protected List<Form17> Collection;
    protected string[]? ApplicableOperationCodes;
    protected string? MsgTitle;
    protected string? MsgQuestionOverride;
    protected string? ErrOpsNotFound;
    protected async Task PassportFillAction()
    {
        if (Collection.Count == 0) return;
        Dictionary<string, List<int>> packageData = [];
        List<Dictionary<string, string>> R;

        #region ReadDictionaries

#if DEBUG
        R = R_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx"));
#else
            R = R_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx"));
#endif
#if DEBUG
        var templatePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "PassportTemplates", "Template_1.7.xlsx");
#else
            var templatePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "PassportTemplates", "Template_1.7.xlsx");
#endif

        #endregion

        for (var i = 0; i < Collection.Count; i++)
        {
            try
            {
                var lineIndex = Storage.Rows17.IndexOf(Collection.ToList()[i]);
                if (lineIndex < 0) continue;
                var lineIndexParent = lineIndex;
                while (lineIndexParent >= 0)
                {
                    if (!string.IsNullOrWhiteSpace(Storage.Rows17[lineIndexParent].PackType_DB) && Storage.Rows17[lineIndexParent].PackType_DB.Trim() != "-"
                        && !string.IsNullOrWhiteSpace(Storage.Rows17[lineIndexParent].PackNumber_DB) && Storage.Rows17[lineIndexParent].PackNumber_DB.Trim() != "-")
                    {
                        break;
                    }
                    lineIndexParent--;
                }
                if (lineIndexParent < 0) continue;
                if (!ApplicableOperationCodes.Contains(Storage.Rows17[lineIndexParent].OperationCode_DB)) continue;
                var packIdentity = $"{Storage.Rows17[lineIndexParent].PackType_DB}_{Storage.Rows17[lineIndexParent].PackFactoryNumber_DB}";
                if (!packageData.ContainsKey(packIdentity)) packageData.Add(packIdentity, []);
                if (packageData[packIdentity].Contains(lineIndex)) continue;
                lineIndex = lineIndexParent;
                while (lineIndex < Storage.Rows17.Count)
                {
                    if (!string.IsNullOrWhiteSpace(Storage.Rows17[lineIndex].PackType_DB) 
                        && Storage.Rows17[lineIndex].PackFactoryNumber_DB.Trim() != "-"
                        && !string.IsNullOrWhiteSpace(Storage.Rows17[lineIndex].PackNumber_DB) 
                        && Storage.Rows17[lineIndex].PackFactoryNumber_DB.Trim() != "-")
                    {
                        if (lineIndex != lineIndexParent) break;
                    }
                    packageData[packIdentity].Add(lineIndex);
                    lineIndex++;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }
        foreach (var key in packageData.Keys
                     .ToList()
                     .Where(key => packageData[key].Count == 0))
        {
            packageData.Remove(key);
        }
        if (packageData.Count == 0)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MsBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = MsgTitle,
                    ContentHeader = "Ошибка",
                    ContentMessage = ErrOpsNotFound,
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowWindowDialogAsync(Desktop.MainWindow));
            return;
        }

        #region Message

        var singlePack = packageData.Count == 1;
        var suffix1 = singlePack ? "" : "а";
        var suffix2 = singlePack ? "ей" : "их";
        var suffix3 = singlePack ? "ки" : "ок";
        var suffix4 = singlePack ? "" : "ы";
        var suffix5 = singlePack ? "а" : "ов";
        var answer = await Dispatcher.UIThread.InvokeAsync(() => MsBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да", IsDefault = true },
                    new ButtonDefinition { Name = "Нет", IsCancel = true }
                ],
                SizeToContent = SizeToContent.WidthAndHeight,
                ContentTitle = MsgTitle,
                ContentHeader = "Уведомление",
                ContentMessage = MsgQuestionOverride ?? $"Сформировать паспорт{suffix1} для текущ{suffix2} упаков{suffix3} РАО?     ",
                MinWidth = 550,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowWindowDialogAsync(Desktop.MainWindow));
        if (answer is not "Да") return;

        #endregion

        OpenFolderDialog dial = new();
        var folderPath = await dial.ShowAsync(Desktop.MainWindow);
        if (folderPath is null) return;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        foreach (var package in packageData)
        {
            try
            {
                var entry = Storage.Rows17[package.Value[0]];
                List<(string, string)> nuclids = [];
                var longNuclidActivitySum = 0.0;
                var tNuclidActivityExists = TryParseDoubleExtended(entry.TritiumActivity_DB ?? string.Empty, out var tNuclidActivity);
                var aNuclidActivityExists = TryParseDoubleExtended(entry.AlphaActivity_DB ?? string.Empty, out var aNuclidActivity);
                var bNuclidActivityExists = TryParseDoubleExtended(entry.BetaGammaActivity_DB ?? string.Empty, out var bNuclidActivity);
                var uNuclidActivityExists = TryParseDoubleExtended(entry.TransuraniumActivity_DB ?? string.Empty, out var uNuclidActivity);
                nuclids.AddRange(package.Value
                    .Select(rowId => (Storage.Rows17[rowId].Radionuclids_DB
                        .Trim()
                        .ToLower(), Storage.Rows17[rowId].SpecificActivity_DB)));

                #region Setup file

                var fileName = $"{Storage.Reports.Master.RegNoRep.Value}_{package.Key}.xlsx".Replace('/', '_');
                var fullPath = Path.Combine(folderPath, fileName);
                if (string.IsNullOrEmpty(fullPath)) return;
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath!);
                    }
                    catch
                    {
                        #region MessageFailedToSaveFile

                        await Dispatcher.UIThread.InvokeAsync(() => MsBox.Avalonia.MessageBoxManager
                            .GetMessageBoxStandard(new MessageBoxStandardParams
                            {
                                ButtonDefinitions = MsBox.Avalonia.Enums.ButtonEnum.Ok,
                                ContentTitle = "Выгрузка в Excel",
                                ContentHeader = "Ошибка",
                                ContentMessage =
                                    $"Не удалось сохранить файл по пути: {fullPath}" +
                                    $"{Environment.NewLine}Файл с таким именем уже существует в этом расположении" +
                                    $"{Environment.NewLine}и используется другим процессом.",
                                MinWidth = 400,
                                MinHeight = 150,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowWindowDialogAsync(Desktop.MainWindow));
                        continue;

                        #endregion
                    }
                }

                #endregion

                using ExcelPackage excelPackage = new(new FileInfo(fullPath), new FileInfo(templatePath));
                var rowShift = 0;
                excelPackage.Workbook.Worksheets[0].Name = package.Key;
                excelPackage.Workbook.Worksheets[0].Cells["K3"].Value = entry.OperationDate_DB;
                excelPackage.Workbook.Worksheets[0].Cells["H5"].Value = $"{entry.PackType_DB}, № {entry.PackFactoryNumber_DB}";
                excelPackage.Workbook.Worksheets[0].Cells["A32"].Value = entry.PackType_DB;
                excelPackage.Workbook.Worksheets[0].Cells["A31"].Value = $"№ {entry.PackFactoryNumber_DB}";
                excelPackage.Workbook.Worksheets[0].Cells["G10"].Value = entry.PackNumber_DB;
                excelPackage.Workbook.Worksheets[0].Cells["A30"].Value = entry.PackNumber_DB;
                excelPackage.Workbook.Worksheets[0].Cells["N14"].Value = entry.FormingDate_DB;
                excelPackage.Workbook.Worksheets[0].Cells["G23"].Value = entry.FormingDate_DB;
                excelPackage.Workbook.Worksheets[0].Cells["Q25"].Value = entry.FormingDate_DB;
                excelPackage.Workbook.Worksheets[0].Cells["G3"].Value = entry.PassportNumber_DB;
                excelPackage.Workbook.Worksheets[0].Cells["L24"].Value = $"({(TryParseDoubleExtended(entry.Volume_DB, out var volume) 
                    ? volume.ToString("0.00") 
                    : entry.Volume_DB)})";
                excelPackage.Workbook.Worksheets[0].Cells["L23"].Value = TryParseDoubleExtended(entry.Mass_DB ?? string.Empty, out var mass)
                    ? (mass * 1000).ToString("0.00") 
                    : "-";
                for (var i = 0; i < nuclids.Count; i++)
                {
                    var phEntry = R.FirstOrDefault(phEntry => phEntry["name"] == nuclids[i].Item1);
                    if (phEntry is null) continue;
                    if (i >= 8)
                    {
                        excelPackage.Workbook.Worksheets[0].InsertRow(30 + i, 1, 30 + i);
                        rowShift++;
                    }
                    excelPackage.Workbook.Worksheets[0].Cells[$"L{30 + i}"].Value = nuclids[i].Item1;
                    excelPackage.Workbook.Worksheets[0].Cells[$"M{30 + i}"].Value = nuclids[i].Item2;
                    var unit = phEntry["unit"];
                    var value = phEntry["value"].Replace('.', ',');
                    if (!TryParseDoubleExtended(value, out var halfLife)) continue;
                    switch (unit)
                    {
                        case "лет":
                            halfLife *= 365.25;
                            break;
                        case "сут":
                            break;
                        case "час":
                            halfLife /= 24.0;
                            break;
                        case "мин":
                            halfLife /= 1440.0;
                            break;
                        default:
                            continue;
                    }
                    if (halfLife > 31*365.25)
                    {
                        if (TryParseDoubleExtended(nuclids[i].Item2, out var nuclidActivity))
                        {
                            longNuclidActivitySum += nuclidActivity;
                        }
                    }
                }
                excelPackage.Workbook.Worksheets[0].Cells[$"O{39 + rowShift}"].Value = entry.FormingDate_DB;
                excelPackage.Workbook.Worksheets[0].Cells["G13"].Value = $"{Storage.Reports.Master.ShortJurLicoRep.Value} ОКПО {entry.ProviderOrRecieverOKPO_DB}";
                excelPackage.Workbook.Worksheets[0].Cells["C31"].Value = entry.CodeRAO_DB;
                excelPackage.Workbook.Worksheets[0].Cells["C7"].Value = entry.StatusRAO_DB;
                excelPackage.Workbook.Worksheets[0].Cells["M24"].Value = $"({(TryParseDoubleExtended(entry.VolumeOutOfPack_DB, out var volumeOutOfPack) 
                    ? volumeOutOfPack.ToString("0.00") 
                    : entry.VolumeOutOfPack_DB)})";
                excelPackage.Workbook.Worksheets[0].Cells["M23"].Value = TryParseDoubleExtended(entry.MassOutOfPack_DB ?? string.Empty, out var massB)
                    ? (massB * 1000).ToString("0.00") 
                    : "-";
                if (TryParseDoubleExtended(entry.MassOutOfPack_DB ?? string.Empty, out var massNuclids) && massNuclids != 0)
                {
                    excelPackage.Workbook.Worksheets[0].Cells["P34"].Value = tNuclidActivityExists
                        ? ToExpString(tNuclidActivity / massNuclids / 1000000) 
                        : "0";
                    excelPackage.Workbook.Worksheets[0].Cells["P33"].Value = bNuclidActivityExists
                        ? ToExpString(bNuclidActivity / massNuclids / 1000000) 
                        : "0";
                    excelPackage.Workbook.Worksheets[0].Cells["P32"].Value = aNuclidActivityExists
                        ? ToExpString(aNuclidActivity / massNuclids / 1000000) 
                        : "0";
                    excelPackage.Workbook.Worksheets[0].Cells["P31"].Value = uNuclidActivityExists
                        ? ToExpString(uNuclidActivity / massNuclids / 1000000) 
                        : "0";
                }
                var codeRao2 = entry.CodeRAO_DB.Length >= 2 
                    ? entry.CodeRAO_DB.Substring(1, 1) 
                    : null;
                var codeRao7 = entry.CodeRAO_DB.Length >= 7 
                    ? entry.CodeRAO_DB.Substring(6, 1) 
                    : null;
                var codeRao8 = entry.CodeRAO_DB.Length >= 8 
                    ? entry.CodeRAO_DB.Substring(7, 1) 
                    : null;
                var codeRao910 = entry.CodeRAO_DB.Length >= 10 
                    ? entry.CodeRAO_DB.Substring(8, 2) 
                    : null;
                var codeRao11 = entry.CodeRAO_DB.Length >= 11 
                    ? entry.CodeRAO_DB.Substring(10, 1) 
                    : null;

                Dictionary<string, string> codeRao2Dict = new()
                {
                    { "0", "очень низкоактивные" },
                    { "1", "низкоактивные" },
                    { "2", "среднеактивные" },
                    { "3", "высокоактивные" },
                    { "4", "отработавшие закрытые источники ионизирующего излучения" },
                    { "9", "прочие" },
                };

                Dictionary<string, string> codeRao8Dict = new()
                {
                    { "0", "класс не установлен" },
                    { "1", "1 класс" },
                    { "2", "2 класс" },
                    { "3", "3 класс" },
                    { "4", "4 класс" },
                    { "5", "5 класс" },
                    { "6", "6 класс" },
                    { "7", "особые РАО" },
                    { "9", "решение отложено" },
                };

                Dictionary<string, string> codeRao910Dict = new()
                {
                    { "01", "газы, аэрозоли" },
                    { "11", "воды загрязненные подземные, в том числе грунтовые и дренажных систем" },
                    { "12", "воды охлаждения реакторного производства, контурные воды, воды цистерн биологической защиты, воды бассейнов выдержки ОЯТ" },
                    { "13", "растворы хвостовые" },
                    { "14", "конденсаты после упаривания, газоочистки, конденсаты технологические прочие" },
                    { "15", "растворы регенерационные (сливы), растворы промывки экстрагентов, сорбентов" },
                    { "16", "воды лабораторий, трапные, обмывочные воды, растворы после дезактивации, включая воды санпропускников и спецпрачечных, сточные воды прочие" },
                    { "17", "воды (декантаты) бассейнов-хранилищ, а также емкостей хранения пульп, фильтратов" },
                    { "18", "воды промысловые" },
                    { "19", "жидкости неорганические прочие" },
                    { "21", "экстрагенты" },
                    { "22", "масла" },
                    { "23", "эмульсии смазочно-охлаждающих жидкостей (СОЖ)" },
                    { "24", "моющие средства (детергенты) за исключением спиртов" },
                    { "25", "спирты" },
                    { "26", "сцинцилляторы органические жидкостные" },
                    { "29", "жидкости органические прочие" },
                    { "31", "хвосты гидрометаллургического, химико-металлургического, разделительного и сублиматного производств, пульпы, образующиеся после нейтрализации" },
                    { "32", "пульпы отработавших ионообменных смол и прочих сорбентов органических" },
                    { "33", "шламы и пульпы отработавших сорбентов неорганических, фильтрующих материалов" },
                    { "34", "солевой остаток с солесодержанием до 250 г/л, кубовый остаток" },
                    { "35", "кубовый остаток после упаривания с солесодержанием более 250 г/л, концентрат солевой" },
                    { "36", "взвеси, содержащие продукты коррозии" },
                    { "37", "илы, иловые осадки водоемов-накопителей, осадки, кеки" },
                    { "38", "шламы после очистки трапных вод" },
                    { "39", "прочие пульпы, шламы технологические" },
                    { "41", "сталь нержавеющая: крупногабаритные изделия и оборудование" },
                    { "42", "сталь нержавеющая, кроме крупногабаритных изделий" },
                    { "43", "металлы черные, кроме нержавеющей стали: крупногабаритные изделия и оборудование" },
                    { "44", "металлы черные, кроме нержавеющей стали и крупногабаритных изделий" },
                    { "45", "лом черных металлов" },
                    { "46", "металлы щелочные, в том числе отработавший теплоноситель" },
                    { "51", "алюминий и его сплавы, изделия и оборудование из алюминия и его сплавов" },
                    { "52", "уран металлический обедненный и изделия из него" },
                    { "53", "медь и сплавы меди" },
                    { "54", "никель и его сплавы, кроме медно-никелевых сплавов, отнесенных к коду 53" },
                    { "55", "свинец и его сплавы" },
                    { "56", "цирконий и его сплавы" },
                    { "57", "лом цветных металлов прочих, не перечисленных в пунктах 51-57" },
                    { "58", "лом цветных металлов несортированный" },
                    { "59", "металлический лом прочий" },
                    { "61", "сорбенты и фильтроматериалы" },
                    { "62", "песок" },
                    { "63", "теплоизоляционные материалы неорганические" },
                    { "64", "изделия из стекла и керамики, лабораторная посуда" },
                    { "65", "отходы плавильного производства (включая шлаки, футеровку.)" },
                    { "66", "зола" },
                    { "67", "плав солевой" },
                    { "68", "рудные материалы" },
                    { "69", "твердые РАО неорганические прочие" },
                    { "71", "смолы отработанные ионообменные" },
                    { "72", "уголь активированный, сульфоуголь" },
                    { "73", "полимеры" },
                    { "74", "сажа" },
                    { "75", "спецодежда и другие средства индивидуальной защиты, обувь, обтирочные материалы, ветошь, вата, фильтроэлементы (фильтровальная ткань) фильтров вентиляции" },
                    { "76", "графит" },
                    { "77", "древесина, бумага, картон" },
                    { "78", "твердые РАО биологические" },
                    { "79", "твердые РАО органические прочие" },
                    { "81", "ОЗИИИ без защитного контейнера" },
                    { "82", "ОЗИИИ в защитном контейнере" },
                    { "83", "изделия с радиоактивной светомассой постоянного действия (ампулированные источники для ночного обозначения габаритов, аварийные и автономные осветители, указатели, источники света для шкал и циферблатов приборов, подстветки оружейных прицелов и подобное)" },
                    { "84", "открытые радионуклидные источники в виде отдельных изделий" },
                    { "85", "радиоизотопные термоэлектрические генераторы (РИТЭГ)" },
                    { "86", "дымо-, пожароизвещатели" },
                    { "87", "приборы и установки с массой менее 30 кг, содержащие ОЗИИИ" },
                    { "88", "приборы и установки с массой 30 кг и более, содержащие ОЗИИИ" },
                    { "89", "прочие учетные единицы, содержащие ОЗИИИ" },
                    { "91", "фильтры вентиляционные в сборе" },
                    { "92", "электрокабели" },
                    { "93", "жидкие РАО, подготовленные к передаче национальному оператору" },
                    { "94", "жидкие РАО, подготовленные к передаче национальному оператору" },
                    { "95", "стройматериалы, строительный и прочий мусор" },
                    { "96", "загрязненный грунт, включая керны" },
                    { "97", "поглощающие элементы (стержни-поглотители)" },
                    { "98", "канал измерения нейтронного потока и температуры" },
                    { "99", "прочие типы РАО" },
                };

                Dictionary<string, string> codeRao11Dict = new()
                {
                    { "1", "горючие" },
                    { "2", "негорючие" },
                };

                List<string> codeRao7Valid = new() { "0", "1", "2", "3", "4", "9" };
                List<string> codeRao7Naliv = new() { "2", "3", "4", "9" };
                var codeRao2TextExists = codeRao2Dict.TryGetValue(codeRao2 ?? string.Empty, out var codeRao2Text);
                var codeRao8TextExists = codeRao8Dict.TryGetValue(codeRao8 ?? string.Empty, out var codeRao8Text);
                var codeRao910TextExists = codeRao910Dict.TryGetValue(codeRao910 ?? string.Empty, out var codeRao910Text);
                var codeRao11TextExists = codeRao11Dict.TryGetValue(codeRao11 ?? string.Empty, out var codeRao11Text);
                if (codeRao2TextExists && codeRao8TextExists)
                {
                    excelPackage.Workbook.Worksheets[0].Cells["L7"].Value = $"твердые, {codeRao2Text}, {codeRao8Text}";
                }
                if (codeRao7Valid.Contains(codeRao7 ?? string.Empty))
                {
                    excelPackage.Workbook.Worksheets[0].Cells["A23"].Value = codeRao7Naliv.Contains(codeRao7 ?? string.Empty) 
                        ? "налив" 
                        : "навал";
                    excelPackage.Workbook.Worksheets[0].Cells["F23"].Value = codeRao7 switch
                    {
                        "0" => "отсутствует",
                        "2" => "битум",
                        "3" => "цементный раствор",
                        "4" => "стекло",
                        _ => string.Empty
                    };
                }
                excelPackage.Workbook.Worksheets[0].Cells["P23"].Value = "альфа-";
                excelPackage.Workbook.Worksheets[0].Cells["P24"].Value = "бета/гамма-";
                excelPackage.Workbook.Worksheets[0].Cells["R23"].Value = "отсутствует";
                excelPackage.Workbook.Worksheets[0].Cells["P25"].Value = "параметры рад.контроля приведены на";
                if (codeRao8TextExists)
                {
                    excelPackage.Workbook.Worksheets[0].Cells["C30"].Value = $"{codeRao8Text}";
                }
                excelPackage.Workbook.Worksheets[0].Cells["E30"].Value = $"Твердые. {(codeRao11TextExists ? codeRao11Text : "")}" +
                                                                         $"{Environment.NewLine}Свободная жидкость - отсутствует." +
                                                                         $"{Environment.NewLine}Взрывоопасные вещества - отсутствуют." +
                                                                         $"{Environment.NewLine}Вещества, реагирующие с водой с выделением самовоспламеняющихся или воспламеняющихся газов - отсутствуют." +
                                                                         $"{Environment.NewLine}Вещества, способные при взаимодействии с воздухом или водой выделять токсичные газы, пары и возгоны - отсутствуют.";
                excelPackage.Workbook.Worksheets[0].Cells["G30"].Value = $"Твердые. {(codeRao910TextExists ? $"{codeRao910Text} ({codeRao910})" : "")}" +
                                                                         $"{Environment.NewLine}Коррозионно-активные вещества - отсутствуют." +
                                                                         $"{Environment.NewLine}Комплексообразующие вещества - отсутствуют." +
                                                                         $"{Environment.NewLine}Химические токсичные вещества - отсутствуют." +
                                                                         $"{Environment.NewLine}Инфицирующие (патогенные) вещества - отсутствуют.";
                excelPackage.Workbook.Worksheets[0].Cells["J30"].Value = "Самовозгорающиеся и легковоспламеняющиеся вещества - отсутствуют.";

                excelPackage.Workbook.Worksheets[0].Cells["P30"].Value = $"{ToExpString(longNuclidActivitySum)}";
                excelPackage.Workbook.Worksheets[0].Cells["Q30"].Value = $"{ToExpString(tNuclidActivity + aNuclidActivity + bNuclidActivity + uNuclidActivity)}";
                excelPackage.Workbook.Worksheets[0].Cells["R30"].Value = "отсутствуют";
                excelPackage.Workbook.Worksheets[0].Cells[$"N{39 + rowShift}"].Value = "активность приведена на";
                await excelPackage.SaveAsync();
            }
            catch (Exception)
            {
                //ignore
            }
        }
        await Dispatcher.UIThread.InvokeAsync(() => MsBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ButtonDefinitions = MsBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = MsgTitle,
                ContentHeader = "Уведомление",
                ContentMessage =
                    $"Паспорт{suffix1} успешно сформирован{suffix4} и помещён{suffix4} в" +
                    $"{Environment.NewLine}{folderPath}.",
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowWindowDialogAsync(Desktop.MainWindow));

        await Dispatcher.UIThread.InvokeAsync(() => MsBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandard(new MessageBoxStandardParams
            {
                ButtonDefinitions = MsBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = MsgTitle,
                ContentHeader = "Уведомление",
                ContentMessage = $"Необходимо закончить заполнение паспорт{suffix5} вручную." +
                                 $"{Environment.NewLine}{folderPath}.",
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowWindowDialogAsync(Desktop.MainWindow));
    }

    private static string ToExpString(double val)
    {
        var res = val.ToString("0.00E00");
        res = res.Contains("E-") 
            ? res.Replace("E-", "e") 
            : res.Replace("E", "e+");
        return res;
    }

    private static string ConvertStringToExponential(string? str) =>
        (str ?? string.Empty)
        .ToLower()
        .Trim()
        .TrimStart('(')
        .TrimEnd(')')
        .Replace(".", ",")
        .Replace('е', 'e');

    private static bool TryParseDoubleExtended(string? str, out double val)
    {
        return double.TryParse(ConvertStringToExponential(str),
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
            CultureInfo.CreateSpecificCulture("ru-RU"),
            out val);
    }

    private static List<Dictionary<string, string>> R_Populate_From_File(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        List<Dictionary<string, string>> R = new();
        if (!File.Exists(filePath)) return R;
        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        var i = 2;
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            R.Add(new Dictionary<string, string>
            {
                {"name", worksheet.Cells[i, 1].Text},
                {"value", worksheet.Cells[i, 5].Text},
                {"unit", worksheet.Cells[i, 6].Text},
                {"code", worksheet.Cells[i, 8].Text},
                {"D", worksheet.Cells[i, 15].Text},
                {"MZUA", worksheet.Cells[i, 16].Text},
                {"MZA", worksheet.Cells[i, 17].Text},
                {"A_Solid", worksheet.Cells[i, 18].Text},
                {"A_Liquid", worksheet.Cells[i, 20].Text},
                {"OSPORB_Solid", worksheet.Cells[i, 22].Text},
                {"OSPORB_Liquid", worksheet.Cells[i, 23].Text}
            });
            if (string.IsNullOrWhiteSpace(R[^1]["D"]) || !double.TryParse(R[^1]["D"], out var val1) || val1 < 0)
            {
                R[^1]["D"] = double.MaxValue.ToString(CultureInfo.CreateSpecificCulture("ru-RU"));
            }
            i++;
        }
        return R;
    }
}