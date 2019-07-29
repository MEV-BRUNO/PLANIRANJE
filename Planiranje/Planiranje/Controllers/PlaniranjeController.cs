using System;
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
		private BazaPodataka baza = new BazaPodataka();
        
		public ActionResult Prijava()
		{
			PlaniranjeSession.Trenutni.PedagogId = 0;
            PlaniranjeSession.Trenutni.OdabranaSkola = 0;			
			return View();
		}

        [ValidateAntiForgeryToken]
		[HttpPost]
		public ActionResult Prijava(Pedagog p)
		{
			ViewBag.Message = null;
			Pedagog pedagog = baza.Pedagog.SingleOrDefault(ped => ped.Email == p.Email && ped.Lozinka.CompareTo(p.Lozinka)==0);
			if (pedagog != null)
			{
                if ((pedagog.Aktivan == true && pedagog.Licenca.CompareTo(DateTime.Now)>=0) || pedagog.Id_Pedagog==1)
                {
                    PlaniranjeSession.Trenutni.PedagogId = pedagog.Id_Pedagog;
                    return RedirectToAction("Index");
                }
                else
                {
                    if (pedagog.Aktivan == false)
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
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
		public ActionResult ZaboravljenaLozinka()
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{				
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}

        [ValidateAntiForgeryToken]
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
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
                return RedirectToAction("Prijava", "Planiranje");                          
			}                  
            PlaniranjeModel model = new PlaniranjeModel();
            model.Pedagog = new Pedagog();
            model.PopisSkola = VratiSelectListSkole();
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Registracija(PlaniranjeModel model)
        {
			Pedagog ped = baza.Pedagog.SingleOrDefault(pedagog => pedagog.Email == model.Pedagog.Email);
            if (ped != null)
            {
                ViewBag.Message = "Korisnik s tom e-mail adresom postoji. Ako ste već registrirani, možete ponovno postaviti lozinku!";
                model.PopisSkola = VratiSelectListSkole();
				return View(model);
            }
            if (!ModelState.IsValid)
            {                
                model.PopisSkola = VratiSelectListSkole();
                return View(model);
            }
            Pedagog_skola ps = new Pedagog_skola();
            string email = model.Pedagog.Email;
            ps.Id_skola = model.SelectedSchool;
			model.Pedagog.Licenca = DateTime.Now.AddDays(30);
			model.Pedagog.Aktivan = true;

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
                model.PopisSkola = VratiSelectListSkole();
				return View(model);
            }
            ViewBag.Message = "Registracija je uspješna. Možete se prijaviti";
            model.Pedagog = new Pedagog();
            ViewBag.uspjesno = true;
            model.PopisSkola = VratiSelectListSkole();
			return View(model);
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
            Pedagog_skola pedagog_Skola = baza.PedagogSkola.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                                                            && s.Id_skola == id);
            if (pedagog_Skola == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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
            pedagog.Lozinka = string.Empty;
            return View(pedagog);
        }
        [HttpPost]
        public ActionResult OsobniPodaci(Pedagog pedagog)
        {
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index");
            }            
            if (string.IsNullOrWhiteSpace(pedagog.Ime)||string.IsNullOrWhiteSpace(pedagog.Prezime)||string.IsNullOrWhiteSpace(pedagog.Titula)
                || (!string.IsNullOrWhiteSpace(pedagog.Lozinka) && (pedagog.Lozinka.Length < 6 || pedagog.Lozinka.Length > 12)))
            {
                if (string.IsNullOrEmpty(pedagog.Lozinka))
                {
                    ModelState.Remove("Lozinka");
                }
                else if (string.IsNullOrWhiteSpace(pedagog.Lozinka))
                {
                    ModelState.Remove("Lozinka");
                    ModelState.AddModelError("Lozinka", "Lozinka ne može biti skup praznih znakova");
                }
                else if (pedagog.Lozinka.Length<6 || pedagog.Lozinka.Length > 12)
                {
                    
                }
                return View(pedagog);
            }
            else if (string.IsNullOrWhiteSpace(pedagog.Lozinka))
            {
                try
                {
                    if(pedagog.Lozinka.Length<6 || pedagog.Lozinka.Length > 12)
                    {                        
                    }
                    ModelState.Remove("Lozinka");
                    ModelState.AddModelError("Lozinka", "Lozinka ne može biti skup praznih znakova");
                    return View(pedagog);
                }
                catch
                {

                }
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
                        if (!string.IsNullOrWhiteSpace(pedagog.Lozinka))
                        {
                            ped.Lozinka = pedagog.Lozinka;
                        }                       
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
        private SelectList VratiSelectListSkole()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            List<Skola> skole = baza.Skola.ToList();
            foreach(var item in skole)
            {
                selectListItems.Add(new SelectListItem { Text = item.Naziv, Value = item.Id_skola.ToString() });
            }
            SelectList selects = new SelectList(selectListItems,"Value","Text");
            return selects;
        }
    }
}