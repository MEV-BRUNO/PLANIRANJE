﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Planiranje.Controllers;
using System.Data;

namespace Planiranje.Models
{
    public class Godisnji_plan_DBHandle
    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public List<Godisnji_plan> ReadGodisnjePlanove()
        {
			int id = 0;
            List<Godisnji_plan> godisnji_planovi = new List<Godisnji_plan>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_god, id_pedagog, ak_godina, br_radnih_dana, br_dana_godina_odmor, ukupni_rad_dana, god_fond_sati, naziv" +
                    " FROM godisnji_plan " +
                    "WHERE id_pedagog = @id_pedagog " +
                    "ORDER BY ak_godina ASC";
                command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
							Godisnji_plan plan = new Godisnji_plan()
							{
								Redni_broj = ++id,
								Id_god = Convert.ToInt32(sdr["id_god"]),
                                Ak_godina = Convert.ToInt32(sdr["ak_godina"]),
                                Br_radnih_dana = Convert.ToInt32(sdr["br_radnih_dana"]),
                                Br_dana_godina_odmor = Convert.ToInt32(sdr["br_dana_godina_odmor"]),
                                Ukupni_rad_dana = Convert.ToInt32(sdr["ukupni_rad_dana"]),
                                God_fond_sati = Convert.ToInt32(sdr["god_fond_sati"]),
                                Naziv = sdr["naziv"].ToString()
                            };
                            godisnji_planovi.Add(plan);
                        }
                    }
                }
                connection.Close();
            }
            return godisnji_planovi;
        }

		public GodisnjiModel ReadGodisnjiDetalji(int _id)
		{
			GodisnjiModel god_detalji = new GodisnjiModel();
			god_detalji.GodisnjiDetalji = new List<Godisnji_detalji>();
			god_detalji.GodisnjiPlan = new Godisnji_plan();

			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT * FROM godisnji_detalji " +
					"WHERE id_god = @id_god " +
					"ORDER BY id_god ASC";
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@id_god", _id);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Godisnji_detalji detalj = new Godisnji_detalji()
							{
								Id = Convert.ToInt32(sdr["id"]),
								Id_god = Convert.ToInt32(sdr["id_god"]),
								Mjesec = Convert.ToInt32(sdr["mjesec"]),
								Naziv_mjeseca = sdr["naziv_mjeseca"].ToString(),
								Ukupno_dana = Convert.ToInt32(sdr["ukupno_dana"]),
								Radnih_dana = Convert.ToInt32(sdr["radnih_dana"]),
								Subota_dana = Convert.ToInt32(sdr["subota_dana"]),
								Nedjelja_dana = Convert.ToInt32(sdr["nedjelja_dana"]),
								Blagdana_dana = Convert.ToInt32(sdr["blagdana_dana"]),
								Nastavnih_dana = Convert.ToInt32(sdr["nastavnih_dana"]),
								Praznika_dana = Convert.ToInt32(sdr["praznika_dana"]),
								Br_sati = Convert.ToInt32(sdr["br_sati"]),
								Odmor_dana = Convert.ToInt32(sdr["odmor_dana"]),
								Odmor_sati = Convert.ToInt32(sdr["odmor_sati"]),
								Mj_fond_sati = Convert.ToInt32(sdr["mj_fond_sati"])
							};
							god_detalji.GodisnjiDetalji.Add(detalj);
						}
					}
				}
				connection.Close();
			}
			god_detalji.GodisnjiPlan = ReadGodisnjiPlan(_id);
			return god_detalji;
		}

		public Godisnji_plan ReadGodisnjiPlan(int _id)
		{
			Godisnji_plan plan = new Godisnji_plan();

			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT * FROM godisnji_plan " +
					"WHERE id_pedagog = @id_pedagog " +
					"AND id_god = @id_god " +
					"ORDER BY id_god ASC";
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@id_god", _id);
				command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							Godisnji_plan g_plan = new Godisnji_plan()
							{
								Id_god = Convert.ToInt32(sdr["id_god"]),
								Id_pedagog = Convert.ToInt32(sdr["id_pedagog"]),
								Ak_godina = Convert.ToInt32(sdr["ak_godina"]),
                                Br_radnih_dana = Convert.ToInt32(sdr["br_radnih_dana"]),
								Br_dana_godina_odmor = Convert.ToInt32(sdr["br_dana_godina_odmor"]),
								Ukupni_rad_dana = Convert.ToInt32(sdr["ukupni_rad_dana"]),
								God_fond_sati = Convert.ToInt32(sdr["god_fond_sati"]),
                                Naziv = sdr["naziv"].ToString()
							};
							plan = g_plan;
						}
					}
				}
				connection.Close();
			}
			return plan;
		}

		public bool CreateGodisnjiPlan(GodisnjiModel model)
		{
			int br_radnih_dana = 0;
			int br_dana_godina_odmor = 0;
			int ukupni_rad_dana = 0;
			int god_fond_sati = 0;
			foreach (Godisnji_detalji detalj in model.GodisnjiDetalji)
			{
				br_radnih_dana += detalj.Radnih_dana;
				br_dana_godina_odmor += detalj.Odmor_dana;
			}
			ukupni_rad_dana += br_radnih_dana - br_dana_godina_odmor;
			god_fond_sati += ukupni_rad_dana * 8;
			try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO godisnji_plan " +
                        "(id_god, id_pedagog, ak_godina, br_radnih_dana, br_dana_godina_odmor, ukupni_rad_dana, god_fond_sati, naziv) " +
                        " VALUES (@id_god, @id_pedagog, @ak_godina, @br_radnih_dana, @br_dana_godina_odmor, @ukupni_rad_dana, @god_fond_sati, @naziv)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_god", model.GodisnjiPlan.Id_god);
                    command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                    command.Parameters.AddWithValue("@ak_godina", model.GodisnjiPlan.Ak_godina);
                    command.Parameters.AddWithValue("@br_radnih_dana", br_radnih_dana);
                    command.Parameters.AddWithValue("@br_dana_godina_odmor", br_dana_godina_odmor);
                    command.Parameters.AddWithValue("@ukupni_rad_dana", ukupni_rad_dana);
                    command.Parameters.AddWithValue("@god_fond_sati", god_fond_sati);
                    command.Parameters.AddWithValue("@naziv",model.GodisnjiPlan.Naziv);
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

			int id = 0;
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT id_god FROM godisnji_plan ORDER BY id_god DESC LIMIT 1";
				command.CommandType = CommandType.Text;
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows && sdr.FieldCount == 1)
					{
						while (sdr.Read())
						{
							id = Convert.ToInt32(sdr["id_god"]);
						}
					}
					else
					{
						return false;
					}
				}
				connection.Close();
			}

			foreach (Godisnji_detalji detalj in model.GodisnjiDetalji)
			{
				try
				{
					this.Connect();
					using (MySqlCommand command = new MySqlCommand())
					{
						command.Connection = connection;
						command.CommandText = "INSERT INTO godisnji_detalji " +
							" (id_god, mjesec, naziv_mjeseca, radnih_dana, subota_dana, nedjelja_dana, blagdana_dana, nastavnih_dana, ukupno_dana, " +
							"praznika_dana, br_sati, odmor_dana, odmor_sati, mj_fond_sati)" +
							" VALUES (@id_god, @mjesec, @naziv_mjeseca, @radnih_dana, @subota_dana, @nedjelja_dana, @blagdana_dana, @nastavnih_dana, @ukupno_dana, " +
							"@praznika_dana, @br_sati, @odmor_dana, @odmor_sati, @mj_fond_sati)";
						command.CommandType = CommandType.Text;
						command.Parameters.AddWithValue("@id_god", id);
						command.Parameters.AddWithValue("@mjesec", detalj.Mjesec);
						command.Parameters.AddWithValue("@naziv_mjeseca", detalj.Naziv_mjeseca);
						command.Parameters.AddWithValue("@radnih_dana", detalj.Radnih_dana);
						command.Parameters.AddWithValue("@subota_dana", detalj.Subota_dana);
						command.Parameters.AddWithValue("@nedjelja_dana", detalj.Nedjelja_dana);
						command.Parameters.AddWithValue("@blagdana_dana", detalj.Blagdana_dana);
						command.Parameters.AddWithValue("@nastavnih_dana", detalj.Nastavnih_dana);
						command.Parameters.AddWithValue("@praznika_dana", detalj.Praznika_dana);
						command.Parameters.AddWithValue("@ukupno_dana", (detalj.Radnih_dana + detalj.Subota_dana + detalj.Nedjelja_dana + detalj.Blagdana_dana));
						command.Parameters.AddWithValue("@br_sati", detalj.Radnih_dana * 8);
						command.Parameters.AddWithValue("@odmor_dana", detalj.Odmor_dana);
						command.Parameters.AddWithValue("@odmor_sati", detalj.Odmor_dana * 8);
						command.Parameters.AddWithValue("@mj_fond_sati", (detalj.Radnih_dana * 8) - (detalj.Odmor_dana * 8));
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
			}
			return true;
		}

        public bool UpdateGodisnjiPlan(GodisnjiModel model)
        {

			int br_radnih_dana = 0;
			int br_dana_godina_odmor = 0;
			int ukupni_rad_dana = 0;
			int god_fond_sati = 0;
			foreach (Godisnji_detalji detalj in model.GodisnjiDetalji)
			{
				br_radnih_dana += detalj.Radnih_dana;
				br_dana_godina_odmor += detalj.Odmor_dana;
			}
			ukupni_rad_dana += br_radnih_dana - br_dana_godina_odmor;
			god_fond_sati += ukupni_rad_dana * 8;

			try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE godisnji_plan " +
                        "SET " +
                        "ak_godina = @ak_godina, " +
                        "br_radnih_dana = @br_radnih_dana, " +
                        "br_dana_godina_odmor = @br_dana_godina_odmor, " +
                        "ukupni_rad_dana = @ukupni_rad_dana, " +
                        "god_fond_sati = @god_fond_sati, "+
                        "naziv = @naziv "+
                        "WHERE id_god = @id_god " +
                        "AND id_pedagog = @id_pedagog";
                    command.CommandType = CommandType.Text;

					command.Parameters.AddWithValue("@id_god", model.GodisnjiPlan.Id_god);
					command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
					command.Parameters.AddWithValue("@ak_godina", model.GodisnjiPlan.Ak_godina);
					command.Parameters.AddWithValue("@br_radnih_dana", br_radnih_dana);
					command.Parameters.AddWithValue("@br_dana_godina_odmor", br_dana_godina_odmor);
					command.Parameters.AddWithValue("@ukupni_rad_dana", ukupni_rad_dana);
					command.Parameters.AddWithValue("@god_fond_sati", god_fond_sati);
                    command.Parameters.AddWithValue("@naziv", model.GodisnjiPlan.Naziv);
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
			foreach (Godisnji_detalji detalj in model.GodisnjiDetalji)
			{
				try
				{
					this.Connect();
					using (MySqlCommand command = new MySqlCommand())
					{
						command.Connection = connection;
						command.CommandText = "UPDATE godisnji_detalji " +
							"SET " +
							"mjesec = @mjesec, " +
							"naziv_mjeseca = @naziv_mjeseca, " +
							"radnih_dana = @radnih_dana, " +
							"subota_dana = @subota_dana, " +
							"nedjelja_dana = @nedjelja_dana, " +
							"blagdana_dana = @blagdana_dana, " +
							"nastavnih_dana = @nastavnih_dana, " +
							"ukupno_dana = @ukupno_dana, " +
							"praznika_dana = @praznika_dana, " +
							"br_sati = @br_sati, " +
							"odmor_dana = @odmor_dana, " +
							"odmor_sati = @odmor_sati, " +
							"mj_fond_sati = @mj_fond_sati " +
							"WHERE " +
							"id = @id AND id_god = @id_god";
						command.CommandType = CommandType.Text;
						command.Parameters.AddWithValue("@id", detalj.Id);
						command.Parameters.AddWithValue("@id_god", detalj.Id_god);
						command.Parameters.AddWithValue("@mjesec", detalj.Mjesec);
						command.Parameters.AddWithValue("@naziv_mjeseca", detalj.Naziv_mjeseca);
						command.Parameters.AddWithValue("@radnih_dana", detalj.Radnih_dana);
						command.Parameters.AddWithValue("@subota_dana", detalj.Subota_dana);
						command.Parameters.AddWithValue("@nedjelja_dana", detalj.Nedjelja_dana);
						command.Parameters.AddWithValue("@blagdana_dana", detalj.Blagdana_dana);
						command.Parameters.AddWithValue("@nastavnih_dana", detalj.Nastavnih_dana);
						command.Parameters.AddWithValue("@praznika_dana", detalj.Praznika_dana);
						command.Parameters.AddWithValue("@ukupno_dana", (detalj.Radnih_dana + detalj.Subota_dana + detalj.Nedjelja_dana + detalj.Blagdana_dana));
						command.Parameters.AddWithValue("@br_sati", detalj.Radnih_dana * 8);
						command.Parameters.AddWithValue("@odmor_dana", detalj.Odmor_dana);
						command.Parameters.AddWithValue("@odmor_sati", detalj.Odmor_dana * 8);
						command.Parameters.AddWithValue("@mj_fond_sati", (detalj.Radnih_dana * 8) - (detalj.Odmor_dana * 8));
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
			}
            return true;
        }

        public bool DeleteGodisnjiPlan(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM godisnji_plan " +
                        "WHERE id_god = @id_god " +
                        "AND id_pedagog = @id_pedagog";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_god", id);
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

 

        public bool DeleteGodisnjiDetalji(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM godisnji_detalji " +
                        "WHERE id_god = @id_god";
                        
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_god", id);
                    
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

        public bool UpdateGodisnjiDetalji(Godisnji_detalji detalji)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE godisnji_detalji " +
                        "SET mjesec = @mjesec, naziv_mjeseca = @naziv_mjeseca, ukupno_dana = @ukupno_dana, " +
                        "radnih_dana = @radnih_dana, subota_dana = @subota_dana, blagdana_dana = @blagdana_dana, " +
                        "nastavnih_dana = @nastavnih_dana, " +
                        "praznika_dana = @praznika_dana, br_sati = @br_sati, odmor_dana = @odmor_dana, odmor_sati = @odmor_sati, " +
                        "mj_fond_sati = @mj_fond_sati, br_rad_dana_sk_god = @br_rad_dana_sk_god, br_dana_god_odmor = @br_dana_god_odmor, " +
                        "ukupno_rad_dana = @ukupno_rad_dana, " +
                        "god_fond_sati = @god_fond_sati " +
                        "WHERE id_god = @id_god";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_god", detalji.Id_god);

                    command.Parameters.AddWithValue("@mjesec", detalji.Mjesec);
                    command.Parameters.AddWithValue("@naziv_mjeseca", detalji.Naziv_mjeseca);
                    command.Parameters.AddWithValue("@ukupno_dana", detalji.Ukupno_dana);
                    command.Parameters.AddWithValue("@radnih_dana", detalji.Radnih_dana);
                    command.Parameters.AddWithValue("@subota_dana", detalji.Subota_dana);
                    command.Parameters.AddWithValue("@blagdana_dana", detalji.Blagdana_dana);
                    command.Parameters.AddWithValue("@nastavnih_dana", detalji.Nastavnih_dana);
                    command.Parameters.AddWithValue("@praznika_dana", detalji.Praznika_dana);
                    command.Parameters.AddWithValue("@br_sati", detalji.Br_sati);
                    command.Parameters.AddWithValue("@odmor_dana", detalji.Odmor_dana);
                    command.Parameters.AddWithValue("@odmor_sati", detalji.Odmor_sati);
                    command.Parameters.AddWithValue("@mj_fond_sati", detalji.Mj_fond_sati);
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
    }
}