﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models.Ucenici;
using Planiranje.Models;
using System.Net;
using Planiranje.Reports;
using System.IO;

namespace Planiranje.Controllers
{
    public class PromatranjeUcenikaController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: PromatranjeUcenika
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.godine = baza.SkolskaGodina.ToList();
            return View();
        }
        public ActionResult Detalji(int id, int godina)
        {
            //id - id učenika
            //godina - id godine
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PromatranjeUcenikaModel model = new PromatranjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.PromatranjaUcenika = (from raz in baza.RazredniOdjel join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred
                                        join prom in baza.PromatranjeUcenika on ur.Id equals prom.Id_ucenik_razred
                                        where ur.Id_ucenik==id && raz.Sk_godina==godina &&
                                        prom.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId select prom).ToList();
            model.RazredniOdjeli = (from ur in baza.UcenikRazred
                                    join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                    where ur.Id_ucenik == id
                                    select raz).ToList();
            model.UcenikRazred = (from ur in baza.UcenikRazred
                                  join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                  where ur.Id_ucenik == id && raz.Sk_godina == godina
                                  select ur).First();
            return View(model);
        }
        public ActionResult Promatranje (int id, int idUcenikRazred)
        {
            // ulazni parametar id je zapravo id ucenika
            PromatranjeUcenikaModel model = new PromatranjeUcenikaModel();
            model.PromatranjaUcenika = (from ur in baza.UcenikRazred join prom in baza.PromatranjeUcenika on 
                                        ur.Id equals prom.Id_ucenik_razred where ur.Id_ucenik==id &&
                                        prom.Id_ucenik_razred == idUcenikRazred &&
                                        prom.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId select prom).ToList();
            model.UcenikRazred = baza.UcenikRazred.SingleOrDefault(s => s.Id==idUcenikRazred);
            return View(model);
        }
        public ActionResult NovoPromatranje (int id)
        {
            //ulazni parametar id je zapravo id tablice ucenik_razred
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult NovoPromatranje (PromatranjeUcenikaModel model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(string.IsNullOrWhiteSpace(model.PromatranjeUcenika.Cilj) || string.IsNullOrWhiteSpace(model.PromatranjeUcenika.SocStatusUcenika)
                || model.PromatranjeUcenika.Nadnevak.CompareTo(new DateTime(1, 1, 1)) == 0
                || model.PromatranjeUcenika.Vrijeme.Hour==0)
            {
                ViewBag.id = model.PromatranjeUcenika.Id_ucenik_razred;
                return View(model);
            }
            model.PromatranjeUcenika.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.PromatranjeUcenika.Add(model.PromatranjeUcenika);
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            int idUR = model.PromatranjeUcenika.Id_ucenik_razred;
            Ucenik_razred ur = baza.UcenikRazred.SingleOrDefault(s => s.Id == idUR);
            return RedirectToAction("Promatranje", new { id = ur.Id_ucenik, idUcenikRazred = ur.Id });
        }
        public ActionResult Osobni (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PromatranjeUcenikaModel model = new PromatranjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.RazredniOdjeli = (from ur in baza.UcenikRazred
                                    join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                    where ur.Id_ucenik == id
                                    select raz).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult PromjenaOsobni (PromatranjeUcenikaModel model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = model.Ucenik.Id_ucenik;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
                    if (result != null)
                    {
                        result.Oib = model.Ucenik.Oib;
                        result.Grad = model.Ucenik.Grad;
                        result.Datum = model.Ucenik.Datum;
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
        public ActionResult UrediPromatranje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PromatranjeUcenikaModel model = new PromatranjeUcenikaModel();
            model.PromatranjeUcenika = baza.PromatranjeUcenika.SingleOrDefault(s => s.Id == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediPromatranje (PromatranjeUcenikaModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (string.IsNullOrWhiteSpace(model.PromatranjeUcenika.Cilj) || string.IsNullOrWhiteSpace(model.PromatranjeUcenika.SocStatusUcenika))                
            {               
                return View(model);
            }
            model.PromatranjeUcenika.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.PromatranjeUcenika.Add(model.PromatranjeUcenika);
                    db.Entry(model.PromatranjeUcenika).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            int idUR = model.PromatranjeUcenika.Id_ucenik_razred;
            Ucenik_razred ur = baza.UcenikRazred.SingleOrDefault(s => s.Id == idUR);
            return RedirectToAction("Promatranje", new { id=ur.Id_ucenik, idUcenikRazred=idUR});
        }
        public ActionResult ObrisiPromatranje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PromatranjeUcenikaModel model = new PromatranjeUcenikaModel();
            model.PromatranjeUcenika = baza.PromatranjeUcenika.SingleOrDefault(s => s.Id == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiPromatranje (PromatranjeUcenikaModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idPromatranje = model.PromatranjeUcenika.Id;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.PromatranjeUcenika.SingleOrDefault(s => s.Id == idPromatranje && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        db.PromatranjeUcenika.Remove(result);
                        db.SaveChanges();
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    }
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            int idUR = model.PromatranjeUcenika.Id_ucenik_razred;
            Ucenik_razred UR = baza.UcenikRazred.SingleOrDefault(s => s.Id == idUR);
            return RedirectToAction("Promatranje", new { id = UR.Id_ucenik, idUcenikRazred = idUR });
        }
        public FileStreamResult Ispis (int id)
        {
            //ulazni parametar id je id promatranja učenika
            PromatranjeUcenikaModel model = new PromatranjeUcenikaModel();
            model.Ucenik = new Ucenik();
            model.PromatranjeUcenika = new Promatranje_ucenika();
            model.PromatranjeUcenika = baza.PromatranjeUcenika.Single(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            int idUR = model.PromatranjeUcenika.Id_ucenik_razred;
            Ucenik_razred UR = new Ucenik_razred();
            UR = baza.UcenikRazred.Single(s => s.Id == idUR);
            int idUcenik = UR.Id_ucenik;
            int idRazred = UR.Id_razred;
            model.Ucenik = baza.Ucenik.Single(s => s.Id_ucenik == idUcenik);
            model.Razred = new RazredniOdjel();
            model.Razred = baza.RazredniOdjel.Single(s => s.Id == idRazred && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            int idRazrednik = model.Razred.Id_razrednik;
            Nastavnik razrednik = new Nastavnik();
            razrednik = baza.Nastavnik.Single(s => s.Id == idRazrednik && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            Pedagog pedagog = new Pedagog();
            pedagog = baza.Pedagog.Single(s => s.Id_Pedagog == PlaniranjeSession.Trenutni.PedagogId);
            Skola skola = new Skola();
            skola = baza.Skola.Single(s => s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);

            PromatranjeUcenikaReport report = new PromatranjeUcenikaReport(model, razrednik, pedagog, skola);
            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
        }
    }
}