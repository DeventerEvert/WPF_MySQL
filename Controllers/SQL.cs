using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace WPF_MySQL.Controllers
{
    public class SQL
    {

        // save the connection object as public
        private MySqlConnection _connection;

        public void closeConnection()
        {
            _connection.Close();
        }

        private void openConnection()
        {
            if (_connection == null)
            {
                _connection = new MySqlConnection();
                _connection.ConnectionString = "Server=localhost;User ID=root; Password=; Database=quiztime; ConvertZeroDateTime=True";
                _connection.Open();
            }

        }

        public MySqlConnection Connection
        {
            get
            {
                openConnection();
                return _connection;
            }
        }
    }
}
