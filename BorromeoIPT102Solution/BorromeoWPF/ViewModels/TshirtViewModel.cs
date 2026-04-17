using Domain.Interfaces;
using Domain.Models;
using BorromeoWPF.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BorromeoWPF.ViewModels;

public class TshirtViewModel : BaseViewModel
{
    private readonly IGetTshirtAll _getAll;
    private readonly ICreateCommand _create;
    private readonly IUpdateCommand _update;
    private readonly IDeleteCommand _delete;

    public ObservableCollection<Tshirt> Tshirts { get; } = new();

    private string _searchTerm = string.Empty;
    public string SearchTerm
    {
        get => _searchTerm;
        set { _searchTerm = value; OnPropertyChanged(); FilterTshirts(); }
    }

    private Tshirt _form = new();
    public Tshirt Form
    {
        get => _form;
        set { _form = value; OnPropertyChanged(); }
    }

    private bool _showForm;
    public bool ShowForm
    {
        get => _showForm;
        set { _showForm = value; OnPropertyChanged(); }
    }

    public ICommand LoadCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand GoHomeCommand { get; }

    private List<Tshirt> _allTshirts = new();

    public TshirtViewModel(
        IGetTshirtAll getAll,
        ICreateCommand create,
        IUpdateCommand update,
        IDeleteCommand delete,
        NavigationService<HomeViewModel> homeNav)
    {
        _getAll = getAll;
        _create = create;
        _update = update;
        _delete = delete;

        LoadCommand = new RelayCommand(async _ => await LoadAsync());
        AddCommand = new RelayCommand(_ => { Form = new Tshirt(); ShowForm = true; });
        EditCommand = new RelayCommand(t => { if (t is Tshirt ts) { Form = new Tshirt { TshirtId = ts.TshirtId, Name = ts.Name, Quantity = ts.Quantity, Price = ts.Price, Brand = ts.Brand }; ShowForm = true; } });
        SaveCommand = new RelayCommand(async _ => await SaveAsync());
        DeleteCommand = new RelayCommand(async t => { if (t is Tshirt ts) await DeleteAsync(ts.TshirtId); });
        CancelCommand = new RelayCommand(_ => ShowForm = false);
        GoHomeCommand = new RelayCommand(_ => homeNav.Navigate());

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        _allTshirts = (await _getAll.GetAllAsync()).ToList();
        FilterTshirts();
    }

    private void FilterTshirts()
    {
        Tshirts.Clear();
        var filtered = string.IsNullOrWhiteSpace(SearchTerm)
            ? _allTshirts
            : _allTshirts.Where(t =>
                t.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                t.Brand.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
        foreach (var t in filtered) Tshirts.Add(t);
    }

    private async Task SaveAsync()
    {
        if (Form.TshirtId == 0)
            await _create.CreateAsync(Form);
        else
            await _update.UpdateAsync(Form);
        ShowForm = false;
        await LoadAsync();
    }

    private async Task DeleteAsync(int id)
    {
        await _delete.DeleteAsync(id);
        await LoadAsync();
    }
}
