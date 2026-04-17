using BorromeoWPF.Stores;
using BorromeoWPF.ViewModels;

namespace BorromeoWPF.Services;

public class NavigationService<TViewModel> where TViewModel : BaseViewModel
{
    private readonly NavigationStore _store;
    private readonly Func<TViewModel> _factory;

    public NavigationService(NavigationStore store, Func<TViewModel> factory)
    {
        _store = store;
        _factory = factory;
    }

    public void Navigate() => _store.CurrentViewModel = _factory();
}
