using StarWarsFilterApp.View;
using System.Windows.Input;

namespace StarWarsFilterApp.ViewModel
{
    public class FilterCharacterViewModel : BaseViewModel
    {
        private readonly MainWindowViewModel _main;

        public ICommand ReturnCommand { get; }
        public ICommand ClearTextFieldsCommand { get; }

        public FilterCharacterViewModel(MainWindowViewModel main)
        {
            _main = main;
            ReturnCommand = new RelayCommand(ReturnToStart);
            ClearTextFieldsCommand = new RelayCommand(ClearTextFields);
        }

        private void ReturnToStart(object? obj)
        {
            _main.CurrentView = new StartView(_main);
        }

        private void ClearTextFields(object? obj)
        {
            Name = string.Empty;
            Species = string.Empty;
            Planet = string.Empty;
            Organization = string.Empty;
            Film = string.Empty;
        }

        // Powiązane właściwości
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _species;
        public string Species
        {
            get => _species;
            set => SetProperty(ref _species, value);
        }

        private string _planet;
        public string Planet
        {
            get => _planet;
            set => SetProperty(ref _planet, value);
        }

        private string _organization;
        public string Organization
        {
            get => _organization;
            set => SetProperty(ref _organization, value);
        }

        private string _film;
        public string Film
        {
            get => _film;
            set => SetProperty(ref _film, value);
        }
    }
}
