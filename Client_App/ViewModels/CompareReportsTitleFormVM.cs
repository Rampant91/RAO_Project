using System.ComponentModel;
using System.Runtime.CompilerServices;
using Models.Collections;

namespace Client_App.ViewModels;

public class CompareReportsTitleFormVM : INotifyPropertyChanged
{
    private Report _repInBase;
    public Report _repImport;

    #region SubjectRF

    private string _subjectRF;
    public string SubjectRF
    {
        get => ReplaceSubjectRF ? NewSubjectRF : OldSubjectRF;
        set
        {
            if (_newSubjectRF == value) return;
            _newSubjectRF = value;
            OnPropertyChanged();
        }
    }


    private bool _replaceSubjectRF = true;
    public bool ReplaceSubjectRF
    {
        get => _replaceSubjectRF;
        set
        {
            if (_replaceSubjectRF == value) return;
            _replaceSubjectRF = value;
            OnPropertyChanged();
        }
    }

    private string _newSubjectRF;
    public string NewSubjectRF
    {
        get => _repImport.Rows10[0].SubjectRF_DB;
        set
        {
            if (_newSubjectRF == value) return;
            _newSubjectRF = value;
            OnPropertyChanged();
        }
    }

    private string _oldSubjectRF;
    public string OldSubjectRF
    {
        get => _repInBase.Rows10[0].SubjectRF_DB;
        set
        {
            if (_oldSubjectRF == value) return;
            _oldSubjectRF = value;
            OnPropertyChanged();
        }
    }

    #endregion

    public CompareReportsTitleFormVM()
    {

    }

    public CompareReportsTitleFormVM(Report repInBase, Report repImport)
    {
        _repInBase = repInBase;
        _repImport = repImport;
    }

    #region PropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}