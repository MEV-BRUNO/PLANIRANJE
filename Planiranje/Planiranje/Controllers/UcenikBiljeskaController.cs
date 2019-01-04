using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models.Ucenici;
using Planiranje.Models;
using System.Net;


namespace Planiranje.Controllers
{
    public class UcenikBiljeskaController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: UcenikBiljeska
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.godine = baza.SkolskaGodina.ToList();
            return View();
        }
        public ActionResult Detalji (int id, int godina)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            UcenikBiljeskaModel model = new UcenikBiljeskaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.RazredniOdjeli = (from raz in baza.RazredniOdjel join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred join
                                    uc in baza.Ucenik on ur.Id_ucenik equals uc.Id_ucenik where uc.Id_ucenik==id select raz).ToList();
            Ucenik_razred ucenikRazred = (from uc in baza.Ucenik
                                          join ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik
                                          join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                          where raz.Sk_godina == godina && uc.Id_ucenik==id
                                          select ur).First();
            int id_ucenikRazred = ucenikRazred.Id;
            model.UcenikBiljeska = baza.UcenikBiljeska.SingleOrDefault(s => s.Id_ucenik_razred == id_ucenikRazred);
            if (model.UcenikBiljeska == null)
            {
                model.UcenikBiljeska = new Ucenik_biljeska();
                model.UcenikBiljeska.Id_ucenik_razred = id_ucenikRazred;
                using(var db = new BazaPodataka())
                {
                    try
                    {
                        db.UcenikBiljeska.Add(model.UcenikBiljeska);
                        db.SaveChanges();
                    }
                    catch
                    {

                    }
                }
                model.UcenikBiljeska = baza.UcenikBiljeska.SingleOrDefault(s => s.Id_ucenik_razred == id_ucenikRazred);
            }
            return View(model);
        }
        public ActionResult Osobni(int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            UcenikBiljeskaModel model = new UcenikBiljeskaModel();
            model.RazredniOdjeli = (from raz in baza.RazredniOdjel join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred join
                                    uc in baza.Ucenik on ur.Id_ucenik equals uc.Id_ucenik where uc.Id_ucenik==id select raz).ToList();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            return View(model);
        }
        public ActionResult PromjenaOsobni (UcenikBiljeskaModel model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idUcenik = model.Ucenik.Id_ucenik;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.Ucenik.SingleOrDefault(s => s.Id_ucenik == idUcenik);
                    if (result != null)
                    {
                        result.Datum = model.Ucenik.Datum;
                        result.Adresa = model.Ucenik.Adresa;
                        result.Grad = model.Ucenik.Grad;
                        result.Oib = model.Ucenik.Oib;
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
        public ActionResult Inicijalni (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            UcenikBiljeskaModel model = new UcenikBiljeskaModel();
            model.UcenikBiljeska = baza.UcenikBiljeska.SingleOrDefault(s => s.Id_biljeska == id);
            return View(model);
        }
        public ActionResult PromjenaInicijalni (UcenikBiljeskaModel model)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = model.UcenikBiljeska.Id_biljeska;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.UcenikBiljeska.SingleOrDefault(s => s.Id_biljeska == id);
                    if (result != null)
                    {
                        result.Inicijalni_podaci = model.UcenikBiljeska.Inicijalni_podaci;
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