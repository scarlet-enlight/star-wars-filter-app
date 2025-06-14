using StarWarsFilterApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StarWarsFilterApp.ViewModel
{
    public class FilterFilmViewModel : BaseViewModel
    {
        public ICommand ReturnCommand { get; }

        private readonly MainWindowViewModel _main;

        public FilterFilmViewModel(MainWindowViewModel main)
        {
            _main = main;
            ReturnCommand = new RelayCommand(ReturnToStart);
        }

        private void ReturnToStart(object? obj)
        {
            _main.CurrentView = new StartView(_main);
        }

    }
}
