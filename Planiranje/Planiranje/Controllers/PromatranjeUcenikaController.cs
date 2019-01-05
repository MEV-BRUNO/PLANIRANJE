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
        public ActionResult Detalji(int id)
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
            return View(model);
        }
        public ActionResult NovoPromatranje (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.id = id;
            return View();
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