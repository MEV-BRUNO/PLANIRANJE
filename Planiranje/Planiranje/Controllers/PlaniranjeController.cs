﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models;
using Planiranje.Models.Ucenici;
using System.Web.Security;
using System.Runtime.Remoting.Messaging;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net;

namespace Planiranje.Controllers
{

	public class PlaniranjeController : Controller
	{
		private Planiranje_DBHandle planovi = new Planiranje_DBHandle();
		private BazaPodataka baza = new BazaPodataka();
        
		public ActionResult Prijava()
		{
			PlaniranjeSession.Trenutni.PedagogId = 0;
			ViewBag.Title = "Prijava";
			return View("Prijava");
		}

		[HttpPost]
		public ActionResult Prijava(Pedagog p)
		{
			ViewBag.Message = null;
			Pedagog pedagog = baza.Pedagog.SingleOrDefault(ped => ped.Email == p.Email && ped.Lozinka == p.Lozinka);
			if (pedagog != null)
			{
                if (pedagog.Aktivan == 1 && pedagog.Licenca.CompareTo(DateTime.Now)>=0)
                {
                    PlaniranjeSession.Trenutni.PedagogId = pedagog.Id_Pedagog;
                    return RedirectToAction("Index");
                }
                else
                {
                    if (pedagog.Aktivan == 0)
                    {
                        ViewBag.Message = "Blokirani ste od strane administratora";
                    }
                    else if (pedagog.Licenca.CompareTo(DateTime.Now) < 0)
                    {
                        ViewBag.Message = "Licenca Vam je istekla " + pedagog.Licenca.ToString() + ". Obratite se administratoru radi produljenja licence.";
                    }                    
                    return View();
                }
				
			}
			else
			{
				ViewBag.Message = "Pogrešno korisničko ime ili lozinka!";
                p = null;
				return View();
			}
		}
		public ActionResult Index()
		{
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Početna";
				return View("Index");
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
		public ActionResult ZaboravljenaLozinka()
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
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
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
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
            
            Pedagog_skola ps = new Pedagog_skola();
            string email = model.Pedagog.Email;
            ps.Id_skola = model.SelectedSchool;
			model.Pedagog.Licenca = DateTime.Now.AddDays(30);
			model.Pedagog.Aktivan = 1;

            try
            {
                baza.Pedagog.Add(model.Pedagog);               
                baza.SaveChanges();
                ps.Id_pedagog = baza.Pedagog.SingleOrDefault(s => s.Email == email).Id_Pedagog;
                baza.PedagogSkola.Add(ps);
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
            model.Pedagog = new Pedagog();
            ViewBag.uspjesno = true;
			model.PopisSkola = new List<SelectListItem>(planovi.ReadSkole().Select(i => new SelectListItem()
			{
				Text = i.Naziv,
				Value = i.Id_skola.ToString()
			}));
			return View("Registracija", model);
        }
        public ActionResult OdabirSkole()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index");
            }
            List<Skola> skole = new List<Skola>();
            int idPed = PlaniranjeSession.Trenutni.PedagogId;
            var result = (from sk in baza.Skola join ps in baza.PedagogSkola on sk.Id_skola equals ps.Id_skola join p in baza.Pedagog
                          on ps.Id_pedagog equals p.Id_Pedagog where p.Id_Pedagog == idPed select sk);
            skole = result.ToList();
            if (PlaniranjeSession.Trenutni.OdabranaSkola != 0)
            {
                TempData["odabrana"] = PlaniranjeSession.Trenutni.OdabranaSkola;
            }
            else
            {
                if (skole.FirstOrDefault() != null)
                {
                    PlaniranjeSession.Trenutni.OdabranaSkola = skole.FirstOrDefault().Id_skola;
                }
            }
            return View(skole);
        }
        public ActionResult PromjenaSkole(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            PlaniranjeSession.Trenutni.OdabranaSkola = id;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public ActionResult OsobniPodaci()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index");
            }
            Pedagog pedagog = baza.Pedagog.SingleOrDefault(s => s.Id_Pedagog == PlaniranjeSession.Trenutni.PedagogId);
            return View(pedagog);
        }
        [HttpPost]
        public ActionResult OsobniPodaci(Pedagog pedagog)
        {
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index");
            }            
            if (string.IsNullOrWhiteSpace(pedagog.Ime)||string.IsNullOrWhiteSpace(pedagog.Prezime)||string.IsNullOrWhiteSpace(pedagog.Titula))
            {               
                return View(pedagog);
            }
            using(var db=new BazaPodataka())
            {
                try
                {
                    var ped = db.Pedagog.SingleOrDefault(s => s.Id_Pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (ped != null)
                    {
                        ped.Ime = pedagog.Ime;
                        ped.Prezime = pedagog.Prezime;
                        ped.Titula = pedagog.Titula;
                        db.SaveChanges();
                    }
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
    }
}