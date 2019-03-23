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

            return View();
        }
    }
}