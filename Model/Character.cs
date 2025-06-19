using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFilterApp.Model
{
    public class Character
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public bool IsFavorite { get; set; }
        public enum GenderType { Female, Male, None }
        public GenderType Gender { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }
        public int Year_born { get; set; }
        public int Year_died { get; set; }
        public string Skin_color { get; set; }
        public string Eye_color { get; set; }
        public string Hair_color { get; set; }

        Character()
        {
            Name = "";
            Species = "";
            Gender = GenderType.None;
            Height = 0;
            Weight = 0;
            Description = "";
            Year_born = 0;
            Year_died = 0;
            Skin_color = "";
            Eye_color = "";
            Hair_color = "";
        }
    }
}
