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
                model.SkGodina = db.SkolskaGodina.ToList();
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
                model.SkGodina = baza.SkolskaGodina.ToList();
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
                return RedirectToAction("Index", "Planiranje");
            }
            model.SkGodina = baza.SkolskaGodina.ToList();
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
                model.SkGodina = baza.SkolskaGodina.ToList();
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
                return RedirectToAction("Index", "Planiranje");
            }
            model.ID_PLAN = pozicija;
            model.Tekst = model.SS_Plan.Ak_godina + "./" + (model.SS_Plan.Ak_godina + 1).ToString() + ".";
            return View(model);
        }
        [HttpPost]
        public ActionResult Obrisi(SSModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || model.SS_Plan.Id_pedagog!=PlaniranjeSession.Trenutni.PedagogId)
            {
                return RedirectToAction("Index", "Planiranje");
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
            SSModel model = new SSModel();
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == id);
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || model.SS_Plan==null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            model.SS_Podrucja = new List<SS_Plan_podrucje>();
            model.SS_Podrucja = baza.SSPodrucje.Where(w => w.ID_plan == id).ToList();
            return View(model);
        }
        public ActionResult NoviDetalji(int id)
        {            
            SSModel model = new SSModel();
            model.SS_Plan = baza.SSPlan.SingleOrDefault(s => s.Id_pedagog == PlaniranjeSession.Trenutni.PedagogId && s.Id_plan == id);
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || model.SS_Plan==null)
            {
                return RedirectToAction("Index", "Planiranje");
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
	}
}
