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
    public class Ciljevi_DBHandle
    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public List<Ciljevi> ReadCiljevi()
        {
			int counter = 0;
            List<Ciljevi> ciljevi = new List<Ciljevi>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_cilj, naziv " +
                    "FROM ciljevi " +
                    "ORDER BY id_cilj ASC";
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Ciljevi cilj = new Ciljevi()
                            {
								Red_br = ++counter,
                                ID_cilj = Convert.ToInt32(sdr["id_cilj"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                            ciljevi.Add(cilj);
                        }
                    }
                }
                connection.Close();
            }
            return ciljevi;
        }

        public List<Ciljevi> ReadCiljevi(string search_string)
        {
            List<Ciljevi> ciljevi = new List<Ciljevi>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_cilj, naziv " +
                    "FROM ciljevi " +
                    "WHERE naziv like '%" + search_string + "%' " +
                    "ORDER BY id_cilj ASC";                
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Ciljevi cilj = new Ciljevi()
                            {
                                ID_cilj = Convert.ToInt32(sdr["id_cilj"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                            ciljevi.Add(cilj);
                        }
                    }
                }
                connection.Close();
            }
            return ciljevi;
        }

        public Ciljevi ReadCiljevi(int _id)
        {
            Ciljevi ciljevi = new Ciljevi();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_cilj, naziv " +
                    "FROM ciljevi " +
                    "WHERE id_cilj = @id ";
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", _id);                
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            ciljevi = new Ciljevi()
                            {
                                ID_cilj = Convert.ToInt32(sdr["id_cilj"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                        }
                    }
                }
                connection.Close();
            }
            return ciljevi;
        }

        public bool CreateCiljevi(Ciljevi cilj)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO ciljevi " +
                        "(naziv) " +
                        " VALUES (@naziv)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@naziv", cilj.Naziv);
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

        public bool UpdateCiljevi(Ciljevi cilj)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE ciljevi " +
                        "SET " +
                        "naziv = @naziv " +
                        "WHERE id_cilj = @id_cilj";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_cilj", cilj.ID_cilj);
                    command.Parameters.AddWithValue("@naziv", cilj.Naziv);
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

        public bool DeleteCiljevi(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM ciljevi " +
                        "WHERE id_cilj = @id_cilj";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_cilj", id);
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