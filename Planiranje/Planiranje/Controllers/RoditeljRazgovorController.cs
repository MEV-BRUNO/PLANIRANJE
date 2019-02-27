using System;
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
    public class RoditeljRazgovorController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: RoditeljRazgovor
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
            List<Roditelj_razgovor> model = (from razgovor in baza.RoditeljRazgovor
                                             join ur in baza.UcenikRazred on razgovor.Id_ucenik_razred equals ur.Id
                                             join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                             where ur.Id_ucenik == id && raz.Sk_godina == godina && razgovor.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                             select razgovor).ToList();
            List<Obitelj> roditelji = baza.Obitelj.Where(w => w.Id_ucenik == id && (w.Svojstvo == 1 || w.Svojstvo == 2 || w.Svojstvo == 3)).ToList();
            List<string> svojstvo = new List<string>() { "", "Otac", "Majka", "Skrbnik" };
            ViewBag.ucenik = ucenik;
            ViewBag.roditelji = roditelji;
            ViewBag.svojstvo = svojstvo;
            ViewBag.godina = godina;
            return View(model);
        }
        public ActionResult NoviRazgovor(int idUcenik, int godina, int id)
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
                Roditelj_razgovor model = baza.RoditeljRazgovor.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
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
        public ActionResult NoviRazgovor(Roditelj_razgovor model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (string.IsNullOrWhiteSpace(model.Biljeska) || model.Id_roditelj == 0 || string.IsNullOrWhiteSpace(model.Dogovor) ||
                string.IsNullOrWhiteSpace(model.Dolazak) || string.IsNullOrWhiteSpace(model.Izvjestiti) || string.IsNullOrWhiteSpace(model.Prijedlog_roditelja) ||
                string.IsNullOrWhiteSpace(model.Prijedlog_skole) || string.IsNullOrWhiteSpace(model.Razlog) || string.IsNullOrWhiteSpace(model.Trazi) ||
                model.Datum.CompareTo(new DateTime(1,1,1))==0 || model.Vrijeme.Hour==0 || model.Datum_slijedeci.CompareTo(new DateTime(1,1,1))==0)
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
                        db.RoditeljRazgovor.Add(model);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (var db = new BazaPodataka())
                    {
                        model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        db.RoditeljRazgovor.Add(model);
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
    }
}