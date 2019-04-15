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
    public class DokumentController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: Dokument
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<Dokument> dokumenti = baza.Dokument.Where(w => w.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && 
            w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola).ToList();
            return View(dokumenti);
        }
        public ActionResult NoviDokument()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Upload()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(Request.Files.Count==0)
            {
                ViewBag.greska = "Nema datoteke";
                return View("NoviDokument");
            }
            try
            {
                var file = Request.Files[0];

                string ekstenzija = Path.GetExtension(file.FileName);
                if (ekstenzija.CompareTo(".exe") == 0 || ekstenzija.CompareTo(".bin") == 0)
                {
                    ViewBag.greska = "Datoteke *.exe i *.bin nisu podržane";
                    return View("NoviDokument");
                }
                string direktorij = Server.MapPath("~/Dokumenti/" + PlaniranjeSession.Trenutni.PedagogId.ToString());
                if (!Directory.Exists(direktorij))
                {
                    Directory.CreateDirectory(direktorij);
                }
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(direktorij, fileName);
                file.SaveAs(path);
                //datoteka je spremljena, slijedi upis u bazu podataka
                Dokument dokument = new Dokument();
                dokument.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                dokument.Id_skola = PlaniranjeSession.Trenutni.OdabranaSkola;
                dokument.Naziv = fileName;
                using(var db = new BazaPodataka())
                {
                    db.Dokument.Add(dokument);
                    db.SaveChanges();
                }                
            }
            catch(Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return RedirectToAction("Index");
        }
    }
}