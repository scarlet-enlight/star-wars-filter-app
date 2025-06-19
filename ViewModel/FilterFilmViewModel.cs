using StarWarsFilterApp.View;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StarWarsFilterApp.Model;
using StarWarsFilterApp.Services;

namespace StarWarsFilterApp.ViewModel
{
    public class FilterFilmViewModel : BaseViewModel
    {
        public ICommand ReturnCommand { get; }
        public ICommand ClearTextFieldsCommand { get; }

        private readonly MainWindowViewModel _main;

        public ObservableCollection<Film> Films { get; set; }

        private readonly FilmService _filmService;

        public FilterFilmViewModel(MainWindowViewModel main)
        {
            _main = main;
            ReturnCommand = new RelayCommand(ReturnToStart);
            ClearTextFieldsCommand = new RelayCommand(ClearTextFields);

            _filmService = new FilmService();
            LoadFilms();
        }

        private void LoadFilms()
        {
            // Pobranie wszystkich postaci z serwisu i przypisanie do kolekcji
            var result = _filmService.GetAllFilms();
            Films = new ObservableCollection<Film>(result);
            OnPropertyChanged(nameof(Films));
        }

        private void ReturnToStart(object? obj)
        {
            _main.CurrentView = new StartView(_main);
        }

        private void ClearTextFields(object? obj)
        {
            Title = string.Empty;
            Music = string.Empty;
            Battles = string.Empty;
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _music;
        public string Music
        {
            get => _music;
            set => SetProperty(ref _music, value);
        }

        private string _battles;
        public string Battles
        {
            get => _battles;
            set => SetProperty(ref _battles, value);
        }
    }
}
