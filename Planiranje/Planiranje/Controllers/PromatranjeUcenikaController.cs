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
        public ActionResult Detalji(int id, int idRazred)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PromatranjeUcenikaModel model = new PromatranjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.PromatranjaUcenika = (from ur in baza.UcenikRazred join prom in baza.PromatranjeUcenika on 
                                        ur.Id equals prom.Id_ucenik_razred where ur.Id_ucenik==id select prom).ToList();
            model.RazredniOdjeli = (from ur in baza.UcenikRazred
                                    join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                    where ur.Id_ucenik == id
                                    select raz).ToList();
            model.UcenikRazred = baza.UcenikRazred.SingleOrDefault(s => s.Id_razred == idRazred && s.Id_ucenik == id);
            return View(model);
        }
        public ActionResult Promatranje (int id, int idUcenikRazred)
        {
            // ulazni parametar id je zapravo id ucenika
            PromatranjeUcenikaModel model = new PromatranjeUcenikaModel();
            model.PromatranjaUcenika = (from ur in baza.UcenikRazred join prom in baza.PromatranjeUcenika on 
                                        ur.Id equals prom.Id_ucenik_razred where ur.Id_ucenik==id select prom).ToList();
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
                || model.PromatranjeUcenika.Nadnevak.CompareTo(new DateTime(1, 1, 1)) == 0)
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
                    return HttpNotFound();
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
                catch { }
            }
            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
    }
}