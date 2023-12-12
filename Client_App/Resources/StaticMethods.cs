using Avalonia.Controls;
using Avalonia;
using Models.Attributes;
using Models.Interfaces;
using System;
using System.Linq;

namespace Client_App.Resources;

internal static class StaticMethods
{
    //  Из строки формы получаем 5 уникальных параметров имени паспорта.
    internal static void PassportUniqParam(object param, out string? okpo, out string? type, out string? date,
        out string? pasNum, out string? factoryNum)
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
}