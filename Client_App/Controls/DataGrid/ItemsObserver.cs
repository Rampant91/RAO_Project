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
    public class ItemsObserver : IObserver<AvaloniaPropertyChangedEventArgs<IEnumerable<object>>>
    {
        private PropertyChangedEventHandler Handler { get; set; }
        public ItemsObserver(PropertyChangedEventHandler handler)
        {
            this.Handler = handler;
        }
        public void OnCompleted()
        {

        }
        public void OnError(Exception e)
        {

        }

        public void OnNext(AvaloniaPropertyChangedEventArgs<IEnumerable<object>> obj)
        {
            if(obj.NewValue.Value!=null)
            {
                if(Handler!=null)
                {
                    if (Handler.Target != null)
                    {
                        if (((Control)obj.Sender).Name == ((Control)Handler.Target).Name)
                        {
                            Handler(obj.NewValue.Value, null);
                        }
                    }
                }
            }
        }
    }
}
