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
    public class RoditeljUgovorController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: RoditeljUgovor
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<Sk_godina> godine = baza.SkolskaGodina.ToList();
            ViewBag.godine = godine;
            return View();
        }
        public ActionResult Detalji(int id, int godina)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            //ulazni parametar id je id učenika
            Ucenik ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            if (ucenik == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Roditelj_ugovor> model = (from pr in baza.RoditeljUgovor
                                             join ur in baza.UcenikRazred on pr.Id_ucenik_razred equals ur.Id
                                             join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                             where ur.Id_ucenik == id && raz.Sk_godina == godina && pr.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                             select pr).ToList();
            List<Obitelj> roditelji = baza.Obitelj.Where(w => w.Id_ucenik == id && (w.Svojstvo == 1 || w.Svojstvo == 2 || w.Svojstvo == 3)).ToList();
            List<string> svojstvo = new List<string>() { "", "Otac", "Majka", "Skrbnik" };
            ViewBag.ucenik = ucenik;
            ViewBag.roditelji = roditelji;
            ViewBag.svojstvo = svojstvo;
            ViewBag.godina = godina;
            return View(model);
        }
        public ActionResult NoviUgovor(int idUcenik, int godina, int id)
        {
            //ulazni parametar id je id procjene, ukoliko je on 0, radi se o novoj procjeni
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (id == 0 && idUcenik > 0 && godina > 0)
            {
                Ucenik_razred UR = (from ur in baza.UcenikRazred
                                    join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                    where ur.Id_ucenik == idUcenik && raz.Sk_godina == godina
                                    select ur).FirstOrDefault();
                if (UR == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                List<Obitelj> roditelji = baza.Obitelj.Where(w => w.Id_ucenik == idUcenik && (w.Svojstvo == 1 || w.Svojstvo == 2 || w.Svojstvo == 3)).ToList();
                IEnumerable<SelectListItem> select = new SelectList(roditelji, "Id_obitelj", "ImePrezime");
                ViewBag.ur = UR.Id;
                ViewBag.roditelji = select;
                return View();
            }
            else if (idUcenik > 0 && id > 0)
            {
                Roditelj_ugovor model = baza.RoditeljUgovor.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                List<Obitelj> roditelji = baza.Obitelj.Where(w => w.Id_ucenik == idUcenik && (w.Svojstvo == 1 || w.Svojstvo == 2 || w.Svojstvo == 3)).ToList();
                IEnumerable<SelectListItem> select = new SelectList(roditelji, "Id_obitelj", "ImePrezime");
                ViewBag.ur = null;
                ViewBag.roditelji = select;
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult NoviUgovor(Roditelj_ugovor model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (string.IsNullOrWhiteSpace(model.Cilj1) || model.Id_roditelj == 0 || string.IsNullOrWhiteSpace(model.Cilj2) ||
                string.IsNullOrWhiteSpace(model.Izvjesce) || string.IsNullOrWhiteSpace(model.Ostala_zapazanja) || string.IsNullOrWhiteSpace(model.Poduzeto) ||
                string.IsNullOrWhiteSpace(model.Predstavnik_skole) || string.IsNullOrWhiteSpace(model.Zapazanje) || 
                model.Datum.CompareTo(new DateTime(1,1,1))==0 || model.Slijedeci_susret.CompareTo(new DateTime(1,1,1))==0)
            {
                int idUR = model.Id_ucenik_razred;
                Ucenik_razred UR = baza.UcenikRazred.SingleOrDefault(s => s.Id == idUR);
                if (UR == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                int idUcenik = UR.Id_ucenik;
                List<Obitelj> roditelji = baza.Obitelj.Where(w => w.Id_ucenik == idUcenik).ToList();
                IEnumerable<SelectListItem> select = new SelectList(roditelji, "Id_obitelj", "ImePrezime");
                if (model.Id > 0)
                {
                    ViewBag.ur = null;
                }
                else
                {
                    ViewBag.ur = UR.Id;
                }
                ViewBag.roditelji = select;
                //provjera ako je potrebno promijeniti početnu karticu
                if (!string.IsNullOrWhiteSpace(model.Cilj1) && model.Id_roditelj != 0 && !string.IsNullOrWhiteSpace(model.Cilj2) &&
                    (string.IsNullOrWhiteSpace(model.Izvjesce) || string.IsNullOrWhiteSpace(model.Ostala_zapazanja)) && !string.IsNullOrWhiteSpace(model.Poduzeto) &&
                    !string.IsNullOrWhiteSpace(model.Predstavnik_skole) && !string.IsNullOrWhiteSpace(model.Zapazanje) &&
                    model.Datum.CompareTo(new DateTime(1, 1, 1)) != 0 && model.Slijedeci_susret.CompareTo(new DateTime(1, 1, 1)) != 0)
                {
                    ViewBag.promijeni = "true";
                }
                    return View(model);
            }

            try
            {
                int idUR = model.Id_ucenik_razred;
                int idUcenik = (from ur in baza.UcenikRazred where ur.Id == idUR select ur.Id_ucenik).First();
                int g = (from ur in baza.UcenikRazred
                         join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                         where ur.Id == idUR
                         select raz.Sk_godina).First();
                if (model.Id == 0)
                {
                    using (var db = new BazaPodataka())
                    {
                        model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        db.RoditeljUgovor.Add(model);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (var db = new BazaPodataka())
                    {
                        model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        db.RoditeljUgovor.Add(model);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Detalji", new { id = idUcenik, godina = g });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
        }
        public ActionResult ObrisiUgovor(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Roditelj_ugovor model = baza.RoditeljUgovor.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            int idRoditelj = model.Id_roditelj;
            Obitelj o = baza.Obitelj.SingleOrDefault(s => s.Id_obitelj == idRoditelj);
            if (o == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.ime = o.ImePrezime;
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiUgovor (Roditelj_ugovor model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idUcenik = 0, g = 0;
            try
            {
                using (var db = new BazaPodataka())
                {
                    int id = model.Id;
                    var result = db.RoditeljUgovor.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        int idUR = result.Id_ucenik_razred;
                        Ucenik_razred ur = db.UcenikRazred.SingleOrDefault(s => s.Id == idUR);
                        idUcenik = ur.Id_ucenik;
                        g = (from urr in db.UcenikRazred
                             join raz in db.RazredniOdjel on urr.Id_razred equals raz.Id
                             where urr.Id == idUR
                             select raz.Sk_godina).FirstOrDefault();
                        db.RoditeljUgovor.Remove(result);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Detalji", new { id = idUcenik, godina = g });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
        }
        public ActionResult Ispis (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Roditelj_ugovor model = baza.RoditeljUgovor.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            Skola skola = (from sk in baza.Skola
                           join raz in baza.RazredniOdjel on sk.Id_skola equals raz.Id_skola
                           join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred
                           join razg in baza.RoditeljUgovor on ur.Id equals razg.Id_ucenik_razred
                           where razg.Id == id && razg.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                           select sk).SingleOrDefault();
            if (skola == null)
            {
                skola = new Skola();
            }
            RazredniOdjel odjel = (from raz in baza.RazredniOdjel
                                   join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred
                                   join razg in baza.RoditeljUgovor on ur.Id equals razg.Id_ucenik_razred
                                   where razg.Id == id && razg.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                   select raz).SingleOrDefault();
            if (odjel == null)
            {
                odjel = new RazredniOdjel();
            }
            Ucenik ucenik = (from uc in baza.Ucenik
                             join ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik
                             join razg in baza.RoditeljUgovor on ur.Id equals razg.Id_ucenik_razred
                             where razg.Id == id && razg.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                             select uc).SingleOrDefault();
            if (ucenik == null)
            {
                ucenik = new Ucenik();
            }
            Obitelj roditelj = (from ob in baza.Obitelj
                                join razg in baza.RoditeljUgovor on ob.Id_obitelj equals razg.Id_roditelj
                                where razg.Id == id && razg.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                select ob).SingleOrDefault();
            if (roditelj == null)
            {
                roditelj = new Obitelj();
            }
            Pedagog pedagog = (from ped in baza.Pedagog
                               join razg in baza.RoditeljUgovor on ped.Id_Pedagog equals razg.Id_pedagog
                               where razg.Id == id && razg.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                               select ped).SingleOrDefault();
            int idRazrednik = odjel.Id_razrednik;
            Nastavnik razrednik = baza.Nastavnik.SingleOrDefault(s => s.Id == idRazrednik);
            if (razrednik == null)
            {
                razrednik = new Nastavnik();
            }
            RoditeljUgovorReport report = new RoditeljUgovorReport(model, skola, razrednik, pedagog, odjel, ucenik, roditelj);
            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
        }
    }
}