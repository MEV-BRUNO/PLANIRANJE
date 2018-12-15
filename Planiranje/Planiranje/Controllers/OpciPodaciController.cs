﻿using System;
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
        public ActionResult NoviRazredniOdjel(int godina)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            List<Nastavnik> razrednici = new List<Nastavnik>();
            razrednici = baza.Nastavnik.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();            
            if (razrednici.Count == 0)
            {
                string tekst = "Nije pronađen niti jedan nastavnik. Za kreiranje razrednog odjela potrebno je prvo dodati nastavnika";
                List<string> lista = new List<string>();
                lista.Add(tekst);
                return View("Info", lista);
            }
            IEnumerable<SelectListItem> select = new SelectList(razrednici, "Id", "ImePrezime");
            ViewBag.razrednici = select;
            ViewBag.godina = godina;
            return View();
        }
        [HttpPost]
        public ActionResult NoviRazredniOdjel(RazredniOdjel odjel)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            if (string.IsNullOrWhiteSpace(odjel.Naziv) || odjel.Razred < 1 || odjel.Razred > 12 || odjel.Id_razrednik <= 0)
            {
                List<Nastavnik> razrednici = new List<Nastavnik>();
                razrednici = baza.Nastavnik.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();
                IEnumerable<SelectListItem> select = new SelectList(razrednici, "Id", "ImePrezime");
                ViewBag.razrednici = select;
                ViewBag.godina = odjel.Sk_godina;
                return View();
            }
            odjel.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            odjel.Id_skola = PlaniranjeSession.Trenutni.OdabranaSkola;
            int god = odjel.Sk_godina;
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.RazredniOdjel.Add(odjel);
                    db.SaveChanges();
                    TempData["poruka"] = "Novi odjel je spremljen";
                }
                catch
                {
                    TempData["poruka"] = "Došlo je do greške! Pokušajte ponovno";
                }
            }
            return RedirectToAction("RazredniOdjel", new { godina=god});
        }
        public ActionResult UrediRazredniOdjel(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            RazredniOdjel odjel = baza.RazredniOdjel.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            if (odjel == null)
            {
                string tekst = "Traženi razredni odjel nije pronađen!";
                List<string> lista = new List<string>();
                lista.Add(tekst);
                return View("Info", lista);
            }
            List<Nastavnik> razrednici = new List<Nastavnik>();
            razrednici = baza.Nastavnik.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();
            IEnumerable<SelectListItem> select = new SelectList(razrednici, "Id", "ImePrezime");
            ViewBag.razrednici = select;
            return View(odjel);
        }
        [HttpPost]
        public ActionResult UrediRazredniOdjel(RazredniOdjel odjel)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            int id = odjel.Id;
            RazredniOdjel raz = baza.RazredniOdjel.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            if (raz == null)
            {
                return HttpNotFound();
            }
            if (string.IsNullOrWhiteSpace(odjel.Naziv) || odjel.Razred < 1 || odjel.Razred > 12 || odjel.Razred <= 0)
            {
                List<Nastavnik> razrednici = new List<Nastavnik>();
                razrednici = baza.Nastavnik.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();
                IEnumerable<SelectListItem> select = new SelectList(razrednici, "Id", "ImePrezime");
                ViewBag.razrednici = select;
                return View(odjel);
            }
            odjel.Id_pedagog = raz.Id_pedagog;
            odjel.Sk_godina = raz.Sk_godina;
            odjel.Id_skola = PlaniranjeSession.Trenutni.OdabranaSkola;
            int god = odjel.Sk_godina;
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.RazredniOdjel.Add(odjel);
                    db.Entry(odjel).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["poruka"] = "Razredni odjel je promijenjen";
                }
                catch
                {
                    TempData["poruka"] = "Razredni odjel nije promijenjen! Pokušajte ponovno";
                }
            }
            return RedirectToAction("RazredniOdjel", new { godina=god});
        }
        public ActionResult ObrisiRazredniOdjel(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            RazredniOdjel odjel = baza.RazredniOdjel.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            if (odjel == null)
            {
                string tekst = "Traženi odjel nije pronađen!";
                List<string> lista = new List<string>();
                lista.Add(tekst);
                return View("Info", lista);
            }
            int idRazrednik = odjel.Id_razrednik;
            Nastavnik razrednik = baza.Nastavnik.SingleOrDefault(s => s.Id == idRazrednik && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            if (razrednik == null)
            {
                string tekst = "Razrednik traženog odjela nije pronađen!";
                List<string> lista = new List<string>();
                lista.Add(tekst);
                return View("Info", lista);
            }
            ViewBag.razrednik = razrednik.ImePrezime;
            return View(odjel);
        }
        [HttpPost]
        public ActionResult ObrisiRazredniOdjel(RazredniOdjel odjel)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                RedirectToAction("Index", "Planiranje");
            }
            int id = odjel.Id;
            RazredniOdjel raz = baza.RazredniOdjel.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            if (raz == null)
            {
                return HttpNotFound();
            }
            int god = raz.Sk_godina;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.RazredniOdjel.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
                    if (result != null)
                    {
                        db.RazredniOdjel.Remove(result);
                        db.SaveChanges();
                        TempData["poruka"] = "Razredni odjel je obrisan";
                    }
                    else
                    {
                        TempData["poruka"] = "Razredni odjel nije pronađen";
                    }
                }
                catch
                {
                    TempData["poruka"] = "Razredni odjel nije obrisan! Pokušajte ponovno";
                }
            }
            return RedirectToAction("RazredniOdjel", new { godina = god });
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