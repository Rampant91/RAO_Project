using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers.AutoCompleteProviders;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.ViewModels.Forms.Forms1.Providers.AutoCompleteProviders
{
    public class UktProvider : AbstractAutoCompleteProvider<UktItem>
    {
        
        #region SpravochnikUktForForm17 
        private ObservableCollection<UktItem> SpravochnikUktForForm17 { get; } =
            [new(){ Name = "Контейнер", Type = "А.2201.00.000", Volume  = 0.2, Mass =0.078 },
new(){ Name = "Контейнер", Type = "А.108.00.000", Volume  = 0.25, Mass =null },
new(){ Name = "Контейнер", Type = "А.2202.00.000", Volume  = 0.25, Mass =null },
new(){ Name = "Контейнер", Type = "НЗК-150-1,5П (ИОС)", Volume  = 3.75, Mass =4.6 },
new(){ Name = "Контейнер", Type = "НЗК-I", Volume  = 3.74, Mass =5.2 },
new(){ Name = "Контейнер", Type = "НЗК-II", Volume  = 3.74, Mass =7.2 },
new(){ Name = "Контейнер", Type = "НЗК-III", Volume  = 3.65, Mass =13.6 },
new(){ Name = "Контейнер", Type = "НЗК-МР", Volume  = 3.65, Mass =null },
new(){ Name = "Контейнер", Type = "НЗК-МР1", Volume  = 3.65, Mass =6.5 },
new(){ Name = "Контейнер", Type = "НЗК-МР2", Volume  = 3.65, Mass =6.5 },
new(){ Name = "Контейнер", Type = "НЗК МР-150-1 ", Volume  = 3.75, Mass =7.8 },
new(){ Name = "Контейнер", Type = "НЗК МР-150-2(ИОС) ", Volume  = 3.75, Mass =7.8 },
new(){ Name = "Контейнер", Type = "НЗК МР-150К", Volume  = 3.7, Mass =null },
new(){ Name = "Контейнер", Type = "НЗК-150-1,5П", Volume  = 3.74, Mass =4.3 },
new(){ Name = "Контейнер", Type = "НЗК-РАДОН", Volume  = 3.74, Mass =4 },
new(){ Name = "Контейнер", Type = "ЗККЭ", Volume  = 12.54, Mass =62 },
new(){ Name = "Контейнер", Type = "ЗКПН", Volume  = 11.65, Mass =55 },
new(){ Name = "Контейнер", Type = "КМЗ", Volume  = 3.74, Mass =1.16 },
new(){ Name = "Контейнер", Type = "КМЗ-3,3", Volume  = 4.1, Mass =null },
new(){ Name = "Контейнер", Type = "КМЗ-М", Volume  = 3.743, Mass =null },
new(){ Name = "Контейнер", Type = "КМЗ МК-3,1А", Volume  = 3.74, Mass =null },
new(){ Name = "Контейнер", Type = "КМЗ-РАДОН", Volume  = 3.835, Mass =1.035 },
new(){ Name = "Контейнер", Type = "КМЗ-РНИ-РАДОН", Volume  = 3.835, Mass =null },
new(){ Name = "Контейнер", Type = "КМ-200", Volume  = 0.26, Mass =null },
new(){ Name = "Контейнер", Type = "КМРАО-2,8", Volume  = 3.3, Mass =null },
new(){ Name = "Контейнер", Type = "ЖЗК-2", Volume  = 4.1, Mass =8.7 },
new(){ Name = "Контейнер", Type = "3080.01.00.000", Volume  = 1.75, Mass =null },
new(){ Name = "Контейнер", Type = "Л.11.222.00.000", Volume  = 0.25, Mass =0.5 },
new(){ Name = "Контейнер", Type = "СКА.0546", Volume  = 2, Mass =null },
new(){ Name = "Контейнер", Type = "К-100", Volume  = null, Mass =null },
new(){ Name = "Контейнер", Type = "К-150", Volume  = null, Mass =null },
new(){ Name = "Контейнер", Type = "К-60", Volume  = null, Mass =null },
new(){ Name = "Контейнер", Type = "КТО-800", Volume  = 1.27, Mass =null },
new(){ Name = "Контейнер", Type = "САО", Volume  = 1.17, Mass =2.25 },
new(){ Name = "Контейнер", Type = "САО  АМЕ.704", Volume  = 1.004, Mass =2.468 },
new(){ Name = "Контейнер", Type = "СМ1546К.17.00.00.00.00", Volume  = 0.42, Mass =null },
new(){ Name = "Контейнер", Type = "СМ1546К.18.00.00.00.00", Volume  = 0.54, Mass =null },
new(){ Name = "Контейнер", Type = "СМ1546К.19.00.00.00.00", Volume  = 0.62, Mass =null },
new(){ Name = "Контейнер", Type = "СМ1546К.10.00.00.00.00", Volume  = 0.432, Mass =null },
new(){ Name = "Контейнер", Type = "ГЕК-6", Volume  = 1.6, Mass =null },
new(){ Name = "Контейнер", Type = "ГЕК-40", Volume  = 1.6, Mass =null },
new(){ Name = "Контейнер", Type = "ГЕК-80", Volume  = 1.6, Mass =null },
new(){ Name = "Контейнер", Type = "КРАД-1,3", Volume  = 1.375, Mass =null },
new(){ Name = "Контейнер", Type = "КРАД-1,36 (аналог: МК-1,36)", Volume  = 1.475, Mass =0.232 },
new(){ Name = "Контейнер", Type = "МК-1,36А", Volume  = 1.56, Mass =0.243 },
new(){ Name = "Контейнер", Type = "КРАД-1,36 с опорами", Volume  = null, Mass =null },
new(){ Name = "Контейнер", Type = "КРАД-1,36 Р-149.13-00", Volume  = 1.6, Mass =null },
new(){ Name = "Контейнер", Type = "КРАД-2,7", Volume  = 3.1, Mass =null },
new(){ Name = "Контейнер", Type = "КРАД-3,0", Volume  = 4.05, Mass =6 },
new(){ Name = "Контейнер", Type = "КРАД-3,0 МК-3,0", Volume  = 3.74, Mass =null },
new(){ Name = "Контейнер", Type = "МЗК-3.0", Volume  = 4.11, Mass =null },
new(){ Name = "Контейнер", Type = "ЗМК-3.0Ц", Volume  = 4.25, Mass =null },
new(){ Name = "Контейнер", Type = "УКТ1А-150-1,5/4 КЗ", Volume  = 4, Mass =null },
new(){ Name = "Контейнер", Type = "УКТ1АЯНМИ.305179.012.ТУ", Volume  = 4, Mass =null },
new(){ Name = "Контейнер", Type = "УКТН-24000", Volume  = 38.27, Mass =null },
new(){ Name = "Контейнер", Type = "УКТ-1А ЭЦ", Volume  = 0.17, Mass =null },
new(){ Name = "Фильтр-контейнер", Type = "Фильтр-контейнер", Volume  = 0.75, Mass =null },
new(){ Name = "Контейнер оборотный", Type = "КО-2", Volume  = null, Mass =null },
new(){ Name = "Капсула", Type = "СКА 0517.03.00.000", Volume  = 0.05, Mass =0.075 },
new(){ Name = "Капсула", Type = "205.15.020 (КНИТ)", Volume  = 0.5, Mass =0.08 },
new(){ Name = "Бочка", Type = "057.1.2000.00.00.00", Volume  = 0.25, Mass =0.6 },
new(){ Name = "Бочка", Type = "А110.000", Volume  = 0.2, Mass =0.35 },
new(){ Name = "Бочка", Type = "А.00.617.000", Volume  = 0.23, Mass =0.5 },
new(){ Name = "Бочка", Type = "А.11.1107.000", Volume  = 0.2, Mass =0.5 },
new(){ Name = "Бочка", Type = "металлическая 200 л", Volume  = 0.25, Mass =null },
new(){ Name = "Бочка", Type = "200 л", Volume  = 0.32, Mass =null },
new(){ Name = "Бочка", Type = "200 л", Volume  = 0.21, Mass =null },
new(){ Name = "Бочка", Type = "200 л ", Volume  = 0.2, Mass =null },
new(){ Name = "Бочка", Type = "200 л", Volume  = 0.22, Mass =null },
new(){ Name = "Бочка", Type = "200 л", Volume  = 0.237, Mass =null },
new(){ Name = "Бочка", Type = "металлическая", Volume  = 0.216, Mass =0.6 },
new(){ Name = "Бочка", Type = "металлическая ", Volume  = 0.5, Mass =null },
new(){ Name = "Бочка", Type = "А.00.617.000", Volume  = null, Mass =null },
new(){ Name = "Бочка", Type = "А.00.659.000", Volume  = null, Mass =null },
new(){ Name = "Бочка", Type = "А.00.884.000", Volume  = null, Mass =null },
new(){ Name = "Бочка", Type = "АФИБ", Volume  = null, Mass =null },
new(){ Name = "Бочка", Type = "Б31А2-216,5", Volume  = null, Mass =null },
new(){ Name = "Бочка", Type = "БЗ 1А2", Volume  = null, Mass =null },
new(){ Name = "Бочка", Type = "БЗ 1А2-100", Volume  = null, Mass =null },
new(){ Name = "Бочка", Type = "БЗ 1А2-200", Volume  = null, Mass =null },
new(){ Name = "Бочка", Type = "БЗII-200", Volume  = null, Mass =null },
new(){ Name = "Сборник", Type = "1194.00.00.00.00", Volume  = 3, Mass =5 },
new(){ Name = "Сборник", Type = "2097.00.00.00.00", Volume  = 2.4, Mass =6 },
new(){ Name = "Клеть для бочек", Type = "Клеть для бочек", Volume  = 1.6, Mass =null },
new(){ Name = "ЖЗК", Type = "ЖЗК", Volume  = 2.088, Mass =null },
new(){ Name = "ЖЗК-1", Type = "ЖЗК-1", Volume  = 4.1, Mass =7.8 },
new(){ Name = "ЖЗК-2", Type = "ЖЗК-2", Volume  = 4.1, Mass =8.7 },
new(){ Name = "ЖБУ", Type = "ЖБУ", Volume  = 2.06, Mass =4.5 },
new(){ Name = "Пенал", Type = "пенал для остеклованных ВАО", Volume  = 1.15, Mass =null },
new(){ Name = "Пенал", Type = "КПЦ-500 ", Volume  = null, Mass =null },
new(){ Name = "Пенал", Type = "Д515МА", Volume  = null, Mass =null },
new(){ Name = "Пенал", Type = "Скат", Volume  = null, Mass =null },
new(){ Name = "Пенал", Type = "УК1", Volume  = null, Mass =null },
new(){ Name = "СКС-60", Type = "СКС-60", Volume  = 3.7, Mass =null },
new(){ Name = "ЧУК-40", Type = "ЧУК-40", Volume  = 3.7, Mass =null },
new(){ Name = "ЧУК-80", Type = "ЧУК-80", Volume  = 3.7, Mass =null },
new(){ Name = "ЧУК-120", Type = "ЧУК-120", Volume  = 3.7, Mass =null },
new(){ Name = "Лифт Пак", Type = "Пак 1,3", Volume  = 1.4, Mass =null },
new(){ Name = "Лифт Пак", Type = "Пак 3,0", Volume  = 3.1, Mass =null },
new(){ Name = "Лифт Пак", Type = "Пак 3,5", Volume  = 3.7, Mass =null },
new(){ Name = "Биг-бэг", Type = "мешок", Volume  = 4.5, Mass =4.5 },
new(){ Name = "Емкость", Type = "0TW10B", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "0TW20B", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "0TW30B", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "ЕВС", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "ЕКО", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "ЕНС", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "ОТВ", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "ТУК-44", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "ТУК44/8", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "ТУК-44/8", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "ТУК-44/9", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "монжус", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "АД-6704", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "АТ-18001", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "АТ-18001/1", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "АТ-18040", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "АТ-5701", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "АТ-5701А", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "6-2348.550", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "№6-2348.550", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "Ф 45.68.1412.000", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "Ф 45.68.1413.000.СБ", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "Ф45.45.324.000", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "Ф45.68.1412.000", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "Ф45.68.1412.000-01", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "Ф45.68.1413.000", Volume  = null, Mass =null },
new(){ Name = "Емкость", Type = "Ф45.68.653.500", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Т-26М", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Т-25М", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Т-26", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Т-34", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Т-34М", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Т-35", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Т-35М", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Т-36М", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "1", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Т-26А", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Чехол Т-26А в ТК-12", Volume  = null, Mass =null },
new(){ Name = "Чехол", Type = "Чехол Т-26М в ТК-12", Volume  = null, Mass =null },
new(){ Name = "Ампула", Type = "III-А-Т-40", Volume  = null, Mass =null },
new(){ Name = "Ампула", Type = "НСУ", Volume  = null, Mass =null },
new(){ Name = "Ампула", Type = "НС-1", Volume  = null, Mass =null },
new(){ Name = "Ампула", Type = "пирекс", Volume  = null, Mass =null },
new(){ Name = "Ампула", Type = "КВ-90-100", Volume  = null, Mass =null },
new(){ Name = "Ампула", Type = "КИЗ-24", Volume  = null, Mass =null },
new(){ Name = "(Авто)цистерна", Type = "АРС", Volume  = null, Mass =null },
new(){ Name = "(Авто)цистерна", Type = "ГОСТ Р 509196", Volume  = null, Mass =null },
new(){ Name = "(Авто)цистерна", Type = "КО 505А", Volume  = null, Mass =null },
new(){ Name = "(Авто)цистерна", Type = "ОЖ-10", Volume  = null, Mass =null },
new(){ Name = "(Спец) цистерна", Type = "610.16-Б5-0971.00", Volume  = null, Mass =null },
new(){ Name = "(Спец) цистерна", Type = "71.Б5-782.00СБ", Volume  = null, Mass =null },
new(){ Name = "(Спец) цистерна", Type = "ч.610.16-Б5-0971.00", Volume  = null, Mass =null },
new(){ Name = "(Спец) цистерна", Type = "ч.610.16-Б5-1043.00", Volume  = null, Mass =null },
new(){ Name = "(Спец) цистерна", Type = "ч.71.Б5-782.00СБ", Volume  = null, Mass =null },
new(){ Name = "Бадья", Type = "Ф156.Л.95.147.130СБ  ", Volume  = null, Mass =null },
new(){ Name = "Бак", Type = "TZ 05В", Volume  = null, Mass =null },
new(){ Name = "Бак", Type = "TZ 05В 01,02", Volume  = null, Mass =null },
new(){ Name = "Бак", Type = "БА-Б", Volume  = null, Mass =null },
new(){ Name = "Бак", Type = "1H2", Volume  = null, Mass =null },
new(){ Name = "Бак", Type = "БКО-А", Volume  = null, Mass =null },
new(){ Name = "Бак", Type = "БКО-Б", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "4D", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "КТО-50", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "УСП", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "4С1", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "НСТ", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "МТ14-07", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "4А2", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "4С1", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "4С2", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "4G", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "4D", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = "4Н2", Volume  = null, Mass =null },
new(){ Name = "Ящик", Type = " 4С1", Volume  = null, Mass =null },
new(){ Name = "Стакан-тара", Type = "Ф156.10.018.000СБ", Volume  = null, Mass =null },
new(){ Name = "Стакан-тара", Type = "Ф156.10.018.000СБ", Volume  = null, Mass =null },
new(){ Name = "Стакан-тара", Type = "Ф156.65.240.000СБ", Volume  = null, Mass =null },
new(){ Name = "Стакан-тара", Type = "Ф156.Л.95.147.130СБ", Volume  = null, Mass =null },
new(){ Name = "Мешок", Type = "5М1", Volume  = null, Mass =null },
new(){ Name = "Мешок", Type = "5Н4", Volume  = null, Mass =null },
new(){ Name = "Мешок", Type = "5Н3", Volume  = null, Mass =null },
new(){ Name = "Мешок", Type = "5Н1", Volume  = null, Mass =null },
new(){ Name = "Мешок", Type = " I-1 (марка HM)", Volume  = null, Mass =null },
            ];
        #endregion


        public override ObservableCollection<UktItem> TypedItemsCollection
        {
            get
            {
                return SpravochnikUktForForm17;
            }
        }


        public override Form WriteItemInForm(Form form, UktItem item)
        {
            if (form is Form17 form17)
                return WriteItemInForm17(form17, item);


            throw new Exception($"UktProvider не поддерживает работу с формами {form.FormNum_DB}");
        }
        private Form17 WriteItemInForm17(Form17 form17, UktItem item)
        {
            #region Write
            form17.PackName.Value = item.Name;
            form17.PackType.Value = item.Type;
            form17.Volume.Value = item.Volume?.ToString() ?? "";

            var mass = item.Mass ?? 0;

            if (double.TryParse(form17.MassOutOfPack_DB, out var massOutOfPack))
                mass += massOutOfPack;

            form17.Mass.Value = mass != 0 
                ? mass.ToString() 
                : "";
            #endregion
            return form17;
        }
    }
}
