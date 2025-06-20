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

        // Komendy do nawigacji i operacji
        public ICommand ReturnCommand { get; }
        public ICommand ClearTextFieldsCommand { get; }
        public ICommand FilterCommand { get; }

        // Serwis do pobierania postaci
        private readonly CharacterService _characterService;

        // Kolekcja postaci
        public ObservableCollection<Character> Characters { get; set; }

        // Powiązane właściwości
        private string _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _species;
        public string? Species
        {
            get => _species;
            set => SetProperty(ref _species, value);
        }

        private string _planet;
        public string? Planet
        {
            get => _planet;
            set => SetProperty(ref _planet, value);
        }

        private string _organization;
        public string? Organization
        {
            get => _organization;
            set => SetProperty(ref _organization, value);
        }

        private string _film;
        public string? Film
        {
            get => _film;
            set => SetProperty(ref _film, value);
        }
        private string _height;
        public string? Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        private string _gender;
        public string? Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        public FilterCharacterViewModel(MainWindowViewModel main)
        {
            _main = main;
            ReturnCommand = new RelayCommand(ReturnToStart);
            ClearTextFieldsCommand = new RelayCommand(ClearTextFields);
            FilterCommand = new RelayCommand(obj => FilterCharacters(Name, Species, Planet, Organization, Film, Height, Gender));


            // Inicjalizacja kolekcji postaci
            _characterService = new CharacterService();

            LoadCharacters();
        }

        // Ładuje wszystkie postacie z bazy danych
        private void LoadCharacters()
        {
            var result = _characterService.GetAllCharacters();
            Characters = new ObservableCollection<Character>(result);
            OnPropertyChanged(nameof(Characters));
        }

        /// Filtruje postacie na podstawie podanych kryteriów.
        public List<Character> FilterCharacters(string name, string species, string planet, string organization, string film, string height, string gender)
        {
            _name = name;
            _species = species;
            _planet = planet;
            _organization = organization;
            _film = film;
            _height = height;
            _gender = gender;

            IEnumerable<Character> result;

            if (_name != null)
            {
                // Jeśli podano imię, pobierz postać o tym imieniu
                result = new List<Character> { _characterService.GetCharacterByName(_name) };
            }
            else
            {
                // Jeśli nie podano imienia, pobierz wszystkie postacie lub zastosuj filtry
                result = _characterService.GetFilteredCharacters(species, planet, organization, film, gender, height);
            }

            Characters = new ObservableCollection<Character>(result);
            OnPropertyChanged(nameof(Characters));

            ClearTextFields(null);

            return Characters.ToList();
        }


        // Nawigacja do widoku StartView
        private void ReturnToStart(object? obj)
        {
            _main.CurrentView = new StartView(_main);
        }

        // Czyści pola tekstowe
        private void ClearTextFields(object? obj)
        {
            Name = null;
            Species = null;
            Planet = null;
            Organization = null;
            Film = null;
            Gender = null;
            Height = null;
        }
    }
}
