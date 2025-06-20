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
                string query =
                    "SELECT " +
                    "  c.name AS character_name, " +
                    "  s.name AS species_name, " +
                    "  p.name AS character_homeworld, " +
                    "  c.gender, " +
                    "  c.height, " +
                    "  (SELECT GROUP_CONCAT(DISTINCT o.name) " +
                    "   FROM character_organization co " +
                    "   JOIN organizations o ON co.organization_id = o.id " +
                    "   WHERE co.character_id = c.id) AS organization, " +
                    "  (SELECT GROUP_CONCAT(DISTINCT f.title) " +
                    "   FROM character_film cf " +
                    "   JOIN films f ON cf.film_id = f.id " +
                    "   WHERE cf.character_id = c.id) AS film_title " +
                    "FROM characters c " +
                    "JOIN species s ON c.species_id = s.id " +
                    "LEFT JOIN planets p ON c.homeworld_id = p.id " +
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
                                Name = reader.IsDBNull(reader.GetOrdinal("character_name")) ? null : reader.GetString("character_name"),
                                Species = reader.IsDBNull(reader.GetOrdinal("species_name")) ? null : reader.GetString("species_name"),
                                Planet = reader.IsDBNull(reader.GetOrdinal("character_homeworld")) ? null : reader.GetString("character_homeworld"),
                                Organization = reader.IsDBNull(reader.GetOrdinal("organization")) ? null : reader.GetString("organization"),
                                Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? Character.GenderType.None : Enum.TryParse<Character.GenderType>(reader.GetString("gender"), out var parsedGender) ? parsedGender : Character.GenderType.None,
                                Height = reader.IsDBNull(reader.GetOrdinal("height")) ? 0f : reader.GetFloat("height"),
                                Film = reader.IsDBNull(reader.GetOrdinal("film_title")) ? null : reader.GetString("film_title"),
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
                string query =
                        "SELECT " +
                        "  c.name AS character_name, " +
                        "  s.name AS species_name, " +
                        "  p.name AS character_homeworld, " +
                        "  ( " +
                        "    SELECT GROUP_CONCAT(f.title) " +
                        "    FROM character_film cf " +
                        "    JOIN films f ON f.id = cf.film_id " +
                        "    WHERE cf.character_id = c.id " +
                        "  ) AS film_title, " +
                        "  ( " +
                        "    SELECT GROUP_CONCAT(o.name) " +
                        "    FROM character_organization co " +
                        "    JOIN organizations o ON o.id = co.organization_id " +
                        "    WHERE co.character_id = c.id " +
                        "  ) AS organization, " +
                        "  c.gender, " +
                        "  c.height " +
                        "FROM characters c " +
                        "JOIN species s ON c.species_id = s.id " +
                        "LEFT JOIN planets p ON c.homeworld_id = p.id";


                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            characters.Add(new Character
                            {
                                Name = reader.IsDBNull(reader.GetOrdinal("character_name")) ? null : reader.GetString("character_name"),
                                Species = reader.IsDBNull(reader.GetOrdinal("species_name")) ? null : reader.GetString("species_name"),
                                Planet = reader.IsDBNull(reader.GetOrdinal("character_homeworld")) ? null : reader.GetString("character_homeworld"),
                                Organization = reader.IsDBNull(reader.GetOrdinal("organization")) ? null : reader.GetString("organization"),
                                Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? Character.GenderType.None : Enum.TryParse<Character.GenderType>(reader.GetString("gender"), out var parsedGender) ? parsedGender : Character.GenderType.None,
                                Height = reader.IsDBNull(reader.GetOrdinal("height")) ? 0f : reader.GetFloat("height"),
                                Film = reader.IsDBNull(reader.GetOrdinal("film_title")) ? null : reader.GetString("film_title"),
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
                string query =
                    "SELECT " +
                    "  c.name AS character_name, " +
                    "  s.name AS species_name, " +
                    "  p.name AS character_homeworld, " +
                    "  c.gender, " +
                    "  c.height, " +
                    "  ( " +
                    "    SELECT GROUP_CONCAT(DISTINCT o2.name) " +
                    "    FROM character_organization co2 " +
                    "    JOIN organizations o2 ON co2.organization_id = o2.id " +
                    "    WHERE co2.character_id = c.id " +
                    "    AND (@organization IS NULL OR o2.name = @organization) " +
                    "  ) AS organization, " +
                    "  ( " +
                    "    SELECT GROUP_CONCAT(DISTINCT f2.title) " +
                    "    FROM character_film cf2 " +
                    "    JOIN films f2 ON cf2.film_id = f2.id " +
                    "    WHERE cf2.character_id = c.id " +
                    "    AND (@film IS NULL OR f2.title = @film) " +
                    "  ) AS film_title " +
                    "FROM characters c " +
                    "JOIN species s ON c.species_id = s.id " +
                    "LEFT JOIN planets p ON c.homeworld_id = p.id " +
                    "WHERE (@species IS NULL OR s.name = @species) " +
                    "  AND (@gender IS NULL OR c.gender = @gender) " +
                    "  AND (@height = 0 OR ABS(c.height - @height) < 0.001) " +
                    "  AND (@planet IS NULL OR p.name = @planet) " +
                    "  AND ( " +
                    "      @organization IS NULL " +
                    "      OR EXISTS ( " +
                    "          SELECT 1 FROM character_organization co3 " +
                    "          JOIN organizations o3 ON co3.organization_id = o3.id " +
                    "          WHERE co3.character_id = c.id AND o3.name LIKE @organization " +
                    "      ) " +
                    "  ) " +
                    "  AND ( " +
                    "      @film IS NULL " +
                    "      OR EXISTS ( " +
                    "          SELECT 1 FROM character_film cf3 " +
                    "          JOIN films f3 ON cf3.film_id = f3.id " +
                    "          WHERE cf3.character_id = c.id AND f3.title LIKE @film " +
                    "      ) " +
                    "  )";


                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@gender", string.IsNullOrWhiteSpace(gender) ? DBNull.Value : gender);
                    cmd.Parameters.AddWithValue("@species", string.IsNullOrWhiteSpace(species) ? DBNull.Value : species);
                    cmd.Parameters.AddWithValue("@height", parsedHeight);
                    cmd.Parameters.AddWithValue("@planet", string.IsNullOrWhiteSpace(planet) ? DBNull.Value : planet);
                    cmd.Parameters.AddWithValue("@organization", string.IsNullOrWhiteSpace(organization) ? DBNull.Value : $"%{organization}%");
                    cmd.Parameters.AddWithValue("@film", string.IsNullOrWhiteSpace(film) ? DBNull.Value : $"%{film}%");


                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            characters.Add(new Character
                            {
                                Name = reader.IsDBNull(reader.GetOrdinal("character_name")) ? null : reader.GetString("character_name"),
                                Species = reader.IsDBNull(reader.GetOrdinal("species_name")) ? null : reader.GetString("species_name"),
                                Planet = reader.IsDBNull(reader.GetOrdinal("character_homeworld")) ? null : reader.GetString("character_homeworld"),
                                Organization = reader.IsDBNull(reader.GetOrdinal("organization")) ? null : reader.GetString("organization"),
                                Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? Character.GenderType.None : Enum.TryParse<Character.GenderType>(reader.GetString("gender"), out var parsedGender) ? parsedGender : Character.GenderType.None,
                                Height = reader.IsDBNull(reader.GetOrdinal("height")) ? 0f : reader.GetFloat("height"),
                                Film = reader.IsDBNull(reader.GetOrdinal("film_title")) ? null : reader.GetString("film_title"),

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
