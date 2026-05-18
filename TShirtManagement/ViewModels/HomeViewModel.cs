using System;
using System.Collections.ObjectModel;
using TShirtManagement.Commands;
using Domain.Models;
using Framework.Services;

namespace TShirtManagement.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly ITShirtService _service;
        private readonly MainViewModel _main;

        public ObservableCollection<TShirt> Records { get; set; }
        public TShirt? SelectedRecord { get; set; }
        public string SearchText { get; set; } = string.Empty;

        public RelayCommand SearchCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand OpenAddCommand { get; }
        public RelayCommand RefreshCommand { get; }

        public HomeViewModel(ITShirtService service, MainViewModel main)
        {
            _service = service;
            _main = main;

            Records = new ObservableCollection<TShirt>(_service.GetAll());

            SearchCommand = new RelayCommand(_ => Search());
            DeleteCommand = new RelayCommand(_ => Delete());
            UpdateCommand = new RelayCommand(_ => Update());
            OpenAddCommand = new RelayCommand(_ => _main.OpenAddCommand.Execute(null));
            RefreshCommand = new RelayCommand(_ => Refresh());
        }

        private void Search()
        {
            var result = _service.Search(SearchText ?? "");
            Records.Clear();
            foreach (var item in result)
                Records.Add(item);
        }

        private void Delete()
        {
            if (SelectedRecord == null || SelectedRecord.Id == null)
                return;

            _service.Remove(SelectedRecord.Id.Value);
            Records.Remove(SelectedRecord);
        }

        private void Update()
        {
            if (SelectedRecord == null)
                return;

            _service.Edit(SelectedRecord);
        }

        private void Refresh()
        {
            Records.Clear();
            foreach (var item in _service.GetAll())
                Records.Add(item);
        }
    }
}
