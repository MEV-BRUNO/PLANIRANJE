using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models;
using System.Web.Security;
using System.Runtime.Remoting.Messaging;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Planiranje.Controllers
{

	public class PlaniranjeController : Controller
	{
		private Planiranje_DBHandle planovi = new Planiranje_DBHandle();
		private BazaPodataka baza = new BazaPodataka();
        

		[HttpGet]
		public ActionResult Prijava()
		{
			PlaniranjeSession.Trenutni.PedagogId = 0;
			ViewBag.Title = "Prijava";
			return View();
		}

		[HttpPost]
		public ActionResult Prijava(Pedagog p)
		{
			ViewBag.Message = null;
			Pedagog pedagog = baza.Pedagog.SingleOrDefault(ped => ped.Email == p.Email && ped.Lozinka == p.Lozinka);
			if (pedagog != null)
			{
				PlaniranjeSession.Trenutni.PedagogId = pedagog.Id_Pedagog;
				return RedirectToAction("Index");
			}
			else
			{
				ViewBag.Message = "Korisnik nije pronadjen!";
				return View("Prijava");
			}
		}
		public ActionResult Index()
		{
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Početna";
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
		public ActionResult ZaboravljenaLozinka()
		{
			if (PlaniranjeSession.Trenutni.PedagogId == 0)
			{
				ViewBag.Title = "Zaboravljena lozinka";
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}

        [HttpPost]
        public ActionResult ZaboravljenaLozinka(Pedagog p)
        {
            Pedagog pedagog = baza.Pedagog.SingleOrDefault(ped => ped.Email == p.Email);
            if (pedagog == null)
            {
                ViewBag.Message = "Korisnik ne postoji";
                return View();
            }
            string[] abeceda = { "a","b", "c", "d", "e", "f", "g" };
            Random r = new Random();
            string lozinka="";
            for(int i = 0; i < 6; i++)
            {
                lozinka += abeceda[r.Next(0, abeceda.Length-1)];
            }
            pedagog.Lozinka = lozinka;

            baza.Pedagog.SqlQuery("UPDATE pedagog SET lozinka = @lozinka WHERE email = @email", new SqlParameter("@email",pedagog.Email)
                ,new SqlParameter("@lozinka",pedagog.Lozinka));
            

                MailMessage mail = new MailMessage("noreply@planiranje.com", pedagog.Email,
                    "Podaci o promjeni lozinke", "Vaša nova lozinka je " + lozinka + ". Molimo promijenite je u što kraćem roku.");
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = true;
                //smtp.Credentials = new System.Net.NetworkCredential()
                try
                {
                    smtp.Send(mail);
                }
                catch
                {

                }            

            baza.SaveChanges();
            return RedirectToAction("Prijava");
        }
		public ActionResult Registracija()
		{
			if (PlaniranjeSession.Trenutni.PedagogId == 0)
			{
                ViewBag.poruka = null;
				ViewBag.Title = "Registracija";
                ViewBag.lozinka = "";
				PlaniranjeModel model = new PlaniranjeModel();
				model.Pedagog = new Pedagog();
				model.PopisSkola = new List<SelectListItem>(planovi.ReadSkole().Select(i => new SelectListItem()
				{
					Text = i.Naziv,
					Value = i.Id_skola.ToString()
				}));
				return View("Registracija", model);
			}
			return RedirectToAction("Prijava", "Planiranje");
		}

        [HttpPost]
        public ActionResult Registracija(PlaniranjeModel model)
        {
			Pedagog ped = baza.Pedagog.SingleOrDefault(pedagog => pedagog.Email == model.Pedagog.Email);
            if (ped != null)
            {
                ViewBag.Message = "Korisnik s tom e-mail adresom postoji. Ako ste već registrirani, možete ponovno postaviti lozinku!";
				model.PopisSkola = new List<SelectListItem>(planovi.ReadSkole().Select(i => new SelectListItem()
				{
					Text = i.Naziv,
					Value = i.Id_skola.ToString()
				}));
				return View("Registracija", model);
            }
			
            model.Pedagog.Id_skola = model.SelectedSchool;
			model.Pedagog.Licenca = DateTime.Now.AddYears(2);
			model.Pedagog.Aktivan = '1';

            try
            {
                baza.Pedagog.Add(model.Pedagog);
                baza.SaveChanges();
            }
            catch
            {
				ViewBag.Message = "Registracija nije uspjela. Pokušajte ponovno";
				model.PopisSkola = new List<SelectListItem>(planovi.ReadSkole().Select(i => new SelectListItem()
				{
					Text = i.Naziv,
					Value = i.Id_skola.ToString()
				}));
				return View("Registracija", model);
            }
            ViewBag.Message = "Registracija je uspješna. Možete se prijaviti";
			model.PopisSkola = new List<SelectListItem>(planovi.ReadSkole().Select(i => new SelectListItem()
			{
				Text = i.Naziv,
				Value = i.Id_skola.ToString()
			}));
			return View("Registracija", model);
        }
    }
}