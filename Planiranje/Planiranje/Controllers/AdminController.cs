using Planiranje.Models;
using Planiranje.Models.Ucenici;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId != 1)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            using(var db=new BazaPodataka())
            {
                List<Pedagog> lista = db.Pedagog.Where(w => w.Id_Pedagog > 1).OrderBy(o=>o.Ime).ThenBy(t=>t.Prezime).ToList();
                ViewBag.pedagogSkola = db.PedagogSkola.ToList();
                ViewBag.skola = db.Skola.ToList();
                return View(lista);
            }            
        }
        [HttpPost]
        public ActionResult Aktivan(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId != 1)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (id == 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            using(var db=new BazaPodataka())
            {
                Pedagog pedagog = db.Pedagog.SingleOrDefault(s => s.Id_Pedagog == id);
                if (pedagog == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                if (pedagog.Aktivan)
                {
                    pedagog.Aktivan = false;
                }
                else
                {
                    pedagog.Aktivan = true;
                }
                db.SaveChanges();
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        public ActionResult PromjenaLicence(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId != 1 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            using(var db = new BazaPodataka())
            {
                Pedagog pedagog = db.Pedagog.SingleOrDefault(s => s.Id_Pedagog == id);
                if (pedagog == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                return View(pedagog);
            }
        }
        [HttpPost]
        public ActionResult PromjenaLicence(Pedagog p)
        {
            if(PlaniranjeSession.Trenutni.PedagogId !=1 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            DateTime vrijeme;
            if(!DateTime.TryParse(Request.Form.Get("vrijeme"),out vrijeme))
            {
                vrijeme = new DateTime();
            }
            using(var db = new BazaPodataka())
            {
                Pedagog pedagog = db.Pedagog.SingleOrDefault(s => s.Id_Pedagog == p.Id_Pedagog);
                if(pedagog==null || pedagog.Id_Pedagog == 1)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }                
                pedagog.Licenca = p.Licenca;
                pedagog.Licenca = pedagog.Licenca.AddHours(vrijeme.Hour);
                pedagog.Licenca = pedagog.Licenca.AddMinutes(vrijeme.Minute);
                db.SaveChanges();
                TempData["note"] = "Licenca je promijenjena";
            }
            return RedirectToAction("Index");
        }
        public ActionResult ObrisiSkolu (int id)
        {
            //id je id od tablice pedagog_skola
            if(PlaniranjeSession.Trenutni.PedagogId!=1 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            using(var db=new BazaPodataka())
            {
                Pedagog_skola pedagog_Skola = db.PedagogSkola.SingleOrDefault(s => s.Id == id);
                if (pedagog_Skola == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                List<Pedagog_skola> trenutneSkole = db.PedagogSkola.Where(w => w.Id_pedagog == pedagog_Skola.Id_pedagog).ToList();
                if (trenutneSkole.Count == 1)
                {
                    string poruka = "Pedagog mora imati upisanu barem jednu školu!";
                    return RedirectToAction("Info", "OpciPodaci", new { poruka = poruka });
                }
                Skola skola = db.Skola.SingleOrDefault(s => s.Id_skola == pedagog_Skola.Id_skola);
                Pedagog pedagog = db.Pedagog.SingleOrDefault(s => s.Id_Pedagog == pedagog_Skola.Id_pedagog);
                ViewBag.skola = skola;
                ViewBag.pedagog = pedagog;
                return View(pedagog_Skola);
            }
        }
        [HttpPost]
        public ActionResult ObrisiSkolu(Pedagog_skola ps)
        {
            if(PlaniranjeSession.Trenutni.PedagogId!=1 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            using(var db=new BazaPodataka())
            {
                Pedagog_skola pedagog_Skola = db.PedagogSkola.SingleOrDefault(s => s.Id == ps.Id);
                if (pedagog_Skola == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                List<Pedagog_skola> trenutneSkole = db.PedagogSkola.Where(w => w.Id_pedagog == pedagog_Skola.Id_pedagog).ToList();
                if (trenutneSkole.Count == 1)
                {
                    string poruka = "Pedagog mora imati upisanu barem jednu školu!";
                    return RedirectToAction("Info", "OpciPodaci", new { poruka = poruka });
                }
                try
                {
                    db.PedagogSkola.Remove(pedagog_Skola);
                    db.SaveChanges();
                }
                catch
                {
                    string poruka = "Ne možete obrisati ovu školu, jer pedagog ima upisane podatke vezane za nju!";
                    return RedirectToAction("Info", "OpciPodaci", new { poruka = poruka });
                }
                TempData["note"] = "Škola je obrisana";
                return RedirectToAction("Index");
            }
        }
        public ActionResult DodajSkolu(int id)
        {
            //id je id pedagoga
            if (PlaniranjeSession.Trenutni.PedagogId != 1 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            using(var db=new BazaPodataka())
            {
                Pedagog pedagog = db.Pedagog.SingleOrDefault(s => s.Id_Pedagog == id);
                if (pedagog == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                ViewBag.select = VratiSelectListSkole(id);
                ViewBag.pedagog = pedagog;
                return View();
            }            
        }
        [HttpPost]
        public ActionResult DodajSkolu(Pedagog_skola ps)
        {
            if (PlaniranjeSession.Trenutni.PedagogId != 1 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            using(var db=new BazaPodataka())
            {
                db.PedagogSkola.Add(ps);
                db.SaveChanges();
                TempData["note"] = "Škola je dodana";
                return RedirectToAction("Index");
            }
        }
        private SelectList VratiSelectListSkole(int id)
        {
            using(var db=new BazaPodataka())
            {
                List<Skola> skole = (from sk in db.Skola
                                     join ps in db.PedagogSkola on sk.Id_skola equals ps.Id_skola
                                     where ps.Id_pedagog != id
                                     select sk).Distinct().ToList();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                foreach (var item in skole)
                {
                    selectListItem.Add(new SelectListItem { Value = item.Id_skola.ToString(), Text = item.Naziv+", "+item.Grad });
                }
                var select = new SelectList(selectListItem, "Value", "Text");
                return select;
            }
        }
    }
}