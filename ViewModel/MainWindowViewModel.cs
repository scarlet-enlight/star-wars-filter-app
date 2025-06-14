using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFilterApp.View;

namespace StarWarsFilterApp.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainWindowViewModel()
        {
            CurrentView = new StartView(this); // ← przekazujemy referencję do siebie
        }
    }
}
