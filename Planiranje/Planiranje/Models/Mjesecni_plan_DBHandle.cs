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
	public class Mjesecni_plan_DBHandle
	{
		private MySqlConnection connection;

		private void Connect()
		{
			string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
			connection = new MySqlConnection(connection_string);
		}

		public List<Mjesecni_plan> ReadMjesecnePlanove()
		{
			List<Mjesecni_plan> mjesecni_planovi = new List<Mjesecni_plan>();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id_plan, ak_godina, naziv, opis " +
					"FROM mjesecni_plan " +
					"WHERE id_pedagog = @id_pedagog " +
					"ORDER BY id_plan ASC";
				command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Mjesecni_plan plan = new Mjesecni_plan()
							{
								ID_plan = Convert.ToInt32(sdr["id_plan"]),
								Naziv = sdr["naziv"].ToString(),
								Ak_godina = sdr["ak_godina"].ToString(),
								Opis = sdr["opis"].ToString(),
							};
							mjesecni_planovi.Add(plan);
						}
					}
				}
				connection.Close();
			}
			return mjesecni_planovi;
		}

		public List<Mjesecni_plan> ReadMjesecnePlanove(string search_string)
		{
			List<Mjesecni_plan> mjesecni_planovi = new List<Mjesecni_plan>();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id_plan, id_godina, naziv, opis " +
					"FROM mjesecni_plan " +
					"WHERE id_pedagog = @id_pedagog " +
					"AND (naziv like '%" + search_string + "%' " +
					"OR opis like '%" + search_string + "%') " +
					"ORDER BY id_plan ASC";
				command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Mjesecni_plan plan = new Mjesecni_plan()
							{
								ID_plan = Convert.ToInt32(sdr["id_plan"]),
								Naziv = sdr["naziv"].ToString(),
								Id_godina = Convert.ToInt32(sdr["ak_godina"]),
								Opis = sdr["opis"].ToString(),
							};
							mjesecni_planovi.Add(plan);
						}
					}
				}
				connection.Close();
			}
			return mjesecni_planovi;
		}

		public List<Mjesecni_plan> ReadMjesecnePlanove(int id)
		{
			int counter = 0;
			List<Mjesecni_plan> mjesecni_planovi = new List<Mjesecni_plan>();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id_plan, id_godina, naziv, opis, godisnji_plan.ak_godina as ak_godina  " +
					"FROM mjesecni_plan " +
					"JOIN godisnji_plan ON mjesecni_plan.id_godina = godisnji_plan.id_god " +
					"WHERE mjesecni_plan.id_pedagog = @id_pedagog " +
					"AND id_godina = " + id + " " +
					"ORDER BY id_plan ASC";
				command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Mjesecni_plan plan = new Mjesecni_plan()
							{
								Red_br = ++counter,
								ID_plan = Convert.ToInt32(sdr["id_plan"]),
								Naziv = sdr["naziv"].ToString(),
								Ak_godina = sdr["ak_godina"].ToString(),
								Id_godina = Convert.ToInt32(sdr["id_godina"]),
								Opis = sdr["opis"].ToString(),
							};
							mjesecni_planovi.Add(plan);
						}
					}
				}
				connection.Close();
			}
			return mjesecni_planovi;
		}

		public Mjesecni_plan ReadMjesecniPlan(int _id)
		{
			Mjesecni_plan mjesecni_plan = new Mjesecni_plan();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id_plan, id_godina, naziv, opis, godisnji_plan.ak_godina as ak_godina " +
					"FROM mjesecni_plan " +
					"JOIN godisnji_plan ON mjesecni_plan.id_godina = godisnji_plan.id_god " +
					"WHERE id_plan = @id_plan " +
					"AND mjesecni_plan.id_pedagog = @id_pedagog";
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@id_plan", _id);
				command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							mjesecni_plan = new Mjesecni_plan()
							{
								ID_plan = Convert.ToInt32(sdr["id_plan"]),
								Ak_godina = sdr["ak_godina"].ToString(),
								Id_godina = Convert.ToInt32(sdr["id_godina"]),
								Naziv = sdr["naziv"].ToString(),
								Opis = sdr["opis"].ToString()
							};
						}
					}
				}
				connection.Close();
			}
			return mjesecni_plan;
		}

		public Mjesecni_plan ReadMjesecniPlanForDel(int _id)
		{
			Mjesecni_plan mjesecni_plan = new Mjesecni_plan();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT * FROM mjesecni_plan " +
					"WHERE id_plan = @id_plan";
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@id_plan", _id);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							mjesecni_plan = new Mjesecni_plan()
							{
								ID_plan = Convert.ToInt32(sdr["id_plan"]),
								Id_godina = Convert.ToInt32(sdr["id_godina"]),
								Naziv = sdr["naziv"].ToString(),
								Opis = sdr["opis"].ToString()
							};
						}
					}
				}
				connection.Close();
			}
			return mjesecni_plan;
		}

		public bool CreateMjesecniPlan(Mjesecni_plan mjesecni_plan)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					command.CommandText = "INSERT INTO mjesecni_plan " +
						"(id_pedagog, id_godina, naziv, opis) " +
						" VALUES (@id_pedagog, @id_godina, @naziv, @opis)";
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
					command.Parameters.AddWithValue("@id_godina", mjesecni_plan.Id_godina);
					command.Parameters.AddWithValue("@naziv", mjesecni_plan.Naziv);
					command.Parameters.AddWithValue("@opis", mjesecni_plan.Opis);
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

		public bool UpdateMjesecniPlan(Mjesecni_plan mjesecni_plan)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					command.CommandText = "UPDATE mjesecni_plan " +
						"SET " +
						"id_godina = @id_godina, " +
						"naziv = @naziv, " +
						"opis = @opis " +
						"WHERE id_plan = @id_plan " +
						"AND id_pedagog = @id_pedagog";
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_plan", mjesecni_plan.ID_plan);
					command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
					command.Parameters.AddWithValue("@id_godina", mjesecni_plan.Id_godina);
					command.Parameters.AddWithValue("@naziv", mjesecni_plan.Naziv);
					command.Parameters.AddWithValue("@opis", mjesecni_plan.Opis);
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

		public bool DeleteMjesecniPlan(int id)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					connection.Open();
					command.CommandText = "DELETE FROM mjesecni_plan " +
						"WHERE id_plan = @id_plan " +
						"AND id_pedagog = @id_pedagog";
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_plan", id);
					command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
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


		public List<Mjesecni_detalji> ReadMjesecneDetalje(int id)
		{
			int counter = 0;
			List<Mjesecni_detalji> detalji = new List<Mjesecni_detalji>();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id, id_plan, podrucje, aktivnost, suradnici, vrijeme, br_sati, biljeska  " +
					"FROM mjesecni_detalji " +
					"WHERE id_plan = @id_plan " +
					"ORDER BY id_plan ASC";
				command.Parameters.AddWithValue("@id_plan", id);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Mjesecni_detalji detalj = new Mjesecni_detalji()
							{
								ID = Convert.ToInt32(sdr["id"]),
								ID_plan = Convert.ToInt32(sdr["id_plan"]),
								Red_br = ++counter,
								Podrucje = sdr["podrucje"].ToString(),
								Aktivnost = sdr["aktivnost"].ToString(),
								Suradnici = sdr["suradnici"].ToString(),
								Vrijeme = Convert.ToDateTime(sdr["vrijeme"]),
								Br_sati = Convert.ToInt32(sdr["br_sati"]),
								Biljeska = sdr["biljeska"].ToString(),
							};
							detalji.Add(detalj);
						}
					}
				}
				connection.Close();
			}
			return detalji;
		}
		public Mjesecni_detalji ReadMjesecniDetalj(int id)
		{
			Mjesecni_detalji detalj = new Mjesecni_detalji();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id, id_plan, podrucje, aktivnost, suradnici, vrijeme, br_sati, biljeska  " +
					"FROM mjesecni_detalji " +
					"WHERE id = @id";
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							detalj.ID = Convert.ToInt32(sdr["id"]);
							detalj.ID_plan = Convert.ToInt32(sdr["id_plan"]);
							detalj.Podrucje = sdr["podrucje"].ToString();
							detalj.Aktivnost = sdr["aktivnost"].ToString();
							detalj.Suradnici = sdr["suradnici"].ToString();
							detalj.Vrijeme = Convert.ToDateTime(sdr["vrijeme"]);
							detalj.Br_sati = Convert.ToInt32(sdr["br_sati"]);
							detalj.Biljeska = sdr["biljeska"].ToString();
						}
					}
				}
				connection.Close();
			}
			return detalj;
		}
		public bool CreateMjesecniDetalj(Mjesecni_detalji mjesecni_detalj)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					command.CommandText = "INSERT INTO mjesecni_detalji " +
						"(id_plan, podrucje , aktivnost, suradnici, vrijeme, br_sati, biljeska) " +
						" VALUES (@id_plan, @podrucje, @aktivnost, @suradnici, @vrijeme, @br_sati, @biljeska)";
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_plan", mjesecni_detalj.ID_plan);
					command.Parameters.AddWithValue("@podrucje", mjesecni_detalj.Podrucje);
					command.Parameters.AddWithValue("@aktivnost", mjesecni_detalj.Aktivnost);
					command.Parameters.AddWithValue("@suradnici", mjesecni_detalj.Suradnici);
					command.Parameters.AddWithValue("@vrijeme", mjesecni_detalj.Vrijeme);
					command.Parameters.AddWithValue("@br_sati", mjesecni_detalj.Br_sati);
					command.Parameters.AddWithValue("@biljeska", mjesecni_detalj.Biljeska);
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
		public bool DeleteMjesecniDetalj(int id)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					connection.Open();
					command.CommandText = "DELETE FROM mjesecni_detalji " +
						"WHERE id = @id";
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
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
		public bool UpdateMjesecniDetalj(Mjesecni_detalji mjesecni_detalj)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					command.CommandText = "UPDATE mjesecni_detalji " +
						"SET " +
						"podrucje = @podrucje, " +
						"aktivnost = @aktivnost, " +
						"suradnici = @suradnici, " +
						"vrijeme = @vrijeme, " +
						"br_sati = @br_sati, " +
						"biljeska = @biljeska " +
						"WHERE id = @id ";
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@podrucje", mjesecni_detalj.Podrucje);
					command.Parameters.AddWithValue("@aktivnost", mjesecni_detalj.Aktivnost);
					command.Parameters.AddWithValue("@suradnici", mjesecni_detalj.Suradnici);
					command.Parameters.AddWithValue("@vrijeme", mjesecni_detalj.Vrijeme);
					command.Parameters.AddWithValue("@br_sati", mjesecni_detalj.Br_sati);
					command.Parameters.AddWithValue("@biljeska", mjesecni_detalj.Biljeska);
					command.Parameters.AddWithValue("@id", mjesecni_detalj.ID);
					connection.Open();
					command.ExecuteNonQuery();
				}
			}
			catch (MySqlException e)
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
