using Avalonia.Data;
using Client_App.ViewModels.Forms.Forms1.Items;
using Models.Forms;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.ViewModels.Forms.Forms1.Providers.AutoCompleteProviders
{
    public class OksmProvider :AbstractAutoCompleteProvider<OksmItem>
    {
        public override ObservableCollection<OksmItem> TypedItemsCollection
        {
            get
            {
                return new(Spravochniks.OKSM.Select(oksmPair => new OksmItem()
                {
                    Code = oksmPair.Key,
                    Country = oksmPair.Value,
                }));
            }
        }
        public override Form WriteItemInForm(Form form, OksmItem item)
        {
            //Если в теле этого метода ничего не сделать, то при вызове из AutoCompleteBoxProviderBehavior сработает базовая логика AutoCompleteBox,
            //То есть в box запишется результат вызова item.ToString(); 
            //Поэтому в типе итема можно перезаписать ToString(), чтобы записать нужное нам значение
            return form;
        }
    }
}
