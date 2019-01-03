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
            model.Postignuca = baza.Postignuce.Where(w => w.Id_ucenik == id).ToList();
            model.RazredniOdjeli = (from uc in baza.Ucenik
                                    join ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik
                                    join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                    where uc.Id_ucenik == id
                                    select raz).ToList();
            model.NeposredniRadovi = baza.NeposredniRad.Where(w => w.Id_ucenik == id).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult OsnovniPodaci(PracenjeUcenikaModel model)
        {
            if(!Request.IsAjaxRequest()|| PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
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
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id);
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Procjena (PracenjeUcenikaModel model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_ucenik = model.PracenjeUcenika.Id_ucenik;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id_ucenik);
                    if (result != null)
                    {
                        result.Inic_Procjena_razrednik = model.PracenjeUcenika.Inic_Procjena_razrednik;
                        result.Inic_Procjena_ucenik = model.PracenjeUcenika.Inic_Procjena_ucenik;
                        result.Inic_Procjena_roditelj = model.PracenjeUcenika.Inic_Procjena_roditelj;
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
        public ActionResult Procjena (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id);
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Uvjeti (PracenjeUcenikaModel model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_ucenik = model.PracenjeUcenika.Id_ucenik;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id_ucenik);
                    if (result != null)
                    {
                        result.Soc_uvjeti = model.PracenjeUcenika.Soc_uvjeti;
                        result.Soc_vjestine = model.PracenjeUcenika.Soc_vjestine;
                        result.Ucenje = model.PracenjeUcenika.Ucenje;
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
        public ActionResult Uvjeti (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id);
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Zakljucak (PracenjeUcenikaModel model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_ucenik = model.PracenjeUcenika.Id_ucenik;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id_ucenik);
                    if (result != null)
                    {
                        result.Zakljucak = model.PracenjeUcenika.Zakljucak;
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
        public ActionResult Zakljucak (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik == id);
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
            }
            return View(model);
        }
        public ActionResult Postignuce (int id, int razred)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Postignuca = baza.Postignuce.Where(w => w.Id_ucenik == id).ToList();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.RazredniOdjeli = (from uc in baza.Ucenik
                                    join ur in baza.UcenikRazred on uc.Id_ucenik equals ur.Id_ucenik
                                    join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                    where uc.Id_ucenik == id
                                    select raz).ToList();
            model.Razred = baza.RazredniOdjel.SingleOrDefault(s => s.Id == razred);
            return View(model);
        }
        public ActionResult DodajPostignuce (int razred, int ucenik)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.razred = razred;
            ViewBag.ucenik = ucenik;
            return View();
        }
        [HttpPost]
        public ActionResult DodajPostignuce (Postignuce model)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (string.IsNullOrWhiteSpace(model.Napomena))
            {
                ViewBag.ucenik = model.Id_ucenik;
                ViewBag.razred = model.Id_razred;
                return View(model);
            }
            int id_razred = model.Id_razred;
            RazredniOdjel raz = baza.RazredniOdjel.SingleOrDefault(s => s.Id == id_razred);
            model.Godina = raz.Sk_godina;
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.Postignuce.Add(model);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
            return RedirectToAction("Postignuce", new { id = model.Id_ucenik, razred = model.Id_razred });
        }
        public ActionResult UrediPostignuce (int id)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId<=0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Postignuce model = baza.Postignuce.SingleOrDefault(s => s.Id_postignuce == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediPostignuce (Postignuce model)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (string.IsNullOrWhiteSpace(model.Napomena))
            {
                return View(model);
            }
            int idPos = model.Id_postignuce;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.Postignuce.SingleOrDefault(s => s.Id_postignuce == idPos);
                    if (result != null)
                    {
                        result.Napomena = model.Napomena;
                        db.SaveChanges();
                    }
                }
                catch
                {

                }
            }
            return RedirectToAction("Postignuce", new { id = model.Id_ucenik, razred = model.Id_razred });
        }
        public ActionResult ObrisiPostignuce (int id)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Postignuce model = baza.Postignuce.SingleOrDefault(s => s.Id_postignuce == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiPostignuce (Postignuce model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idPos = model.Id_postignuce;
            int idUcenik = model.Id_ucenik;
            int idRazred = model.Id_razred;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.Postignuce.SingleOrDefault(s => s.Id_postignuce == idPos);
                    if (result != null)
                    {
                        db.Postignuce.Remove(result);
                        db.SaveChanges();
                    }
                }
                catch
                {

                }
            }
            return RedirectToAction("Postignuce", new { id = idUcenik, razred = idRazred });
        }
        public ActionResult NeposredniRad (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
            model.NeposredniRadovi = baza.NeposredniRad.Where(w => w.Id_ucenik == id).ToList();
            return View(model);
        }
        public ActionResult DodajNeposredniRad (int id)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.ucenik = id;
            return View();
        }
        [HttpPost]
        public ActionResult DodajNeposredniRad (Neposredni_rad model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(string.IsNullOrWhiteSpace(model.Napomena)||model.Datum.CompareTo(new DateTime(1, 1, 1)) == 0)
            {
                ViewBag.ucenik = model.Id_ucenik;
                return View(model);
            }
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.NeposredniRad.Add(model);
                    db.SaveChanges();
                }
                catch { }
            }
            return RedirectToAction("NeposredniRad", new { id = model.Id_ucenik });
        }
        public ActionResult UrediNeposredniRad (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Neposredni_rad model = baza.NeposredniRad.SingleOrDefault(s => s.Id_rad == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediNeposredniRad (Neposredni_rad model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (string.IsNullOrWhiteSpace(model.Napomena) || model.Datum.CompareTo(new DateTime(1, 1, 1)) == 0)
            {                
                return View(model);
            }            
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.NeposredniRad.Add(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch { }
            }
            return RedirectToAction("NeposredniRad", new { id = model.Id_ucenik });
        }
        public ActionResult ObrisiNeposredniRad (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Neposredni_rad model = baza.NeposredniRad.SingleOrDefault(s => s.Id_rad == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiNeposredniRad (Neposredni_rad model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idRad = model.Id_rad;
            int idUc = model.Id_ucenik;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.NeposredniRad.SingleOrDefault(s => s.Id_rad == idRad);
                    if (result != null)
                    {
                        db.NeposredniRad.Remove(result);
                        db.SaveChanges();
                    }
                }
                catch { }
            }
            return RedirectToAction("NeposredniRad", new { id = idUc });
        }
    }
}