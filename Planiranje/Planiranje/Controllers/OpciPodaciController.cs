using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models.Ucenici;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class OpciPodaciController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        public ActionResult RazredniOdjel(int? godina)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            int god;
            List<RazredniOdjel> odjeli = new List<RazredniOdjel>();
            odjeli = baza.RazredniOdjel.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();
            if (godina != null)
            {
                god = (Int32)godina;
            }
            else
            {
                if (odjeli.Count > 0)
                {
                    god = odjeli.Min(m => m.Sk_godina);
                }
                else
                {
                    god = baza.SkolskaGodina.Min(m => m.Sk_Godina);
                }
            }            
            odjeli = baza.RazredniOdjel.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola && w.Sk_godina==god).ToList();
            List<Nastavnik> razrednici = new List<Nastavnik>();
            razrednici = baza.Nastavnik.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();
            ViewBag.razrednici = razrednici;
            ViewBag.selected = god;
            ViewBag.godina = baza.SkolskaGodina.ToList();
            return View(odjeli);
        }
        public ActionResult NoviRazredniOdjel()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            List<Nastavnik> razrednici = new List<Nastavnik>();
            razrednici = baza.Nastavnik.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();            
            if (razrednici.Count == 0)
            {
                string tekst = "Nije pronađen niti jedan nastavnik.<br/>Da biste kreirali razredni odjel, prvo morate dodati nastavika";
                List<string> lista = new List<string>();
                lista.Add(tekst);
                return View("Info", lista);
            }
            ViewBag.razrednici = razrednici;
            return View();
        }
        public ActionResult Nastavnik()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            List<Nastavnik> nastavnici = new List<Nastavnik>();
            nastavnici = baza.Nastavnik.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).OrderBy(o=>o.Id).ToList();
            return View(nastavnici);
        }
        public ActionResult NoviNastavnik()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            return View();
        }
        [HttpPost]
        public ActionResult NoviNastavnik(Nastavnik model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            if(string.IsNullOrWhiteSpace(model.Ime) || string.IsNullOrWhiteSpace(model.Prezime))
            {
                return View(model);
            }
            model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            model.Id_skola = PlaniranjeSession.Trenutni.OdabranaSkola;
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.Nastavnik.Add(model);
                    db.SaveChanges();
                    TempData["poruka"] = "Novi nastavnik je spremljen";
                }
                catch
                {
                    TempData["poruka"] = "Novi nastavnik nije spremljen! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Nastavnik");
        }
        public ActionResult UrediNastavnik(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            Nastavnik model = baza.Nastavnik.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediNastavnik(Nastavnik model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            if (string.IsNullOrWhiteSpace(model.Ime) || string.IsNullOrWhiteSpace(model.Prezime))
            {
                return View(model);
            }
            int id = model.Id;
            Nastavnik nas = baza.Nastavnik.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            if (nas == null)
            {
                return HttpNotFound();
            }
            model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            model.Id_skola = PlaniranjeSession.Trenutni.OdabranaSkola;
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.Nastavnik.Add(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["poruka"] = "Nastavnik je promijenjen";
                }
                catch
                {
                    TempData["poruka"] = "Nastavnik nije promijenjen! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Nastavnik");
        }
        public ActionResult ObrisiNastavnika (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            Nastavnik model = baza.Nastavnik.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiNastavnika (Nastavnik model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            int id = model.Id;
            RazredniOdjel odjel = baza.RazredniOdjel.FirstOrDefault(s => s.Id_razrednik == id && s.Id_skola==PlaniranjeSession.Trenutni.OdabranaSkola);
            if (odjel != null)
            {
                string tekst = "Ne možete obrisati ovog nastavnika jer ste ga Vi ili netko drugi dodijelili kao razrednika nekom razrednom odjelu";
                List<string> lista = new List<string>();
                lista.Add(tekst);
                return View("Info",lista);
            }   
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.Nastavnik.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
                    if (result != null)
                    {
                        db.Nastavnik.Remove(result);
                        db.SaveChanges();
                        TempData["poruka"] = "Nastavnik je obrisan";
                    }
                    else
                    {
                        TempData["poruka"] = "Nastavnik nije pronađen";
                    }
                }
                catch
                {
                    TempData["poruka"] = "Nastavnik nije obrisan! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Nastavnik");
        }
        public ActionResult SkolskaGodina()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            List<Sk_godina> godine = new List<Sk_godina>();
            godine = baza.SkolskaGodina.ToList();
            return View(godine);
        }
    }
}