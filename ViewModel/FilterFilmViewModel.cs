using StarWarsFilterApp.View;
using System.Windows.Input;

namespace StarWarsFilterApp.ViewModel
{
    public class FilterFilmViewModel : BaseViewModel
    {
        public ICommand ReturnCommand { get; }
        public ICommand ClearTextFieldsCommand { get; }

        private readonly MainWindowViewModel _main;

        public FilterFilmViewModel(MainWindowViewModel main)
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
