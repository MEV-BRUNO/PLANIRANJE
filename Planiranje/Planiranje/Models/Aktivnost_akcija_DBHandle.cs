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
    public class Aktivnost_akcija_DBHandle
    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public List<Aktivnost_akcija> ReadAktivnostAkcija()
        {

            List<Aktivnost_akcija> aktivnost_akcija = new List<Aktivnost_akcija>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_akcija, naziv,id_aktivnost " +
                    "FROM aktivnost_akcija " +
                    "ORDER BY id_akcija ASC";
                //command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Aktivnost_akcija akt = new Aktivnost_akcija()
                            {

                                Id_akcija = Convert.ToInt32(sdr["id_akcija"]),
                                Naziv = sdr["naziv"].ToString(),
                                Id_aktivnost = Convert.ToInt32(sdr["id_aktivnost"])
                            };
                            aktivnost_akcija.Add(akt);
                        }
                    }
                }
                connection.Close();
            }
            return aktivnost_akcija;
        }

        public List<Aktivnost_akcija> ReadAktivnostAkcija(string search_string)
        {
            List<Aktivnost_akcija> aktivnost_akcija = new List<Aktivnost_akcija>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_akcija, naziv, id_aktivnost " +
                    "FROM aktivnost_akcija " +
                    "WHERE naziv like '%" + search_string + "%' " +
                    "ORDER BY id_akcija ASC";
                command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            Aktivnost_akcija akt = new Aktivnost_akcija()
                            {
                                Id_akcija = Convert.ToInt32(sdr["id_akcija"]),
                                Naziv = sdr["naziv"].ToString(),
                                Id_aktivnost = Convert.ToInt32(sdr["id_aktivnost"])
                            };
                            aktivnost_akcija.Add(akt);
                        }
                    }
                }
                connection.Close();
            }
            return aktivnost_akcija;
        }

        public Aktivnost_akcija ReadAktivnostAkcija(int _id)
        {
            Aktivnost_akcija akcija = new Aktivnost_akcija();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_akcija, naziv,id_aktivnost " +
                    "FROM aktivnost_akcija " +
                    "WHERE id_akcija = @id ";
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", _id);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            akcija = new Aktivnost_akcija()
                            {
                                Id_akcija = Convert.ToInt32(sdr["id_akcija"]),
                                Naziv = sdr["naziv"].ToString(),
                                Id_aktivnost = Convert.ToInt32(sdr["id_aktivnost"])
                            };
                        }
                    }
                }
                connection.Close();
            }
            return akcija;
        }

        public bool CreateAktivnostAkcija(Aktivnost_akcija akcija)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO aktivnost_akcija " +
						"(id_aktivnost, naziv) " +
						" VALUES (@id_aktivnost, @naziv)";
                    command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_aktivnost", akcija.Id_aktivnost);
					command.Parameters.AddWithValue("@naziv", akcija.Naziv);
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

        public bool UpdateAktivnostAkcija(Aktivnost_akcija akcija)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE aktivnost_akcija " +
                        "SET " +
						"naziv = @naziv, " +
						"id_aktivnost = @id_aktivnost " +
						"WHERE id_akcija = @id_akcija";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_akcija", akcija.Id_akcija);
                    command.Parameters.AddWithValue("@naziv", akcija.Naziv);
                    command.Parameters.AddWithValue("@id_aktivnost", akcija.Id_aktivnost);
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

        public bool DeleteAktivnostAkcija(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM aktivnost_akcija " +
                        "WHERE id_akcija = @id_akcija";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_akcija", id);
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
		public List<Aktivnost> ReadAktivnosti()
		{
			List<Aktivnost> aktivnosti = new List<Aktivnost>();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id_aktivnost, naziv " +
					"FROM aktivnost " +
					"ORDER BY naziv ASC";
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Aktivnost akt = new Aktivnost()
							{
								Naziv = sdr["naziv"].ToString(),
								Id_aktivnost = Convert.ToInt32(sdr["id_aktivnost"])
							};
							aktivnosti.Add(akt);
						}
					}
				}
				connection.Close();
			}
			return aktivnosti;
		}
	}
}