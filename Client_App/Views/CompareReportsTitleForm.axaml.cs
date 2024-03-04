using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Models.Collections;

namespace Client_App.Views;

public class CompareReportsTitleForm : BaseWindow<CompareReportsTitleFormVM>
{
    private Report _repInBase = null!;
    private CompareReportsTitleFormVM _vm = null!;

    #region Constructor
    
    public CompareReportsTitleForm() { }

    public CompareReportsTitleForm(Report repInBase, Report repImport, List<(string, string)> repsWhereTitleFormCheckIsCancel)
    {
        InitializeComponent(repInBase, repImport, repsWhereTitleFormCheckIsCancel);
    }

    #endregion

    #region InitializeComponent
    
    private void InitializeComponent(Report repInBase, Report repImport, List<(string, string)> repsWhereTitleFormCheckIsCancel)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new CompareReportsTitleFormVM(repInBase, repImport, repsWhereTitleFormCheckIsCancel);
        _vm = (DataContext as CompareReportsTitleFormVM)!;
        _repInBase = repInBase;
    }

    #endregion

    #region OnClosing
    
    protected override void OnClosing(CancelEventArgs e)
    {
        if (_vm.Replace is false || _vm.RepsWhereTitleFormCheckIsCancel.Contains((_vm.RegNo, _vm.Okpo)))
        {
            base.OnClosing(e);
            return;
        }
        switch (_repInBase.FormNum_DB)
        {
            case "1.0":

                #region BindData

                _repInBase.Rows10[0].OrganUprav_DB = _vm.OrganUprav;
                _repInBase.Rows10[0].SubjectRF_DB = _vm.SubjectRF0;
                _repInBase.Rows10[0].JurLico_DB = _vm.JurLico0;
                _repInBase.Rows10[0].ShortJurLico_DB = _vm.ShortJurLico0;
                _repInBase.Rows10[0].JurLicoAddress_DB = _vm.JurLicoAddress0;
                _repInBase.Rows10[0].JurLicoFactAddress_DB = _vm.JurLicoFactAddress0;
                _repInBase.Rows10[0].GradeFIO_DB = _vm.GradeFIO0;
                _repInBase.Rows10[0].Telephone_DB = _vm.Telephone0;
                _repInBase.Rows10[0].Fax_DB = _vm.Fax0;
                _repInBase.Rows10[0].Email_DB = _vm.Email0;
                _repInBase.Rows10[0].Okpo_DB = _vm.Okpo0;
                _repInBase.Rows10[0].Okved_DB = _vm.Okved0;
                _repInBase.Rows10[0].Okogu_DB = _vm.Okogu0;
                _repInBase.Rows10[0].Oktmo_DB = _vm.Oktmo0;
                _repInBase.Rows10[0].Inn_DB = _vm.Inn0;
                _repInBase.Rows10[0].Kpp_DB = _vm.Kpp0;
                _repInBase.Rows10[0].Okopf_DB = _vm.Okopf0;
                _repInBase.Rows10[0].Okfs_DB = _vm.Okfs0;

                _repInBase.Rows10[1].OrganUprav_DB = _vm.OrganUprav;
                _repInBase.Rows10[1].SubjectRF_DB = _vm.SubjectRF1;
                _repInBase.Rows10[1].JurLico_DB = _vm.JurLico1;
                _repInBase.Rows10[1].ShortJurLico_DB = _vm.ShortJurLico1;
                _repInBase.Rows10[1].JurLicoAddress_DB = _vm.JurLicoAddress1;
                _repInBase.Rows10[1].JurLicoFactAddress_DB = _vm.JurLicoFactAddress1;
                _repInBase.Rows10[1].GradeFIO_DB = _vm.GradeFIO1;
                _repInBase.Rows10[1].Telephone_DB = _vm.Telephone1;
                _repInBase.Rows10[1].Fax_DB = _vm.Fax1;
                _repInBase.Rows10[1].Email_DB = _vm.Email1;
                _repInBase.Rows10[1].Okpo_DB = _vm.Okpo1;
                _repInBase.Rows10[1].Okved_DB = _vm.Okved1;
                _repInBase.Rows10[1].Okogu_DB = _vm.Okogu1;
                _repInBase.Rows10[1].Oktmo_DB = _vm.Oktmo1;
                _repInBase.Rows10[1].Inn_DB = _vm.Inn1;
                _repInBase.Rows10[1].Kpp_DB = _vm.Kpp1;
                _repInBase.Rows10[1].Okopf_DB = _vm.Okopf1;
                _repInBase.Rows10[1].Okfs_DB = _vm.Okfs1;

                #endregion

                break;
            case "2.0":

                #region BindData

                _repInBase.Rows20[0].OrganUprav_DB = _vm.OrganUprav;
                _repInBase.Rows20[0].SubjectRF_DB = _vm.SubjectRF0;
                _repInBase.Rows20[0].JurLico_DB = _vm.JurLico0;
                _repInBase.Rows20[0].ShortJurLico_DB = _vm.ShortJurLico0;
                _repInBase.Rows20[0].JurLicoAddress_DB = _vm.JurLicoAddress0;
                _repInBase.Rows20[0].JurLicoFactAddress_DB = _vm.JurLicoFactAddress0;
                _repInBase.Rows20[0].GradeFIO_DB = _vm.GradeFIO0;
                _repInBase.Rows20[0].Telephone_DB = _vm.Telephone0;
                _repInBase.Rows20[0].Fax_DB = _vm.Fax0;
                _repInBase.Rows20[0].Email_DB = _vm.Email0;
                _repInBase.Rows20[0].Okpo_DB = _vm.Okpo0;
                _repInBase.Rows20[0].Okved_DB = _vm.Okved0;
                _repInBase.Rows20[0].Okogu_DB = _vm.Okogu0;
                _repInBase.Rows20[0].Oktmo_DB = _vm.Oktmo0;
                _repInBase.Rows20[0].Inn_DB = _vm.Inn0;
                _repInBase.Rows20[0].Kpp_DB = _vm.Kpp0;
                _repInBase.Rows20[0].Okopf_DB = _vm.Okopf0;
                _repInBase.Rows20[0].Okfs_DB = _vm.Okfs0;

                _repInBase.Rows20[1].OrganUprav_DB = _vm.OrganUprav;
                _repInBase.Rows20[1].SubjectRF_DB = _vm.SubjectRF1;
                _repInBase.Rows20[1].JurLico_DB = _vm.JurLico1;
                _repInBase.Rows20[1].ShortJurLico_DB = _vm.ShortJurLico1;
                _repInBase.Rows20[1].JurLicoAddress_DB = _vm.JurLicoAddress1;
                _repInBase.Rows20[1].JurLicoFactAddress_DB = _vm.JurLicoFactAddress1;
                _repInBase.Rows20[1].GradeFIO_DB = _vm.GradeFIO1;
                _repInBase.Rows20[1].Telephone_DB = _vm.Telephone1;
                _repInBase.Rows20[1].Fax_DB = _vm.Fax1;
                _repInBase.Rows20[1].Email_DB = _vm.Email1;
                _repInBase.Rows20[1].Okpo_DB = _vm.Okpo1;
                _repInBase.Rows20[1].Okved_DB = _vm.Okved1;
                _repInBase.Rows20[1].Okogu_DB = _vm.Okogu1;
                _repInBase.Rows20[1].Oktmo_DB = _vm.Oktmo1;
                _repInBase.Rows20[1].Inn_DB = _vm.Inn1;
                _repInBase.Rows20[1].Kpp_DB = _vm.Kpp1;
                _repInBase.Rows20[1].Okopf_DB = _vm.Okopf1;
                _repInBase.Rows20[1].Okfs_DB = _vm.Okfs1;

                #endregion

                break;
        }
        base.OnClosing(e);
    }

    #endregion

    #region Buttons

    #region ReplaceButtonClick
    
    private void ReplaceButtonClick(object? sender, RoutedEventArgs e)
    {
        _vm.Replace = true;
        Close();
    }

    #endregion

    #region CloseWithoutReplaceButtonClick
    
    private void CloseWithoutReplaceButtonClick(object? sender, RoutedEventArgs e)
    {
        _vm.Replace = false;
        Close();
    }

    #endregion

    #region CloseWithoutReplaceForThisRepsButtonClick
    
    private void CloseWithoutReplaceForThisRepsButtonClick(object? sender, RoutedEventArgs e)
    {
        _vm.Replace = false;
        _vm.RepsWhereTitleFormCheckIsCancel.Add((_vm.RegNo, _vm.Okpo));
        Close();
    }

    #endregion

    #region SelectAllButtonClick
    
    private void SelectAllButtonClick(object? sender, RoutedEventArgs e)
    {
        if (_vm is
            {
                ReplaceOrganUprav: true, ReplaceSubjectRF0: true, ReplaceJurLico0: true, ReplaceShortJurLico0: true,
                ReplaceJurLicoAddress0: true, ReplaceJurLicoFactAddress0: true, ReplaceGradeFIO0: true,
                ReplaceTelephone0: true, ReplaceFax0: true, ReplaceEmail0: true, ReplaceOkpo0: true,
                ReplaceOkved0: true, ReplaceOkogu0: true, ReplaceOktmo0: true, ReplaceInn0: true, ReplaceKpp0: true,
                ReplaceOkopf0: true, ReplaceOkfs0: true, ReplaceSubjectRF1: true, ReplaceJurLico1: true,
                ReplaceShortJurLico1: true, ReplaceJurLicoAddress1: true, ReplaceJurLicoFactAddress1: true,
                ReplaceGradeFIO1: true, ReplaceTelephone1: true, ReplaceFax1: true, ReplaceEmail1: true,
                ReplaceOkpo1: true, ReplaceOkved1: true, ReplaceOkogu1: true, ReplaceOktmo1: true,
                ReplaceInn1: true, ReplaceKpp1: true, ReplaceOkopf1: true, ReplaceOkfs1: true
            })
        {
            _vm.ReplaceOrganUprav = _vm.ReplaceSubjectRF0 = _vm.ReplaceJurLico0 = _vm.ReplaceShortJurLico0 =
                _vm.ReplaceJurLicoAddress0 = _vm.ReplaceJurLicoFactAddress0 = _vm.ReplaceGradeFIO0 =
                    _vm.ReplaceTelephone0 = _vm.ReplaceFax0 = _vm.ReplaceEmail0 = _vm.ReplaceOkpo0 =
                        _vm.ReplaceOkved0 = _vm.ReplaceOkogu0 = _vm.ReplaceOktmo0 = _vm.ReplaceInn0 =
                            _vm.ReplaceKpp0 = _vm.ReplaceOkopf0 = _vm.ReplaceOkfs0 = _vm.ReplaceSubjectRF1
                                = _vm.ReplaceJurLico1 = _vm.ReplaceShortJurLico1 = _vm.ReplaceJurLicoAddress1
                                    = _vm.ReplaceJurLicoFactAddress1 = _vm.ReplaceGradeFIO1 = _vm.ReplaceTelephone1
                                        = _vm.ReplaceFax1 = _vm.ReplaceEmail1 = _vm.ReplaceOkpo1 = _vm.ReplaceOkved1
                                            = _vm.ReplaceOkogu1 = _vm.ReplaceOktmo1 = _vm.ReplaceInn1
                                                = _vm.ReplaceKpp1 = _vm.ReplaceOkopf1 = _vm.ReplaceOkfs1 = false;
        }
        else
        {
            _vm.ReplaceOrganUprav = _vm.ReplaceSubjectRF0 = _vm.ReplaceJurLico0 = _vm.ReplaceShortJurLico0 =
                _vm.ReplaceJurLicoAddress0 = _vm.ReplaceJurLicoFactAddress0 = _vm.ReplaceGradeFIO0 =
                    _vm.ReplaceTelephone0 = _vm.ReplaceFax0 = _vm.ReplaceEmail0 = _vm.ReplaceOkpo0 =
                        _vm.ReplaceOkved0 = _vm.ReplaceOkogu0 = _vm.ReplaceOktmo0 = _vm.ReplaceInn0 =
                            _vm.ReplaceKpp0 = _vm.ReplaceOkopf0 = _vm.ReplaceOkfs0 = _vm.ReplaceSubjectRF1 =
                                _vm.ReplaceJurLico1 = _vm.ReplaceShortJurLico1 = _vm.ReplaceJurLicoAddress1
                                    = _vm.ReplaceJurLicoFactAddress1 = _vm.ReplaceGradeFIO1 = _vm.ReplaceTelephone1
                                        = _vm.ReplaceFax1 = _vm.ReplaceEmail1 = _vm.ReplaceOkpo1 = _vm.ReplaceOkved1
                                            = _vm.ReplaceOkogu1 = _vm.ReplaceOktmo1 = _vm.ReplaceInn1
                                                = _vm.ReplaceKpp1 = _vm.ReplaceOkopf1 = _vm.ReplaceOkfs1 = true;
        }
    }

    #endregion

    #endregion
}