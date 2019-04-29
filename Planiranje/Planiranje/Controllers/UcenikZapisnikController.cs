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
            ViewBag.ucenik = ucenik;
            Ucenik_razred UR = (from raz in baza.RazredniOdjel
                                join ur in baza.UcenikRazred on raz.Id equals ur.Id_razred
                                join uc in baza.Ucenik on ur.Id_ucenik equals uc.Id_ucenik
                                where raz.Sk_godina == godina && uc.Id_ucenik == id && raz.Id_skola == PlaniranjeSession.Trenutni.OdabranaSkola
                                select ur).SingleOrDefault();
            if (UR == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            int idUR = UR.Id;

            Ucenik_zapisnik model = baza.UcenikZapisnik.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model == null)
            {
                model = new Ucenik_zapisnik();
                model.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                model.Id_ucenik_razred = idUR;
                using(var db=new BazaPodataka())
                {
                    try
                    {
                        db.UcenikZapisnik.Add(model);
                        db.SaveChanges();
                    }
                    catch
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
                model = baza.UcenikZapisnik.SingleOrDefault(s => s.Id_ucenik_razred == idUR && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            }
            return View(model);
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
        public ActionResult Odgoj(int id)
        {
            //id je id od ucenik_zapisnik modela
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik_zapisnik model = baza.UcenikZapisnik.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.selectOdgojniUtjecaj = VratiSelectOdgojniUtjecaj();
            ViewBag.selectOdnos = VratiSelectOdnos();
            ViewBag.selectSuradnja = VratiSelectSuradnja();
            return View(model);
        }
        [HttpPost]
        public ActionResult Odgoj (Ucenik_zapisnik model)
        {
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            using(var db = new BazaPodataka())
            {
                try
                {
                    int id = model.Id;
                    var result = db.UcenikZapisnik.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        result.Razlog = model.Razlog;
                        result.Odgojni_utjecaj_majka = model.Odgojni_utjecaj_majka;
                        result.Odgojni_utjecaj_otac = model.Odgojni_utjecaj_otac;
                        result.Procjena_statusa_obitelji = model.Procjena_statusa_obitelji;
                        result.Odnos_prema_ucenju_majka = model.Odnos_prema_ucenju_majka;
                        result.Odnos_prema_ucenju_otac = model.Odnos_prema_ucenju_otac;
                        result.Suradnja_roditelja_majka = model.Suradnja_roditelja_majka;
                        result.Suradnja_roditelja_otac = model.Suradnja_roditelja_otac;
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
        public ActionResult Ostalo (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ucenik_zapisnik model = baza.UcenikZapisnik.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Ostalo(Ucenik_zapisnik model)
        {
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = model.Id;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var result = db.UcenikZapisnik.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        result.Odnos_s_prijateljima = model.Odnos_s_prijateljima;
                        result.Kako_provodi_slobodno_vrijeme = model.Kako_provodi_slobodno_vrijeme;
                        result.Procjena_mogucih_losih_utjecaja = model.Procjena_mogucih_losih_utjecaja;
                        result.Zdravstvene_poteskoce_ucenika = model.Zdravstvene_poteskoce_ucenika;
                        result.Podaci_o_naglim_promjenama = model.Podaci_o_naglim_promjenama;
                        result.Izrecene_pedagoske_mjere = model.Izrecene_pedagoske_mjere;
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
        public ActionResult Biljeska (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(!ZapisnikIsValid(id)) return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            List<Ucenik_zapisnik_biljeska> model = baza.UcenikZapisnikBiljeska.Where(w => w.Id_ucenik_zapisnik == id).ToList();
            ViewBag.id = id;
            return View(model);
        }
        public ActionResult NovaBiljeska(int id)
        {
            //id je id od ucenik_zapisnik
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult NovaBiljeska (Ucenik_zapisnik_biljeska model)
        {
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.id = model.Id_ucenik_zapisnik;
                return View(model);
            }
            if (!ZapisnikIsValid(model.Id_ucenik_zapisnik))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.UcenikZapisnikBiljeska.Add(model);
                    db.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
            }
            return RedirectToAction("Biljeska", new { id = model.Id_ucenik_zapisnik });
        }
        private SelectList VratiSelectOdgojniUtjecaj()
        {
            SelectList select = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem{Value="0", Text="-"},
                new SelectListItem{Value="1", Text="Strogi"},
                new SelectListItem{Value="2", Text="Ravnodušni"},
                new SelectListItem{Value="3", Text="Blagi"},
                new SelectListItem{Value="4", Text="Autoritativni"}
            }, "Value","Text");
            return select;
        }
        private SelectList VratiSelectOdnos()
        {
            SelectList select = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem{Value="0", Text="-"},
                new SelectListItem{Value="1", Text="Ne pokazuju zanimanje"},
                new SelectListItem{Value="2", Text="Povremeno pokazuju zanimanje"},
                new SelectListItem{Value="3", Text="Redovito surađuju i potiču na učenje"}                
            },"Value","Text");
            return select;
        }
        private SelectList VratiSelectSuradnja()
        {
            SelectList select = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem{Value="0", Text="-"},
                new SelectListItem{Value="1", Text="Redovita"},
                new SelectListItem{Value="2", Text="Povremena"},
                new SelectListItem{Value="3", Text="Ne surađuju"}
            }, "Value", "Text");
            return select;
        }
        private bool ZapisnikIsValid (int id)
        {
            Ucenik_zapisnik model = baza.UcenikZapisnik.SingleOrDefault(s => s.Id == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model == null)
            {
                return false;
            }
            else return true;
        }
        private bool BiljeskaIsValid(int id)
        {
            Ucenik_zapisnik_biljeska model = baza.UcenikZapisnikBiljeska.SingleOrDefault(s => s.Id == id);
            if (model == null) return false;
            else return ZapisnikIsValid(model.Id_ucenik_zapisnik);            
        }
    }
}