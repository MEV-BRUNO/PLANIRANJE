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
     public class Zadaci_DBHandle

    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public List<Zadaci> ReadZadaci()
        {
			int counter = 0;
            List<Zadaci> zadaci = new List<Zadaci>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_zadatak, naziv " +
                    "FROM zadaci " +
                    "ORDER BY id_zadatak ASC";
                //command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
							Zadaci zad = new Zadaci()
							{
								Red_br = ++counter,
                                ID_zadatak = Convert.ToInt32(sdr["id_zadatak"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                            zadaci.Add(zad);
                        }
                    }
                }
                connection.Close();
            }
            return zadaci;
        }

        public List<Zadaci> ReadZadaci(string search_string)
        {
            List<Zadaci> zadaci = new List<Zadaci>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_zadatak, naziv " +
                    "FROM zadaci " +
                    "WHERE naziv like '%" + search_string + "%' " +
                    "ORDER BY id_zadatak ASC";
                command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Zadaci zad = new Zadaci()
                            {
                                ID_zadatak = Convert.ToInt32(sdr["id_zadatak"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                            zadaci.Add(zad);
                        }
                    }
                }
                connection.Close();
            }
            return zadaci;
        }

        public Zadaci ReadZadaci(int _id)
        {
            Zadaci zadaci = new Zadaci();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_zadatak, naziv " +
                    "FROM zadaci " +
                    "WHERE id_zadatak = @id ";
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", _id);
                //command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            zadaci = new Zadaci()
                            {
                                ID_zadatak = Convert.ToInt32(sdr["id_zadatak"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                        }
                    }
                }
                connection.Close();
            }
            return zadaci;
        }

        public bool CreateZadaci(Zadaci zadaci)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO zadaci " +
                        "(naziv) " +
                        " VALUES (@naziv)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@naziv", zadaci.Naziv);
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

        public bool UpdateZadaci(Zadaci zadaci)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE zadaci " +
                        "SET " +
                        "naziv = @naziv " +
                        "WHERE id_zadatak = @id_zadatak";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_zadatak", zadaci.ID_zadatak);
                    command.Parameters.AddWithValue("@naziv", zadaci.Naziv);
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

        public bool DeleteZadaci(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM zadaci " +
						"WHERE id_zadatak = @id_zadatak";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_zadatak", id);
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