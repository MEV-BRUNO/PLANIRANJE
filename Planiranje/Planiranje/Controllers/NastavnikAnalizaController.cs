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
    public class NastavnikAnalizaController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: NastavniciAnaliza
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
        public ActionResult OdabirNastavnika()
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId<=0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<Nastavnik> nastavnici = baza.Nastavnik.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).OrderBy(o => o.Id).ToList();
            return View(nastavnici);
        }
        public ActionResult Detalji(int id, int godina)
        {
            //ulazni parametar id je id nastavnika, a godina je skolska godina
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<Nastavnik_analiza> model = baza.NastavnikAnaliza.Where(w => w.Id_nastavnik==id && w.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId &&
            w.Sk_godina == godina && w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();
            Nastavnik nastavnik = baza.Nastavnik.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            ViewBag.nastavnik = nastavnik;
            ViewBag.godina = godina;
            return View(model);
        }
        public ActionResult NovaAnaliza(int idNastavnik, int godina, int id)
        {
            //ulazni parametar id je id analize, ukoliko je on 0, radi se o novoj analizi
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(id == 0 && godina > 0 && idNastavnik > 0)
            {
                Nastavnik nastavnik = baza.Nastavnik.SingleOrDefault(s => s.Id == idNastavnik);
                if (nastavnik == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                ViewBag.godina = godina;
                ViewBag.idNastavnik = idNastavnik;
                return View();
            }
            else if (id > 0)
            {
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }
        }
        [HttpPost]
        public ActionResult NovaAnaliza(Nastavnik_analiza model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (false)
            {
                ViewBag.godina = model.Sk_godina;
                ViewBag.idNastavnik = model.Id_nastavnik;
                return View(model);
            }
            model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            model.Id_skola = PlaniranjeSession.Trenutni.OdabranaSkola;
            //spremanje podataka
            int idNastavnik = model.Id_nastavnik;
            int idAnaliza = model.Id;
            int god = model.Sk_godina;
            try
            {
                if (model.Id == 0)
                {
                    using (var db = new BazaPodataka())
                    {
                        db.NastavnikAnaliza.Add(model);
                        db.SaveChanges();
                    }
                }
                else
                {                    
                    var v = baza.NastavnikAnaliza.SingleOrDefault(s => s.Id == idAnaliza && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (v == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    using(var db = new BazaPodataka())
                    {
                        db.NastavnikAnaliza.Add(model);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Detalji", new { id = idNastavnik, godina = god });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}