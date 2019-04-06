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
    public class NastavnikObrazacController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: NastavnikObrazac
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
            //ulazni parametar id je id nastavnika, a godina je skolska godina
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<Nastavnik_obrazac> model = baza.NastavnikObrazac.Where(w => w.Id_nastavnik == id && w.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId &&
            w.Sk_godina == godina && w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();
            Nastavnik nastavnik = baza.Nastavnik.SingleOrDefault(s => s.Id == id && s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            ViewBag.nastavnik = nastavnik;
            ViewBag.godina = godina;
            ViewBag.listaodjela = baza.RazredniOdjel.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.PedagogId &&
            w.Sk_godina == godina).ToList();
            return View(model);            
        }
        public ActionResult NoviObrazac (int idNastavnik, int godina, int id)
        {
            //ulazni parametar id je id analize, ukoliko je on 0, radi se o novoj analizi
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (id == 0 && godina > 0 && idNastavnik > 0)
            {
                Nastavnik nastavnik = baza.Nastavnik.SingleOrDefault(s => s.Id == idNastavnik);
                if (nastavnik == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                ViewBag.godina = godina;
                ViewBag.idNastavnik = idNastavnik;
                ViewBag.select = VratiSelectListu(godina);
                ViewBag.selectDaNe = VratiSelectDaNe();
                ViewBag.select4 = VratiSelect4();
                return View();
            }
            else if (id > 0)
            {
                Nastavnik_obrazac model = baza.NastavnikObrazac.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                if (model == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                ViewBag.select = VratiSelectListu(model.Sk_godina);
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }
        }

        private IEnumerable<SelectListItem> VratiSelectListu(int godina)
        {
            List<RazredniOdjel> odjeli = baza.RazredniOdjel.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.PedagogId
            && w.Sk_godina == godina).ToList();
            IEnumerable<SelectListItem> select = new SelectList(odjeli, "Id", "Naziv");
            return select;
        }
        private SelectList VratiSelectDaNe()
        {
            var select = new SelectList(new List<SelectListItem> {
             new SelectListItem {Text="-", Value="0"},
             new SelectListItem {Text="Da", Value="1"},
             new SelectListItem {Text="Ne", Value="2"}             
            }, "Value", "Text");
            return select;
        }
        private SelectList VratiSelect4()
        {
            var select = new SelectList(new List<SelectListItem> {
             new SelectListItem {Text="-", Value="0"},
             new SelectListItem {Text="Pretežno slabosti", Value="1"},
             new SelectListItem {Text="Više slabosti nego prednosti", Value="2"},
             new SelectListItem {Text="Više prednosti nego slabosti", Value="3"},
             new SelectListItem {Text="Pretežno prednosti", Value="4"}
            }, "Value", "Text");
            return select;
        }
    }
}