using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Planiranje.Controllers;
using PagedList;

namespace Planiranje.Models
{
    public class Pedagog_DBHandle
    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public bool UpdatePedagog(Pedagog pedagog)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE pedagog " +
                        "SET " +
                        "lozinka = @lozinka, " +
                        "ime = @ime, " +
                        "prezime = @prezime " +
                        "WHERE email = @email";
                        
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@lozinka", pedagog.Lozinka);
                    command.Parameters.AddWithValue("@ime", pedagog.Ime);
                    command.Parameters.AddWithValue("@prezime", pedagog.Prezime);
                    command.Parameters.AddWithValue("@email", pedagog.Email);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                connection.Close();
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

    }
}