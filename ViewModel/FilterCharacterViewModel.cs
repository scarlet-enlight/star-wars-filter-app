using StarWarsFilterApp.Model;
using StarWarsFilterApp.Services;
using StarWarsFilterApp.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StarWarsFilterApp.ViewModel
{
    public class FilterCharacterViewModel : BaseViewModel
    {
        // Referencja do głównego widoku
        private readonly MainWindowViewModel _main;

        // Komendy do nawigacji
        public ICommand ReturnCommand { get; }
        public ICommand ClearTextFieldsCommand { get; }

        public ICommand FilterCommand { get; }

        // Serwis do pobierania postaci
        private readonly CharacterService _characterService;

        // Kolekcja postaci
        public ObservableCollection<Character> Characters { get; set; }



        public FilterCharacterViewModel(MainWindowViewModel main)
        {
            _main = main;
            ReturnCommand = new RelayCommand(ReturnToStart);
            ClearTextFieldsCommand = new RelayCommand(ClearTextFields);
            FilterCommand = new RelayCommand(obj => FilterCharacters(Name, Species, Planet, Organization, Film));

            // Inicjalizacja kolekcji postaci
            _characterService = new CharacterService();

            LoadCharacters();
        }

        public List<Character> FilterCharacters(string name, string species, string planet, string organization, string film)
        {
            _name = name;
            _species = species;
            _planet = planet;
            _organization = organization;
            _film = film;

            IEnumerable<Character> result;

            if (_name != null)
            {

                result = new List<Character> { _characterService.GetCharacterByName(_name) };

            }
            else
            {
                result = _characterService.GetAllCharacters();
            }

            Characters = new ObservableCollection<Character>(result);
            OnPropertyChanged(nameof(Characters));

            return Characters.ToList();
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
        private void LoadCharacters()
        {

            var result = _characterService.GetAllCharacters();
            Characters = new ObservableCollection<Character>(result);
            OnPropertyChanged(nameof(Characters));
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
