using Avalonia.Controls;
using Client_App.ViewModels.Forms.Forms1.Items;
using Models.Forms;
using Models.JSON.TableDataMain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.ViewModels.Forms.Forms1.Providers.AutoCompleteProviders
{
    /// <summary>
    /// Необобщенный интерфейс провайдера для AutoCompleteBox
    /// Предоставляет доступ коллекции объектов и вызову операции записи выбранного элемента в форму,
    /// без привязки к типу элементов коллекции
    /// 
    /// Разработан для Client_App.Behaviors.Input.AutoCompleteBoxProviderBehavior
    /// </summary>
    public interface IAutoCompleteProvider
    {
        /// <summary>
        /// Нетипизированная коллекция элементов
        /// </summary>
        ObservableCollection<object> ItemsCollection { get; }

        /// <summary>
        /// Метод записывающий данные объекта в строчку таблицы отчета
        /// используется для вызова из тех областей, в которых не важен тип провайдера и записываемого объекта
        ///  
        /// </summary>
        /// <param name="form">Строчка из таблицы отчета</param>
        /// <param name="item">Объект хранящий данные для записи в строчку отчета</param>
        /// <returns></returns>
        Form WriteItemInForm(Form form, object item);
    }
    //Обобщенный класс
    //задает типизацию для базового функционала провайдера
    public abstract class AbstractAutoCompleteProvider<T> : IAutoCompleteProvider
    {
        ///Типизированная коллекция
        public abstract ObservableCollection<T> TypedItemsCollection { get; }

        // элементы из типизированной коллекции автоматически проецируются в унаследованную нетипизированную коллекцию
        public ObservableCollection<object> ItemsCollection => new(TypedItemsCollection.Cast<object>());

        /// <summary>
        /// Метод записывающий данные объекта в строчку таблицы отчета
        /// с учетом типа объекта
        /// </summary>
        /// <param name="form"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract Form WriteItemInForm(Form form, T item);

        //Унаследованный из интерфейса метод, в который задаем по умолчанию функцию записи с учетом типа объекта
        public Form WriteItemInForm(Form form, object item)
        {
            if (item is T typedItem)
                return WriteItemInForm(form, typedItem);
           
                throw new TypeAccessException();

        }


    }
}
