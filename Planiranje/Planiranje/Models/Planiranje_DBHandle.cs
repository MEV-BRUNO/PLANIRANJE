using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Planiranje.Controllers;
using System.Data;

namespace Planiranje.Models
{
	public class Planiranje_DBHandle
	{
		private MySqlConnection connection;

		private void Connect()
		{
			string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
			connection = new MySqlConnection(connection_string);
		}
		public List<Skola> ReadSkole()
		{
			List<Skola> skole = new List<Skola>();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT * FROM skola ORDER BY naziv ASC";
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Skola sk = new Skola()
							{
								Id_skola = Convert.ToInt32(sdr["id_skola"]),
								Naziv = sdr["naziv"].ToString(),
								Adresa = sdr["adresa"].ToString(),
								Grad = sdr["grad"].ToString(),
								Tel = sdr["tel"].ToString(),
								URL = sdr["url"].ToString(),
								Kontakt = sdr["kontakt"].ToString()
							};
							skole.Add(sk);
						}
					}
				}
				connection.Close();
			}
			return skole;
		}
	}
}