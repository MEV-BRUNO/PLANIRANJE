using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Planiranje.Controllers;

namespace Planiranje.Models
{
    public class OS_Plan_2_DBHandle
    {
        private MySqlConnection con;
        string str = ConfigurationManager.ConnectionStrings["BazaPodataka"].ConnectionString;

        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["BazaPodataka"].ToString();
            con = new MySqlConnection(constring);
        }

        public List<OS_Plan_2> DohvatiOS_Plan_2()
        {
            connection();
            List<OS_Plan_2> planovi = new List<OS_Plan_2>();
            con.ConnectionString = str;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "SELECT id_plan, naziv, ak_godina, opis FROM OS_Plan_2 where id_pedagog = " + AppSession.Current.UserId + " ORDER BY id_plan ASC";
                con.Open();
                using (MySqlDataReader sdr = cmd.ExecuteReader())
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
                            if (plan.Naziv.Length > 0)
                                planovi.Add(plan);
                        }
                    }
                }
                con.Close();
            }
            return planovi;
        }
    }
}
