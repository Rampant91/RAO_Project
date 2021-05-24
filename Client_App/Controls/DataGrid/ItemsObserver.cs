using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Collections;
using System.Collections;
using System.Collections.ObjectModel;
using Avalonia.Collections;
using System.ComponentModel;

namespace Client_App.Controls.DataGrid
{
    public class ItemsObserver : IObserver<AvaloniaPropertyChangedEventArgs<IEnumerable>>
    {
        public Panel _ctrl { get; set; }
        private PropertyChangedEventHandler Handler{ get; set; }
        public ItemsObserver(Panel ctrl,PropertyChangedEventHandler handler)
        {
            this._ctrl = ctrl;
            this.Handler = handler;
        }
        public void OnCompleted()
        {

        }
        public void OnError(Exception e)
        {

        }

        public void OnNext(AvaloniaPropertyChangedEventArgs<IEnumerable> obj)
        {
            if(obj.NewValue.Value!=null)
            {
                var val = obj.NewValue.Value;
                foreach (var item in val)
                {
                    ((Report)item).PropertyChanged += Handler;
                }
                Handler("ALL",null);
            }
        }
    }
}
