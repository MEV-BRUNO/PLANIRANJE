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
                    " FROM godisnji_plan " +
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
                command.CommandText = "SELECT id_god, ak_godina, br_radnih_dana, br_dana_godina_odmor, ukupni_rad_dana, god_fond_sati " +
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
                command.CommandText = "SELECT id_god, id_pedagog, ak_godina, br_radnih_dana, br_dana_godina_odmor, ukupni_rad_dana, god_fond_sati " +
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
                        "br_dana_godina_odmor = @br_dana_godina_odmor, " +
                        "ukupni_rad_dana = @ukupni_rad_dana, " +
                        "god_fond_sati = @god_fond_sati "+
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

        public Godisnji_detalji ReadGodisnjiDetalji(int _id)
        {
            Godisnji_detalji detalji = new Godisnji_detalji();
            this.Connect();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id_god, mjesec, naziv_mjeseca, ukupno_dana, radnih_dana, subota_dana, blagdana_dana, " +
                    "nastavnih_dana, praznika_dana, br_sati, odmor_dana, mj_fond_sati, br_rad_dana_sk_god, br_dana_god_odmor, " +
                    "ukupno_rad_dana, god_fond_sati " +
                    "FROM godisnji_detalji " +
                    "WHERE id_god = @id " +                    
                    "ORDER BY id_god ASC";
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", _id);
                
                connection.Open();
                using (MySqlDataReader sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows)
                    {
                        while (sdr.Read())
                        {
                            detalji = new  Godisnji_detalji()
                            {
                                Id_god = Convert.ToInt32(sdr["id_god"]),
                                //Naziv = sdr["naziv"].ToString(),
                                Naziv_mjeseca = sdr["naziv_mjeseca"].ToString(),
                                //Opis = sdr["opis"].ToString(),
                                Mjesec = Convert.ToInt32(sdr["mjesec"]),
                                Ukupno_dana = Convert.ToInt32(sdr["ukupno_dana"]),
                                Radnih_dana = Convert.ToInt32(sdr["radnih_dana"]),
                                Subota_dana = Convert.ToInt32(sdr["subota_dana"]),
                                Blagdana_dana = Convert.ToInt32(sdr["blagdana_dana"]),
                                Nastavnih_dana = Convert.ToInt32(sdr["nastavnih_dana"]),
                                Praznika_dana = Convert.ToInt32(sdr["praznika_dana"]),
                                Br_sati = Convert.ToInt32(sdr["br_sati"]),
                                Odmor_dana = Convert.ToInt32(sdr["odmor_dana"]),
                                Mj_fond_sati = Convert.ToInt32(sdr["mj_fond_sati"]),
                                Br_rad_dana_sk_god = Convert.ToInt32(sdr["br_rad_dana_sk_god"]),
                                Br_dana_god_odmor = Convert.ToInt32(sdr["br_dana_god_odmor"]),
                                Ukupno_rad_dana = Convert.ToInt32(sdr["ukupno_rad_dana"]),
                                God_fond_sati = Convert.ToInt32(sdr["god_fond_sati"]),
                            };
                        }
                    }
                }
                connection.Close();
            }
            return detalji;
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

        public bool CreateGodisnjiDetalji(Godisnji_detalji detalji)
        {
            try
            {
                this.Connect();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO godisnji_detalji " +
                        /*"(id_god, id_pedagog, ak_godina, br_radnih_dana, br_dana_godina_odmor, ukupni_rad_dana, god_fond_sati) " +*/
                        " VALUES (@id_god, @mjesec, @naziv_mjeseca, @ukupno_dana, @radnih_dana, @subota_dana, @blagdana_dana, @nastavnih_dana" +
                        ", @praznika_dana, @br_sati, @odmor_dana, @odmor_sati, @mj_fond_sati, @br_rad_dana_sk_god, @br_dana_god_odmor, @ukupno_rad_dana" +
                        ", @god_fond_sati)";
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
                    command.Parameters.AddWithValue("@br_rad_dana_sk_god", detalji.Br_rad_dana_sk_god);
                    command.Parameters.AddWithValue("@br_dana_god_odmor", detalji.Br_dana_god_odmor);
                    command.Parameters.AddWithValue("@ukupno_rad_dana", detalji.Ukupno_rad_dana);
                    command.Parameters.AddWithValue("@god_fond_sati", detalji.God_fond_sati);
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
                    command.Parameters.AddWithValue("@br_rad_dana_sk_god", detalji.Br_rad_dana_sk_god);
                    command.Parameters.AddWithValue("@br_dana_god_odmor", detalji.Br_dana_god_odmor);
                    command.Parameters.AddWithValue("@ukupno_rad_dana", detalji.Ukupno_rad_dana);
                    command.Parameters.AddWithValue("@god_fond_sati", detalji.God_fond_sati);
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