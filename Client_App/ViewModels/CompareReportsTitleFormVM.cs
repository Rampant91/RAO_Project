using System.ComponentModel;
using System.Runtime.CompilerServices;
using Models.Collections;

namespace Client_App.ViewModels;

public class CompareReportsTitleFormVM : INotifyPropertyChanged
{
    private readonly Report _repInBase = null!;
    internal bool Replace = false;

    #region Properties

    #region Title
    
    public string Title
    {
        get
        {
            var okpo = string.IsNullOrEmpty(Okpo0) ? Okpo0 : Okpo1;
            return $"Сравнение титульных форм {_repInBase.FormNum_DB} организации {RegNo}_{okpo}";
        }
    }

    #endregion

    #region RegNo

    public string RegNo { get; set; } = null!; 
    
    #endregion

    #region OrganUprav

    public string OrganUprav => ReplaceOrganUprav ? NewOrganUprav : OldOrganUprav;

    private bool _replaceOrganUprav = true;
    public bool ReplaceOrganUprav
    {
        get => _replaceOrganUprav;
        set
        {
            if (_replaceOrganUprav == value) return;
            _replaceOrganUprav = value;
            OnPropertyChanged();
        }
    }

    private string _newOrganUprav = null!;
    public string NewOrganUprav
    {
        get => _newOrganUprav;
        set
        {
            if (_newOrganUprav == value) return;
            _newOrganUprav = value;
            OnPropertyChanged();
        }
    }

    public string OldOrganUprav
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => !string.IsNullOrEmpty(_repInBase.Rows10[0].OrganUprav_DB)
                    ? _repInBase.Rows10[0].OrganUprav_DB
                    : !string.IsNullOrEmpty(_repInBase.Rows10[1].OrganUprav_DB)
                        ? _repInBase.Rows10[1].OrganUprav_DB
                        : string.Empty,
                "2.0" => !string.IsNullOrEmpty(_repInBase.Rows20[0].OrganUprav_DB)
                    ? _repInBase.Rows20[0].OrganUprav_DB
                    : !string.IsNullOrEmpty(_repInBase.Rows20[1].OrganUprav_DB)
                        ? _repInBase.Rows20[1].OrganUprav_DB
                        : string.Empty,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region JurLico

    #region SubjectRF0

    public string SubjectRF0 => ReplaceSubjectRF0 ? NewSubjectRF0 : OldSubjectRF0;

    private bool _replaceSubjectRF0 = true;
    public bool ReplaceSubjectRF0
    {
        get => _replaceSubjectRF0;
        set
        {
            if (_replaceSubjectRF0 == value) return;
            _replaceSubjectRF0 = value;
            OnPropertyChanged();
        }
    }

    private string _newSubjectRF0 = null!;
    public string NewSubjectRF0
    {
        get => _newSubjectRF0;
        set
        {
            if (_newSubjectRF0 == value) return;
            _newSubjectRF0 = value;
            OnPropertyChanged();
        }
    }

    public string OldSubjectRF0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].SubjectRF_DB,
                "2.0" => _repInBase.Rows20[0].SubjectRF_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region JurLico0

    public string JurLico0 => ReplaceJurLico0 ? NewJurLico0 : OldJurLico0;

    private bool _replaceJurLico0 = true;
    public bool ReplaceJurLico0
    {
        get => _replaceJurLico0;
        set
        {
            if (_replaceJurLico0 == value) return;
            _replaceJurLico0 = value;
            OnPropertyChanged();
        }
    }

    private string _newJurLico0 = null!;
    public string NewJurLico0
    {
        get => _newJurLico0;
        set
        {
            if (_newJurLico0 == value) return;
            _newJurLico0 = value;
            OnPropertyChanged();
        }
    }

    public string OldJurLico0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].JurLico_DB,
                "2.0" => _repInBase.Rows20[0].JurLico_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region ShortJurLico0

    public string ShortJurLico0 => ReplaceShortJurLico0 ? NewShortJurLico0 : OldShortJurLico0;

    private bool _replaceShortJurLico0 = true;
    public bool ReplaceShortJurLico0
    {
        get => _replaceShortJurLico0;
        set
        {
            if (_replaceShortJurLico0 == value) return;
            _replaceShortJurLico0 = value;
            OnPropertyChanged();
        }
    }

    private string _newShortJurLico0 = null!;
    public string NewShortJurLico0
    {
        get => _newShortJurLico0;
        set
        {
            if (_newShortJurLico0 == value) return;
            _newShortJurLico0 = value;
            OnPropertyChanged();
        }
    }

    public string OldShortJurLico0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].ShortJurLico_DB,
                "2.0" => _repInBase.Rows20[0].ShortJurLico_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region JurLicoAddress0

    public string JurLicoAddress0 => ReplaceJurLicoAddress0 ? NewJurLicoAddress0 : OldJurLicoAddress0;

    private bool _replaceJurLicoAddress0 = true;
    public bool ReplaceJurLicoAddress0
    {
        get => _replaceJurLicoAddress0;
        set
        {
            if (_replaceJurLicoAddress0 == value) return;
            _replaceJurLicoAddress0 = value;
            OnPropertyChanged();
        }
    }

    private string _newJurLicoAddress0 = null!;
    public string NewJurLicoAddress0
    {
        get => _newJurLicoAddress0;
        set
        {
            if (_newJurLicoAddress0 == value) return;
            _newJurLicoAddress0 = value;
            OnPropertyChanged();
        }
    }

    public string OldJurLicoAddress0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].JurLicoAddress_DB,
                "2.0" => _repInBase.Rows20[0].JurLicoAddress_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region JurLicoFactAddress0

    public string JurLicoFactAddress0 => ReplaceJurLicoFactAddress0 ? NewJurLicoFactAddress0 : OldJurLicoFactAddress0;

    private bool _replaceJurLicoFactAddress0 = true;
    public bool ReplaceJurLicoFactAddress0
    {
        get => _replaceJurLicoFactAddress0;
        set
        {
            if (_replaceJurLicoFactAddress0 == value) return;
            _replaceJurLicoFactAddress0 = value;
            OnPropertyChanged();
        }
    }

    private string _newJurLicoFactAddress0 = null!;
    public string NewJurLicoFactAddress0
    {
        get => _newJurLicoFactAddress0;
        set
        {
            if (_newJurLicoFactAddress0 == value) return;
            _newJurLicoFactAddress0 = value;
            OnPropertyChanged();
        }
    }

    public string OldJurLicoFactAddress0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].JurLicoFactAddress_DB,
                "2.0" => _repInBase.Rows20[0].JurLicoFactAddress_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region GradeFIO0

    public string GradeFIO0 => ReplaceGradeFIO0 ? NewGradeFIO0 : OldGradeFIO0;

    private bool _replaceGradeFIO0 = true;
    public bool ReplaceGradeFIO0
    {
        get => _replaceGradeFIO0;
        set
        {
            if (_replaceGradeFIO0 == value) return;
            _replaceGradeFIO0 = value;
            OnPropertyChanged();
        }
    }

    private string _newGradeFIO0 = null!;
    public string NewGradeFIO0
    {
        get => _newGradeFIO0;
        set
        {
            if (_newGradeFIO0 == value) return;
            _newGradeFIO0 = value;
            OnPropertyChanged();
        }
    }

    public string OldGradeFIO0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].GradeFIO_DB,
                "2.0" => _repInBase.Rows20[0].GradeFIO_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Telephone0

    public string Telephone0 => ReplaceTelephone0 ? NewTelephone0 : OldTelephone0;

    private bool _replaceTelephone0 = true;
    public bool ReplaceTelephone0
    {
        get => _replaceTelephone0;
        set
        {
            if (_replaceTelephone0 == value) return;
            _replaceTelephone0 = value;
            OnPropertyChanged();
        }
    }

    private string _newTelephone0 = null!;
    public string NewTelephone0
    {
        get => _newTelephone0;
        set
        {
            if (_newTelephone0 == value) return;
            _newTelephone0 = value;
            OnPropertyChanged();
        }
    }

    public string OldTelephone0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Telephone_DB,
                "2.0" => _repInBase.Rows20[0].Telephone_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Fax0

    public string Fax0 => ReplaceFax0 ? NewFax0 : OldFax0;

    private bool _replaceFax0 = true;
    public bool ReplaceFax0
    {
        get => _replaceFax0;
        set
        {
            if (_replaceFax0 == value) return;
            _replaceFax0 = value;
            OnPropertyChanged();
        }
    }

    private string _newFax0 = null!;
    public string NewFax0
    {
        get => _newFax0;
        set
        {
            if (_newFax0 == value) return;
            _newFax0 = value;
            OnPropertyChanged();
        }
    }

    public string OldFax0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Fax_DB,
                "2.0" => _repInBase.Rows20[0].Fax_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Email0

    public string Email0 => ReplaceEmail0 ? NewEmail0 : OldEmail0;

    private bool _replaceEmail0 = true;
    public bool ReplaceEmail0
    {
        get => _replaceEmail0;
        set
        {
            if (_replaceEmail0 == value) return;
            _replaceEmail0 = value;
            OnPropertyChanged();
        }
    }

    private string _newEmail0 = null!;
    public string NewEmail0
    {
        get => _newEmail0;
        set
        {
            if (_newEmail0 == value) return;
            _newEmail0 = value;
            OnPropertyChanged();
        }
    }

    public string OldEmail0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Email_DB,
                "2.0" => _repInBase.Rows20[0].Email_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okpo0

    public string Okpo0 => ReplaceOkpo0 ? NewOkpo0 : OldOkpo0;

    private bool _replaceOkpo0;
    public bool ReplaceOkpo0
    {
        get => _replaceOkpo0;
        set
        {
            if (_replaceOkpo0 == value) return;
            _replaceOkpo0 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkpo0 = null!;
    public string NewOkpo0
    {
        get => _newOkpo0;
        set
        {
            if (_newOkpo0 == value) return;
            _newOkpo0 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkpo0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Okpo_DB,
                "2.0" => _repInBase.Rows20[0].Okpo_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okved0

    public string Okved0 => ReplaceOkved0 ? NewOkved0 : OldOkved0;

    private bool _replaceOkved0 = true;
    public bool ReplaceOkved0
    {
        get => _replaceOkved0;
        set
        {
            if (_replaceOkved0 == value) return;
            _replaceOkved0 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkved0 = null!;
    public string NewOkved0
    {
        get => _newOkved0;
        set
        {
            if (_newOkved0 == value) return;
            _newOkved0 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkved0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Okved_DB,
                "2.0" => _repInBase.Rows20[0].Okved_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okogu0

    public string Okogu0 => ReplaceOkogu0 ? NewOkogu0 : OldOkogu0;

    private bool _replaceOkogu0 = true;
    public bool ReplaceOkogu0
    {
        get => _replaceOkogu0;
        set
        {
            if (_replaceOkogu0 == value) return;
            _replaceOkogu0 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkogu0 = null!;
    public string NewOkogu0
    {
        get => _newOkogu0;
        set
        {
            if (_newOkogu0 == value) return;
            _newOkogu0 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkogu0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Okogu_DB,
                "2.0" => _repInBase.Rows20[0].Okogu_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Oktmo0

    public string Oktmo0 => ReplaceOktmo0 ? NewOktmo0 : OldOktmo0;

    private bool _replaceOktmo0 = true;
    public bool ReplaceOktmo0
    {
        get => _replaceOktmo0;
        set
        {
            if (_replaceOktmo0 == value) return;
            _replaceOktmo0 = value;
            OnPropertyChanged();
        }
    }

    private string _newOktmo0 = null!;
    public string NewOktmo0
    {
        get => _newOktmo0;
        set
        {
            if (_newOktmo0 == value) return;
            _newOktmo0 = value;
            OnPropertyChanged();
        }
    }

    public string OldOktmo0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Oktmo_DB,
                "2.0" => _repInBase.Rows20[0].Oktmo_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Inn0

    public string Inn0 => ReplaceInn0 ? NewInn0 : OldInn0;

    private bool _replaceInn0 = true;
    public bool ReplaceInn0
    {
        get => _replaceInn0;
        set
        {
            if (_replaceInn0 == value) return;
            _replaceInn0 = value;
            OnPropertyChanged();
        }
    }

    private string _newInn0 = null!;
    public string NewInn0
    {
        get => _newInn0;
        set
        {
            if (_newInn0 == value) return;
            _newInn0 = value;
            OnPropertyChanged();
        }
    }

    public string OldInn0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Inn_DB,
                "2.0" => _repInBase.Rows20[0].Inn_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Kpp0

    public string Kpp0 => ReplaceKpp0 ? NewKpp0 : OldKpp0;

    private bool _replaceKpp0 = true;
    public bool ReplaceKpp0
    {
        get => _replaceKpp0;
        set
        {
            if (_replaceKpp0 == value) return;
            _replaceKpp0 = value;
            OnPropertyChanged();
        }
    }

    private string _newKpp0 = null!;
    public string NewKpp0
    {
        get => _newKpp0;
        set
        {
            if (_newKpp0 == value) return;
            _newKpp0 = value;
            OnPropertyChanged();
        }
    }

    public string OldKpp0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Kpp_DB,
                "2.0" => _repInBase.Rows20[0].Kpp_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okopf0

    public string Okopf0 => ReplaceOkopf0 ? NewOkopf0 : OldOkopf0;

    private bool _replaceOkopf0 = true;
    public bool ReplaceOkopf0
    {
        get => _replaceOkopf0;
        set
        {
            if (_replaceOkopf0 == value) return;
            _replaceOkopf0 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkopf0 = null!;
    public string NewOkopf0
    {
        get => _newOkopf0;
        set
        {
            if (_newOkopf0 == value) return;
            _newOkopf0 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkopf0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Okopf_DB,
                "2.0" => _repInBase.Rows20[0].Okopf_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okfs0

    public string Okfs0 => ReplaceOkfs0 ? NewOkfs0 : OldOkfs0;

    private bool _replaceOkfs0 = true;
    public bool ReplaceOkfs0
    {
        get => _replaceOkfs0;
        set
        {
            if (_replaceOkfs0 == value) return;
            _replaceOkfs0 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkfs0 = null!;
    public string NewOkfs0
    {
        get => _newOkfs0;
        set
        {
            if (_newOkfs0 == value) return;
            _newOkfs0 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkfs0
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[0].Okfs_DB,
                "2.0" => _repInBase.Rows20[0].Okfs_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #endregion

    #region ObosoblPodr

    #region SubjectRF1

    public string SubjectRF1 => ReplaceSubjectRF1 ? NewSubjectRF1 : OldSubjectRF1;

    private bool _replaceSubjectRF1 = true;
    public bool ReplaceSubjectRF1
    {
        get => _replaceSubjectRF1;
        set
        {
            if (_replaceSubjectRF1 == value) return;
            _replaceSubjectRF1 = value;
            OnPropertyChanged();
        }
    }

    private string _newSubjectRF1 = null!;
    public string NewSubjectRF1
    {
        get => _newSubjectRF1;
        set
        {
            if (_newSubjectRF1 == value) return;
            _newSubjectRF1 = value;
            OnPropertyChanged();
        }
    }

    public string OldSubjectRF1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].SubjectRF_DB,
                "2.0" => _repInBase.Rows20[1].SubjectRF_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region JurLico1

    public string JurLico1 => ReplaceJurLico1 ? NewJurLico1 : OldJurLico1;

    private bool _replaceJurLico1 = true;
    public bool ReplaceJurLico1
    {
        get => _replaceJurLico1;
        set
        {
            if (_replaceJurLico1 == value) return;
            _replaceJurLico1 = value;
            OnPropertyChanged();
        }
    }

    private string _newJurLico1 = null!;
    public string NewJurLico1
    {
        get => _newJurLico1;
        set
        {
            if (_newJurLico1 == value) return;
            _newJurLico1 = value;
            OnPropertyChanged();
        }
    }

    public string OldJurLico1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].JurLico_DB,
                "2.0" => _repInBase.Rows20[1].JurLico_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region ShortJurLico1

    public string ShortJurLico1 => ReplaceShortJurLico1 ? NewShortJurLico1 : OldShortJurLico1;

    private bool _replaceShortJurLico1 = true;
    public bool ReplaceShortJurLico1
    {
        get => _replaceShortJurLico1;
        set
        {
            if (_replaceShortJurLico1 == value) return;
            _replaceShortJurLico1 = value;
            OnPropertyChanged();
        }
    }

    private string _newShortJurLico1 = null!;
    public string NewShortJurLico1
    {
        get => _newShortJurLico1;
        set
        {
            if (_newShortJurLico1 == value) return;
            _newShortJurLico1 = value;
            OnPropertyChanged();
        }
    }

    public string OldShortJurLico1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].ShortJurLico_DB,
                "2.0" => _repInBase.Rows20[1].ShortJurLico_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region JurLicoAddress1

    public string JurLicoAddress1 => ReplaceJurLicoAddress1 ? NewJurLicoAddress1 : OldJurLicoAddress1;

    private bool _replaceJurLicoAddress1 = true;
    public bool ReplaceJurLicoAddress1
    {
        get => _replaceJurLicoAddress1;
        set
        {
            if (_replaceJurLicoAddress1 == value) return;
            _replaceJurLicoAddress1 = value;
            OnPropertyChanged();
        }
    }

    private string _newJurLicoAddress1 = null!;
    public string NewJurLicoAddress1
    {
        get => _newJurLicoAddress1;
        set
        {
            if (_newJurLicoAddress1 == value) return;
            _newJurLicoAddress1 = value;
            OnPropertyChanged();
        }
    }

    public string OldJurLicoAddress1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].JurLicoAddress_DB,
                "2.0" => _repInBase.Rows20[1].JurLicoAddress_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region JurLicoFactAddress1

    public string JurLicoFactAddress1 => ReplaceJurLicoFactAddress1 ? NewJurLicoFactAddress1 : OldJurLicoFactAddress1;

    private bool _replaceJurLicoFactAddress1 = true;
    public bool ReplaceJurLicoFactAddress1
    {
        get => _replaceJurLicoFactAddress1;
        set
        {
            if (_replaceJurLicoFactAddress1 == value) return;
            _replaceJurLicoFactAddress1 = value;
            OnPropertyChanged();
        }
    }

    private string _newJurLicoFactAddress1 = null!;
    public string NewJurLicoFactAddress1
    {
        get => _newJurLicoFactAddress1;
        set
        {
            if (_newJurLicoFactAddress1 == value) return;
            _newJurLicoFactAddress1 = value;
            OnPropertyChanged();
        }
    }

    public string OldJurLicoFactAddress1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].JurLicoFactAddress_DB,
                "2.0" => _repInBase.Rows20[1].JurLicoFactAddress_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region GradeFIO1

    public string GradeFIO1 => ReplaceGradeFIO1 ? NewGradeFIO1 : OldGradeFIO1;

    private bool _replaceGradeFIO1 = true;
    public bool ReplaceGradeFIO1
    {
        get => _replaceGradeFIO1;
        set
        {
            if (_replaceGradeFIO1 == value) return;
            _replaceGradeFIO1 = value;
            OnPropertyChanged();
        }
    }

    private string _newGradeFIO1 = null!;
    public string NewGradeFIO1
    {
        get => _newGradeFIO1;
        set
        {
            if (_newGradeFIO1 == value) return;
            _newGradeFIO1 = value;
            OnPropertyChanged();
        }
    }

    public string OldGradeFIO1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].GradeFIO_DB,
                "2.0" => _repInBase.Rows20[1].GradeFIO_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Telephone1

    public string Telephone1 => ReplaceTelephone1 ? NewTelephone1 : OldTelephone1;

    private bool _replaceTelephone1 = true;
    public bool ReplaceTelephone1
    {
        get => _replaceTelephone1;
        set
        {
            if (_replaceTelephone1 == value) return;
            _replaceTelephone1 = value;
            OnPropertyChanged();
        }
    }

    private string _newTelephone1 = null!;
    public string NewTelephone1
    {
        get => _newTelephone1;
        set
        {
            if (_newTelephone1 == value) return;
            _newTelephone1 = value;
            OnPropertyChanged();
        }
    }

    public string OldTelephone1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Telephone_DB,
                "2.0" => _repInBase.Rows20[1].Telephone_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Fax1

    public string Fax1 => ReplaceFax1 ? NewFax1 : OldFax1;

    private bool _replaceFax1 = true;
    public bool ReplaceFax1
    {
        get => _replaceFax1;
        set
        {
            if (_replaceFax1 == value) return;
            _replaceFax1 = value;
            OnPropertyChanged();
        }
    }

    private string _newFax1 = null!;
    public string NewFax1
    {
        get => _newFax1;
        set
        {
            if (_newFax1 == value) return;
            _newFax1 = value;
            OnPropertyChanged();
        }
    }

    public string OldFax1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Fax_DB,
                "2.0" => _repInBase.Rows20[1].Fax_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Email1

    public string Email1 => ReplaceEmail1 ? NewEmail1 : OldEmail1;

    private bool _replaceEmail1 = true;
    public bool ReplaceEmail1
    {
        get => _replaceEmail1;
        set
        {
            if (_replaceEmail1 == value) return;
            _replaceEmail1 = value;
            OnPropertyChanged();
        }
    }

    private string _newEmail1 = null!;
    public string NewEmail1
    {
        get => _newEmail1;
        set
        {
            if (_newEmail1 == value) return;
            _newEmail1 = value;
            OnPropertyChanged();
        }
    }

    public string OldEmail1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Email_DB,
                "2.0" => _repInBase.Rows20[1].Email_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okpo1

    public string Okpo1 => ReplaceOkpo1 ? NewOkpo1 : OldOkpo1;

    private bool _replaceOkpo1;
    public bool ReplaceOkpo1
    {
        get => _replaceOkpo1;
        set
        {
            if (_replaceOkpo1 == value) return;
            _replaceOkpo1 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkpo1 = null!;
    public string NewOkpo1
    {
        get => _newOkpo1;
        set
        {
            if (_newOkpo1 == value) return;
            _newOkpo1 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkpo1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Okpo_DB,
                "2.0" => _repInBase.Rows20[1].Okpo_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okved0

    public string Okved1 => ReplaceOkved1 ? NewOkved1 : OldOkved1;

    private bool _replaceOkved1 = true;
    public bool ReplaceOkved1
    {
        get => _replaceOkved1;
        set
        {
            if (_replaceOkved1 == value) return;
            _replaceOkved1 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkved1 = null!;
    public string NewOkved1
    {
        get => _newOkved1;
        set
        {
            if (_newOkved1 == value) return;
            _newOkved1 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkved1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Okved_DB,
                "2.0" => _repInBase.Rows20[1].Okved_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okogu1

    public string Okogu1 => ReplaceOkogu1 ? NewOkogu1 : OldOkogu1;

    private bool _replaceOkogu1 = true;
    public bool ReplaceOkogu1
    {
        get => _replaceOkogu1;
        set
        {
            if (_replaceOkogu1 == value) return;
            _replaceOkogu1 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkogu1 = null!;
    public string NewOkogu1
    {
        get => _newOkogu1;
        set
        {
            if (_newOkogu1 == value) return;
            _newOkogu1 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkogu1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Okogu_DB,
                "2.0" => _repInBase.Rows20[1].Okogu_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Oktmo1

    public string Oktmo1 => ReplaceOktmo1 ? NewOktmo1 : OldOktmo1;

    private bool _replaceOktmo1= true;
    public bool ReplaceOktmo1
    {
        get => _replaceOktmo1;
        set
        {
            if (_replaceOktmo1 == value) return;
            _replaceOktmo1 = value;
            OnPropertyChanged();
        }
    }

    private string _newOktmo1= null!;
    public string NewOktmo1
    {
        get => _newOktmo1;
        set
        {
            if (_newOktmo1 == value) return;
            _newOktmo1 = value;
            OnPropertyChanged();
        }
    }

    public string OldOktmo1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Oktmo_DB,
                "2.0" => _repInBase.Rows20[1].Oktmo_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Inn1

    public string Inn1 => ReplaceInn1 ? NewInn1: OldInn1;

    private bool _replaceInn1 = true;
    public bool ReplaceInn1
    {
        get => _replaceInn1;
        set
        {
            if (_replaceInn1 == value) return;
            _replaceInn1 = value;
            OnPropertyChanged();
        }
    }

    private string _newInn1 = null!;
    public string NewInn1
    {
        get => _newInn1;
        set
        {
            if (_newInn1 == value) return;
            _newInn1 = value;
            OnPropertyChanged();
        }
    }

    public string OldInn1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Inn_DB,
                "2.0" => _repInBase.Rows20[1].Inn_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Kpp0

    public string Kpp1 => ReplaceKpp1 ? NewKpp1 : OldKpp1;

    private bool _replaceKpp1 = true;
    public bool ReplaceKpp1
    {
        get => _replaceKpp1;
        set
        {
            if (_replaceKpp1 == value) return;
            _replaceKpp1 = value;
            OnPropertyChanged();
        }
    }

    private string _newKpp1 = null!;
    public string NewKpp1
    {
        get => _newKpp1;
        set
        {
            if (_newKpp1 == value) return;
            _newKpp1 = value;
            OnPropertyChanged();
        }
    }

    public string OldKpp1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Kpp_DB,
                "2.0" => _repInBase.Rows20[1].Kpp_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okopf1

    public string Okopf1 => ReplaceOkopf1 ? NewOkopf1 : OldOkopf1;

    private bool _replaceOkopf1 = true;
    public bool ReplaceOkopf1
    {
        get => _replaceOkopf1;
        set
        {
            if (_replaceOkopf1 == value) return;
            _replaceOkopf1 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkopf1 = null!;
    public string NewOkopf1
    {
        get => _newOkopf1;
        set
        {
            if (_newOkopf1 == value) return;
            _newOkopf1 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkopf1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Okopf_DB,
                "2.0" => _repInBase.Rows20[1].Okopf_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #region Okfs1

    public string Okfs1 => ReplaceOkfs1 ? NewOkfs1 : OldOkfs1;

    private bool _replaceOkfs1 = true;
    public bool ReplaceOkfs1
    {
        get => _replaceOkfs1;
        set
        {
            if (_replaceOkfs1 == value) return;
            _replaceOkfs1 = value;
            OnPropertyChanged();
        }
    }

    private string _newOkfs1 = null!;
    public string NewOkfs1
    {
        get => _newOkfs1;
        set
        {
            if (_newOkfs1 == value) return;
            _newOkfs1 = value;
            OnPropertyChanged();
        }
    }

    public string OldOkfs1
    {
        get
        {
            return _repInBase.FormNum_DB switch
            {
                "1.0" => _repInBase.Rows10[1].Okfs_DB,
                "2.0" => _repInBase.Rows20[1].Okfs_DB,
                _ => string.Empty
            };
        }
    }

    #endregion

    #endregion

    #endregion

    #region Constructor
    
    public CompareReportsTitleFormVM() {}

    public CompareReportsTitleFormVM(Report repInBase, Report repImport)
    {
        _repInBase = repInBase;
        RegNo = repInBase.RegNoRep.Value;
        
        switch (repInBase.FormNum_DB)
        {
            case "1.0":
            {
                NewOrganUprav = !string.IsNullOrEmpty(repImport.Rows10[0].OrganUprav_DB)
                    ? repImport.Rows10[0].OrganUprav_DB
                    : !string.IsNullOrEmpty(repImport.Rows10[1].OrganUprav_DB)
                        ? repImport.Rows10[1].OrganUprav_DB
                        : string.Empty;

                #region JurLico
        
                NewSubjectRF0 = repImport.Rows10[0].SubjectRF_DB;
                NewJurLico0 = repImport.Rows10[0].JurLico_DB;
                NewShortJurLico0 = repImport.Rows10[0].ShortJurLico_DB;
                NewJurLicoAddress0 = repImport.Rows10[0].JurLicoAddress_DB;
                NewJurLicoFactAddress0 = repImport.Rows10[0].JurLicoFactAddress_DB;
                NewGradeFIO0 = repImport.Rows10[0].GradeFIO_DB;
                NewTelephone0 = repImport.Rows10[0].Telephone_DB;
                NewFax0 = repImport.Rows10[0].Fax_DB;
                NewEmail0 = repImport.Rows10[0].Email_DB;
                NewOkpo0 = repImport.Rows10[0].Okpo_DB;
                NewOkved0 = repImport.Rows10[0].Okved_DB;
                NewOkogu0 = repImport.Rows10[0].Okogu_DB;
                NewOktmo0 = repImport.Rows10[0].Oktmo_DB;
                NewInn0 = repImport.Rows10[0].Inn_DB;
                NewKpp0 = repImport.Rows10[0].Kpp_DB;
                NewOkopf0 = repImport.Rows10[0].Okopf_DB;
                NewOkfs0 = repImport.Rows10[0].Okfs_DB;
            
                #endregion

                #region ObosoblPodr
            
                NewSubjectRF1 = repImport.Rows10[1].SubjectRF_DB;
                NewJurLico1 = repImport.Rows10[1].JurLico_DB;
                NewShortJurLico1 = repImport.Rows10[1].ShortJurLico_DB;
                NewJurLicoAddress1 = repImport.Rows10[1].JurLicoAddress_DB;
                NewJurLicoFactAddress1 = repImport.Rows10[1].JurLicoFactAddress_DB;
                NewGradeFIO1 = repImport.Rows10[1].GradeFIO_DB;
                NewTelephone1 = repImport.Rows10[1].Telephone_DB;
                NewFax1 = repImport.Rows10[1].Fax_DB;
                NewEmail1 = repImport.Rows10[1].Email_DB;
                NewOkpo1 = repImport.Rows10[1].Okpo_DB;
                NewOkved1 = repImport.Rows10[1].Okved_DB;
                NewOkogu1 = repImport.Rows10[1].Okogu_DB;
                NewOktmo1 = repImport.Rows10[1].Oktmo_DB;
                NewInn1 = repImport.Rows10[1].Inn_DB;
                NewKpp1 = repImport.Rows10[1].Kpp_DB;
                NewOkopf1 = repImport.Rows10[1].Okopf_DB;
                NewOkfs1 = repImport.Rows10[1].Okfs_DB;
            
                #endregion

                break;
            }

            case "2.0":
            {
                NewOrganUprav = !string.IsNullOrEmpty(repImport.Rows20[0].OrganUprav_DB)
                    ? repImport.Rows20[0].OrganUprav_DB
                    : !string.IsNullOrEmpty(repImport.Rows20[1].OrganUprav_DB)
                        ? repImport.Rows20[1].OrganUprav_DB
                        : string.Empty;

                #region JurLico
        
                NewSubjectRF0 = repImport.Rows20[0].SubjectRF_DB;
                NewJurLico0 = repImport.Rows20[0].JurLico_DB;
                NewShortJurLico0 = repImport.Rows20[0].ShortJurLico_DB;
                NewJurLicoAddress0 = repImport.Rows20[0].JurLicoAddress_DB;
                NewJurLicoFactAddress0 = repImport.Rows20[0].JurLicoFactAddress_DB;
                NewGradeFIO0 = repImport.Rows20[0].GradeFIO_DB;
                NewTelephone0 = repImport.Rows20[0].Telephone_DB;
                NewFax0 = repImport.Rows20[0].Fax_DB;
                NewEmail0 = repImport.Rows20[0].Email_DB;
                NewOkpo0 = repImport.Rows20[0].Okpo_DB;
                NewOkved0 = repImport.Rows20[0].Okved_DB;
                NewOkogu0 = repImport.Rows20[0].Okogu_DB;
                NewOktmo0 = repImport.Rows20[0].Oktmo_DB;
                NewInn0 = repImport.Rows20[0].Inn_DB;
                NewKpp0 = repImport.Rows20[0].Kpp_DB;
                NewOkopf0 = repImport.Rows20[0].Okopf_DB;
                NewOkfs0 = repImport.Rows20[0].Okfs_DB;
                
                #endregion

                #region ObosoblPodr
                
                NewSubjectRF1 = repImport.Rows20[1].SubjectRF_DB;
                NewJurLico1 = repImport.Rows20[1].JurLico_DB;
                NewShortJurLico1 = repImport.Rows20[1].ShortJurLico_DB;
                NewJurLicoAddress1 = repImport.Rows20[1].JurLicoAddress_DB;
                NewJurLicoFactAddress1 = repImport.Rows20[1].JurLicoFactAddress_DB;
                NewGradeFIO1 = repImport.Rows20[1].GradeFIO_DB;
                NewTelephone1 = repImport.Rows20[1].Telephone_DB;
                NewFax1 = repImport.Rows20[1].Fax_DB;
                NewEmail1 = repImport.Rows20[1].Email_DB;
                NewOkpo1 = repImport.Rows20[1].Okpo_DB;
                NewOkved1 = repImport.Rows20[1].Okved_DB;
                NewOkogu1 = repImport.Rows20[1].Okogu_DB;
                NewOktmo1 = repImport.Rows20[1].Oktmo_DB;
                NewInn1 = repImport.Rows20[1].Inn_DB;
                NewKpp1 = repImport.Rows20[1].Kpp_DB;
                NewOkopf1 = repImport.Rows20[1].Okopf_DB;
                NewOkfs1 = repImport.Rows20[1].Okfs_DB;
                
                #endregion

                break;
            }
        }

        #region JurLicoCheckBox

        ReplaceSubjectRF0 = !OldSubjectRF0.Equals(NewSubjectRF0);
        ReplaceJurLico0 = !OldJurLico0.Equals(NewJurLico0);
        ReplaceShortJurLico0 = !OldShortJurLico0.Equals(NewShortJurLico0);
        ReplaceJurLicoAddress0 = !OldJurLicoAddress0.Equals(NewJurLicoAddress0);
        ReplaceJurLicoFactAddress0 = !OldJurLicoFactAddress0.Equals(NewJurLicoFactAddress0);
        ReplaceGradeFIO0 = !OldGradeFIO0.Equals(NewGradeFIO0);
        ReplaceTelephone0 = !OldTelephone0.Equals(NewTelephone0);
        ReplaceFax0 = !OldFax0.Equals(NewFax0);
        ReplaceEmail0 = !OldEmail0.Equals(NewEmail0);
        ReplaceOkpo0 = !OldOkpo0.Equals(NewOkpo0);
        ReplaceOkved0 = !OldOkved0.Equals(NewOkved0);
        ReplaceOkogu0 = !OldOkogu0.Equals(NewOkogu0);
        ReplaceOktmo0 = !OldOktmo0.Equals(NewOktmo0);
        ReplaceInn0 = !OldInn0.Equals(NewInn0);
        ReplaceKpp0 = !OldKpp0.Equals(NewKpp0);
        ReplaceOkopf0 = !OldOkopf0.Equals(NewOkopf0);
        ReplaceOkfs0 = !OldOkfs0.Equals(NewOkfs0);

        #endregion

        #region ObosoblPodrCheckBox

        ReplaceSubjectRF1 = !OldSubjectRF1.Equals(NewSubjectRF1);
        ReplaceJurLico1 = !OldJurLico1.Equals(NewJurLico1);
        ReplaceShortJurLico1 = !OldShortJurLico1.Equals(NewShortJurLico1);
        ReplaceJurLicoAddress1 = !OldJurLicoAddress1.Equals(NewJurLicoAddress1);
        ReplaceJurLicoFactAddress1 = !OldJurLicoFactAddress1.Equals(NewJurLicoFactAddress1);
        ReplaceGradeFIO1 = !OldGradeFIO1.Equals(NewGradeFIO1);
        ReplaceTelephone1 = !OldTelephone1.Equals(NewTelephone1);
        ReplaceFax1 = !OldFax1.Equals(NewFax1);
        ReplaceEmail1 = !OldEmail1.Equals(NewEmail1);
        ReplaceOkpo1 = !OldOkpo1.Equals(NewOkpo1);
        ReplaceOkved1 = !OldOkved1.Equals(NewOkved1);
        ReplaceOkogu1 = !OldOkogu1.Equals(NewOkogu1);
        ReplaceOktmo1 = !OldOktmo1.Equals(NewOktmo1);
        ReplaceInn1 = !OldInn1.Equals(NewInn1);
        ReplaceKpp1 = !OldKpp1.Equals(NewKpp1);
        ReplaceOkopf1 = !OldOkopf1.Equals(NewOkopf1);
        ReplaceOkfs1 = !OldOkfs1.Equals(NewOkfs1);

        #endregion
    }

    #endregion

    #region PropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}