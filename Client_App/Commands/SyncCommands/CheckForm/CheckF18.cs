using Models.CheckForm;
using Models.Collections;
using Models.Forms.Form1;
using System.Collections.Generic;
using Models.Forms;
using System.Linq;

namespace Client_App.Commands.SyncCommands.CheckForm;

public abstract class CheckF18 : CheckBase
{
    #region CheckTotal
    
    public static List<CheckError> Check_Total(Reports reps, Report rep)
    {
        var currentFormLine = 0;
        List<CheckError> errorList = new();
        LoadDictionaries();
        var formsList = rep.Rows15.ToList<Form18>();
        errorList.AddRange(Check_001(formsList, rep));
        errorList.AddRange(Check_002(rep));
        errorList.AddRange(Check_003(formsList));
        foreach (var key in rep.Rows18)
        {
            var form = (Form18)key;
            var notes = rep.Notes.ToList<Note>();
            var forms10 = reps.Master_DB.Rows10.ToList<Form10>();

            currentFormLine++;
        }
        var index = 0;
        foreach (var error in errorList)
        {
            if (GraphsList.TryGetValue(error.Column, out var columnFrontName))
            {
                error.Column = columnFrontName;
            }
            index++;
            error.Index = index;
        }
        return errorList;
    }

    #endregion

    #region Checks

    #region Check001

    //Проверка даты окончания периода
    private static List<CheckError> Check_001(List<Form18> forms18, Report rep)
    {
        var forms = forms18.Cast<Form1>().ToList();
        return CheckRepPeriod(forms, rep);
    }

    #endregion

    #region Check002

    //Отчёт только от определённых организаций
    private static List<CheckError> Check_002(Report rep)
    {
        List<CheckError> result = new();
        var regNum = rep.RegNoRep.Value ?? "";
        if (regNum is not ("24038" or "24072" or "70025" or "70032" or "73010" or "73012"))
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = string.Empty,
                Column = string.Empty,
                Value = string.Empty,
                Message = $"Жидкие РАО должны быть отверждены. Сведения об отверждённых РАО, " +
                          $"приведённых к критериям приемлемости, приводятся в форме 1.7."
            });
        }
        return result;
    }

    #endregion

    #region Check003

    //Наличие строк дубликатов
    private static List<CheckError> Check_003(List<Form18> forms)
    {
        List<CheckError> result = new();
        HashSet<int> duplicatesLinesSet = new();
        var comparator = new CustomNullStringWithTrimComparer();
        for (var i = 0; i < forms.Count; i++)
        {
            var currentForm = forms[i];
            for (var j = i + 1; j < forms.Count; j++)
            {
                var formToCompare = forms[j];
                var isDuplicate = comparator.Compare(formToCompare.OperationCode_DB, currentForm.OperationCode_DB) == 0
                                  && comparator.Compare(formToCompare.OperationDate_DB, currentForm.OperationDate_DB) == 0
                                  && comparator.Compare(formToCompare.IndividualNumberZHRO_DB, currentForm.IndividualNumberZHRO_DB) == 0
                                  && comparator.Compare(formToCompare.PassportNumber_DB, currentForm.PassportNumber_DB) == 0
                                  && comparator.Compare(formToCompare.Volume6_DB, currentForm.Volume6_DB) == 0
                                  && comparator.Compare(formToCompare.Mass7_DB, currentForm.Mass7_DB) == 0
                                  && comparator.Compare(formToCompare.SaltConcentration_DB, currentForm.SaltConcentration_DB) == 0
                                  && comparator.Compare(formToCompare.Radionuclids_DB, currentForm.Radionuclids_DB) == 0
                                  && comparator.Compare(formToCompare.SpecificActivity_DB, currentForm.SpecificActivity_DB) == 0
                                  && formToCompare.DocumentVid_DB == currentForm.DocumentVid_DB
                                  && comparator.Compare(formToCompare.DocumentNumber_DB, currentForm.DocumentNumber_DB) == 0
                                  && comparator.Compare(formToCompare.DocumentDate_DB, currentForm.DocumentDate_DB) == 0
                                  && comparator.Compare(formToCompare.ProviderOrRecieverOKPO_DB, currentForm.ProviderOrRecieverOKPO_DB) == 0
                                  && comparator.Compare(formToCompare.TransporterOKPO_DB, currentForm.TransporterOKPO_DB) == 0
                                  && comparator.Compare(formToCompare.StoragePlaceName_DB, currentForm.StoragePlaceName_DB) == 0
                                  && comparator.Compare(formToCompare.StoragePlaceCode_DB, currentForm.StoragePlaceCode_DB) == 0
                                  && comparator.Compare(formToCompare.CodeRAO_DB, currentForm.CodeRAO_DB) == 0
                                  && comparator.Compare(formToCompare.StatusRAO_DB, currentForm.StatusRAO_DB) == 0
                                  && comparator.Compare(formToCompare.Volume20_DB, currentForm.Volume20_DB) == 0
                                  && comparator.Compare(formToCompare.Mass21_DB, currentForm.Mass21_DB) == 0
                                  && comparator.Compare(formToCompare.TritiumActivity_DB, currentForm.TritiumActivity_DB) == 0
                                  && comparator.Compare(formToCompare.BetaGammaActivity_DB, currentForm.BetaGammaActivity_DB) == 0
                                  && comparator.Compare(formToCompare.AlphaActivity_DB, currentForm.AlphaActivity_DB) == 0
                                  && comparator.Compare(formToCompare.TransuraniumActivity_DB, currentForm.TransuraniumActivity_DB) == 0
                                  && comparator.Compare(formToCompare.RefineOrSortRAOCode_DB, currentForm.RefineOrSortRAOCode_DB) == 0
                                  && comparator.Compare(formToCompare.Subsidy_DB, currentForm.Subsidy_DB) == 0
                                  && comparator.Compare(formToCompare.FcpNumber_DB, currentForm.FcpNumber_DB) == 0;
                if (!isDuplicate) continue;
                duplicatesLinesSet.Add(i + 1);
                duplicatesLinesSet.Add(j + 1);
            }
        }
        var duplicateLines = string.Join(", ", duplicatesLinesSet.Order());
        if (duplicatesLinesSet.Count != 0)
        {
            result.Add(new CheckError
            {
                FormNum = "form_18",
                Row = duplicateLines,
                Column = "2 - 28",
                Value = string.Empty,
                Message = $"Данные граф 2-28 в строках {duplicateLines} продублированы. " +
                          $"Следует проверить правильность предоставления данных."
            });
        }
        return result;
    }

    #endregion



    #endregion

    #region GraphList

    private static readonly Dictionary<string, string> GraphsList = new()
    {
        { "NumberInOrder_DB", "01 - № п/п" },
        { "OperationCode_DB", "02 - Код операции" },
        { "OperationDate_DB", "03 - Дата операции" },
        { "IndividualNumberZHRO_DB", "04 - индивидуальный номер (идентификационный код) партии ЖРО" },
        { "PassportNumber_DB", "05 - Номер паспорта" },
        { "Volume6_DB", "06 - Объем (сведения о партии), куб. м" },
        { "Mass7_DB", "07 - Масса (сведения о партии), т" },
        { "SaltConcentration_DB", "08 - Солесодержание, г/л" },
        { "Radionuclids_DB", "09 - Наименование радионуклида" },
        { "SpecificActivity_DB", "10 - Удельная активность, Бк/г" },
        { "DocumentVid_DB", "11 - Вид документа" },
        { "DocumentNumber_DB", "12 - Номер документа" },
        { "DocumentDate_DB", "13 - Дата документа" },
        { "ProviderOrRecieverOKPO_DB", "14 - Код ОКПО поставщика или получателя" },
        { "TransporterOKPO_DB", "15 - Код ОКПО перевозчика" },
        { "StoragePlaceName_DB", "16 - Наименование пункта хранения" },
        { "StoragePlaceCode_DB", "17 - Код пункта хранения" },
        { "CodeRAO_DB", "18 - Код РАО" },
        { "StatusRAO_DB", "19 - Статус РАО" },
        { "Volume20_DB", "20 - Объем, куб. м" },
        { "Mass21_DB", "21 - Масса, т" },
        { "TritiumActivity_DB", "22 - Суммарная активность, тритий" },
        { "BetaGammaActivity_DB", "23 - Суммарная активность, бета-, гамма-излучающие радионуклиды (исключая тритий)" },
        { "AlphaActivity_DB", "24 - Суммарная активность, альфа-излучающие радионуклиды (исключая трансурановые)" },
        { "TransuraniumActivity_DB", "25 - Суммарная активность, трансурановые" },
        { "RefineOrSortRAOCode_DB", "26 - Код переработки/сортировки" },
        { "Subsidy_DB", "27 - Субсидия" },
        { "FcpNumber_DB", "28 - Номер мероприятия ФЦП" }
    };

    #endregion
}