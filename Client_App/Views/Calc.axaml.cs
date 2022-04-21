using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Data;
using Models.Collections;
using System.Threading.Tasks;
using ReactiveUI;
using Client_App.Controls.DataGrid;
namespace Client_App.Views
{
    public class Calc : ReactiveWindow<ViewModels.CalcVM>
    {

        #region Contructures
        public Calc(ViewModels.CalcVM dataContext)
        {
            DataContext = dataContext;
            Init();
        }
        public Calc()
        {
            DataContext = new ViewModels.CalcVM();
            Init();
        }
        private void Init()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            //this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        #endregion

        #region ShowDialog
        private async Task DoShowDialogAsync(InteractionContext<ViewModels.CalcVM, object> interaction)
        {

            Calc frm = new Calc(interaction.Input);

            await frm.ShowDialog(this);
            interaction.SetOutput(null);
        }
        #endregion

        #region Events
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        #endregion

    }
}
