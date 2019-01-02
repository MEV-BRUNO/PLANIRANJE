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
    public class PracenjeUcenikaController : Controller
    {
        private BazaPodataka baza = new BazaPodataka();
        // GET: PracenjeUcenika
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.SkGodine = baza.SkolskaGodina.ToList();
            return View(model);
        }
        public ActionResult OdabirRazreda (int godina)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<RazredniOdjel> razredi = new List<RazredniOdjel>();
            razredi = baza.RazredniOdjel.Where(w => w.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola && w.Sk_godina == godina).ToList();
            return View(razredi);
        }
        public ActionResult OdabirUcenika (int razred)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            RazredniOdjel razredniOdjel = baza.RazredniOdjel.SingleOrDefault(s => s.Id == razred && s.Id_skola==PlaniranjeSession.Trenutni.OdabranaSkola);
            List<Ucenik> ucenici = new List<Ucenik>();
            if (razredniOdjel == null)
            {
                return View(ucenici);
            }
            ucenici = (from ur in baza.UcenikRazred
                       join uc in baza.Ucenik on ur.Id_ucenik equals uc.Id_ucenik
                       where ur.Id_razred == razred
                       select uc).ToList();
            return View(ucenici);
        }
        public ActionResult Detalji (int id, int godina)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId<=0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();            
            model.Ucenik = (from uc in baza.Ucenik join ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik join
                            raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id where uc.Id_ucenik == id && raz.Id_skola == 
                            PlaniranjeSession.Trenutni.OdabranaSkola select uc).FirstOrDefault();
            if (model.Ucenik == null)
            {
                return HttpNotFound();
            }
            model.Razred = (from uc in baza.Ucenik join ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik join
                            raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id where raz.Sk_godina == godina && uc.Id_ucenik==id
                            select raz).FirstOrDefault();
            if (model.Razred == null)
            {
                return HttpNotFound();
            }
            int idRazrednik = model.Razred.Id_razrednik;
            model.Razrednik = baza.Nastavnik.SingleOrDefault(s => s.Id == idRazrednik);
            if (model.Razred == null)
            {
                return HttpNotFound();
            }
            model.ListaObitelji = new List<Obitelj>();
            model.ListaObitelji = baza.Obitelj.Where(w => w.Id_ucenik == id).ToList();
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id);
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult OsnovniPodaci(PracenjeUcenikaModel model)
        {
            int id = model.Ucenik.Id_ucenik;
            try
            {
                using(var db = new BazaPodataka())
                {
                    var ucenik = db.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
                    if (ucenik != null)
                    {
                        ucenik.Datum = model.Ucenik.Datum;
                        ucenik.PocetakPracenja = model.Ucenik.PocetakPracenja;
                        ucenik.Adresa = model.Ucenik.Adresa;
                        ucenik.Grad = model.Ucenik.Grad;
                        db.SaveChanges();
                    }
                }
            }
            catch { return new HttpStatusCodeResult(HttpStatusCode.NotModified); }
            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
        public ActionResult OsnovniPodaci(int id, int razred)
        {
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.Razred = baza.RazredniOdjel.SingleOrDefault(s => s.Id == razred);
            if(model.Ucenik ==null || model.Razred == null)
            {
                return HttpNotFound();
            }
            int idRazrednik = model.Razred.Id_razrednik;
            model.Razrednik = baza.Nastavnik.SingleOrDefault(s => s.Id == idRazrednik);
            if (model.Razrednik == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        public ActionResult Obitelj (int id)
        {
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);            
            if (model.Ucenik == null)
            {
                return HttpNotFound();
            }
            model.ListaObitelji = baza.Obitelj.Where(w => w.Id_ucenik == id).ToList();
            return View(model);
        }
        public ActionResult DodajObitelj (int id)
        {
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            if (model.Ucenik == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUcenik = id;
            List<string> lista = new List<string>()
            {
                "Otac", "Majka","Brat","Sestra"
            };
            ViewBag.select = lista;
            return View();
        }
        [HttpPost]
        public ActionResult DodajObitelj (Obitelj model)
        {
            List<string> lista = new List<string>()
            {
                "Otac", "Majka","Brat","Sestra"
            };
            if (string.IsNullOrWhiteSpace(model.Ime) || string.IsNullOrWhiteSpace(model.Prezime) || !lista.Contains(model.Svojstvo))
            {
                ViewBag.select = lista;
                if (lista.Contains(model.Svojstvo))
                {
                    ViewBag.selected = model.Svojstvo;
                }
                ViewBag.idUcenik = model.Id_ucenik;
                return View(model);
            }
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.Obitelj.Add(model);
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Dogodila se greška prilikom spremanja podataka");
                }
            }
            return RedirectToAction("Obitelj", new { id = model.Id_ucenik });
        }
        public ActionResult UrediObitelj(int id)
        {
            Obitelj model = baza.Obitelj.SingleOrDefault(s => s.Id_obitelj == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            List<string> lista = new List<string>()
            {
                "Otac", "Majka","Brat","Sestra"
            };
            ViewBag.select = lista;
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediObitelj(Obitelj model)
        {
            List<string> lista = new List<string>()
            {
                "Otac", "Majka","Brat","Sestra"
            };
            if (string.IsNullOrWhiteSpace(model.Ime) || string.IsNullOrWhiteSpace(model.Prezime) || !lista.Contains(model.Svojstvo))
            {
                ViewBag.select = lista;             
                return View(model);
            }
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.Obitelj.Add(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Greška prilikom zapisivanja podataka u bazu podataka");
                }
            }
            return RedirectToAction("Obitelj", new { id = model.Id_ucenik });
        }
        public ActionResult ObrisiObitelj (int id)
        {
            Obitelj model = baza.Obitelj.SingleOrDefault(s => s.Id_obitelj == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            List<string> lista = new List<string>()
            {
                "Otac", "Majka","Brat","Sestra"
            };
            ViewBag.select = lista;
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiObitelj (Obitelj model)
        {
            int idU = model.Id_ucenik;
            int idO = model.Id_obitelj;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.Obitelj.SingleOrDefault(s => s.Id_obitelj == idO);
                    if (result != null)
                    {
                        db.Obitelj.Remove(result);
                        db.SaveChanges();
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Nije pronađeno");
                    }
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Greška prilikom brisanja podataka u bazi podataka");
                }
            }
            return RedirectToAction("Obitelj", new { id = idU });
        }
        [HttpPost]
        public ActionResult Razlog (PracenjeUcenikaModel model)
        {
            int id_ucenik = model.PracenjeUcenika.Id_ucenik;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id_ucenik);
                    if (result != null)
                    {
                        result.Razlog = model.PracenjeUcenika.Razlog;
                    }
                    else
                    {
                        db.PracenjeUcenika.Add(model.PracenjeUcenika);
                    }
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public ActionResult Razlog (int id)
        {
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id);
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
            }
            return View(model);
        }
    }
}