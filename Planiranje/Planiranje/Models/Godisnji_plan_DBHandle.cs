using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
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
            List<Godisnji_plan> godisnji_planovi = new List<Godisnji_plan>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_god, id_pedagog, ak_godina, br_radnih_dana, br_dana_godina_odmor, ukupni_rad_dana, god_fond_sati" +
                    "FROM godisnji_plan " +
                    "WHERE id_pedagog = @id_pedagog " +
                    "ORDER BY id_god ASC";
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
                                //Id_pedagog = Convert.ToInt32(sdr["id_pedagog"]),
                                Id_god = Convert.ToInt32(sdr["id_god"]),
                                //Naziv = sdr["naziv"].ToString(),
                                Ak_godina = sdr["ak_godina"].ToString(),
                                //Opis = sdr["opis"].ToString(),
                                Br_radnih_dana = Convert.ToInt32(sdr["br_radnih_dana"]),
                                Br_dana_godina_odmor = Convert.ToInt32(sdr["br_dana_godina_odmor"]),
                                Ukupni_rad_dana = Convert.ToInt32(sdr["ukupni_rad_dana"]),
                                God_fond_sati = Convert.ToInt32(sdr["god_fond_sati"]),
                            };
                            godisnji_planovi.Add(plan);
                        }
                    }
                }
                connection.Close();
            }
            return godisnji_planovi;
        }

        public List<Godisnji_plan> ReadGodisnjePlanove(string search_string)
        {
            List<Godisnji_plan> godisnji_planovi = new List<Godisnji_plan>();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                //command.CommandText = "SELECT id_plan, ak_godina, naziv, opis " +
                //    "FROM mjesecni_plan " +
                //    "WHERE id_pedagog = @id_pedagog " +
                //    "AND (ak_godina like '%" + search_string + "%' " +
                //    "OR naziv like '%" + search_string + "%' " +
                //    "OR opis like '%" + search_string + "%') " +
                //    "ORDER BY id_plan ASC";
                command.CommandText = "SELECT id_god, ak_godina, br_radnih_dana, br_dana_godina_odmor, ukupni_rad_dana, god_fond_sati" +
                    "FROM godisnji_plan " +
                    "WHERE id_pedagog = @id_pedagog " +
                    "AND (ak_godina like '%" + search_string + "%' " +
                    "OR br_radnih_dana like '%" + search_string +"%')"+
                    "ORDER BY id_god ASC";
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
                                Id_god = Convert.ToInt32(sdr["id_god"]),
                                //Naziv = sdr["naziv"].ToString(),
                                Ak_godina = sdr["ak_godina"].ToString(),
                                //Opis = sdr["opis"].ToString(),
                                Br_radnih_dana = Convert.ToInt32(sdr["br_radnih_dana"]),
                                Br_dana_godina_odmor = Convert.ToInt32(sdr["br_dana_godina_odmor"]),
                                Ukupni_rad_dana = Convert.ToInt32(sdr["ukupni_rad_dana"]),
                                God_fond_sati = Convert.ToInt32(sdr["god_fond_sati"]),
                            };
                            godisnji_planovi.Add(plan);
                        }
                    }
                }
                connection.Close();
            }
            return godisnji_planovi;
        }

        public Godisnji_plan ReadGodisnjiPlan(int _id)
        {
            Godisnji_plan godisnji_plan = new Godisnji_plan();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_god, id_pedagog, ak_godina, br_radnih_dana, br_dana_godina_odmor, ukupni_rad_dana, god_fond_sati" +
                    "FROM godisnji_plan " +
                    "WHERE id_pedagog = @id_pedagog " +
                    "AND id_god = @id "+
                    "ORDER BY id_god ASC";
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
                            godisnji_plan = new Godisnji_plan()
                            {
                                Id_god = Convert.ToInt32(sdr["id_god"]),
                                //Naziv = sdr["naziv"].ToString(),
                                Ak_godina = sdr["ak_godina"].ToString(),
                                //Opis = sdr["opis"].ToString(),
                                Br_radnih_dana = Convert.ToInt32(sdr["br_radnih_dana"]),
                                Br_dana_godina_odmor = Convert.ToInt32(sdr["br_dana_godina_odmor"]),
                                Ukupni_rad_dana = Convert.ToInt32(sdr["ukupni_rad_dana"]),
                                God_fond_sati = Convert.ToInt32(sdr["god_fond_sati"]),
                            };
                        }
                    }
                }
                connection.Close();
            }
            return godisnji_plan;
        }

        public bool CreateGodisnjiPlan(Godisnji_plan godisnji_plan)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO godisnji_plan " +
                        "(id_god, id_pedagog, ak_godina, br_radnih_dana, br_dana_godina_odmor, ukupni_rad_dana, god_fond_sati) " +
                        " VALUES (@id_god, @id_pedagog, @ak_godina, @br_radnih_dana, @br_dana_godina_odmor, @ukupni_rad_dana, @god_fond_sati)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_god", godisnji_plan.Id_god);
                    command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                    command.Parameters.AddWithValue("@ak_godina", godisnji_plan.Ak_godina);
                    command.Parameters.AddWithValue("@br_radnih_dana", godisnji_plan.Br_radnih_dana);
                    command.Parameters.AddWithValue("@br_dana_godina_odmor", godisnji_plan.Br_dana_godina_odmor);
                    command.Parameters.AddWithValue("@ukupni_rad_dana", godisnji_plan.Ukupni_rad_dana);
                    command.Parameters.AddWithValue("@god_fond_sati", godisnji_plan.God_fond_sati);
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

        public bool UpdateGodisnjiPlan(Godisnji_plan godisnji_plan)
        {
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
                        "br_dana_godina_odmor = @br_dana_godina_odmor " +
                        "ukupni_rad_dana = @ukupni_rad_dana" +
                        "god_fond_sati = @god_fond_sati"+
                        "WHERE id_god = @id_god " +
                        "AND id_pedagog = @id_pedagog";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id_god", godisnji_plan.Id_god);
                    command.Parameters.AddWithValue("@id_pedagog", PlaniranjeSession.Trenutni.PedagogId);
                    command.Parameters.AddWithValue("@ak_godina", godisnji_plan.Ak_godina);
                    command.Parameters.AddWithValue("@br_radnih_dana", godisnji_plan.Br_radnih_dana);
                    command.Parameters.AddWithValue("@br_dana_godina_odmor", godisnji_plan.Br_dana_godina_odmor);
                    command.Parameters.AddWithValue("@ukupni_rad_dana", godisnji_plan.Ukupni_rad_dana);
                    command.Parameters.AddWithValue("@god_fond_sati", godisnji_plan.God_fond_sati);
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
    }
}