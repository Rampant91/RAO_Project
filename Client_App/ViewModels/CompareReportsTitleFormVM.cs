using System.ComponentModel;
using System.Runtime.CompilerServices;
using Models.Collections;

namespace Client_App.ViewModels;

public class CompareReportsTitleFormVM : INotifyPropertyChanged
{
    private readonly Report _repInBase = null!;
    internal bool Replace = false;

    #region Properties

    public string Title
    {
        get
        {
            var okpo = _repInBase.FormNum_DB is "1.0" ? Okpo0 : Okpo1;
            return $"Сравнение титульных форм {_repInBase.FormNum_DB} организации {RegNo}_{okpo}";
        }
    }

    public string RegNo { get; set; } = null!;

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
        get => _repInBase.Rows10[0].SubjectRF_DB;
        set => _repInBase.Rows10[0].SubjectRF_DB = value;
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
        get => _repInBase.Rows10[0].JurLico_DB;
        set => _repInBase.Rows10[0].JurLico_DB = value;
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
        get => _repInBase.Rows10[0].ShortJurLico_DB;
        set => _repInBase.Rows10[0].ShortJurLico_DB = value;
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
        get => _repInBase.Rows10[0].JurLicoAddress_DB;
        set => _repInBase.Rows10[0].JurLicoAddress_DB = value;
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
        get => _repInBase.Rows10[0].JurLicoFactAddress_DB;
        set => _repInBase.Rows10[0].JurLicoFactAddress_DB = value;
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
        get => _repInBase.Rows10[0].GradeFIO_DB;
        set => _repInBase.Rows10[0].GradeFIO_DB = value;
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
        get => _repInBase.Rows10[0].Telephone_DB;
        set => _repInBase.Rows10[0].Telephone_DB = value;
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
        get => _repInBase.Rows10[0].Fax_DB;
        set => _repInBase.Rows10[0].Fax_DB = value;
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
        get => _repInBase.Rows10[0].Email_DB;
        set => _repInBase.Rows10[0].Email_DB = value;
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
        get => _repInBase.Rows10[0].Okpo_DB;
        set => _repInBase.Rows10[0].Okpo_DB = value;
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
        get => _repInBase.Rows10[0].Okved_DB;
        set => _repInBase.Rows10[0].Okved_DB = value;
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
        get => _repInBase.Rows10[0].Okogu_DB;
        set => _repInBase.Rows10[0].Okogu_DB = value;
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
        get => _repInBase.Rows10[0].Oktmo_DB;
        set => _repInBase.Rows10[0].Oktmo_DB = value;
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
        get => _repInBase.Rows10[0].Inn_DB;
        set => _repInBase.Rows10[0].Inn_DB = value;
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
        get => _repInBase.Rows10[0].Kpp_DB;
        set => _repInBase.Rows10[0].Kpp_DB = value;
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
        get => _repInBase.Rows10[0].Okopf_DB;
        set => _repInBase.Rows10[0].Okopf_DB = value;
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
        get => _repInBase.Rows10[0].Okfs_DB;
        set => _repInBase.Rows10[0].Okfs_DB = value;
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
        get => _repInBase.Rows10[1].SubjectRF_DB;
        set => _repInBase.Rows10[1].SubjectRF_DB = value;
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
        get => _repInBase.Rows10[1].JurLico_DB;
        set => _repInBase.Rows10[1].JurLico_DB = value;
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
        get => _repInBase.Rows10[1].ShortJurLico_DB;
        set => _repInBase.Rows10[1].ShortJurLico_DB = value;
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
        get => _repInBase.Rows10[1].JurLicoAddress_DB;
        set => _repInBase.Rows10[1].JurLicoAddress_DB = value;
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
        get => _repInBase.Rows10[1].JurLicoFactAddress_DB;
        set => _repInBase.Rows10[1].JurLicoFactAddress_DB = value;
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
        get => _repInBase.Rows10[1].GradeFIO_DB;
        set => _repInBase.Rows10[1].GradeFIO_DB = value;
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
        get => _repInBase.Rows10[1].Telephone_DB;
        set => _repInBase.Rows10[1].Telephone_DB = value;
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
        get => _repInBase.Rows10[1].Fax_DB;
        set => _repInBase.Rows10[1].Fax_DB = value;
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
        get => _repInBase.Rows10[1].Email_DB;
        set => _repInBase.Rows10[1].Email_DB = value;
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
        get => _repInBase.Rows10[1].Okpo_DB;
        set => _repInBase.Rows10[1].Okpo_DB = value;
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
        get => _repInBase.Rows10[1].Okved_DB;
        set => _repInBase.Rows10[1].Okved_DB = value;
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
        get => _repInBase.Rows10[1].Okogu_DB;
        set => _repInBase.Rows10[1].Okogu_DB = value;
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
        get => _repInBase.Rows10[1].Oktmo_DB;
        set => _repInBase.Rows10[1].Oktmo_DB = value;
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
        get => _repInBase.Rows10[1].Inn_DB;
        set => _repInBase.Rows10[1].Inn_DB = value;
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
        get => _repInBase.Rows10[1].Kpp_DB;
        set => _repInBase.Rows10[1].Kpp_DB = value;
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
        get => _repInBase.Rows10[1].Okopf_DB;
        set => _repInBase.Rows10[1].Okopf_DB = value;
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
        get => _repInBase.Rows10[1].Okfs_DB;
        set => _repInBase.Rows10[1].Okfs_DB = value;
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

        #region JurLico
        
        NewSubjectRF0 = repImport.Rows10[0].SubjectRF_DB;
        OldSubjectRF0 = repInBase.Rows10[0].SubjectRF_DB;
        ReplaceSubjectRF0 = !OldSubjectRF0.Equals(NewSubjectRF0);
        NewJurLico0 = repImport.Rows10[0].JurLico_DB;
        OldJurLico0 = repInBase.Rows10[0].JurLico_DB;
        ReplaceJurLico0 = !OldJurLico0.Equals(NewJurLico0);
        NewShortJurLico0 = repImport.Rows10[0].ShortJurLico_DB;
        OldShortJurLico0 = repInBase.Rows10[0].ShortJurLico_DB;
        ReplaceShortJurLico0 = !OldShortJurLico0.Equals(NewShortJurLico0);
        NewJurLicoAddress0 = repImport.Rows10[0].JurLicoAddress_DB;
        OldJurLicoAddress0 = repInBase.Rows10[0].JurLicoAddress_DB;
        ReplaceJurLicoAddress0 = !OldJurLicoAddress0.Equals(NewJurLicoAddress0);
        NewJurLicoFactAddress0 = repImport.Rows10[0].JurLicoFactAddress_DB;
        OldJurLicoFactAddress0 = repInBase.Rows10[0].JurLicoFactAddress_DB;
        ReplaceJurLicoFactAddress0 = !OldJurLicoFactAddress0.Equals(NewJurLicoFactAddress0);
        NewGradeFIO0 = repImport.Rows10[0].GradeFIO_DB;
        OldGradeFIO0 = repInBase.Rows10[0].GradeFIO_DB;
        ReplaceGradeFIO0 = !OldGradeFIO0.Equals(NewGradeFIO0);
        NewTelephone0 = repImport.Rows10[0].Telephone_DB;
        OldTelephone0 = repInBase.Rows10[0].Telephone_DB;
        ReplaceTelephone0 = !OldTelephone0.Equals(NewTelephone0);
        NewFax0 = repImport.Rows10[0].Fax_DB;
        OldFax0 = repInBase.Rows10[0].Fax_DB;
        ReplaceFax0 = !OldFax0.Equals(NewFax0);
        NewEmail0 = repImport.Rows10[0].Email_DB;
        OldEmail0 = repInBase.Rows10[0].Email_DB;
        ReplaceEmail0 = !OldEmail0.Equals(NewEmail0);
        NewOkpo0 = repImport.Rows10[0].Okpo_DB;
        OldOkpo0 = repInBase.Rows10[0].Okpo_DB;
        ReplaceOkpo0 = !OldOkpo0.Equals(NewOkpo0);
        NewOkved0 = repImport.Rows10[0].Okved_DB;
        OldOkved0 = repInBase.Rows10[0].Okved_DB;
        ReplaceOkved0 = !OldOkved0.Equals(NewOkved0);
        NewOkogu0 = repImport.Rows10[0].Okogu_DB;
        OldOkogu0 = repInBase.Rows10[0].Okogu_DB;
        ReplaceOkogu0 = !OldOkogu0.Equals(NewOkogu0);
        NewOktmo0 = repImport.Rows10[0].Oktmo_DB;
        OldOktmo0 = repInBase.Rows10[0].Oktmo_DB;
        ReplaceOktmo0 = !OldOktmo0.Equals(NewOktmo0);
        NewInn0 = repImport.Rows10[0].Inn_DB;
        OldInn0 = repInBase.Rows10[0].Inn_DB;
        ReplaceInn0 = !OldInn0.Equals(NewInn0);
        NewKpp0 = repImport.Rows10[0].Kpp_DB;
        OldKpp0 = repInBase.Rows10[0].Kpp_DB;
        ReplaceKpp0 = !OldKpp0.Equals(NewKpp0);
        NewOkopf0 = repImport.Rows10[0].Okopf_DB;
        OldOkopf0 = repInBase.Rows10[0].Okopf_DB;
        ReplaceOkopf0 = !OldOkopf0.Equals(NewOkopf0);
        NewOkfs0 = repImport.Rows10[0].Okfs_DB;
        OldOkfs0 = repInBase.Rows10[0].Okfs_DB;
        ReplaceOkfs0 = !OldOkfs0.Equals(NewOkfs0);

        #endregion

        #region ObosoblPodr
        
        NewSubjectRF1 = repImport.Rows10[1].SubjectRF_DB;
        OldSubjectRF1 = repInBase.Rows10[1].SubjectRF_DB;
        ReplaceSubjectRF1 = !OldSubjectRF1.Equals(NewSubjectRF1);
        NewJurLico1 = repImport.Rows10[1].JurLico_DB;
        OldJurLico1 = repInBase.Rows10[1].JurLico_DB;
        ReplaceJurLico1 = !OldJurLico1.Equals(NewJurLico1);
        NewShortJurLico1 = repImport.Rows10[1].ShortJurLico_DB;
        OldShortJurLico1 = repInBase.Rows10[1].ShortJurLico_DB;
        ReplaceShortJurLico1 = !OldShortJurLico1.Equals(NewShortJurLico1);
        NewJurLicoAddress1 = repImport.Rows10[1].JurLicoAddress_DB;
        OldJurLicoAddress1 = repInBase.Rows10[1].JurLicoAddress_DB;
        ReplaceJurLicoAddress1 = !OldJurLicoAddress1.Equals(NewJurLicoAddress1);
        NewJurLicoFactAddress1 = repImport.Rows10[1].JurLicoFactAddress_DB;
        OldJurLicoFactAddress1 = repInBase.Rows10[1].JurLicoFactAddress_DB;
        ReplaceJurLicoFactAddress1 = !OldJurLicoFactAddress1.Equals(NewJurLicoFactAddress1);
        NewGradeFIO1 = repImport.Rows10[1].GradeFIO_DB;
        OldGradeFIO1 = repInBase.Rows10[1].GradeFIO_DB;
        ReplaceGradeFIO1 = !OldGradeFIO1.Equals(NewGradeFIO1);
        NewTelephone1 = repImport.Rows10[1].Telephone_DB;
        OldTelephone1 = repInBase.Rows10[1].Telephone_DB;
        ReplaceTelephone1 = !OldTelephone1.Equals(NewTelephone1);
        NewFax1 = repImport.Rows10[1].Fax_DB;
        OldFax1 = repInBase.Rows10[1].Fax_DB;
        ReplaceFax1 = !OldFax1.Equals(NewFax1);
        NewEmail1 = repImport.Rows10[1].Email_DB;
        OldEmail1 = repInBase.Rows10[1].Email_DB;
        ReplaceEmail1 = !OldEmail1.Equals(NewEmail1);
        NewOkpo1 = repImport.Rows10[1].Okpo_DB;
        OldOkpo1 = repInBase.Rows10[1].Okpo_DB;
        ReplaceOkpo1 = !OldOkpo1.Equals(NewOkpo1);
        NewOkved1 = repImport.Rows10[1].Okved_DB;
        OldOkved1 = repInBase.Rows10[1].Okved_DB;
        ReplaceOkved1 = !OldOkved1.Equals(NewOkved1);
        NewOkogu1 = repImport.Rows10[1].Okogu_DB;
        OldOkogu1 = repInBase.Rows10[1].Okogu_DB;
        ReplaceOkogu1 = !OldOkogu1.Equals(NewOkogu1);
        NewOktmo1 = repImport.Rows10[1].Oktmo_DB;
        OldOktmo1 = repInBase.Rows10[1].Oktmo_DB;
        ReplaceOktmo1 = !OldOktmo1.Equals(NewOktmo1);
        NewInn1 = repImport.Rows10[1].Inn_DB;
        OldInn1 = repInBase.Rows10[1].Inn_DB;
        ReplaceInn1 = !OldInn1.Equals(NewInn1);
        NewKpp1 = repImport.Rows10[1].Kpp_DB;
        OldKpp1 = repInBase.Rows10[1].Kpp_DB;
        ReplaceKpp1 = !OldKpp1.Equals(NewKpp1);
        NewOkopf1 = repImport.Rows10[1].Okopf_DB;
        OldOkopf1 = repInBase.Rows10[1].Okopf_DB;
        ReplaceOkopf1 = !OldOkopf1.Equals(NewOkopf1);
        NewOkfs1 = repImport.Rows10[1].Okfs_DB;
        OldOkfs1 = repInBase.Rows10[1].Okfs_DB;
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