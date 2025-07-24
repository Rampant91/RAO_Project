using Models.Attributes;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Forms.Forms1;
public class Form_10VM : BaseVM, INotifyPropertyChanged
{
    private Report _repInBase => Storage;



    #region Storage

    private Report _Storage;
    public Report Storage
    {
        get => _Storage;
        set
        {
            if (_Storage != value)
            {
                _Storage = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region DBO

    private DBObservable _DBO;
    public DBObservable DBO
    {
        get => _DBO;
        set
        {
            if (_DBO != value)
            {
                _DBO = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region FormType

    public string FormType { get; set; } = "1.0";

    #endregion

    private string WindowHeader { get; set; } = "1.0";

    #region Properties

    #region IsSeparateDivision

    private bool _isSeparateDivision = true;
    public bool IsSeparateDivision
    {
        get => _isSeparateDivision;
        set
        {
            if (_isSeparateDivision == value) return;
            _isSeparateDivision = value;
            OnPropertyChanged();
        }
    }

    #endregion



    #region RegNo

    public string RegNo { get; } = null!;

    #endregion

    #region Okpo
    
    public string Okpo => !string.IsNullOrEmpty(Okpo1) ? Okpo1 : Okpo0; 
    
    #endregion

    #region OrganUprav

    private string _organUprav;

    public string OrganUprav
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

    #region ShortJurLico
    
    public string ShortJurLico => !string.IsNullOrEmpty(ShortJurLico1) ? ShortJurLico1 : ShortJurLico0; 
    
    #endregion

    #region JurLico

    #region SubjectRF0

    private string _subjectRF0 = null!;
    public string SubjectRF0
    {
        get => _subjectRF0;
        set
        {
            if (_subjectRF0 == value) return;
            _subjectRF0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region JurLico0

    private string _jurLico0 = null!;
    public string JurLico0
    {
        get => _jurLico0;
        set
        {
            if (_jurLico0 == value) return;
            _jurLico0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region ShortJurLico0

    private string _shortJurLico0 = null!;
    public string ShortJurLico0
    {
        get => _shortJurLico0;
        set
        {
            if (_shortJurLico0 == value) return;
            _shortJurLico0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region JurLicoAddress0

    private string _jurLicoAddress0 = null!;
    public string JurLicoAddress0
    {
        get => _jurLicoAddress0;
        set
        {
            if (_jurLicoAddress0 == value) return;
            _jurLicoAddress0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region JurLicoFactAddress0

    private string _jurLicoFactAddress0 = null!;
    public string JurLicoFactAddress0
    {
        get => _jurLicoFactAddress0;
        set
        {
            if (_jurLicoFactAddress0 == value) return;
            _jurLicoFactAddress0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region GradeFIO0

    private string _gradeFIO0 = null!;
    public string GradeFIO0
    {
        get => _gradeFIO0;
        set
        {
            if (_gradeFIO0 == value) return;
            _gradeFIO0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Telephone0

    private string _telephone0 = null!;
    public string Telephone0
    {
        get => _telephone0;
        set
        {
            if (_telephone0 == value) return;
            _telephone0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Fax0

    private string _fax0 = null!;
    public string Fax0
    {
        get => _fax0;
        set
        {
            if (_fax0 == value) return;
            _fax0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Email0

    private string _email0 = null!;
    public string Email0
    {
        get => _email0;
        set
        {
            if (_email0 == value) return;
            _email0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okpo0

    private string _okpo0 = null!;
    public string Okpo0
    {
        get => _okpo0;
        set
        {
            if (_okpo0 == value) return;
            _okpo0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okved0

    private string _okved0 = null!;
    public string Okved0
    {
        get => _okved0;
        set
        {
            if (_okved0 == value) return;
            _okved0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okogu0

    private string _okogu0 = null!;
    public string Okogu0
    {
        get => _okogu0;
        set
        {
            if (_okogu0 == value) return;
            _okogu0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Oktmo0

    private string _oktmo0 = null!;
    public string Oktmo0
    {
        get => _oktmo0;
        set
        {
            if (_oktmo0 == value) return;
            _oktmo0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Inn0

    private string _inn0 = null!;
    public string Inn0
    {
        get => _inn0;
        set
        {
            if (_inn0 == value) return;
            _inn0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Kpp0

    private string _kpp0 = null!;
    public string Kpp0
    {
        get => _kpp0;
        set
        {
            if (_kpp0 == value) return;
            _kpp0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okopf0

    private string _okopf0 = null!;
    public string Okopf0
    {
        get => _okopf0;
        set
        {
            if (_okopf0 == value) return;
            _okopf0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okfs0

    private string _okfs0 = null!;
    public string Okfs0
    {
        get => _okfs0;
        set
        {
            if (_okfs0 == value) return;
            _okfs0 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    #region ObosobPodr

    #region SubjectRF1

    private string _subjectRF1 = null!;
    public string SubjectRF1
    {
        get => _subjectRF1;
        set
        {
            if (_subjectRF1 == value) return;
            _subjectRF1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region JurLico1

    private string _jurLico1 = null!;
    public string JurLico1
    {
        get => _jurLico1;
        set
        {
            if (_jurLico1 == value) return;
            _jurLico1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region ShortJurLico1

    private string _shortJurLico1 = null!;
    public string ShortJurLico1
    {
        get => _shortJurLico1;
        set
        {
            if (_shortJurLico1 == value) return;
            _shortJurLico1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region JurLicoAddress1

    private string _jurLicoAddress1 = null!;
    public string JurLicoAddress1
    {
        get => _jurLicoAddress1;
        set
        {
            if (_jurLicoAddress1 == value) return;
            _jurLicoAddress1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region JurLicoFactAddress1

    private string _jurLicoFactAddress1 = null!;
    public string JurLicoFactAddress1
    {
        get => _jurLicoFactAddress1;
        set
        {
            if (_jurLicoFactAddress1 == value) return;
            _jurLicoFactAddress1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region GradeFIO1

    private string _gradeFIO1 = null!;
    public string GradeFIO1
    {
        get => _gradeFIO1;
        set
        {
            if (_gradeFIO1 == value) return;
            _gradeFIO1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Telephone1

    private string _telephone1 = null!;
    public string Telephone1
    {
        get => _telephone1;
        set
        {
            if (_telephone1 == value) return;
            _telephone1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Fax1

    private string _fax1 = null!;
    public string Fax1
    {
        get => _fax1;
        set
        {
            if (_fax1 == value) return;
            _fax1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Email1

    private string _email1 = null!;
    public string Email1
    {
        get => _email1;
        set
        {
            if (_email1 == value) return;
            _email1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okpo1

    private string _okpo1 = null!;
    public string Okpo1
    {
        get => _okpo1;
        set
        {
            if (_okpo1 == value) return;
            _okpo1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okved1

    private string _okved1 = null!;
    public string Okved1
    {
        get => _okved1;
        set
        {
            if (_okved1 == value) return;
            _okved1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okogu1

    private string _okogu1 = null!;
    public string Okogu1
    {
        get => _okogu1;
        set
        {
            if (_okogu1 == value) return;
            _okogu1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Oktmo1

    private string _oktmo1 = null!;
    public string Oktmo1
    {
        get => _oktmo1;
        set
        {
            if (_oktmo1 == value) return;
            _oktmo1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Inn1

    private string _inn1 = null!;
    public string Inn1
    {
        get => _inn1;
        set
        {
            if (_inn1 == value) return;
            _inn1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Kpp1

    private string _kpp1 = null!;
    public string Kpp1
    {
        get => _kpp1;
        set
        {
            if (_kpp1 == value) return;
            _kpp1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okopf1

    private string _okopf1 = null!;
    public string Okopf1
    {
        get => _okopf1;
        set
        {
            if (_okopf1 == value) return;
            _okopf1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Okfs1

    private string _okfs1 = null!;
    public string Okfs1
    {
        get => _okfs1;
        set
        {
            if (_okfs1 == value) return;
            _okfs1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    #endregion

    public Form_10VM() { }

    public Form_10VM(in DBObservable reps)
    {
        Storage = new Report { FormNum_DB = "1.0" };

        var ty1 = (Form10)FormCreator.Create("1.0");
        ty1.NumberInOrder_DB = 1;
        var ty2 = (Form10)FormCreator.Create("1.0");
        ty2.NumberInOrder_DB = 2;
        Storage.Rows10.Add(ty1);
        Storage.Rows10.Add(ty2);
        DBO = reps;
        WindowHeader = ((Form_ClassAttribute)Type.GetType($"Models.Forms.Form1.Form10,Models")!.GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;

    }

    #region OnPropertyChanged
    
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}