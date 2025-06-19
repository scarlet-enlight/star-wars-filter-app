using StarWarsFilterApp.View;
using System;
using System.Windows;
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
        public ICommand ExitCommand { get; }

        public StartViewModel(MainWindowViewModel main)
        {
            _main = main;

            ShowCharactersCommand = new RelayCommand(ShowCharacters);
            ShowFilmsCommand = new RelayCommand(ShowFilms);
            ExitCommand = new RelayCommand(Exit);
        }

        private void ShowCharacters(object? obj)
        {
            _main.CurrentView = new FilterCharacterView(_main);
        }

        private void ShowFilms(object? obj)
        {
            _main.CurrentView = new FilterFilmView(_main);
        }
        private void Exit(object? obj)
        {
            Application.Current.Shutdown();
        }

    }

}
