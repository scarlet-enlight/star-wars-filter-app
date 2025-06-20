using StarWarsFilterApp.View;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StarWarsFilterApp.Model;
using StarWarsFilterApp.Services;
using System.Numerics;
using ZstdSharp.Unsafe;
using MySqlX.XDevAPI.Common;

namespace StarWarsFilterApp.ViewModel
{
    public class FilterFilmViewModel : BaseViewModel
    {
        public ICommand ReturnCommand { get; }
        public ICommand ClearTextFieldsCommand { get; }
        public ICommand FilterCommand { get; }

        private readonly FilmService _filmService;

        public ObservableCollection<Film> Films { get; set; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _producer;
        public string Producer
        {
            get => _producer;
            set => SetProperty(ref _producer, value);
        }

        private string _director;
        public string Director
        {
            get => _director;
            set => SetProperty(ref _director, value);
        }
        private string _release_date;
        public string Release_date
        {
            get => _release_date;
            set => SetProperty(ref _release_date, value);
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

        private readonly MainWindowViewModel _main;



        public FilterFilmViewModel(MainWindowViewModel main)
        {
            _main = main;
            ReturnCommand = new RelayCommand(ReturnToStart);
            FilterCommand = new RelayCommand(obj => FilterFilms(Title, Producer, Director, Release_date));
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

        public List<Film> FilterFilms(string title, string producer, string director, string release_date)
        {
            _title = title;
            _producer = producer;
            _director = director;
            _release_date = release_date;

            var result = _filmService.GetFilteredFilms(title, producer, director, release_date); 

            Films = new ObservableCollection<Film>(result);
            OnPropertyChanged(nameof(Films));

            ClearTextFields(null);

            return Films.ToList();
        }

        private void ReturnToStart(object? obj)
        {
            _main.CurrentView = new StartView(_main);
        }

        private void ClearTextFields(object? obj)
        {
            Title = null;
            Director= null;
            Producer = null;
            Release_date = null;
        }

        
    }
}
