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
    public class SS_Plan_DBHandle
    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public List<SS_Plan> ReadSSPlanove(int id_god)
        {
			int counter = 0;
            List<SS_Plan> Ss_planovi = new List<SS_Plan>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_plan, id_godina, naziv, opis, godisnji_plan.ak_godina as ak_godina " +
                    "FROM Ss_plan " +
					"JOIN godisnji_plan ON Ss_plan.id_godina = godisnji_plan.id_god " +
					"WHERE ss_plan.id_pedagog = @id_pedagog " +
					"AND id_godina = @id_godina " +
					"ORDER BY naziv ASC";
				command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
				command.Parameters.AddWithValue("@id_godina", id_god);
				connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
							SS_Plan plan = new SS_Plan()
							{
								Red_br = ++counter,
                                Id_plan = Convert.ToInt32(sdr["id_plan"]),
                                Naziv = sdr["naziv"].ToString(),
								Ak_godina = sdr["ak_godina"].ToString(),
								Id_godina = Convert.ToInt32(sdr["Id_godina"]),
                                Opis = sdr["opis"].ToString(),
                            };
                            Ss_planovi.Add(plan);
                        }
                    }
                }
                connection.Close();
            }
            return Ss_planovi;
        }

        public List<SS_Plan> ReadSSPlanove(string search_string)
        {
            List<SS_Plan>Ss_planovi = new List<SS_Plan>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_plan, ak_godina, naziv, opis " +
                    "FROM Ss_plan " +
                    "WHERE id_pedagog = @id_pedagog " +
                    "AND (ak_godina like '%" + search_string + "%' " +
                    "OR naziv like '%" + search_string + "%' " +
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
                            SS_Plan plan = new SS_Plan()
                            {
                                Id_plan = Convert.ToInt32(sdr["id_plan"]),
                                Naziv = sdr["naziv"].ToString(),
                                Id_godina = Convert.ToInt32(sdr["ak_godina"]),
                                Opis = sdr["opis"].ToString(),
                            };
                            Ss_planovi.Add(plan);
                        }
                    }
                }
                connection.Close();
            }
            return Ss_planovi;
        }

        public SS_Plan ReadSSPlan(int _id)
        {
            SS_Plan Ss_plan = new SS_Plan();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_plan, id_godina, naziv, opis " +
                    "FROM Ss_plan " +
                    "WHERE id_plan = @id " +
                    "AND id_pedagog = @id_pedagog";
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                           Ss_plan = new SS_Plan()
                            {
                                Id_plan = Convert.ToInt32(sdr["id_plan"]),
                                Id_godina = Convert.ToInt32(sdr["id_godina"]),
                                Naziv = sdr["naziv"].ToString(),
                                Opis = sdr["opis"].ToString()
                            };
                        }
                    }
                }
                connection.Close();
            }
            return Ss_plan;
        }

        public bool CreateSSPlan(SS_Plan Ss_plan)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Ss_plan " +
                        "(id_pedagog, id_godina, naziv, opis) " +
                        " VALUES (@id_pedagog, @id_godina, @naziv, @opis)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                    command.Parameters.AddWithValue("@id_godina", Ss_plan.Id_godina);
                    command.Parameters.AddWithValue("@naziv", Ss_plan.Naziv);
                    command.Parameters.AddWithValue("@opis",Ss_plan.Opis);
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

        public bool UpdateSSPlan(SS_Plan Ss_plan)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Ss_plan " +
                        "SET " +
                        "id_godina = @id_godina, " +
                        "naziv = @naziv, " +
                        "opis = @opis " +
                        "WHERE id_plan = @id_plan " +
                        "AND id_pedagog = @id_pedagog";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_plan", Ss_plan.Id_plan);
                    command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                    command.Parameters.AddWithValue("@id_godina", Ss_plan.Id_godina);
                    command.Parameters.AddWithValue("@naziv", Ss_plan.Naziv);
                    command.Parameters.AddWithValue("@opis", Ss_plan.Opis);
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

        public bool DeleteSSPlan(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM Ss_plan " +
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

		public List<SS_Plan_podrucje> ReadSsPodrucja(int id_plan)
		{
			int counter = 0;
			List<SS_Plan_podrucje> ss_podrucja = new List<SS_Plan_podrucje>();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT * " +
					"FROM ss_plan_podrucje " +
					"WHERE id_plan = @id_plan " +
					"ORDER BY id_plan ASC";
				command.Parameters.AddWithValue("@id_plan", id_plan);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							SS_Plan_podrucje plan = new SS_Plan_podrucje()
							{
								Red_br = ++counter,
								Id = Convert.ToInt32(sdr["id"]),
								ID_plan = Convert.ToInt32(sdr["id_plan"]),
								Opis_podrucje = sdr["opis_podrucje"].ToString(),
								Svrha = sdr["svrha"].ToString(),
								Zadaca = sdr["zadaca"].ToString(),
								Sadrzaj = sdr["sadrzaj"].ToString(),
								Oblici = sdr["oblici"].ToString(),
								Suradnici = sdr["suradnici"].ToString(),
								Mjesto = sdr["mjesto"].ToString(),
								Vrijeme  = Convert.ToDateTime(sdr["vrijeme"]),
								Ishodi = sdr["ishodi"].ToString(),
								Sati = Convert.ToInt32(sdr["sati"])
							};
							ss_podrucja.Add(plan);
						}
					}
				}
				connection.Close();
			}
			return ss_podrucja;
		}

		public bool CreateSSPlanPodrucje(SSModel model)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					command.CommandText = "INSERT INTO ss_plan_podrucje " +
						"(id_plan, opis_podrucje, svrha, zadaca, sadrzaj, oblici, suradnici, mjesto, vrijeme, ishodi, sati) " +
						" VALUES (@id_plan, @opis_podrucje, @svrha, @zadaca, @sadrzaj, @oblici, @suradnici, @mjesto, @vrijeme, @ishodi, @sati)";
					command.CommandType = CommandType.Text;

					command.Parameters.AddWithValue("@id_plan", model.ID_PLAN);
					command.Parameters.AddWithValue("@opis_podrucje", model.SS_Plan_Podrucje.Opis_podrucje);
					command.Parameters.AddWithValue("@svrha", model.SS_Plan_Podrucje.Svrha);
					command.Parameters.AddWithValue("@zadaca", model.SS_Plan_Podrucje.Zadaca);
					command.Parameters.AddWithValue("@sadrzaj", model.SS_Plan_Podrucje.Sadrzaj);
					command.Parameters.AddWithValue("@oblici", model.SS_Plan_Podrucje.Oblici);
					command.Parameters.AddWithValue("@suradnici", model.SS_Plan_Podrucje.Suradnici);
					command.Parameters.AddWithValue("@mjesto", model.SS_Plan_Podrucje.Mjesto);
					command.Parameters.AddWithValue("@vrijeme", model.SS_Plan_Podrucje.Vrijeme);
					command.Parameters.AddWithValue("@ishodi", model.SS_Plan_Podrucje.Ishodi);
					command.Parameters.AddWithValue("@sati", model.SS_Plan_Podrucje.Sati);

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


		public SS_Plan_podrucje ReadSsPodrucje(int id)
		{
			SS_Plan_podrucje ss_podrucje = new SS_Plan_podrucje();
			this.Connect();
			using (MySqlCommand command = new MySqlCommand())
			{
				command.Connection = connection;
				command.CommandText = "SELECT * " +
					"FROM ss_plan_podrucje " +
					"WHERE id = @id";
				command.Parameters.AddWithValue("@id", id);
				connection.Open();
				using (MySqlDataReader sdr = command.ExecuteReader())
				{
					if (sdr.HasRows)
					{
						while (sdr.Read())
						{
							SS_Plan_podrucje plan = new SS_Plan_podrucje()
							{
								Id = Convert.ToInt32(sdr["id"]),
								ID_plan = Convert.ToInt32(sdr["id_plan"]),
								Opis_podrucje = sdr["opis_podrucje"].ToString(),
								Svrha = sdr["svrha"].ToString(),
								Zadaca = sdr["zadaca"].ToString(),
								Sadrzaj = sdr["sadrzaj"].ToString(),
								Oblici = sdr["oblici"].ToString(),
								Suradnici = sdr["suradnici"].ToString(),
								Mjesto = sdr["mjesto"].ToString(),
								Vrijeme = Convert.ToDateTime(sdr["vrijeme"]),
								Ishodi = sdr["ishodi"].ToString(),
								Sati = Convert.ToInt32(sdr["sati"])
							};
							ss_podrucje = plan;
						}
					}
				}
				connection.Close();
			}
			return ss_podrucje;
		}
		public bool DeleteSSPlanPodrucje(int id)
		{
			try
			{
				this.Connect();
				using (MySqlCommand command = new MySqlCommand())
				{
					command.Connection = connection;
					connection.Open();
					command.CommandText = "DELETE FROM Ss_plan_podrucje " +
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
	}
}



