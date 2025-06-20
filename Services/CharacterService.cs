using StarWarsFilterApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Media.Media3D;
using System.Globalization;

namespace StarWarsFilterApp.Services
{
    class CharacterService
    {
        private readonly MySQLConnectionService _connectionService;

        public CharacterService()
        {
            _connectionService = new MySQLConnectionService();
        }

        // Metoda dla wyszukiwania postaci po imieniu
        public Character GetCharacterByName(string name)
        {
            using (var conn = _connectionService.GetConnection())
            {
                string query = "SELECT c.name AS character_name, s.name AS species_name, gender, height " +
                    "FROM characters AS c " +
                    "INNER JOIN species AS s ON c.species_id = s.id " +
                    "WHERE c.name LIKE @name";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", $"%{name}%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Character
                            {
                                Name = reader.GetString("character_name"),
                                Species = reader.GetString("species_name"),
                                Gender = Enum.TryParse<Character.GenderType>(
                                     reader.GetString("gender"),
                                     out var gender) ? gender : Character.GenderType.None,
                                Height = reader.GetFloat("height")
                            };
                        }
                    }
                }
            }
            return null; // Zwróć null, jeśli nie znaleziono postaci
        }

        // Metoda dla pobierania wszystkich postaci
        public List<Character> GetAllCharacters()
        {
            var characters = new List<Character>();
            using (var conn = _connectionService.GetConnection())
            {
                string query = "SELECT c.name AS character_name, s.name AS species_name, gender, height " +
                    "FROM characters AS c " +
                    "INNER JOIN species AS s ON c.species_id = s.id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            characters.Add(new Character
                            {
                                Name = reader.GetString("character_name"),
                                Species = reader.GetString("species_name"),
                                Gender = Enum.TryParse<Character.GenderType>(
                                     reader.GetString("gender"),
                                     out var gender) ? gender : Character.GenderType.None,
                                Height = reader.GetFloat("height")
                            });
                        }
                    }
                }
                return characters;
            }
        }

        // Metoda dla filtrowania postaci na podstawie filtrów
        public List<Character> GetFilteredCharacters(string species, string planet, string organization, string film,
            string gender, string height)
        {
            float parsedHeight = 0f;
            if (!string.IsNullOrWhiteSpace(height))
            {
                float.TryParse(height, NumberStyles.Any, CultureInfo.CurrentCulture, out parsedHeight);
            }
            else
            {
                parsedHeight = 0f; // Domyślny wzrost, jeśli nie podano
            }

            var characters = new List<Character>();

            using (var conn = _connectionService.GetConnection())
            {
                // Zapytanie SQL z parametrami dla płci i wzrostu
                string query = "SELECT c.name AS character_name, s.name AS species_name, gender, height " +
                    "FROM characters AS c " +
                    "INNER JOIN species AS s " +
                    "ON c.species_id = s.id " +
                    "WHERE (@species IS NULL OR s.name = @species) " +
                    "AND (@gender IS NULL OR gender = @gender) " +
                    "AND (@height = 0 OR ABS(height - @height) < 0.001) ";
                    
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@gender", string.IsNullOrWhiteSpace(gender) ? DBNull.Value : gender);
                    cmd.Parameters.AddWithValue("@species", string.IsNullOrWhiteSpace(species) ? DBNull.Value : species);
                    cmd.Parameters.AddWithValue("@height", parsedHeight);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            characters.Add(new Character
                            {
                                Name = reader.GetString("character_name"),
                                Species = reader.GetString("species_name"),
                                Gender = Enum.TryParse<Character.GenderType>(reader.GetString("gender"), out var parsedGender)
                                    ? parsedGender
                                    : Character.GenderType.None,
                                Height = reader.GetFloat("height")
                            });
                        }
                    }
                }
                return characters;
            }
        }

        //public List<Character> GetCharactersBySpecies(string species)
        //{
        //    var characters = new List<Character>();
        //    using (var conn = _connectionService.GetConnection())
        //    {
        //        string query = "SELECT id, name, species FROM characters WHERE species = @species";
        //        using (var cmd = new MySqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@species", species);

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    characters.Add(new Character
        //                    {
        //                        Id = reader.GetInt32("id"),
        //                        Name = reader.GetString("name"),
        //                        Species = reader.GetString("species")
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    return characters;
        // }
    }
}
