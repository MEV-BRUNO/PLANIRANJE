using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Planiranje.Controllers;

namespace Planiranje.Models
{
	public class Mjesecni_plan_DBHandle
	{
		private MySqlConnection con;
		string str = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;

		private void connection()
		{
			string constring = ConfigurationManager.ConnectionStrings["BazaPodataka"].ToString();
			con = new MySqlConnection(constring);
		}

		public List<Mjesecni_plan> DohvatiMjesecnePlanove()
		{
			connection();
			List<Mjesecni_plan> planovi = new List<Mjesecni_plan>();
			con.ConnectionString = str;
			using (MySqlCommand cmd = new MySqlCommand())
			{
				cmd.Connection = con;
				cmd.CommandText = "SELECT id_plan, naziv, ak_godina, opis FROM mjesecni_plan where id_pedagog = " + PlaniranjeSession.Trenutni.PedagogId + " ORDER BY id_plan ASC";
				con.Open();
				using (MySqlDataReader sdr = cmd.ExecuteReader())
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
							if (plan.Naziv.Length > 0)
								planovi.Add(plan);
						}
					}
				}
				con.Close();
			}
			return planovi;
		}

		public Mjesecni_plan DohvatiMjesecniPlan(int _id)
		{
			connection();
			Mjesecni_plan mjesecniPlan = new Mjesecni_plan();
			con.ConnectionString = str;
			using (MySqlCommand cmd = new MySqlCommand())
			{
				cmd.Connection = con;
				cmd.CommandText = "SELECT * FROM mjesecni_plan WHERE id_plan = 1";
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@id", _id);
				con.Open();
				using (MySqlDataReader sdr = cmd.ExecuteReader())
				{
					if (sdr.HasRows && sdr.FieldCount == 1)
					{
						while (sdr.Read())
						{
							mjesecniPlan = new Mjesecni_plan()
							{
								ID_plan = Convert.ToInt32(sdr["id_plan"]),
								Ak_godina = sdr["ak_godina"].ToString(),
								Naziv = sdr["naziv"].ToString(),
								Opis = sdr["opis"].ToString()
							};
						}
					}
				}
				con.Close();
			}
			return mjesecniPlan;
		}

		public bool ObrisiMjesecniPlan(int id)
		{
			bool result = false;
			try
			{
				connection();
				using (MySqlCommand cmd = new MySqlCommand())
				{
					cmd.Connection = con;
					con.Open();

					cmd.CommandText = "DELETE FROM mjesecni_plan WHERE id_plan = " + id + ";";
					cmd.CommandType = CommandType.Text;
					cmd.ExecuteNonQuery();
					result = true;
				}
			}
			catch (Exception err)
			{
				con.Close();
				result = false;
			}
			finally
			{
				con.Close();
			}
			return result;
		}
	}
}
