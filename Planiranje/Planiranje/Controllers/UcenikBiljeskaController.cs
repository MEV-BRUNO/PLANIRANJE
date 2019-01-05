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
            model.ListaObitelji = baza.Obitelj.Where(w => w.Id_ucenik == id).ToList();
            int id_ucenik_biljeska = model.UcenikBiljeska.Id_biljeska;
            model.MjesecneBiljeske = baza.MjesecnaBiljeska.Where(w => w.Id_ucenik_biljeska == id_ucenik_biljeska).ToList();
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
        public ActionResult Zapazanja (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            UcenikBiljeskaModel model = new UcenikBiljeskaModel();
            model.UcenikBiljeska = baza.UcenikBiljeska.SingleOrDefault(s => s.Id_biljeska == id);
            return View(model);
        }
        public ActionResult PromjenaZapazanja (UcenikBiljeskaModel model)
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
                        result.Zapazanje = model.UcenikBiljeska.Zapazanje;
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
        public ActionResult Biljeska (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            UcenikBiljeskaModel model = new UcenikBiljeskaModel();
            model.UcenikBiljeska = baza.UcenikBiljeska.SingleOrDefault(s => s.Id_biljeska == id);
            model.MjesecneBiljeske = baza.MjesecnaBiljeska.Where(w => w.Id_ucenik_biljeska == id).ToList();
            return View(model);
        }
        public ActionResult NovaBiljeska (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.id = id;
            List<string> mjeseci = new List<string> { "Rujan","Listopad","Studeni","Prosinac","Siječanj","Veljača","Ožujak","Travanj","Svibanj","LIpanj"};
            ViewBag.mjeseci = mjeseci;
            return View();
        }        
        [HttpPost]
        public ActionResult NovaBiljeska (Mjesecna_biljeska model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<string> mjeseci = new List<string> { "Rujan", "Listopad", "Studeni", "Prosinac", "Siječanj", "Veljača", "Ožujak", "Travanj", "Svibanj", "LIpanj" };
            if (string.IsNullOrWhiteSpace(model.Biljeska) || !mjeseci.Contains(model.Mjesec))
            {
                ViewBag.id = model.Id_ucenik_biljeska;                
                ViewBag.mjeseci = mjeseci;
                ViewBag.selected = model.Mjesec;
                return View(model);
            }
            int id_biljeska = model.Id_ucenik_biljeska;
            model.Sk_godina = (from bilj in baza.UcenikBiljeska
                               join ur in baza.UcenikRazred on bilj.Id_ucenik_razred equals ur.Id
                               join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                               where bilj.Id_biljeska == id_biljeska
                               select raz.Sk_godina).First();
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.MjesecnaBiljeska.Add(model);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
            return RedirectToAction("Biljeska", new { id = model.Id_ucenik_biljeska });
        }
        public ActionResult UrediBiljeska (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Mjesecna_biljeska model = baza.MjesecnaBiljeska.SingleOrDefault(s => s.Id == id);
            ViewBag.selected = model.Mjesec;
            List<string> mjeseci = new List<string> { "Rujan", "Listopad", "Studeni", "Prosinac", "Siječanj", "Veljača", "Ožujak", "Travanj", "Svibanj", "LIpanj" };
            ViewBag.mjeseci = mjeseci;
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediBiljeska (Mjesecna_biljeska model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<string> mjeseci = new List<string> { "Rujan", "Listopad", "Studeni", "Prosinac", "Siječanj", "Veljača", "Ožujak", "Travanj", "Svibanj", "LIpanj" };
            if (string.IsNullOrWhiteSpace(model.Biljeska) || !mjeseci.Contains(model.Mjesec))
            {
                ViewBag.id = model.Id_ucenik_biljeska;
                ViewBag.mjeseci = mjeseci;
                ViewBag.selected = model.Mjesec;
                return View(model);
            }
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.MjesecnaBiljeska.Add(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch { }
            }
            return RedirectToAction("Biljeska", new { id = model.Id_ucenik_biljeska });
        }
        public ActionResult ObrisiBiljeska (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<string> mjeseci = new List<string> { "Rujan", "Listopad", "Studeni", "Prosinac", "Siječanj", "Veljača", "Ožujak", "Travanj", "Svibanj", "LIpanj" };
            ViewBag.mjeseci = mjeseci;
            Mjesecna_biljeska model = baza.MjesecnaBiljeska.SingleOrDefault(s => s.Id == id);
            ViewBag.selected = model.Mjesec;
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiBiljeska (Mjesecna_biljeska model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_biljeska = model.Id_ucenik_biljeska;
            int idB = model.Id;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.MjesecnaBiljeska.SingleOrDefault(s => s.Id == idB);
                    if (result != null)
                    {
                        db.MjesecnaBiljeska.Remove(result);
                        db.SaveChanges();
                    }
                }
                catch { }
            }
            return RedirectToAction("Biljeska", new { id = id_biljeska });
        }
    }
}