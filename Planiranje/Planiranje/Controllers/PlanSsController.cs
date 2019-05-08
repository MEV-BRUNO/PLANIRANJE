using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Planiranje.Models;
using Planiranje.Reports;

namespace Planiranje.Controllers
{
    public class PlanSsController : Controller
    {		
		private Podrucje_rada_DBHandle podrucja_rada = new Podrucje_rada_DBHandle();
		private Subjekt_DBHandle subjekti = new Subjekt_DBHandle();
		private Aktivnost_DBHandle aktivnosti = new Aktivnost_DBHandle();
		private Zadaci_DBHandle zadaci = new Zadaci_DBHandle();		
		private Oblici_DBHandle oblici = new Oblici_DBHandle();
		private Ciljevi_DBHandle ciljevi = new Ciljevi_DBHandle();
        private BazaPodataka baza = new BazaPodataka();

		public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SSModel model = new SSModel();
            using(var db = new BazaPodataka())
            {
                int id = PlaniranjeSession.Trenutni.PedagogId;
                model.SS_Planovi = db.SSPlan.Where(w => w.Id_pedagog == id).ToList();
            }
            return View(model);
        }
        public ActionResult NoviPlan()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SSModel model = new SSModel();
            using(var db = new BazaPodataka())
            {
                model.SKGodinaItems = VratiSelectList();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult NoviPlan(SSModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(model.SS_Plan.Naziv==null || model.SS_Plan.Ak_godina==0 || model.SS_Plan.Opis == null)
            {
                model.SKGodinaItems = VratiSelectList();
                return View(model);
            }
            using(var db = new BazaPodataka())
            {
                try
                {
                    model.SS_Plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
                    db.SSPlan.Add(model.SS_Plan);
                    db.SaveChanges();
                    TempData["poruka"] = "Plan je spremljen";
                }
                catch
                {
                    TempData["poruka"] = "Plan nije spremljen! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Uredi(int id, int pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SSModel model = new SSModel();
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == id);
            if (model.SS_Plan == null)
            {
                return HttpNotFound();
            }
            model.SKGodinaItems = VratiSelectList();
            model.ID_PLAN = pozicija;
            return View(model);
        }
        [HttpPost]
        public ActionResult Uredi(SSModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.SS_Plan.Naziv == null || model.SS_Plan.Opis == null || model.SS_Plan.Ak_godina < baza.SkolskaGodina.Min(m => m.Sk_Godina))
            {
                model.SKGodinaItems = VratiSelectList();
                return View(model);
            }
            model.SS_Plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.SSPlan.Add(model.SS_Plan);
                    db.Entry(model.SS_Plan).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["poruka"] = "Plan je promijenjen";
                }
                catch
                {
                    TempData["poruka"] = "Plan nije promijenjen! Pokušajte ponovno.";
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Obrisi (int id, int pozicija)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SSModel model = new SSModel();
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == id);
            if (model.SS_Plan == null)
            {
                return HttpNotFound();
            }
            model.ID_PLAN = pozicija;
            model.Tekst = model.SS_Plan.Ak_godina + "./" + (model.SS_Plan.Ak_godina + 1).ToString() + ".";
            return View(model);
        }
        [HttpPost]
        public ActionResult Obrisi(SSModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(model.SS_Plan.Id_pedagog != PlaniranjeSession.Trenutni.PedagogId)
            {
                return HttpNotFound();
            }
            int id = model.SS_Plan.Id_plan;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.SSPlan.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (result != null)
                    {
                        db.SSPlan.Remove(result);
                        db.SaveChanges();
                        TempData["poruka"] = "Plan je obrisan";
                    }
                    else
                    {
                        TempData["poruka"] = "Plan kojeg želite obrisati ne postoji!";
                    }
                }
                catch
                {
                    TempData["poruka"] = "Plan nije obrisan! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Detalji(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SSModel model = new SSModel();
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == id);
            if (model.SS_Plan==null)
            {
                return HttpNotFound();
            }
            model.SS_Podrucja = new List<SS_Plan_podrucje>();
            model.SS_Podrucja = baza.SSPodrucje.Where(w => w.ID_plan == id).ToList().OrderBy(o=>o.Red_br).ToList();
            return View(model);
        }
        public ActionResult NoviDetalji(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SSModel model = new SSModel();
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == id);
            if (model.SS_Plan==null)
            {
                return HttpNotFound();
            }
            model.Ciljevi = ciljevi.ReadCiljevi();
            model.PodrucjeRada = podrucja_rada.ReadPodrucjeRada();
            model.Oblici = oblici.ReadOblici();
            model.Subjekti = subjekti.ReadSubjekti();
            model.Zadaci = zadaci.ReadZadaci();
            return View(model);
        }
        [HttpPost]
        public ActionResult NoviDetalji(SSModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(model.SS_Plan_Podrucje.Opis_podrucje==null || model.SS_Plan_Podrucje.Svrha==null || model.SS_Plan_Podrucje.Zadaca==null ||
                model.SS_Plan_Podrucje.Sadrzaj==null || model.SS_Plan_Podrucje.Oblici==null || model.SS_Plan_Podrucje.Suradnici==null ||
                model.SS_Plan_Podrucje.Mjesto==null || model.SS_Plan_Podrucje.Vrijeme==null || model.SS_Plan_Podrucje.Ishodi == null ||
                model.SS_Plan_Podrucje.Sati<0)
            {
                model.Ciljevi = ciljevi.ReadCiljevi();
                model.PodrucjeRada = podrucja_rada.ReadPodrucjeRada();
                model.Oblici = oblici.ReadOblici();
                model.Subjekti = subjekti.ReadSubjekti();
                model.Zadaci = zadaci.ReadZadaci();
                return View(model);
            }
            model.SS_Plan_Podrucje.ID_plan = model.SS_Plan.Id_plan;
            int idPlan = model.SS_Plan.Id_plan;
            List<SS_Plan_podrucje> podrucja = new List<SS_Plan_podrucje>();
            podrucja = baza.SSPodrucje.Where(w => w.ID_plan == idPlan).ToList();
            if (podrucja.Count != 0)
            {
                model.SS_Plan_Podrucje.Red_br = podrucja.Max(m => m.Red_br) + 1;
            }
            else
            {
                model.SS_Plan_Podrucje.Red_br = 1;
            }
            
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.SSPodrucje.Add(model.SS_Plan_Podrucje);
                    db.SaveChanges();
                    TempData["poruka"] = "Detalj je spremljen";
                }
                catch
                {
                    TempData["poruka"] = "Detalj nije spremljen! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Detalji", new { id = model.SS_Plan.Id_plan });
        }
        public ActionResult UrediDetalje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SSModel model = new SSModel();
            model.SS_Plan_Podrucje = baza.SSPodrucje.SingleOrDefault(s => s.Id == id);
            if (model.SS_Plan_Podrucje == null) return RedirectToAction("Index", "Planiranje");
            int idPlan = model.SS_Plan_Podrucje.ID_plan;
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_plan == idPlan);
            if (model.SS_Plan == null)
            {
                return HttpNotFound();
            }
            model.Ciljevi = ciljevi.ReadCiljevi();
            model.PodrucjeRada = podrucja_rada.ReadPodrucjeRada();
            model.Oblici = oblici.ReadOblici();
            model.Subjekti = subjekti.ReadSubjekti();
            model.Zadaci = zadaci.ReadZadaci();
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediDetalje(SSModel model)
        {            
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.SS_Plan_Podrucje.Opis_podrucje == null || model.SS_Plan_Podrucje.Svrha == null || model.SS_Plan_Podrucje.Zadaca == null ||
                model.SS_Plan_Podrucje.Sadrzaj == null || model.SS_Plan_Podrucje.Oblici == null || model.SS_Plan_Podrucje.Suradnici == null ||
                model.SS_Plan_Podrucje.Mjesto == null || model.SS_Plan_Podrucje.Vrijeme == null || model.SS_Plan_Podrucje.Ishodi == null ||
                model.SS_Plan_Podrucje.Sati < 0)
            {
                model.Ciljevi = ciljevi.ReadCiljevi();
                model.PodrucjeRada = podrucja_rada.ReadPodrucjeRada();
                model.Oblici = oblici.ReadOblici();
                model.Subjekti = subjekti.ReadSubjekti();
                model.Zadaci = zadaci.ReadZadaci();
                return View(model);
            }
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.SSPodrucje.Add(model.SS_Plan_Podrucje);
                    db.Entry(model.SS_Plan_Podrucje).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["poruka"] = "Datalji su promijenjeni";
                }
                catch
                {
                    TempData["poruka"] = "Detalji nisu promijenjeni! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Detalji", new { id = model.SS_Plan.Id_plan });
        }
        public ActionResult ObrisiDetalje (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SSModel model = new SSModel();
            model.SS_Plan_Podrucje = baza.SSPodrucje.SingleOrDefault(s => s.Id == id);
            if (model.SS_Plan_Podrucje == null)
            {
                return HttpNotFound();
            }
            int idPlan = model.SS_Plan_Podrucje.ID_plan;
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == idPlan);
            if (model.SS_Plan == null) RedirectToAction("Index", "Planiranje");
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiDetalje (SSModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = model.SS_Plan_Podrucje.Id;
            int idPlan = model.SS_Plan_Podrucje.ID_plan;
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == idPlan);
            if (model.SS_Plan == null) return HttpNotFound();
            using (var db = new BazaPodataka())
            {
                try
                {
                    var result = db.SSPodrucje.SingleOrDefault(s => s.Id == id && s.ID_plan == idPlan);
                    if (result != null)
                    {
                        db.SSPodrucje.Remove(result);
                        db.SaveChanges();
                        TempData["poruka"] = "Detalj je obrisan";
                    }
                    else
                    {
                        TempData["poruka"] = "Traženi detalj nije pronađen! Pokušajte ponovno";
                    }
                }
                catch
                {
                    TempData["poruka"] = "Detalj nije obrisan! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Detalji", new { id = idPlan });
        }
        public ActionResult DetaljPomakGore(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idPlan = 0;
            SS_Plan_podrucje podrucje = baza.SSPodrucje.SingleOrDefault(s => s.Id == id);
            if (podrucje != null)
            {
                idPlan = podrucje.ID_plan;
            }
            SS_Plan plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == idPlan);
            if (podrucje == null || plan==null)
            {
                return HttpNotFound();
            }
            int pozicijaTrenutni = podrucje.Red_br;
            List<SS_Plan_podrucje> podrucja = new List<SS_Plan_podrucje>();
            podrucja = baza.SSPodrucje.Where(w => w.ID_plan == idPlan && w.Red_br<pozicijaTrenutni).ToList().OrderBy(o=>o.Red_br).ToList();
            if (podrucja.Count == 0)
            {
                return RedirectToAction("Detalji", new { id = idPlan });
            }
            int pozicijaPrethodni = podrucja.ElementAt(podrucja.Count - 1).Red_br;
            int idPrethodni = podrucja.ElementAt(podrucja.Count - 1).Id;
            using(var db=new BazaPodataka())
            {
                try
                {
                    var result = db.SSPodrucje.SingleOrDefault(s => s.Id == id);
                    var result1 = db.SSPodrucje.SingleOrDefault(s => s.Id == idPrethodni);
                    if(result!=null && result1 != null)
                    {
                        result.Red_br = pozicijaPrethodni;
                        result1.Red_br = pozicijaTrenutni;
                        db.SaveChanges();
                        TempData["poruka"] = "Detalj je pomaknut za 1 mjesto gore";
                    }
                }
                catch
                {
                    TempData["poruka"] = "Detalj nije pomaknut! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Detalji", new { id = idPlan });
        }
        public ActionResult DetaljPomakDolje(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idPlan = 0;
            SS_Plan_podrucje podrucje = baza.SSPodrucje.SingleOrDefault(s => s.Id == id);
            if (podrucje != null)
            {
                idPlan = podrucje.ID_plan;
            }
            SS_Plan plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == idPlan);
            if (podrucje == null || plan == null)
            {
                return HttpNotFound();
            }
            int pozicijaTrenutni = podrucje.Red_br;
            List<SS_Plan_podrucje> podrucja = new List<SS_Plan_podrucje>();
            podrucja = baza.SSPodrucje.Where(w => w.ID_plan == idPlan && w.Red_br > pozicijaTrenutni).ToList().OrderBy(o => o.Red_br).ToList();
            if (podrucja.Count == 0)
            {
                return RedirectToAction("Detalji", new { id = idPlan });
            }
            int pozicijaPrethodni = podrucja.ElementAt(0).Red_br;
            int idPrethodni = podrucja.ElementAt(0).Id;
            using (var db = new BazaPodataka())
            {
                try
                {
                    var result = db.SSPodrucje.SingleOrDefault(s => s.Id == id);
                    var result1 = db.SSPodrucje.SingleOrDefault(s => s.Id == idPrethodni);
                    if (result != null && result1 != null)
                    {
                        result.Red_br = pozicijaPrethodni;
                        result1.Red_br = pozicijaTrenutni;
                        db.SaveChanges();
                        TempData["poruka"] = "Detalj je pomaknut za 1 mjesto dolje";
                    }
                }
                catch
                {
                    TempData["poruka"] = "Detalj nije pomaknut! Pokušajte ponovno";
                }
            }
            return RedirectToAction("Detalji", new { id = idPlan });
        }
        public ActionResult Kopiraj(int id)
        {
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SS_Plan plan = baza.SSPlan.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.naziv = plan.Naziv;
            ViewBag.godina = plan.Ak_godina;
            ViewBag.select = VratiSelectList();

            plan.Naziv = string.Empty;
            plan.Opis = string.Empty;
            return View(plan);
        }
        [HttpPost]
        public ActionResult Kopiraj(SS_Plan model)
        {
            if(PlaniranjeSession.Trenutni.PedagogId<=0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id = model.Id_plan;
            SS_Plan plan = baza.SSPlan.SingleOrDefault(s => s.Id_plan == id && s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (!ModelState.IsValid)
            {                
                ViewBag.naziv = plan.Naziv;
                ViewBag.select = VratiSelectList();
                ViewBag.godina = plan.Ak_godina;
                return View(model);
            }
            plan.Naziv = model.Naziv;
            plan.Opis = model.Opis;
            plan.Ak_godina = model.Ak_godina;
            
            List<SS_Plan_podrucje> podrucja = baza.SSPodrucje.Where(w => w.ID_plan == id).ToList();
            using (var db = new BazaPodataka())
            {
                db.SSPlan.Add(plan);
                db.SaveChanges();

                id = db.SSPlan.Where(w => w.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId).Max(m => m.Id_plan);
                foreach (var item in podrucja)
                {
                    item.ID_plan = id;
                    db.SSPodrucje.Add(item);
                    db.SaveChanges();
                }
            }
            TempData["poruka"] = "Plan je kopiran";
            return RedirectToAction("Index");
        }
        public FileStreamResult Ispis (int id)
        {
            SSModel model = new SSModel();
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == id);
            model.SS_Podrucja = new List<SS_Plan_podrucje>();
            if (model.SS_Plan != null)
            {
                model.SS_Podrucja = baza.SSPodrucje.Where(w => w.ID_plan == id).ToList().OrderBy(o=>o.Red_br).ToList();
            }
            PlanSsPodrucjaReport report = new PlanSsPodrucjaReport(model.SS_Podrucja);
            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
        }
        private SelectList VratiSelectList()
        {
            List<Sk_godina> skGodina = baza.SkolskaGodina.ToList();
            var selectListItem = new List<SelectListItem>();
            foreach (var item in skGodina)
            {
                selectListItem.Add(new SelectListItem { Value = item.Sk_Godina.ToString(), Text = item.Sk_Godina + "./" + (item.Sk_Godina + 1).ToString() + "." });
            }
            var select = new SelectList(selectListItem, "Value", "Text");
            return select;
        }        
    }
}
