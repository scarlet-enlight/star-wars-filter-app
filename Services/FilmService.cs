using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWarsFilterApp.Model;
using MySql.Data.MySqlClient;

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
                string query = "SELECT id, title FROM films";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        films.Add(new Film
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                        });
                    }
                }
            }
            return films;
        }

        public List<Film> GetFilmsByTitles(string title)
        {
            var films = new List<Film>();
            using (var conn = _connectionService.GetConnection())
            {
                string query = "SELECT id, title FROM films WHERE title = @title";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            films.Add(new Film
                            {
                                Id = reader.GetInt32("id"),
                                Title = reader.GetString("title")
                            });
                        }
                    }
                }
            }
            return films;
        }
    }
}
