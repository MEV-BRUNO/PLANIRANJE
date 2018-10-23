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
    public class Subjekt_DBHandle
    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public List<Subjekti> ReadSubjekti()
        {
			int counter = 0;
            List<Subjekti> subjekti = new List<Subjekti>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_subjekt, naziv, vrsta " +
                    "FROM subjekti WHERE vrsta IN (0, @id_pedagog) " +
                    "ORDER BY id_subjekt ASC";
                command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Subjekti subj = new Subjekti()
                            {
								Red_br = ++counter,
                                ID_subjekt = Convert.ToInt32(sdr["id_subjekt"]),
                                Naziv = sdr["naziv"].ToString(),
                                Vrsta = Convert.ToInt32(sdr["vrsta"])
                            };
                            subjekti.Add(subj);
                        }
                    }
                }
                connection.Close();
            }
            return subjekti;
        }

        public List<Subjekti> ReadSubjekti(string search_string)
        {
            List<Subjekti> subjekti = new List<Subjekti>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_subjekt, naziv " +
                    "FROM subjekti " +
                    "WHERE naziv like '%" + search_string + "%' " +
                    "ORDER BY id_subjekt ASC";
                command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Subjekti subj = new Subjekti()
                            {
                                ID_subjekt = Convert.ToInt32(sdr["id_subjekt"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                            subjekti.Add(subj);
                        }
                    }
                }
                connection.Close();
            }
            return subjekti;
        }

        public Subjekti ReadSubjekti(int _id)
        {
            Subjekti subjekti = new Subjekti();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_subjekt, naziv, vrsta " +
                    "FROM subjekti " +
                    "WHERE id_subjekt = @id ";
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
                            subjekti = new Subjekti()
                            {
                                ID_subjekt = Convert.ToInt32(sdr["id_subjekt"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                        }
                    }
                }
                connection.Close();
            }
            return subjekti;
        }

		public bool CreateSubjekti(Subjekti subjekt)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					command.CommandText = "INSERT INTO subjekti " +
						"(naziv, vrsta) " +
						" VALUES (@naziv, @id_pedagog)";
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@naziv", subjekt.Naziv);
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

        public bool UpdateSubjekti(Subjekti subjekti)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE subjekti " +
                        "SET " +
                        "naziv = @naziv " +
                        "WHERE id_subjekt = @id_subjekt";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_subjekt", subjekti.ID_subjekt);
                    command.Parameters.AddWithValue("@naziv", subjekti.Naziv);
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

        public bool DeleteSubjekti(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM subjekti " +
                        "WHERE id_subjekt = @id_subjekt";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_subjekt", id);
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