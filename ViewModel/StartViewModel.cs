using StarWarsFilterApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StarWarsFilterApp.ViewModel
{
    public class StartViewModel : BaseViewModel
    {
        private readonly MainWindowViewModel _main;

        public ICommand ShowCharactersCommand { get; }
        public ICommand ShowFilmsCommand { get; }

        public StartViewModel(MainWindowViewModel main)
        {
            _main = main;

            ShowCharactersCommand = new RelayCommand(ShowCharacters);
            ShowFilmsCommand = new RelayCommand(ShowFilms);
        }

        private void ShowCharacters(object? obj)
        {
            _main.CurrentView = new FilterCharacterView(_main);
        }

        private void ShowFilms(object? obj)
        {
            _main.CurrentView = new FilterFilmView(_main);
        }
    }

}
