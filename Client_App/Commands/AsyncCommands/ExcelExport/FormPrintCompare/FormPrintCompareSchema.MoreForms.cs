namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

public static partial class FormPrintCompareSchema
{
    private static readonly CompareColumn[] Columns12 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Дата операции", CompareColumnKind.Date, InFingerprint: true),
        new("Номер паспорта", CompareColumnKind.Id, InFingerprint: true),
        new("Наименование", CompareColumnKind.Text, InFingerprint: true),
        new("Заводской номер", CompareColumnKind.Id, InFingerprint: true, IsFactoryId: true),
        new("Масса", CompareColumnKind.Numeric),
        new("ОКПО изготовителя", CompareColumnKind.Id),
        new("Дата выпуска", CompareColumnKind.Date),
        new("НСС, мес.", CompareColumnKind.Text),
        new("Код формы собственности", CompareColumnKind.Code),
        new("Владелец", CompareColumnKind.Text),
        new("Вид документа", CompareColumnKind.Code),
        new("Номер документа", CompareColumnKind.Id),
        new("Дата документа", CompareColumnKind.Date),
        new("ОКПО поставщика/получателя", CompareColumnKind.Id),
        new("ОКПО перевозчика", CompareColumnKind.Id),
        new("Наименование упаковки", CompareColumnKind.Text),
        new("Тип УКТ", CompareColumnKind.Text),
        new("Номер УКТ", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns15 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Дата операции", CompareColumnKind.Date, InFingerprint: true),
        new("Номер паспорта", CompareColumnKind.Id, InFingerprint: true),
        new("Тип", CompareColumnKind.Type, InFingerprint: true),
        new("Радионуклиды", CompareColumnKind.Radionuclids),
        new("Заводской номер", CompareColumnKind.Id, InFingerprint: true, IsFactoryId: true),
        new("Количество", CompareColumnKind.Quantity),
        new("Активность, Бк", CompareColumnKind.Numeric),
        new("Дата выпуска", CompareColumnKind.Date),
        new("Статус РАО", CompareColumnKind.Code, InFingerprint: true),
        new("Вид документа", CompareColumnKind.Code),
        new("Номер документа", CompareColumnKind.Id),
        new("Дата документа", CompareColumnKind.Date),
        new("ОКПО поставщика/получателя", CompareColumnKind.Id),
        new("ОКПО перевозчика", CompareColumnKind.Id),
        new("Наименование упаковки", CompareColumnKind.Text),
        new("Тип УКТ", CompareColumnKind.Text),
        new("Номер УКТ", CompareColumnKind.Id),
        new("Пункт хранения", CompareColumnKind.Text),
        new("Код пункта хранения", CompareColumnKind.Code),
        new("Код переработки/сортировки", CompareColumnKind.Code),
        new("Субсидия", CompareColumnKind.Text),
        new("Номер ФЦП", CompareColumnKind.Id),
        new("Номер договора", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns16 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Дата операции", CompareColumnKind.Date, InFingerprint: true),
        new("Код РАО", CompareColumnKind.Code, InFingerprint: true),
        new("Статус РАО", CompareColumnKind.Code, InFingerprint: true),
        new("Объём", CompareColumnKind.Numeric),
        new("Масса", CompareColumnKind.Numeric),
        new("Количество ОЗИИИ", CompareColumnKind.Quantity),
        new("Радионуклиды", CompareColumnKind.Radionuclids),
        new("Активность трития, Бк", CompareColumnKind.Numeric),
        new("Активность бета/гамма, Бк", CompareColumnKind.Numeric),
        new("Активность альфа, Бк", CompareColumnKind.Numeric),
        new("Активность трансурановых, Бк", CompareColumnKind.Numeric),
        new("Дата измерения активности", CompareColumnKind.Date),
        new("Вид документа", CompareColumnKind.Code),
        new("Номер документа", CompareColumnKind.Id),
        new("Дата документа", CompareColumnKind.Date),
        new("ОКПО поставщика/получателя", CompareColumnKind.Id),
        new("ОКПО перевозчика", CompareColumnKind.Id),
        new("Пункт хранения", CompareColumnKind.Text),
        new("Код пункта хранения", CompareColumnKind.Code),
        new("Код переработки/сортировки", CompareColumnKind.Code),
        new("Наименование упаковки", CompareColumnKind.Text),
        new("Тип УКТ", CompareColumnKind.Text),
        new("Номер УКТ", CompareColumnKind.Id, InFingerprint: true),
        new("Субсидия", CompareColumnKind.Text),
        new("Номер ФЦП", CompareColumnKind.Id),
        new("Номер договора", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns17 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Дата операции", CompareColumnKind.Date, InFingerprint: true),
        new("Наименование упаковки", CompareColumnKind.Text, InFingerprint: true),
        new("Тип УКТ", CompareColumnKind.Text, InFingerprint: true),
        new("Заводской номер упаковки", CompareColumnKind.Id, InFingerprint: true, IsFactoryId: true),
        new("Номер УКТ", CompareColumnKind.Id, InFingerprint: true),
        new("Дата формирования", CompareColumnKind.Date),
        new("Номер паспорта", CompareColumnKind.Id, InFingerprint: true),
        new("Объём", CompareColumnKind.Numeric),
        new("Масса", CompareColumnKind.Numeric),
        new("Радионуклиды", CompareColumnKind.Radionuclids),
        new("Удельная активность", CompareColumnKind.Numeric),
        new("Вид документа", CompareColumnKind.Code),
        new("Номер документа", CompareColumnKind.Id),
        new("Дата документа", CompareColumnKind.Date),
        new("ОКПО поставщика/получателя", CompareColumnKind.Id),
        new("ОКПО перевозчика", CompareColumnKind.Id),
        new("Пункт хранения", CompareColumnKind.Text),
        new("Код пункта хранения", CompareColumnKind.Code),
        new("Код РАО", CompareColumnKind.Code, InFingerprint: true),
        new("Статус РАО", CompareColumnKind.Code),
        new("Объём без упаковки", CompareColumnKind.Numeric),
        new("Масса без упаковки", CompareColumnKind.Numeric),
        new("Количество", CompareColumnKind.Quantity),
        new("Активность трития, Бк", CompareColumnKind.Numeric),
        new("Активность бета/гамма, Бк", CompareColumnKind.Numeric),
        new("Активность альфа, Бк", CompareColumnKind.Numeric),
        new("Активность трансурановых, Бк", CompareColumnKind.Numeric),
        new("Код переработки/сортировки", CompareColumnKind.Code),
        new("Субсидия", CompareColumnKind.Text),
        new("Номер ФЦП", CompareColumnKind.Id),
        new("Номер договора", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns18 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Дата операции", CompareColumnKind.Date, InFingerprint: true),
        new("Индивидуальный номер ЖРО", CompareColumnKind.Id, InFingerprint: true),
        new("Номер паспорта", CompareColumnKind.Id, InFingerprint: true),
        new("Объём", CompareColumnKind.Numeric),
        new("Масса", CompareColumnKind.Numeric),
        new("Солесодержание", CompareColumnKind.Numeric),
        new("Радионуклиды", CompareColumnKind.Radionuclids),
        new("Удельная активность", CompareColumnKind.Numeric),
        new("Вид документа", CompareColumnKind.Code),
        new("Номер документа", CompareColumnKind.Id),
        new("Дата документа", CompareColumnKind.Date),
        new("ОКПО поставщика/получателя", CompareColumnKind.Id),
        new("ОКПО перевозчика", CompareColumnKind.Id),
        new("Пункт хранения", CompareColumnKind.Text),
        new("Код пункта хранения", CompareColumnKind.Code),
        new("Код РАО", CompareColumnKind.Code, InFingerprint: true),
        new("Статус РАО", CompareColumnKind.Code),
        new("Объём (без упаковки)", CompareColumnKind.Numeric),
        new("Масса (без упаковки)", CompareColumnKind.Numeric),
        new("Активность трития, Бк", CompareColumnKind.Numeric),
        new("Активность бета/гамма, Бк", CompareColumnKind.Numeric),
        new("Активность альфа, Бк", CompareColumnKind.Numeric),
        new("Активность трансурановых, Бк", CompareColumnKind.Numeric),
        new("Код переработки/сортировки", CompareColumnKind.Code),
        new("Субсидия", CompareColumnKind.Text),
        new("Номер ФЦП", CompareColumnKind.Id),
        new("Номер договора", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns19 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код операции", CompareColumnKind.Code, InFingerprint: true),
        new("Дата операции", CompareColumnKind.Date, InFingerprint: true),
        new("Вид документа", CompareColumnKind.Code),
        new("Номер документа", CompareColumnKind.Id, InFingerprint: true),
        new("Дата документа", CompareColumnKind.Date),
        new("Код типа объекта учёта", CompareColumnKind.Code, InFingerprint: true),
        new("Радионуклиды", CompareColumnKind.Radionuclids, InFingerprint: true),
        new("Активность, Бк", CompareColumnKind.Numeric)
    ];

    private static readonly CompareColumn[] Columns21 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Наименование установки", CompareColumnKind.Text, InFingerprint: true),
        new("Код установки", CompareColumnKind.Code, InFingerprint: true),
        new("Мощность", CompareColumnKind.Numeric),
        new("Часов в году", CompareColumnKind.Numeric),
        new("Код РАО на входе", CompareColumnKind.Code, InFingerprint: true),
        new("Статус РАО на входе", CompareColumnKind.Code),
        new("Объём на входе", CompareColumnKind.Numeric),
        new("Масса на входе", CompareColumnKind.Numeric),
        new("Количество на входе", CompareColumnKind.Quantity),
        new("Тритий на входе, Бк", CompareColumnKind.Numeric),
        new("Бета/гамма на входе, Бк", CompareColumnKind.Numeric),
        new("Альфа на входе, Бк", CompareColumnKind.Numeric),
        new("Трансурановые на входе, Бк", CompareColumnKind.Numeric),
        new("Код РАО на выходе", CompareColumnKind.Code, InFingerprint: true),
        new("Статус РАО на выходе", CompareColumnKind.Code),
        new("Объём на выходе", CompareColumnKind.Numeric),
        new("Масса на выходе", CompareColumnKind.Numeric),
        new("Количество ОЗИИИ на выходе", CompareColumnKind.Quantity),
        new("Тритий на выходе, Бк", CompareColumnKind.Numeric),
        new("Бета/гамма на выходе, Бк", CompareColumnKind.Numeric),
        new("Альфа на выходе, Бк", CompareColumnKind.Numeric),
        new("Трансурановые на выходе, Бк", CompareColumnKind.Numeric)
    ];

    private static readonly CompareColumn[] Columns22 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Пункт хранения", CompareColumnKind.Text, InFingerprint: true),
        new("Код пункта хранения", CompareColumnKind.Code, InFingerprint: true),
        new("Наименование упаковки", CompareColumnKind.Text),
        new("Тип УКТ", CompareColumnKind.Text),
        new("Количество упаковок", CompareColumnKind.Quantity),
        new("Код РАО", CompareColumnKind.Code, InFingerprint: true),
        new("Статус РАО", CompareColumnKind.Code, InFingerprint: true),
        new("Объём вне упаковки", CompareColumnKind.Numeric),
        new("Объём в упаковке", CompareColumnKind.Numeric),
        new("Масса вне упаковки", CompareColumnKind.Numeric),
        new("Масса в упаковке", CompareColumnKind.Numeric),
        new("Количество ОЗИИИ", CompareColumnKind.Quantity),
        new("Активность трития, Бк", CompareColumnKind.Numeric),
        new("Активность бета/гамма, Бк", CompareColumnKind.Numeric),
        new("Активность альфа, Бк", CompareColumnKind.Numeric),
        new("Активность трансурановых, Бк", CompareColumnKind.Numeric),
        new("Радионуклиды", CompareColumnKind.Radionuclids),
        new("Субсидия", CompareColumnKind.Text),
        new("Номер ФЦП", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns23 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Пункт хранения", CompareColumnKind.Text, InFingerprint: true),
        new("Код пункта хранения", CompareColumnKind.Code, InFingerprint: true),
        new("Проектный объём", CompareColumnKind.Numeric),
        new("Код РАО", CompareColumnKind.Code, InFingerprint: true),
        new("Объём", CompareColumnKind.Numeric),
        new("Масса", CompareColumnKind.Numeric),
        new("Количество ОЗИИИ", CompareColumnKind.Quantity),
        new("Суммарная активность, Бк", CompareColumnKind.Numeric),
        new("Номер документа", CompareColumnKind.Id, InFingerprint: true),
        new("Дата документа", CompareColumnKind.Date),
        new("Срок действия", CompareColumnKind.Date),
        new("Наименование документа", CompareColumnKind.Text)
    ];

    private static readonly CompareColumn[] Columns24 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Код ОЯТ", CompareColumnKind.Code, InFingerprint: true),
        new("Номер ФЦП", CompareColumnKind.Id, InFingerprint: true),
        new("Масса образованных", CompareColumnKind.Numeric),
        new("Количество образованных", CompareColumnKind.Quantity),
        new("Масса поступивших", CompareColumnKind.Numeric),
        new("Количество поступивших", CompareColumnKind.Quantity),
        new("Масса поступивших импорт", CompareColumnKind.Numeric),
        new("Количество поступивших импорт", CompareColumnKind.Quantity),
        new("Масса иных причин", CompareColumnKind.Numeric),
        new("Количество иных причин", CompareColumnKind.Quantity),
        new("Масса переданных", CompareColumnKind.Numeric),
        new("Количество переданных", CompareColumnKind.Quantity),
        new("Масса переработанных", CompareColumnKind.Numeric),
        new("Количество переработанных", CompareColumnKind.Quantity),
        new("Масса снятых с учёта", CompareColumnKind.Numeric),
        new("Количество снятых с учёта", CompareColumnKind.Quantity)
    ];

    private static readonly CompareColumn[] Columns25 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Пункт хранения", CompareColumnKind.Text, InFingerprint: true),
        new("Код пункта хранения", CompareColumnKind.Code, InFingerprint: true),
        new("Код ОЯТ", CompareColumnKind.Code, InFingerprint: true),
        new("Номер ФЦП", CompareColumnKind.Id),
        new("Масса топлива", CompareColumnKind.Numeric),
        new("Масса ТВС", CompareColumnKind.Numeric),
        new("Количество", CompareColumnKind.Quantity),
        new("Активность альфа, Бк", CompareColumnKind.Numeric),
        new("Активность бета/гамма, Бк", CompareColumnKind.Numeric)
    ];

    private static readonly CompareColumn[] Columns26 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Номер наблюдательного пункта", CompareColumnKind.Id, InFingerprint: true),
        new("Наименование зоны", CompareColumnKind.Text, InFingerprint: true),
        new("Предполагаемый источник", CompareColumnKind.Text),
        new("Расстояние до источника", CompareColumnKind.Numeric),
        new("Глубина отбора", CompareColumnKind.Numeric),
        new("Радионуклид", CompareColumnKind.Radionuclids, InFingerprint: true),
        new("Среднегодовая концентрация", CompareColumnKind.Numeric)
    ];

    private static readonly CompareColumn[] Columns27 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Номер источника", CompareColumnKind.Id, InFingerprint: true),
        new("Радионуклид", CompareColumnKind.Radionuclids, InFingerprint: true),
        new("Разрешённый выброс", CompareColumnKind.Numeric),
        new("Фактический выброс", CompareColumnKind.Numeric),
        new("Выброс предыдущего года", CompareColumnKind.Numeric)
    ];

    private static readonly CompareColumn[] Columns28 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Наименование источника", CompareColumnKind.Text, InFingerprint: true),
        new("Наименование приёмника", CompareColumnKind.Text, InFingerprint: true),
        new("Код типа приёмника", CompareColumnKind.Code, InFingerprint: true),
        new("Водохозяйственный участок", CompareColumnKind.Text),
        new("Разрешённый объём", CompareColumnKind.Numeric),
        new("Фактический объём", CompareColumnKind.Numeric)
    ];

    private static readonly CompareColumn[] Columns29 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Наименование источника", CompareColumnKind.Text, InFingerprint: true),
        new("Радионуклид", CompareColumnKind.Radionuclids, InFingerprint: true),
        new("Разрешённая активность", CompareColumnKind.Numeric),
        new("Фактическая активность", CompareColumnKind.Numeric)
    ];

    private static readonly CompareColumn[] Columns210 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Наименование показателя", CompareColumnKind.Text, InFingerprint: true),
        new("Наименование участка", CompareColumnKind.Text, InFingerprint: true),
        new("Кадастровый номер", CompareColumnKind.Id, InFingerprint: true),
        new("Код участка", CompareColumnKind.Code, InFingerprint: true),
        new("Площадь загрязнения", CompareColumnKind.Numeric),
        new("Средняя МЭД", CompareColumnKind.Numeric),
        new("Максимальная МЭД", CompareColumnKind.Numeric),
        new("Плотность загрязнения альфа", CompareColumnKind.Numeric),
        new("Плотность загрязнения бета", CompareColumnKind.Numeric),
        new("Номер ФЦП", CompareColumnKind.Id)
    ];

    private static readonly CompareColumn[] Columns211 =
    [
        new("№ п/п", CompareColumnKind.Quantity, IsRowNumber: true),
        new("Наименование участка", CompareColumnKind.Text, InFingerprint: true),
        new("Кадастровый номер", CompareColumnKind.Id, InFingerprint: true),
        new("Код участка", CompareColumnKind.Code, InFingerprint: true),
        new("Площадь загрязнения", CompareColumnKind.Numeric),
        new("Радионуклиды", CompareColumnKind.Radionuclids, InFingerprint: true),
        new("Уд. активность участка", CompareColumnKind.Numeric),
        new("Уд. активность жидкой фазы", CompareColumnKind.Numeric),
        new("Уд. активность твёрдой фазы", CompareColumnKind.Numeric)
    ];
}
