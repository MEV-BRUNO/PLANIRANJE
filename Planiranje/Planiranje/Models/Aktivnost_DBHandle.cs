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
    public class Aktivnost_DBHandle
    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public List<Aktivnost> ReadAktivnost()
        {
            List<Aktivnost> aktivnost = new List<Aktivnost>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_aktivnost, naziv " +
                    "FROM aktivnost " +
                    "ORDER BY id_aktivnost ASC";
                //command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Aktivnost akt = new Aktivnost()
                            {
                                Id_aktivnost = Convert.ToInt32(sdr["id_aktivnost"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                            aktivnost.Add(akt);
                        }
                    }
                }
                connection.Close();
            }
            return aktivnost;
        }

        public List<Aktivnost> ReadAktivnost(string search_string)
        {
            List<Aktivnost> aktivnost = new List<Aktivnost>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_aktivnost, naziv " +
                    "FROM aktivnost " +
                    "WHERE naziv like '%" + search_string + "%' " +
                    "ORDER BY id_aktivnost ASC";
                command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Aktivnost akt = new Aktivnost()
                            {
                                Id_aktivnost = Convert.ToInt32(sdr["id_podrucje"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                            aktivnost.Add(akt);
                        }
                    }
                }
                connection.Close();
            }
            return aktivnost;
        }

        public Aktivnost ReadAktivnost(int _id)
        {
            Aktivnost aktivnost = new Aktivnost();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_aktivnost, naziv " +
                    "FROM aktivnost " +
                    "WHERE id_aktivnost = @id ";
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
                            aktivnost = new Aktivnost()
                            {
                                Id_aktivnost = Convert.ToInt32(sdr["id_aktivnost"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                        }
                    }
                }
                connection.Close();
            }
            return aktivnost;
        }

        public bool CreateAktivnost(Aktivnost aktivnost)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO aktivnost " +
                        "(naziv) " +
                        " VALUES (@naziv)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@naziv", aktivnost.Naziv);
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

        public bool UpdateAktivnost(Aktivnost aktivnost)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE aktivnost " +
                        "SET " +
                        "naziv = @naziv " +
                        "WHERE id_aktivnost = @id_aktivnost";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_aktivnost", aktivnost.Id_aktivnost);
                    command.Parameters.AddWithValue("@naziv", aktivnost.Naziv);
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

        public bool DeleteAktivnost(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM aktivnost " +
                        "WHERE id_aktivnost = @id_aktivnost";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_aktivnost", id);
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