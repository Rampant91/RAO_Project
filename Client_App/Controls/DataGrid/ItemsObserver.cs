using Avalonia;
using Avalonia.Controls;
using Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Client_App.Controls.DataGrid
{
    public class ItemsObserver : IObserver<AvaloniaPropertyChangedEventArgs<IEnumerable<IChanged>>>
    {
        private PropertyChangedEventHandler Handler { get; set; }
        public ItemsObserver(PropertyChangedEventHandler handler)
        {
            Handler = handler;
        }
        public void OnCompleted()
        {

        }
        public void OnError(Exception e)
        {

        }

        public void OnNext(AvaloniaPropertyChangedEventArgs<IEnumerable<IChanged>> obj)
        {
            if (obj.NewValue.Value != null)
            {
                if (Handler != null)
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
