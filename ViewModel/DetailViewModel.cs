using StarWarsFilterApp.Model;
using StarWarsFilterApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StarWarsFilterApp.ViewModel
{
    public class DetailViewModel : BaseViewModel
    {
        private readonly MainWindowViewModel _main;
        public ICommand BackCommand { get; }

        // właściwości do wyświetlenia
        public string Name { get; }
        public string Species { get; }
        public string Planet { get; }
        public string Organization { get; }
        public string Film { get; }
        public string Gender { get; }
        public string Description { get; }
        public string Height { get; }
        public string Weight { get; }
        public string HairColor { get; }
        public string EyeColor { get; }
        public string SkinColor { get; }
        public string YearBorn { get; }
        public string YearDied { get; }

        public DetailViewModel(MainWindowViewModel main, Character ch)
        {
            _main = main;
            Name = ch.Name;
            Species = ch.Species;
            Planet = ch.Planet;
            Organization = ch.Organization;
            Film = ch.Film;
            Gender = ch.Gender.ToString();
            Description = ch.Description;
            Height = ch.Height.ToString();
            Weight = ch.Weight.ToString();
            HairColor = ch.Hair_color;
            EyeColor = ch.Eye_color;
            SkinColor = ch.Skin_color;
            YearBorn = ch.Year_born.ToString() ?? "";
            YearDied = ch.Year_died.ToString() ?? "";

            BackCommand = new RelayCommand(_ => _main.CurrentView = new FilterCharacterView(_main));
        }
    }

}
