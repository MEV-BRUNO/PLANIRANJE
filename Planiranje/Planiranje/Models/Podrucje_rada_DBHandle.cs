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
	public class Podrucje_rada_DBHandle
	{
		private MySqlConnection connection;

		private void Connect()
		{
			string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
			connection = new MySqlConnection(connection_string);
		}

		public List<Podrucje_rada> ReadPodrucjeRada()
		{
			int counter = 0;
			List<Podrucje_rada> podrucje_rada = new List<Podrucje_rada>();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id_podrucje, naziv, vrsta " +
					"FROM podrucje_rada WHERE vrsta IN (0, @id_pedagog) " +					
					"ORDER BY id_podrucje ASC";
                command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Podrucje_rada rad = new Podrucje_rada()
							{
								Red_br = ++counter,
								Id_podrucje = Convert.ToInt32(sdr["id_podrucje"]),
								Naziv = sdr["naziv"].ToString(),
                                Vrsta = Convert.ToInt32(sdr["vrsta"])
							};
							podrucje_rada.Add(rad);
						}
					}
				}
				connection.Close();
			}
			return podrucje_rada;
		}

		public List<Podrucje_rada> ReadPodrucjeRada(string search_string)
		{
			List<Podrucje_rada> podrucje_rada = new List<Podrucje_rada>();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id_podrucje, naziv " +
					"FROM podrucje_rada " +					
					"WHERE naziv like '%" + search_string + "%' " +					
					"ORDER BY id_podrucje ASC";
				
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Podrucje_rada rad = new Podrucje_rada()
							{
								Id_podrucje = Convert.ToInt32(sdr["id_podrucje"]),
								Naziv = sdr["naziv"].ToString()								
							};
							podrucje_rada.Add(rad);
						}
					}
				}
				connection.Close();
			}
			return podrucje_rada;
		}

		public Podrucje_rada ReadPodrucjeRada(int _id)
		{
			Podrucje_rada podrucje = new Podrucje_rada();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
                command.CommandText = "SELECT id_podrucje, naziv, vrsta " +
                    "FROM podrucje_rada " +
                    "WHERE id_podrucje = @id ";					
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
							podrucje = new Podrucje_rada()
							{
								Id_podrucje = Convert.ToInt32(sdr["id_podrucje"]),								
								Naziv = sdr["naziv"].ToString(),
                                Vrsta = Convert.ToInt32(sdr["vrsta"])
							};
						}
					}
				}
				connection.Close();
			}
			return podrucje;
		}

		public bool CreatePodrucjeRada(Podrucje_rada podrucje)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					command.CommandText = "INSERT INTO podrucje_rada " +
						"(naziv, vrsta) " +
						" VALUES (@naziv, @id_pedagog)";
					command.CommandType = CommandType.Text;					
					command.Parameters.AddWithValue("@naziv", podrucje.Naziv);
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

		public bool UpdatePodrucjeRada(Podrucje_rada podrucje)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
                    command.CommandText = "UPDATE podrucje_rada " +
                        "SET " +
                        "naziv = @naziv " +
                        "WHERE id_podrucje = @id_podrucje";						
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_podrucje", podrucje.Id_podrucje);					
					command.Parameters.AddWithValue("@naziv", podrucje.Naziv);					
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

		public bool DeletePodrucjeRada(int id)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					connection.Open();
                    command.CommandText = "DELETE FROM podrucje_rada " +
                        "WHERE id_podrucje = @id_podrucje";						
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_podrucje", id);					
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