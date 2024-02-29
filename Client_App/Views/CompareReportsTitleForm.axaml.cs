using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Models.Collections;

namespace Client_App.Views;

public partial class CompareReportsTitleForm : BaseWindow<CompareReportsTitleFormVM>
{
    public Report NewMasterRep = new();
    public Report RepInBase = null!;
    private CompareReportsTitleFormVM _vm = null!;

    public CompareReportsTitleForm(){}

    public CompareReportsTitleForm(Report repInBase, Report repImport)
    {
        var desktop = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        InitializeComponent(repInBase, repImport);
        //ShowDialog(desktop?.MainWindow);
    }

    private void InitializeComponent(Report repInBase, Report repImport)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new CompareReportsTitleFormVM(repInBase, repImport);
        _vm = (DataContext as CompareReportsTitleFormVM)!;
        RepInBase = repInBase;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (_vm.Replace is false)
        {
            base.OnClosing(e);
            return;
        }

        switch (RepInBase.FormNum_DB)
        {
            case "1.0":
                RepInBase.Rows10[0].OrganUprav_DB = _vm.OrganUprav;
                RepInBase.Rows10[0].SubjectRF_DB = _vm.SubjectRF0;
                RepInBase.Rows10[0].JurLico_DB = _vm.JurLico0;
                RepInBase.Rows10[0].ShortJurLico_DB = _vm.ShortJurLico0;
                RepInBase.Rows10[0].JurLicoAddress_DB = _vm.JurLicoAddress0;
                RepInBase.Rows10[0].JurLicoFactAddress_DB = _vm.JurLicoFactAddress0;
                RepInBase.Rows10[0].GradeFIO_DB = _vm.GradeFIO0;
                RepInBase.Rows10[0].Telephone_DB = _vm.Telephone0;
                RepInBase.Rows10[0].Fax_DB = _vm.Fax0;
                RepInBase.Rows10[0].Email_DB = _vm.Email0;
                RepInBase.Rows10[0].Okpo_DB = _vm.Okpo0;
                RepInBase.Rows10[0].Okved_DB = _vm.Okved0;
                RepInBase.Rows10[0].Okogu_DB = _vm.Okogu0;
                RepInBase.Rows10[0].Oktmo_DB = _vm.Oktmo0;
                RepInBase.Rows10[0].Inn_DB = _vm.Inn0;
                RepInBase.Rows10[0].Kpp_DB = _vm.Kpp0;
                RepInBase.Rows10[0].Okopf_DB = _vm.Okopf0;
                RepInBase.Rows10[0].Okfs_DB = _vm.Okfs0;

                RepInBase.Rows10[1].OrganUprav_DB = _vm.OrganUprav;
                RepInBase.Rows10[1].SubjectRF_DB = _vm.SubjectRF1;
                RepInBase.Rows10[1].JurLico_DB = _vm.JurLico1;
                RepInBase.Rows10[1].ShortJurLico_DB = _vm.ShortJurLico1;
                RepInBase.Rows10[1].JurLicoAddress_DB = _vm.JurLicoAddress1;
                RepInBase.Rows10[1].JurLicoFactAddress_DB = _vm.JurLicoFactAddress1;
                RepInBase.Rows10[1].GradeFIO_DB = _vm.GradeFIO1;
                RepInBase.Rows10[1].Telephone_DB = _vm.Telephone1;
                RepInBase.Rows10[1].Fax_DB = _vm.Fax1;
                RepInBase.Rows10[1].Email_DB = _vm.Email1;
                RepInBase.Rows10[1].Okpo_DB = _vm.Okpo1;
                RepInBase.Rows10[1].Okved_DB = _vm.Okved1;
                RepInBase.Rows10[1].Okogu_DB = _vm.Okogu1;
                RepInBase.Rows10[1].Oktmo_DB = _vm.Oktmo1;
                RepInBase.Rows10[1].Inn_DB = _vm.Inn1;
                RepInBase.Rows10[1].Kpp_DB = _vm.Kpp1;
                RepInBase.Rows10[1].Okopf_DB = _vm.Okopf1;
                RepInBase.Rows10[1].Okfs_DB = _vm.Okfs1;
                break;
            case "2.0":
                RepInBase.Rows20[0].OrganUprav_DB = _vm.OrganUprav;
                RepInBase.Rows20[0].SubjectRF_DB = _vm.SubjectRF0;
                RepInBase.Rows20[0].JurLico_DB = _vm.JurLico0;
                RepInBase.Rows20[0].ShortJurLico_DB = _vm.ShortJurLico0;
                RepInBase.Rows20[0].JurLicoAddress_DB = _vm.JurLicoAddress0;
                RepInBase.Rows20[0].JurLicoFactAddress_DB = _vm.JurLicoFactAddress0;
                RepInBase.Rows20[0].GradeFIO_DB = _vm.GradeFIO0;
                RepInBase.Rows20[0].Telephone_DB = _vm.Telephone0;
                RepInBase.Rows20[0].Fax_DB = _vm.Fax0;
                RepInBase.Rows20[0].Email_DB = _vm.Email0;
                RepInBase.Rows20[0].Okpo_DB = _vm.Okpo0;
                RepInBase.Rows20[0].Okved_DB = _vm.Okved0;
                RepInBase.Rows20[0].Okogu_DB = _vm.Okogu0;
                RepInBase.Rows20[0].Oktmo_DB = _vm.Oktmo0;
                RepInBase.Rows20[0].Inn_DB = _vm.Inn0;
                RepInBase.Rows20[0].Kpp_DB = _vm.Kpp0;
                RepInBase.Rows20[0].Okopf_DB = _vm.Okopf0;
                RepInBase.Rows20[0].Okfs_DB = _vm.Okfs0;

                RepInBase.Rows20[1].OrganUprav_DB = _vm.OrganUprav;
                RepInBase.Rows20[1].SubjectRF_DB = _vm.SubjectRF1;
                RepInBase.Rows20[1].JurLico_DB = _vm.JurLico1;
                RepInBase.Rows20[1].ShortJurLico_DB = _vm.ShortJurLico1;
                RepInBase.Rows20[1].JurLicoAddress_DB = _vm.JurLicoAddress1;
                RepInBase.Rows20[1].JurLicoFactAddress_DB = _vm.JurLicoFactAddress1;
                RepInBase.Rows20[1].GradeFIO_DB = _vm.GradeFIO1;
                RepInBase.Rows20[1].Telephone_DB = _vm.Telephone1;
                RepInBase.Rows20[1].Fax_DB = _vm.Fax1;
                RepInBase.Rows20[1].Email_DB = _vm.Email1;
                RepInBase.Rows20[1].Okpo_DB = _vm.Okpo1;
                RepInBase.Rows20[1].Okved_DB = _vm.Okved1;
                RepInBase.Rows20[1].Okogu_DB = _vm.Okogu1;
                RepInBase.Rows20[1].Oktmo_DB = _vm.Oktmo1;
                RepInBase.Rows20[1].Inn_DB = _vm.Inn1;
                RepInBase.Rows20[1].Kpp_DB = _vm.Kpp1;
                RepInBase.Rows20[1].Okopf_DB = _vm.Okopf1;
                RepInBase.Rows20[1].Okfs_DB = _vm.Okfs1;
                break;
        }

        base.OnClosing(e);
    }

    private void ReplaceButtonClick(object? sender, RoutedEventArgs e)
    {
        _vm.Replace = true;
        Close();
    }

    private void CloseWithoutReplaceButtonClick(object? sender, RoutedEventArgs e)
    {
        _vm.Replace = false;
        Close();
    }

    private void SelectAllButtonClick(object? sender, RoutedEventArgs e)
    {
        if (_vm is
            {
                ReplaceOrganUprav:true,
                ReplaceSubjectRF0: true, ReplaceJurLico0: true, ReplaceShortJurLico0: true,
                ReplaceJurLicoAddress0: true, ReplaceJurLicoFactAddress0: true, ReplaceGradeFIO0: true,
                ReplaceTelephone0: true, ReplaceFax0: true, ReplaceEmail0: true, ReplaceOkpo0: true,
                ReplaceOkved0: true, ReplaceOkogu0: true, ReplaceOktmo0: true, ReplaceInn0: true, ReplaceKpp0: true,
                ReplaceOkopf0: true, ReplaceOkfs0: true,
                ReplaceSubjectRF1: true, ReplaceJurLico1: true, ReplaceShortJurLico1: true,
                ReplaceJurLicoAddress1: true, ReplaceJurLicoFactAddress1: true, ReplaceGradeFIO1: true,
                ReplaceTelephone1: true, ReplaceFax1: true, ReplaceEmail1: true, ReplaceOkpo1: true,
                ReplaceOkved1: true, ReplaceOkogu1: true, ReplaceOktmo1: true, ReplaceInn1: true, ReplaceKpp1: true,
                ReplaceOkopf1: true, ReplaceOkfs1: true
            })
        {
            _vm.ReplaceOrganUprav = false;
            _vm.ReplaceSubjectRF0 = _vm.ReplaceJurLico0 = _vm.ReplaceShortJurLico0 = _vm.ReplaceJurLicoAddress0 =
                _vm.ReplaceJurLicoFactAddress0 = _vm.ReplaceGradeFIO0 = _vm.ReplaceTelephone0 = _vm.ReplaceFax0 =
                    _vm.ReplaceEmail0 = _vm.ReplaceOkpo0 = _vm.ReplaceOkved0 = _vm.ReplaceOkogu0 = _vm.ReplaceOktmo0 =
                        _vm.ReplaceInn0 = _vm.ReplaceKpp0 = _vm.ReplaceOkopf0 = _vm.ReplaceOkfs0 = false;
            _vm.ReplaceSubjectRF1 = _vm.ReplaceJurLico1 = _vm.ReplaceShortJurLico1 = _vm.ReplaceJurLicoAddress1 =
                _vm.ReplaceJurLicoFactAddress1 = _vm.ReplaceGradeFIO1 = _vm.ReplaceTelephone1 = _vm.ReplaceFax1 =
                    _vm.ReplaceEmail1 = _vm.ReplaceOkpo1 = _vm.ReplaceOkved1 = _vm.ReplaceOkogu1 = _vm.ReplaceOktmo1 =
                        _vm.ReplaceInn1 = _vm.ReplaceKpp1 = _vm.ReplaceOkopf1 = _vm.ReplaceOkfs1 = false;
        }
        else
        {
            _vm.ReplaceOrganUprav = true;
            _vm.ReplaceSubjectRF0 = _vm.ReplaceJurLico0 = _vm.ReplaceShortJurLico0 = _vm.ReplaceJurLicoAddress0 =
                _vm.ReplaceJurLicoFactAddress0 = _vm.ReplaceGradeFIO0 = _vm.ReplaceTelephone0 = _vm.ReplaceFax0 =
                    _vm.ReplaceEmail0 = _vm.ReplaceOkpo0 = _vm.ReplaceOkved0 = _vm.ReplaceOkogu0 = _vm.ReplaceOktmo0 =
                        _vm.ReplaceInn0 = _vm.ReplaceKpp0 = _vm.ReplaceOkopf0 = _vm.ReplaceOkfs0 = true;
            _vm.ReplaceSubjectRF1 = _vm.ReplaceJurLico1 = _vm.ReplaceShortJurLico1 = _vm.ReplaceJurLicoAddress1 =
                _vm.ReplaceJurLicoFactAddress1 = _vm.ReplaceGradeFIO1 = _vm.ReplaceTelephone1 = _vm.ReplaceFax1 =
                    _vm.ReplaceEmail1 = _vm.ReplaceOkpo1 = _vm.ReplaceOkved1 = _vm.ReplaceOkogu1 = _vm.ReplaceOktmo1 =
                        _vm.ReplaceInn1 = _vm.ReplaceKpp1 = _vm.ReplaceOkopf1 = _vm.ReplaceOkfs1 = true;
        }
    }
}