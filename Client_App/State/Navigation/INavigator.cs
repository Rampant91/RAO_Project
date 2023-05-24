using System.Collections.Generic;
using System.Windows.Input;
using Client_App.ViewModels;
using Models.Collections;

namespace Client_App.State.Navigation;

public enum ViewType
{
    Annual,
    Oper,
    UpdateOrg
}

public interface INavigator
{
    BaseVM CurrentViewModel { get; set; }

    List<Report>? ReportStorage { get; set; }

    ICommand UpdateCurrentViewModelCommand { get; }
}