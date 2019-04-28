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
    public class UcenikZapisnikController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: UcenikZapisnik
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
            //ulazni parametar id je id učenika
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            if (ucenik == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(ucenik);
        }        
        public ActionResult Osnovni (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            //id učenika
            Ucenik ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            ViewBag.odjeli = (from raz in baza.RazredniOdjel join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred
                              join uc in baza.Ucenik on ur.Id_ucenik equals uc.Id_ucenik where raz.Id_skola==PlaniranjeSession.Trenutni.OdabranaSkola
                              && uc.Id_ucenik==id select raz).ToList();
            return View(ucenik);
        }
        [HttpPost]
        public ActionResult Osnovni (Ucenik model)
        {
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            using (var db = new BazaPodataka())
            {
                try
                {
                    int id = model.Id_ucenik;
                    var result = db.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
                    if (result != null)
                    {
                        result.Datum = model.Datum;
                        result.Oib = model.Oib;
                        result.Adresa = model.Adresa;
                        result.Grad = model.Grad;
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
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}