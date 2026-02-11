using DynamicData.Binding;
using Models.Attributes;
using Models.Forms;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using Models.Forms.Form1;
using static Models.Collections.Report;

namespace Client_App.Resources;

public static class StaticMethods
{
    //  Из строки формы получаем 5 уникальных параметров имени паспорта.
    public static void PassportUniqParam(object param, out string? okpo, out string? type, out string? date, out string? pasNum, out string? factoryNum)
    {
        var par = param as object[];
        var collection = par?[0] as IKeyCollection;
        var item = collection?.GetEnumerable().MinBy(x => x.Order);
        var props = item?.GetType().GetProperties();
        okpo = "";
        type = "";
        date = "";
        pasNum = "";
        factoryNum = "";
        foreach (var prop in props!)
        {
            var attr = (FormPropertyAttribute?)prop
                .GetCustomAttributes(typeof(FormPropertyAttribute), false)
                .FirstOrDefault();
            if (attr is null
                || attr.Names.Length <= 1
                || attr.Names[0] is not ("Сведения из паспорта (сертификата) на закрытый радионуклидный источник"
                    or "Сведения об отработавших закрытых источниках ионизирующего излучения")
                || attr.Names[1] is not ("код ОКПО изготовителя" or "тип" or "дата выпуска"
                    or "номер паспорта (сертификата)"
                    or "номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ" or "номер"))
            {
                continue;
            }
            var midValue = prop.GetMethod?.Invoke(item, null);
            if (midValue?.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null) is not (null or ""))
            {
                switch (attr.Names[1])
                {
                    case "код ОКПО изготовителя":
                        okpo = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        okpo = StaticStringMethods.ConvertPrimToDash(okpo);
                        break;
                    case "тип":
                        type = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        type = StaticStringMethods.ConvertPrimToDash(type);
                        break;
                    case "дата выпуска":
                        date = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        date = DateTime.TryParse(date, out var dateTime)
                            ? dateTime.ToShortDateString()
                            : date;
                        break;
                    case "номер паспорта (сертификата)" or "номер паспорта (сертификата) ЗРИ, акта определения характеристик ОЗИИ":
                        pasNum = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        pasNum = StaticStringMethods.ConvertPrimToDash(pasNum);
                        break;
                    case "номер":
                        factoryNum = midValue.GetType().GetProperty("Value")?.GetMethod?.Invoke(midValue, null)?.ToString();
                        factoryNum = StaticStringMethods.ConvertPrimToDash(factoryNum);
                        break;
                }
            }
        }
    }

    public static void NewPassportUniqParam(object param, out string? okpo, out string? type, out string? date, out string? pasNum, out string? factoryNum)
    {
        var forms = ((IEnumerable<Form>)param).ToArray();

        okpo = string.Empty;
        type = string.Empty;
        date = string.Empty;
        pasNum = string.Empty;
        factoryNum = string.Empty;

        if (forms.Any(x => x is Form11))
        {
            var forms11 = forms.Cast<Form11>();
            var form11 = forms11.MinBy(x => x.Order)!;
            okpo = StaticStringMethods.ConvertPrimToDash(form11.CreatorOKPO.Value);
            type = StaticStringMethods.ConvertPrimToDash(form11.Type.Value);
            date = DateTime.TryParse(form11.CreationDate.Value, out var dateTime1)
                ? dateTime1.ToShortDateString()
                : date;
            pasNum = StaticStringMethods.ConvertPrimToDash(form11.PassportNumber.Value);
            factoryNum = StaticStringMethods.ConvertPrimToDash(form11.FactoryNumber.Value);

        }
        else if (forms.Any(x => x is Form15))
        {
            var forms15 = forms.Cast<Form15>();
            var form15 = forms15.MinBy(x => x.Order)!;
            type = StaticStringMethods.ConvertPrimToDash(form15.Type.Value);
            date = DateTime.TryParse(form15.CreationDate.Value, out var dateTime2)
                ? dateTime2.ToShortDateString()
                : date;
            pasNum = StaticStringMethods.ConvertPrimToDash(form15.PassportNumber.Value);
            factoryNum = StaticStringMethods.ConvertPrimToDash(form15.FactoryNumber.Value);
        }
    }
}