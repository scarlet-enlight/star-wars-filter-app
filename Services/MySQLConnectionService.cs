using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace StarWarsFilterApp.Services
{
    public class MySQLConnectionService
    {
        private readonly string _connStr = "server=127.0.0.1;user=root;database=star_wars;port=3306;password=";

        public MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(_connStr);
            conn.Open();
            return conn;
        }
    }
}
