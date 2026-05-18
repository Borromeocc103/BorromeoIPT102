using TShirtManagement.ViewModels;
using TShirtManagement.Commands;
using Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TShirtManagement.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentView;
        public BaseViewModel CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OpenHomeCommand { get; }
        public RelayCommand OpenAddCommand { get; }

        private readonly ITShirtService _service;

        public MainViewModel(ITShirtService service)
        {
            _service = service;

            OpenHomeCommand = new RelayCommand(_ => OpenHome());
            OpenAddCommand = new RelayCommand(_ => OpenAdd());

            OpenHome();
        }

        private void OpenHome()
        {
            CurrentView = new HomeViewModel(_service, this);
        }

        private void OpenAdd()
        {
            CurrentView = new AddTShirtViewModel(_service, this);
        }
    }
}
