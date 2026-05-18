using TShirtManagement.Commands;
using Domain.Models;
using Framework.Services;

namespace TShirtManagement.ViewModels
{
    public class AddTShirtViewModel : BaseViewModel
    {
        private readonly ITShirtService _service;
        private readonly MainViewModel _main;

        public string? Brand { get; set; }
        public string? Type { get; set; }
        public string? Design { get; set; }
        public decimal? Price { get; set; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand BackCommand { get; }

        public AddTShirtViewModel(ITShirtService service, MainViewModel main)
        {
            _service = service;
            _main = main;

            SaveCommand = new RelayCommand(_ => Save());
            BackCommand = new RelayCommand(_ => _main.OpenHomeCommand.Execute(null));
        }

        private void Save()
        {
            var TShirt = new TShirt
            {
                Brand  = Brand,
                Type   = Type,
                Design = Design,
                Price  = Price
            };

            _service.Create(TShirt);
            _main.OpenHomeCommand.Execute(null);
        }
    }
}
