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
        public ActionResult OdabirUcenika (int razred, int godina)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }            
            RazredniOdjel razredniOdjel = baza.RazredniOdjel.SingleOrDefault(s => s.Id == razred && s.Id_skola==PlaniranjeSession.Trenutni.OdabranaSkola);
            List<Ucenik> ucenici = new List<Ucenik>();
            if (razredniOdjel == null)
            {
                if (razred != -1)
                {
                    return View(ucenici);
                }
                ucenici = (from raz in baza.RazredniOdjel
                           join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred
                           join uc in baza.Ucenik on ur.Id_ucenik equals uc.Id_ucenik
                           where raz.Sk_godina == godina
                           select uc).ToList();
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
            //ulazni parametar id je id učenika
            //ulazni parametar godina je školska godina
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
            //model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == id);
            model.PracenjeUcenika = (from pracenje in baza.PracenjeUcenika join ur in baza.UcenikRazred on pracenje.Id_ucenik_razred equals ur.Id
                                     join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                     where ur.Id_ucenik == id && pracenje.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId &&
                                     raz.Sk_godina == godina
                                     select pracenje).FirstOrDefault();
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
                model.PracenjeUcenika.Pocetak_pracenja = new DateTime();
                model.PracenjeUcenika.Pocetak_pracenja = DateTime.Now;
            }
            //postignuća su vezana za id učenika u određenom razredu (id->table ucenik_razred) i id pedagoga 
            model.Postignuca = (from pos in baza.Postignuce
                                join ur in baza.UcenikRazred on pos.Id_ucenik_razred equals ur.Id
                                join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                where raz.Sk_godina == godina && ur.Id_ucenik == id && pos.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId
                                select pos).ToList();
            
            //neposredni radovi su vezani za id učenika u određenom razredu i id pedagoga
            model.NeposredniRadovi = (from rad in baza.NeposredniRad join ur in baza.UcenikRazred on rad.Id_ucenik_razred equals
                                      ur.Id join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                                      where raz.Sk_godina == godina && ur.Id_ucenik==id && rad.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId
                                      select rad).ToList();
            List<string> lista = new List<string>
            {
                "","Otac","Majka","Skrbnik","Brat","Sestra"
            };
            ViewBag.tekst = lista;
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
            int idRazred = model.Razred.Id;
            Ucenik_razred ur = baza.UcenikRazred.SingleOrDefault(s => s.Id_razred == idRazred && s.Id_ucenik == id);
            int idUR = ur.Id;
            try
            {
                using(var db = new BazaPodataka())
                {
                    var ucenik = db.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);
                    var result1 = db.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId);
                    if (result1 == null)
                    {
                        result1 = new Pracenje_ucenika();
                        result1.Pocetak_pracenja = model.PracenjeUcenika.Pocetak_pracenja;
                        result1.Id_ucenik_razred = idUR;
                        result1.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        db.PracenjeUcenika.Add(result1);
                    }
                    else
                    {
                        result1.Pocetak_pracenja = model.PracenjeUcenika.Pocetak_pracenja;                        
                    }
                    if (ucenik != null)
                    {
                        ucenik.Datum = model.Ucenik.Datum;                        
                        ucenik.Adresa = model.Ucenik.Adresa;
                        ucenik.Grad = model.Ucenik.Grad;                        
                    }
                    db.SaveChanges();
                }
            }
            catch { return new HttpStatusCodeResult(HttpStatusCode.NotModified); }
            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
        public ActionResult OsnovniPodaci(int id, int razred)
        {
            //ulazni parametar id je id učenika a razred je id razrednog odjela
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
            model.PracenjeUcenika = (from pracenje in baza.PracenjeUcenika join ur in baza.UcenikRazred on pracenje.Id_ucenik_razred
                                     equals ur.Id where ur.Id_ucenik==id && ur.Id_razred==razred 
                                     && pracenje.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId select pracenje).FirstOrDefault();
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
            List<string> lista = new List<string>
            {
                "","Otac","Majka","Skrbnik","Brat","Sestra"
            };
            ViewBag.tekst = lista;
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
            List<SelectListItem> select = new List<SelectListItem>()
            {
                new SelectListItem{Value="1", Text="Otac"},
                new SelectListItem{Value="2", Text="Majka"},
                new SelectListItem{Value="3", Text="Skrbnik"},
                new SelectListItem{Value="4", Text="Brat"},
                new SelectListItem{Value="5", Text="Sestra"}
            };
            ViewBag.select = select;
            return View();
        }
        [HttpPost]
        public ActionResult DodajObitelj (Obitelj model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }            
            if (string.IsNullOrWhiteSpace(model.Ime) || string.IsNullOrWhiteSpace(model.Prezime) || model.Svojstvo<=0 || model.Svojstvo>5)
            {
                List<SelectListItem> select = new List<SelectListItem>()
                {
                new SelectListItem{Value="1", Text="Otac"},
                new SelectListItem{Value="2", Text="Majka"},
                new SelectListItem{Value="3", Text="Skrbnik"},
                new SelectListItem{Value="4", Text="Brat"},
                new SelectListItem{Value="5", Text="Sestra"}
                };
                ViewBag.select = select;
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
            List<SelectListItem> select = new List<SelectListItem>()
            {
                new SelectListItem{Value="1", Text="Otac"},
                new SelectListItem{Value="2", Text="Majka"},
                new SelectListItem{Value="3", Text="Skrbnik"},
                new SelectListItem{Value="4", Text="Brat"},
                new SelectListItem{Value="5", Text="Sestra"}
            };
            ViewBag.select = select;
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediObitelj(Obitelj model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            
            if (string.IsNullOrWhiteSpace(model.Ime) || string.IsNullOrWhiteSpace(model.Prezime) || model.Svojstvo<=0 || model.Svojstvo>5)
            {
                List<SelectListItem> select = new List<SelectListItem>()
                {
                new SelectListItem{Value="1", Text="Otac"},
                new SelectListItem{Value="2", Text="Majka"},
                new SelectListItem{Value="3", Text="Skrbnik"},
                new SelectListItem{Value="4", Text="Brat"},
                new SelectListItem{Value="5", Text="Sestra"}
                };
                ViewBag.select = select;
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
            List<SelectListItem> select = new List<SelectListItem>()
                {
                new SelectListItem{Value="1", Text="Otac"},
                new SelectListItem{Value="2", Text="Majka"},
                new SelectListItem{Value="3", Text="Skrbnik"},
                new SelectListItem{Value="4", Text="Brat"},
                new SelectListItem{Value="5", Text="Sestra"}
                };
            ViewBag.select = select;
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
            int id_ucenik = model.Ucenik.Id_ucenik;
            int id_razred = model.Razred.Id;
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_razred==id_razred && s.Id_ucenik==id_ucenik);
            int idUR = ur.Id;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        result.Razlog = model.PracenjeUcenika.Razlog;
                    }
                    else
                    {
                        model.PracenjeUcenika.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        model.PracenjeUcenika.Id_ucenik_razred = idUR;
                        model.PracenjeUcenika.Pocetak_pracenja = new DateTime();
                        model.PracenjeUcenika.Pocetak_pracenja = DateTime.Now;
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
        public ActionResult Razlog (int idU, int idR)
        {
            //ulazni parametar id je id učenika -- PAST
            //idU - id učenika
            //idR - id razrednog odjela
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == idU);
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_ucenik == idU && s.Id_razred == idR);
            int idUR = ur.Id;
            model.Razred = new RazredniOdjel();
            model.Razred = baza.RazredniOdjel.Single(s => s.Id == idR);
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUR && 
                                                                         s.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId);
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
            int id_ucenik = model.Ucenik.Id_ucenik;
            int id_razred = model.Razred.Id;
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_razred == id_razred && s.Id_ucenik == id_ucenik);
            int idUR = ur.Id;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        result.Inic_Procjena_razrednik = model.PracenjeUcenika.Inic_Procjena_razrednik;
                        result.Inic_Procjena_ucenik = model.PracenjeUcenika.Inic_Procjena_ucenik;
                        result.Inic_Procjena_roditelj = model.PracenjeUcenika.Inic_Procjena_roditelj;                        
                    }
                    else
                    {
                        model.PracenjeUcenika.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        model.PracenjeUcenika.Id_ucenik_razred = idUR;
                        model.PracenjeUcenika.Pocetak_pracenja = new DateTime();
                        model.PracenjeUcenika.Pocetak_pracenja = DateTime.Now;
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
        public ActionResult Procjena (int idU, int idR)
        {
            //idU - id učenika
            //idR - id razrednog odjela
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_razred == idR && s.Id_ucenik == idU);
            int idUR = ur.Id;
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == idU);
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId);
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
            }
            model.Razred = new RazredniOdjel();
            model.Razred = baza.RazredniOdjel.Single(s => s.Id == idR);
            return View(model);
        }
        [HttpPost]
        public ActionResult Uvjeti (PracenjeUcenikaModel model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_ucenik = model.Ucenik.Id_ucenik;
            int id_razred = model.Razred.Id;
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_razred == id_razred && s.Id_ucenik == id_ucenik);
            int idUR = ur.Id;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        result.Soc_uvjeti = model.PracenjeUcenika.Soc_uvjeti;
                        result.Soc_vjestine = model.PracenjeUcenika.Soc_vjestine;
                        result.Ucenje = model.PracenjeUcenika.Ucenje;
                    }
                    else
                    {
                        model.PracenjeUcenika.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        model.PracenjeUcenika.Id_ucenik_razred = idUR;
                        model.PracenjeUcenika.Pocetak_pracenja = new DateTime();
                        model.PracenjeUcenika.Pocetak_pracenja = DateTime.Now;
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
        public ActionResult Uvjeti (int idU, int idR)
        {
            //idU - id učenika
            //idR - id razreda
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_razred == idR && s.Id_ucenik == idU);
            int idUR = ur.Id;
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == idU);
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
            }
            model.Razred = new RazredniOdjel();
            model.Razred = baza.RazredniOdjel.Single(s => s.Id == idR);
            return View(model);
        }
        [HttpPost]
        public ActionResult Zakljucak (PracenjeUcenikaModel model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id_ucenik = model.Ucenik.Id_ucenik;
            int id_razred = model.Razred.Id;
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_razred == id_razred && s.Id_ucenik == id_ucenik);
            int idUR = ur.Id;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        result.Zakljucak = model.PracenjeUcenika.Zakljucak;
                    }
                    else
                    {
                        model.PracenjeUcenika.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                        model.PracenjeUcenika.Id_ucenik_razred = idUR;
                        model.PracenjeUcenika.Pocetak_pracenja = new DateTime();
                        model.PracenjeUcenika.Pocetak_pracenja = DateTime.Now;
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
        public ActionResult Zakljucak (int idU, int idR)
        {
            //idU - id učenika
            //idR - id razrednog odjela
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_ucenik == idU && s.Id_razred == idR);
            int idUR = ur.Id;
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == idU);
            model.PracenjeUcenika = baza.PracenjeUcenika.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId);
            if (model.PracenjeUcenika == null)
            {
                model.PracenjeUcenika = new Pracenje_ucenika();
            }
            model.Razred = new RazredniOdjel();
            model.Razred = baza.RazredniOdjel.Single(s => s.Id == idR);
            return View(model);
        }
        public ActionResult Postignuce (int id, int razred)
        {
            //razred - id razrednog odjela
            //id - id učenika
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik_razred UR = new Ucenik_razred();
            UR = baza.UcenikRazred.Single(s => s.Id_ucenik == id && s.Id_razred == razred);
            int idUR = UR.Id;
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Postignuca = baza.Postignuce.Where(w => w.Id_ucenik_razred == idUR && w.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId).ToList();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == id);            
            model.Razred = baza.RazredniOdjel.SingleOrDefault(s => s.Id == razred);
            return View(model);
        }
        public ActionResult DodajPostignuce (int razred, int ucenik)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_ucenik == ucenik && s.Id_razred == razred);
            ViewBag.id_ucenik_razred = ur.Id;           
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
                ViewBag.id_ucenik_razred = model.Id_ucenik_razred;               
                return View(model);
            }
            int id_ucenik_razred = model.Id_ucenik_razred;
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id == id_ucenik_razred);
            int id_razred = ur.Id_razred;
            int id_ucenik = ur.Id_ucenik;
            RazredniOdjel raz = baza.RazredniOdjel.SingleOrDefault(s => s.Id == id_razred);
            model.Godina = raz.Sk_godina;
            model.Razred = raz.Razred;
            model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.Postignuce.Add(model);
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return RedirectToAction("Postignuce", new { id = id_ucenik, razred = id_razred });
        }
        public ActionResult UrediPostignuce (int id)
        {
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId<=0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Postignuce model = baza.Postignuce.SingleOrDefault(s => s.Id_postignuce == id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
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
            int idUR;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.Postignuce.SingleOrDefault(s => s.Id_postignuce == idPos);
                    if (result != null && result.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId)
                    {
                        result.Napomena = model.Napomena;
                        idUR = result.Id_ucenik_razred;
                        db.SaveChanges();
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.SingleOrDefault(s => s.Id == idUR);
            return RedirectToAction("Postignuce", new { id = ur.Id_ucenik, razred = ur.Id_razred });
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
            int idUR;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.Postignuce.SingleOrDefault(s => s.Id_postignuce == idPos);
                    if (result != null && result.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId)
                    {
                        idUR = result.Id_ucenik_razred;
                        db.Postignuce.Remove(result);
                        db.SaveChanges();
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id == idUR);
            return RedirectToAction("Postignuce", new { id = ur.Id_ucenik, razred = ur.Id_razred });
        }
        public ActionResult NeposredniRad (int idUR)
        {
            //idUR - id učenika u određenom razredu; id_ucenik_razred> tablica ucenik_razred (id)          
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id == idUR);
            int idU = ur.Id_ucenik;
            int idR = ur.Id_razred;
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            model.Ucenik = baza.Ucenik.SingleOrDefault(s => s.Id_ucenik == idU);
            model.NeposredniRadovi = baza.NeposredniRad.Where(w => w.Id_ucenik_razred == idUR && w.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId).ToList();
            model.Razred = baza.RazredniOdjel.SingleOrDefault(s => s.Id == idR);
            return View(model);
        }
        public ActionResult DodajNeposredniRad (int ucenik, int razred)
        {
            //ucenik - id učenika
            //razred - id razrednog odjela
            if(!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik_razred ur = new Ucenik_razred();
            ur = baza.UcenikRazred.Single(s => s.Id_razred == razred && s.Id_ucenik == ucenik);
            ViewBag.id_ucenik_razred = ur.Id;
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
                ViewBag.id_ucenik_razred = model.Id_ucenik_razred;
                return View(model);
            }
            model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.NeposredniRad.Add(model);
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return RedirectToAction("NeposredniRad", new { idUR = model.Id_ucenik_razred });
        }
        public ActionResult UrediNeposredniRad (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Neposredni_rad model = baza.NeposredniRad.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
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
            int id = model.Id;
            int IdUR;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.NeposredniRad.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        result.Datum = model.Datum;
                        result.Napomena = model.Napomena;
                        IdUR = result.Id_ucenik_razred;
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
            return RedirectToAction("NeposredniRad", new { idUR = IdUR });
        }
        public ActionResult ObrisiNeposredniRad (int id)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Neposredni_rad model = baza.NeposredniRad.SingleOrDefault(s => s.Id == id && s.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId);
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiNeposredniRad (Neposredni_rad model)
        {
            if (!Request.IsAjaxRequest() || PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = model.Id;
            int IdUR;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.NeposredniRad.SingleOrDefault(s => s.Id == id && s.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        IdUR = result.Id_ucenik_razred;
                        db.NeposredniRad.Remove(result);
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
            return RedirectToAction("NeposredniRad", new { idUR = IdUR });
        }
        public FileStreamResult Ispis (int godina, int id)
        {
            //ulazni parametar id je id učenika
            PracenjeUcenikaModel model = new PracenjeUcenikaModel();
            Skola skola = new Skola();
            skola = baza.Skola.Single(s => s.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola);
            model.Razred = new RazredniOdjel();
            Ucenik_razred UR = new Ucenik_razred();
            UR = (from ur in baza.UcenikRazred
                  join raz in baza.RazredniOdjel on ur.Id_razred equals raz.Id
                  where ur.Id_ucenik == id && raz.Sk_godina == godina && raz.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola
                  select ur).Single();
            int idUR = UR.Id;
            int idRazred = UR.Id_razred;
            model.Razred = baza.RazredniOdjel.Single(s => s.Id == idRazred);
            model.Ucenik = new Ucenik();
            model.Ucenik = baza.Ucenik.Single(s => s.Id_ucenik == id);
            model.ListaObitelji = new List<Obitelj>();
            model.ListaObitelji = baza.Obitelj.Where(w => w.Id_ucenik == id).ToList();
            model.PracenjeUcenika = new Pracenje_ucenika();
            model.PracenjeUcenika = baza.PracenjeUcenika.Single(s => s.Id_ucenik_razred == idUR && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            model.Postignuca = new List<Postignuce>();
            model.Postignuca = baza.Postignuce.Where(w => w.Id_ucenik_razred == idUR && w.Id_pedagog==PlaniranjeSession.Trenutni.PedagogId).ToList();
            model.NeposredniRadovi = new List<Neposredni_rad>();
            model.NeposredniRadovi = baza.NeposredniRad.Where(w => w.Id_ucenik_razred == idUR && w.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId).ToList();
            model.Razrednik = new Nastavnik();
            int idRazrednik = model.Razred.Id_razrednik;
            model.Razrednik = baza.Nastavnik.Single(s => s.Id == idRazrednik);
            Pedagog pedagog = new Pedagog();
            pedagog = baza.Pedagog.Single(s => s.Id_Pedagog == PlaniranjeSession.Trenutni.PedagogId);

            PracenjeUcenikaReport report = new PracenjeUcenikaReport(model, skola, pedagog);
            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
        }
    }
}