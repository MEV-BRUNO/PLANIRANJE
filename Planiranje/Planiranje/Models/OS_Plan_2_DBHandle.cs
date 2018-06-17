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
    public class OS_Plan_2_DBHandle
    {
        private MySqlConnection connection;

        private void Connect()
        {
            string connection_string = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;
            connection = new MySqlConnection(connection_string);
        }

        public List<OS_Plan_2> ReadOS_Plan_2()
        {
            List<OS_Plan_2> os_plan_2 = new List<OS_Plan_2>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_plan, ak_godina, naziv, opis " +
                    "FROM os_plan_2 " +
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
                            OS_Plan_2 plan = new OS_Plan_2()
                            {
                                Id_plan = Convert.ToInt32(sdr["id_plan"]),
                                Naziv = sdr["naziv"].ToString(),
                                Ak_godina = sdr["ak_godina"].ToString(),
                                Opis = sdr["opis"].ToString(),
                            };
                            os_plan_2.Add(plan);
                        }
                    }
                }
                connection.Close();
            }
            return os_plan_2;
        }

        public List<OS_Plan_2> ReadOS_Plan_2(string search_string)
        {
            List<OS_Plan_2> os_plan_2 = new List<OS_Plan_2>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_plan, ak_godina, naziv, opis " +
                    "FROM os_plan_2 " +
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
                            OS_Plan_2 plan = new OS_Plan_2()
                            {
                                Id_plan = Convert.ToInt32(sdr["id_plan"]),
                                Naziv = sdr["naziv"].ToString(),
                                Ak_godina = sdr["ak_godina"].ToString(),
                                Opis = sdr["opis"].ToString(),
                            };
                            os_plan_2.Add(plan);
                        }
                    }
                }
                connection.Close();
            }
            return os_plan_2;
        }

        public OS_Plan_2 ReadOS_Plan_2(int _id)
        {
            OS_Plan_2 os_plan_2 = new OS_Plan_2();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_plan, ak_godina, naziv, opis " +
                    "FROM os_plan_2 " +
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
							os_plan_2 = new OS_Plan_2()
                            {
                                Id_plan = Convert.ToInt32(sdr["id_plan"]),
                                Ak_godina = sdr["ak_godina"].ToString(),
                                Naziv = sdr["naziv"].ToString(),
                                Opis = sdr["opis"].ToString()
                            };
                        }
                    }
                }
                connection.Close();
            }
            return os_plan_2;
        }

        public bool CreateOS_Plan_2(OS_Plan_2 os_plan_2)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO os_plan_2 " +
                        "(id_pedagog, ak_godina, naziv, opis) " +
                        " VALUES (@id_pedagog, @ak_godina, @naziv, @opis)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                    command.Parameters.AddWithValue("@ak_godina", os_plan_2.Ak_godina);
                    command.Parameters.AddWithValue("@naziv", os_plan_2.Naziv);
                    command.Parameters.AddWithValue("@opis", os_plan_2.Opis);
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

        public bool UpdateOS_Plan_2(OS_Plan_2 os_plan_2)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE os_plan_2 " +
                        "SET " +
                        "ak_godina = @ak_godina, " +
                        "naziv = @naziv, " +
                        "opis = @opis " +
                        "WHERE id_plan = @id_plan " +
                        "AND id_pedagog = @id_pedagog";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_plan", os_plan_2.Id_plan);
                    command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                    command.Parameters.AddWithValue("@ak_godina", os_plan_2.Ak_godina);
                    command.Parameters.AddWithValue("@naziv", os_plan_2.Naziv);
                    command.Parameters.AddWithValue("@opis", os_plan_2.Opis);
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

        public bool DeleteOS_Plan_2(int id)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "DELETE FROM os_plan_2 " +
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
    }
}