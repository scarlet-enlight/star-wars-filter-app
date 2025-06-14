using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFilterApp.Model
{
    internal class Film
    {
        public string Director { get; set; }
        public string Title { get; set; }
        public DateOnly Release_date { get; set; }
        public string Producer { get; set; }
        public string Opening_crawl { get; set; }

        Film()
        {
            Director = "";
            Title = "";
            Release_date = DateOnly.Parse("1900-01-01");
            Producer = "";
            Opening_crawl = "";
        }
    }
}
