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


        public Character GetCharacterByName(string name)
        {
            using (var conn = _connectionService.GetConnection())
            {
                string query = "SELECT id, name FROM characters WHERE name = @name";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Character
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                            };
                        }
                    }
                }
            }
            return null; // Return null if no character found
        }
        public List<Character> GetAllCharacters()
        {
            var characters = new List<Character>();
            using (var conn = _connectionService.GetConnection())
            {
                string query = "SELECT name, gender, height  FROM characters";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        characters.Add(new Character
                        {
                            Name = reader.GetString("name"),
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

        public List<Character> GetCharactersBySpecies(string species)
        {
            var characters = new List<Character>();
            using (var conn = _connectionService.GetConnection())
            {
                string query = "SELECT id, name, species FROM characters WHERE species = @species";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@species", species);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            characters.Add(new Character
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Species = reader.GetString("species")
                            });
                        }
                    }
                }
            }
            return characters;
        }

        public List<Character> GetFilteredCharacters(string gender, string Height)
        {
            float height = 0f;
            if (!string.IsNullOrWhiteSpace(Height))
            {
                float.TryParse(Height, NumberStyles.Any, CultureInfo.CurrentCulture, out height);
            }

            var characters = new List<Character>();
            using (var conn = _connectionService.GetConnection())
            {

                string query = "SELECT name, gender, height FROM characters WHERE gender=@gender OR (height BETWEEN @height - 0.01 AND @height + 0.01)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@height", height);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            characters.Add(new Character
                            {
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
    }
}
