using StarWarsFilterApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

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
                string query = "SELECT id, name FROM characters";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        characters.Add(new Character
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
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
    }
}
