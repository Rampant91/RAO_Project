using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Forms.Form1;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.PassportFill
{
    internal abstract class PassportFillBaseCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
    {
        protected Report Storage => changeOrCreateViewModel.Storage;
        protected List<Form17> collection;
        protected string[] applicableOperationCodes;
        protected string msgTitle;
        protected string? msgQuestionOverride;
        protected string errOpsNotfound;
        protected async Task PassportFillAction()
        {
            if (collection.Count == 0) return;
            Dictionary<string, List<int>> packageData = [];
            List<Dictionary<string, string>> R;
#if DEBUG
            R = R_Populate_From_File(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx"));
#else
            R = R_Populate_From_File(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx"));
#endif
#if DEBUG
            var templatePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "PassportTemplates", "Template_1.7.xlsx");
#else
            var templatePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "PassportTemplates", "Template_1.7.xlsx");
#endif
            for (int i = 0; i < collection.Count; i++)
            {
                try
                {
                    int lineIndex = Storage.Rows17.IndexOf(collection.ToList<Models.Forms.Form1.Form17>()[i]);
                    if (lineIndex < 0) continue;
                    int lineIndexParent = lineIndex;
                    while (lineIndexParent >= 0)
                    {
                        if (!string.IsNullOrWhiteSpace(Storage.Rows17[lineIndexParent].PackType_DB) && Storage.Rows17[lineIndexParent].PackFactoryNumber_DB.Trim() != "-"
                            && !string.IsNullOrWhiteSpace(Storage.Rows17[lineIndexParent].PackNumber_DB) && Storage.Rows17[lineIndexParent].PackFactoryNumber_DB.Trim() != "-")
                        {
                            break;
                        }
                        lineIndexParent--;
                    }
                    if (lineIndexParent < 0) continue;
                    if (!applicableOperationCodes.Contains(Storage.Rows17[lineIndexParent].OperationCode_DB)) continue;
                    string packIdentity = $"{Storage.Rows17[lineIndexParent].PackType_DB}_{Storage.Rows17[lineIndexParent].PackFactoryNumber_DB}";
                    if (!packageData.ContainsKey(packIdentity)) packageData.Add(packIdentity, []);
                    packageData[packIdentity].Add(lineIndex);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if (packageData.Count == 0)
            {
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = msgTitle,
                        ContentHeader = "Ошибка",
                        ContentMessage = errOpsNotfound,
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));
                return;
            }

            #region Message
            bool singlePack = packageData.Count == 1;
            var suffix1 = singlePack ? "" : "а";
            var suffix2 = singlePack ? "ей" : "их";
            var suffix3 = singlePack ? "ки" : "ок";
            var suffix4 = singlePack ? "" : "ы";
            var suffix5 = singlePack ? "а" : "ов";
            var answer = await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да", IsDefault = true },
                        new ButtonDefinition { Name = "Нет", IsCancel = true }
                    ],
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ContentTitle = msgTitle,
                    ContentHeader = "Уведомление",
                    ContentMessage = msgQuestionOverride ?? $"Сформировать паспорт{suffix1} для текущ{suffix2} упаков{suffix3} РАО?     ",
                    MinWidth = 550,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow);
            if (answer is not "Да") return;
            #endregion

            string? folderPath = "";
            OpenFolderDialog dial = new();
            folderPath = await dial.ShowAsync(Desktop.MainWindow);
            if (folderPath is null) return;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            foreach (KeyValuePair<string, List<int>> package in packageData)
            {
                try
                {
                    Form17 entry = Storage.Rows17[package.Value[0]];
                    List<(string, string)> nuclids = [];
                    double longNuclidActivitySum = 0.0;
                    double tNuclidActivity = 0.0;
                    bool tNuclidActivityExists = TryParseDoubleExtended(entry.TritiumActivity_DB ?? string.Empty, out tNuclidActivity);
                    double aNuclidActivity = 0.0;
                    bool aNuclidActivityExists = TryParseDoubleExtended(entry.AlphaActivity_DB ?? string.Empty, out aNuclidActivity);
                    double bNuclidActivity = 0.0;
                    bool bNuclidActivityExists = TryParseDoubleExtended(entry.BetaGammaActivity_DB ?? string.Empty, out bNuclidActivity);
                    double uNuclidActivity = 0.0;
                    bool uNuclidActivityExists = TryParseDoubleExtended(entry.TransuraniumActivity_DB ?? string.Empty, out uNuclidActivity);
                    foreach (int rowId in package.Value)
                    {
                        nuclids.Add((Storage.Rows17[rowId].Radionuclids_DB.Trim().ToLower(), Storage.Rows17[rowId].SpecificActivity_DB));
                    }
                    #region Setup file
                    string fileName = $"{Storage.Reports.Master.RegNoRep.Value}_{package.Key}.xlsx".Replace('/', '_');
                    string fullPath = Path.Combine(folderPath, fileName);
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

                            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                                {
                                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
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
                                .ShowDialog(Desktop.MainWindow));
                            continue;
                            #endregion
                        }
                    }
                    #endregion
                    using ExcelPackage excelPackage = new(new FileInfo(fullPath), new FileInfo(templatePath));
                    int rowShift = 0;
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
                    excelPackage.Workbook.Worksheets[0].Cells["L24"].Value = $"({entry.Volume_DB})";
                    excelPackage.Workbook.Worksheets[0].Cells["L23"].Value = TryParseDoubleExtended(entry.Mass_DB ?? string.Empty, out double mass)
                        ? (mass * 1000).ToString("0.00") : "-";
                    for (int i = 0; i < nuclids.Count; i++)
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
                            if (TryParseDoubleExtended(nuclids[i].Item2, out double nuclid_activity))
                            {
                                longNuclidActivitySum += nuclid_activity;
                            }
                        }
                    }
                    excelPackage.Workbook.Worksheets[0].Cells[$"O{39 + rowShift}"].Value = entry.FormingDate_DB;
                    excelPackage.Workbook.Worksheets[0].Cells["G13"].Value = $"{Storage.Reports.Master.ShortJurLicoRep.Value} ОКПО {entry.ProviderOrRecieverOKPO_DB}";
                    excelPackage.Workbook.Worksheets[0].Cells["C31"].Value = entry.CodeRAO_DB;
                    excelPackage.Workbook.Worksheets[0].Cells["C7"].Value = entry.StatusRAO_DB;
                    excelPackage.Workbook.Worksheets[0].Cells["M24"].Value = $"({entry.VolumeOutOfPack_DB})";
                    excelPackage.Workbook.Worksheets[0].Cells["M23"].Value = TryParseDoubleExtended(entry.MassOutOfPack_DB ?? string.Empty, out double mass_b)
                        ? (mass_b * 1000).ToString("0.00") : "-";
                    if (TryParseDoubleExtended(entry.MassOutOfPack_DB ?? string.Empty, out double mass_nuclids) && mass_nuclids != 0)
                    {
                        excelPackage.Workbook.Worksheets[0].Cells["P34"].Value = tNuclidActivityExists
                            ? (tNuclidActivity / mass_nuclids / 1000000).ToString("0.00E00") : "0";
                        excelPackage.Workbook.Worksheets[0].Cells["P33"].Value = bNuclidActivityExists
                            ? (bNuclidActivity / mass_nuclids / 1000000).ToString("0.00E00") : "0";
                        excelPackage.Workbook.Worksheets[0].Cells["P32"].Value = aNuclidActivityExists
                            ? (aNuclidActivity / mass_nuclids / 1000000).ToString("0.00E00") : "0";
                        excelPackage.Workbook.Worksheets[0].Cells["P31"].Value = uNuclidActivityExists
                            ? (uNuclidActivity / mass_nuclids / 1000000).ToString("0.00E00") : "0";
                    }
                    string? CodeRAO_2 = entry.CodeRAO_DB.Length >= 2 ? entry.CodeRAO_DB.Substring(1, 1) : null;
                    string? CodeRAO_7 = entry.CodeRAO_DB.Length >= 7 ? entry.CodeRAO_DB.Substring(6, 1) : null;
                    string? CodeRAO_8 = entry.CodeRAO_DB.Length >= 8 ? entry.CodeRAO_DB.Substring(7, 1) : null;
                    string? CodeRAO_910 = entry.CodeRAO_DB.Length >= 10 ? entry.CodeRAO_DB.Substring(8, 2) : null;
                    string? CodeRAO_11 = entry.CodeRAO_DB.Length >= 11 ? entry.CodeRAO_DB.Substring(10, 1) : null;
                    Dictionary<string, string> CodeRAO_2_Dict = new()
                    {
                        { "0", "очень низкоактивные" },
                        { "1", "низкоактивные" },
                        { "2", "среднеактивные" },
                        { "3", "высокоактивные" },
                        { "4", "отработавшие закрытые источники ионизирующего излучения" },
                        { "9", "прочие" },
                    };
                    Dictionary<string, string> CodeRAO_8_Dict = new()
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
                    Dictionary<string, string> CodeRAO_910_Dict = new()
                    {
                        { "01", "газы, аэрозоли" },
                        { "11", "воды загрязненные подземные, в том числе грунтовые и дренажных систем" },
                        { "12", "воды охлаждения реакторного производства, контурные воды, воды цистерн биологической защиты, воды бассейнов выдержки ОЯТ" },
                        { "13", "растворы хвостовые" },
                        { "14", "конденсаты после упаривания, газоочистки, конденсаты технологические прочие" },
                        { "15", "растворы регенерационные (сливы), растворы промывки экстрагентов, сорбентов" },
                        { "16", "воды лабораторий, трапные, обмывочные воды, растворы после дезактивации, включая воды сампропускников и спецпрачечных, сточные воды прочие" },
                        { "17", "воды (декантаты) бассейнов-хранилиц, а также емкостей хранения пульп, фильтратов" },
                        { "18", "воды промысловые" },
                        { "19", "жидкости неорганические прочие" },
                        { "21", "экстрагенты" },
                        { "22", "масла" },
                        { "23", "эмульсии смазочно-охлаждающих жидкостей (СОЖ)" },
                        { "24", "моющие средства (детергенты) за исключением спиртов" },
                        { "25", "спирты" },
                        { "26", "сциоцилляторы органические жидкостные" },
                        { "29", "жидкости органические прочие" },
                        { "31", "хвосты гидрометаллургического, химико-металлургического, разделительного и сублиматного производств, пульпы, образующиеся после нейтрализации" },
                        { "32", "пульпы отработавщих ионообменных смол и прочих собрентов органических" },
                        { "33", "шламы и пульпы отработавших сорбентов неорганических, фильтрующих материалов" },
                        { "34", "солевой остаток с солесодержанием до 250 г/л, кубовый остаток" },
                        { "35", "кубовый остаток после упаривания с солесодержанием более 250 г/л, концентрат солевой" },
                        { "36", "взвеси, содержащие прокуты коррозии" },
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
                        { "61", "сорбенты и фльтроматериалы" },
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
                        { "83", "изделия с радоактивной светомассой постоянного действия (ампулированные источники для ночного обозначения габаритов, аварийные и автономные осветители, указатели, источники света для шкал и циферблатов приборов, подстветки оружейных прицелов и подобное)" },
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
                        { "96", "загразненный грунт, включая керны" },
                        { "97", "поглощающие элементы (стержни-поглотители)" },
                        { "98", "канал измерения нейтронного потока и температуры" },
                        { "99", "прочие типы РАО" },
                    };
                    Dictionary<string, string> CodeRAO_11_Dict = new()
                    {
                        { "1", "горючие" },
                        { "2", "негорючие" },
                    };
                    List<string> CodeRAO_7_valid = new() { "0", "1", "2", "3", "4", "9" };
                    List<string> CodeRAO_7_naliv = new() { "2", "3", "4", "9" };
                    bool CodeRAO_2_Text_Exists = CodeRAO_2_Dict.TryGetValue(CodeRAO_2 ?? string.Empty, out string? CodeRAO_2_Text);
                    bool CodeRAO_8_Text_Exists = CodeRAO_8_Dict.TryGetValue(CodeRAO_8 ?? string.Empty, out string? CodeRAO_8_Text);
                    bool CodeRAO_910_Text_Exists = CodeRAO_910_Dict.TryGetValue(CodeRAO_910 ?? string.Empty, out string? CodeRAO_910_Text);
                    bool CodeRAO_11_Text_Exists = CodeRAO_11_Dict.TryGetValue(CodeRAO_11 ?? string.Empty, out string? CodeRAO_11_Text);
                    if (CodeRAO_2_Text_Exists && CodeRAO_8_Text_Exists)
                    {
                        excelPackage.Workbook.Worksheets[0].Cells["L7"].Value = $"твердые, {CodeRAO_2_Text}, {CodeRAO_8_Text}";
                    }
                    if (CodeRAO_7_valid.Contains(CodeRAO_7 ?? string.Empty))
                    {
                        excelPackage.Workbook.Worksheets[0].Cells["A23"].Value = CodeRAO_7_naliv.Contains(CodeRAO_7 ?? string.Empty) ? "налив" : "навал";
                        excelPackage.Workbook.Worksheets[0].Cells["F23"].Value = CodeRAO_7 == "0" ? "отсутствует"
                            : CodeRAO_7 == "2" ? "битум"
                            : CodeRAO_7 == "3" ? "цементный раствор"
                            : CodeRAO_7 == "4" ? "стекло"
                            : string.Empty;
                    }
                    excelPackage.Workbook.Worksheets[0].Cells["P23"].Value = "альфа-";
                    excelPackage.Workbook.Worksheets[0].Cells["P24"].Value = "бета/гамма-";
                    excelPackage.Workbook.Worksheets[0].Cells["R23"].Value = "отсутствует";
                    excelPackage.Workbook.Worksheets[0].Cells["P25"].Value = "параметры рад.контроля приведены на";
                    if (CodeRAO_8_Text_Exists)
                    {
                        excelPackage.Workbook.Worksheets[0].Cells["C30"].Value = $"{CodeRAO_8_Text}";
                    }
                    excelPackage.Workbook.Worksheets[0].Cells["E30"].Value = $"Твердые. {(CodeRAO_11_Text_Exists ? CodeRAO_11_Text : "")}" +
                        $"{Environment.NewLine}Свободная жидкость - отсутствует." +
                        $"{Environment.NewLine}Взрывоорасные вещества - отсутствуют." +
                        $"{Environment.NewLine}Вещества, реагирующие с водой с выделением самовоспламеняющихся или воспламеняющихся газов - отсутствуют." +
                        $"{Environment.NewLine}Вещества, способные при взаимодействии с воздухом или водой выделять токсичные газы, пары и возгоны - отсутствуют.";
                    excelPackage.Workbook.Worksheets[0].Cells["G30"].Value = $"Твердые. {(CodeRAO_910_Text_Exists ? $"{CodeRAO_910_Text} ({CodeRAO_910})" : "")}" +
                        $"{Environment.NewLine}Коррозионно-активные вещества - отсутствуют." +
                        $"{Environment.NewLine}Комплексообразующие вещества - отсутствуют." +
                        $"{Environment.NewLine}Химические токсичные вещества - отсутствуют." +
                        $"{Environment.NewLine}Инфицирующие (патогенные) вещества - отсутствуют.";
                    excelPackage.Workbook.Worksheets[0].Cells["J30"].Value = "Самовозгорающиеся и легковоспламеняющиеся вещества - отсутствуют.";

                    excelPackage.Workbook.Worksheets[0].Cells["P30"].Value = $"{longNuclidActivitySum}";
                    excelPackage.Workbook.Worksheets[0].Cells["Q30"].Value = $"{(tNuclidActivity + aNuclidActivity + bNuclidActivity + uNuclidActivity).ToString("0.00E00")}";
                    excelPackage.Workbook.Worksheets[0].Cells["R30"].Value = "отсутствуют";
                    excelPackage.Workbook.Worksheets[0].Cells[$"N{39 + rowShift}"].Value = "активность приведена на";
                    excelPackage.Save();
                }
                catch (Exception)
                {
                    continue;
                }
            }
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = msgTitle,
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Паспорт{suffix1} успешно сформирован{suffix4}." +
                        $"{Environment.NewLine}и помещён{suffix4} в" +
                        $"{Environment.NewLine}{folderPath}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = msgTitle,
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Необходимо закончить заполнение паспорт{suffix5} вручную." +
                        $"{Environment.NewLine}{folderPath}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));
        }
        protected static string ConvertStringToExponential(string? str) =>
            (str ?? string.Empty)
            .ToLower()
            .Trim()
            .TrimStart('(')
            .TrimEnd(')')
            .Replace(".", ",")
            .Replace('е', 'e');
        protected static bool TryParseDoubleExtended(string? str, out double val)
        {
            return double.TryParse(ConvertStringToExponential(str),
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out val);
        }
        protected static List<Dictionary<string, string>> R_Populate_From_File(string filePath)
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
}
