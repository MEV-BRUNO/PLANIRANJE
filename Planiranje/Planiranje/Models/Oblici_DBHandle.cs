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
    public class Oblici_DBHandle
    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public List<Oblici> ReadOblici()
        {
			int counter = 0;
            List<Oblici> oblici = new List<Oblici>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_oblici, naziv, vrsta " +
                    "FROM oblici WHERE vrsta IN (0,@id_pedagog) " +                    
                    "ORDER BY id_oblici ASC";
                command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
							Oblici oblik = new Oblici()
							{
								Red_br = ++counter,
                                Id_oblici = Convert.ToInt32(sdr["id_oblici"]),
                                Naziv = sdr["naziv"].ToString(),
                                Vrsta = Convert.ToInt32(sdr["vrsta"])
                            };
                            oblici.Add(oblik);
                        }
                    }
                }
                connection.Close();
            }
            return oblici;
        }

        public List<Oblici> ReadOblici(string search_string)
        {
            List<Oblici> oblici = new List<Oblici>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_oblici, naziv " +
                    "FROM oblici " +                    
                    "WHERE naziv like '%" + search_string + "%' " +                    
                    "ORDER BY id_oblici ASC";                
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Oblici oblik = new Oblici()
                            {
                                Id_oblici = Convert.ToInt32(sdr["id_oblici"]),
                                Naziv = sdr["naziv"].ToString()                               
                            };
                            oblici.Add(oblik);
                        }
                    }
                }
                connection.Close();
            }
            return oblici;
        }

        public Oblici ReadOblici(int _id)
        {
            Oblici oblik = new Oblici();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_oblici, naziv, vrsta " +
                    "FROM oblici " +
                    "WHERE id_oblici = @id";                    
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", _id);                
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            oblik = new Oblici()
                            {
                                Id_oblici = Convert.ToInt32(sdr["id_oblici"]),                               
                                Naziv = sdr["naziv"].ToString()                                
                            };
                        }
                    }
                }
                connection.Close();
            }
            return oblik;
        }

        public bool CreateOblici(Oblici oblik)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO oblici " +
                        "(naziv, vrsta) " +
                        " VALUES (@naziv, @id_pedagog)";
                    command.CommandType = CommandType.Text;                    
                    command.Parameters.AddWithValue("@naziv", oblik.Naziv);
                    command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
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

        public bool UpdateOblici(Oblici oblik)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE oblici " +
                        "SET " +
                        "naziv = @naziv " +
                        "WHERE id_oblici = @id_oblici";                        
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_oblici", oblik.Id_oblici); 
                    command.Parameters.AddWithValue("@naziv", oblik.Naziv);                    
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

        public bool DeleteOblici(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM oblici " +
                        "WHERE id_oblici = @id_oblici";                        
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_oblici", id);                    
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