using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFilterApp.Model;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Linq;
using System.Reflection;

namespace StarWarsFilterApp.Services
{
    class FilmService
    {
        private readonly MySQLConnectionService _connectionService;

        public FilmService()
        {
            _connectionService = new MySQLConnectionService();
        }

        public List<Film> GetAllFilms()
        {
            var films = new List<Film>();
            using (var conn = _connectionService.GetConnection())
            {
                string query = "SELECT director, title, release_date, producer FROM films";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            films.Add(new Film
                            {
                                Director = reader.GetString("director"),
                                Title = reader.GetString("title"),
                                Release_date = reader.GetDateTime("release_date").ToString("yyyy-MM-dd"),
                                Producer = reader.GetString("producer"),
                            });
                        }
                    }
                }
            }
            return films;
        }

        

        public List<Film> GetFilteredFilms(string title, string producer, string director, string release_date)
        {
            var films = new List<Film>();
            using (var conn = _connectionService.GetConnection())
            {
                string query = "SELECT director, title, release_date, producer FROM films "+
                    "WHERE (title LIKE @title OR @title IS NULL) " +
                               "AND (producer LIKE @producer OR @producer IS NULL) " +
                               "AND (director LIKE @director OR @director IS NULL) " +
                               "AND (release_date = @release_date OR @release_date IS NULL)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", string.IsNullOrWhiteSpace(title) ? DBNull.Value : title);
                    cmd.Parameters.AddWithValue("@producer", string.IsNullOrWhiteSpace(producer) ? DBNull.Value : producer);
                    cmd.Parameters.AddWithValue("@director", string.IsNullOrWhiteSpace(director) ? DBNull.Value : director);
                    cmd.Parameters.AddWithValue("@release_date", string.IsNullOrWhiteSpace(release_date) ? DBNull.Value : DateTime.Parse(release_date));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            films.Add(new Film
                            {
                                Director = reader.GetString("director"),
                                Title = reader.GetString("title"),
                                Release_date = reader.GetDateTime("release_date").ToString("yyyy-MM-dd"),
                                Producer = reader.GetString("producer"),
                            });
                        }
                    }
                }
            }
            return films;
        }
    }
}
