using BorromeoWPF.Services;
using System.Windows.Input;

namespace BorromeoWPF.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public ICommand NavigateToTshirtCommand { get; }

    public HomeViewModel(NavigationService<TshirtViewModel> tshirtNav)
    {
        NavigateToTshirtCommand = new RelayCommand(_ => tshirtNav.Navigate());
    }
}
