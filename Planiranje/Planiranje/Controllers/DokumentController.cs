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
            if(Request.Files.Count==0 || Request.Files[0].ContentLength==0)
            {
                ViewBag.greska = "Nema datoteke";
                return View("NoviDokument");
            }
            var forma = Request.Form;
            string opis = forma.Get("Opis");
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
                var path = Path.Combine(direktorij+"/", fileName);
                //provjera ukoliko datoteka postoji ne upisuje se u bazu već se samo zamijeni na disku
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    file.SaveAs(path);
                    string poruka = "Dokument koji želite spremiti već postoji. Novi dokument je zamijenjen starim." +
                        " Opis dokumenat nije promijenjen";
                    return RedirectToAction("Info", "OpciPodaci", new { poruka = poruka });
                }
                else
                {
                    file.SaveAs(path);
                }                
                //datoteka je spremljena, slijedi upis u bazu podataka
                Dokument dokument = new Dokument();
                dokument.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                dokument.Id_skola = PlaniranjeSession.Trenutni.OdabranaSkola;
                dokument.Path = fileName;
                dokument.Opis = opis;
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
        public ActionResult ObrisiDokument(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Dokument model = baza.Dokument.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiDokument(Dokument model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Obrisi(model.Id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
        public ActionResult Download(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Dokument Model = baza.Dokument.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (Model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            string direktorij = Server.MapPath("~/Dokumenti/" + PlaniranjeSession.Trenutni.PedagogId.ToString());
            FileInfo file = new FileInfo(direktorij + "/" + Model.Path);
            if (!file.Exists)
            {
                Obrisi(Model.Id);
                return RedirectToAction("Index");                
            }
            else
            {
                return File(Path.Combine(direktorij + "/", Model.Path), MimeMapping.GetMimeMapping(direktorij + "/" + Model.Path), Model.Path);
            }
        }
        private bool Obrisi(int id)
        {
            try
            {
                using (var db = new BazaPodataka())
                {
                    var result = db.Dokument.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result == null)
                    {
                        return false;
                    }
                    string direktorij = Server.MapPath("~/Dokumenti/" + PlaniranjeSession.Trenutni.PedagogId.ToString());
                    string path = Path.Combine(direktorij + "/", result.Path);
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    db.Dokument.Remove(result);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}