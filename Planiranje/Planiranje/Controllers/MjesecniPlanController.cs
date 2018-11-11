using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models;
using Planiranje.Reports;

namespace Planiranje.Controllers
{
    public class MjesecniPlanController : Controller
    {
        private Mjesecni_plan_DBHandle mjesecni_planovi = new Mjesecni_plan_DBHandle();
        private BazaPodataka baza = new BazaPodataka();
        private Podrucje_rada_DBHandle podrucja_rada = new Podrucje_rada_DBHandle();
        private Subjekt_DBHandle subjekti = new Subjekt_DBHandle();
        private Aktivnost_DBHandle aktivnosti = new Aktivnost_DBHandle();

        public ActionResult Index(int? godina)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 )
            {
                return RedirectToAction("Index", "Planiranje");
            }
            MjesecniModel mjesecniModel = new MjesecniModel();
            mjesecniModel.SkolskaGodina = new List<Sk_godina>();
            mjesecniModel.SkolskaGodina = baza.SkolskaGodina.ToList();
            mjesecniModel.MjesecniPlanovi = baza.MjesecniPlan.Where(w => w.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId).ToList();
            if (godina != null)
            {
                mjesecniModel.GODINA = (Int32)godina;
            }
            else
            {
                if (mjesecniModel.MjesecniPlanovi.Count==0)
                {
                    mjesecniModel.GODINA = mjesecniModel.SkolskaGodina.Min(m => m.Sk_Godina);
                }
                else
                {
                    mjesecniModel.GODINA = mjesecniModel.MjesecniPlanovi.Min(m => m.Ak_godina);
                }
            }
            int god = mjesecniModel.GODINA;
            mjesecniModel.MjesecniPlanovi = baza.MjesecniPlan.Where(w => w.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId && w.Ak_godina == god).ToList();
            return View("Index", mjesecniModel);
        }
        public ActionResult NoviPlan(int godina)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            MjesecniModel model = new MjesecniModel();
            model.GODINA = godina;
            return View(model);
        }
        [HttpPost]
        public ActionResult NoviPlan(MjesecniModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.MjesecniPlan.Naziv == null)
            {
                return View(model);
            }
            model.MjesecniPlan.Ak_godina = model.GODINA;
            model.MjesecniPlan.ID_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.MjesecniPlan.Add(model.MjesecniPlan);
                    db.SaveChanges();
                    TempData["poruka"] = "Plan je spremljen!";
                }
                catch
                {
                    TempData["poruka"] = "Plan nije spremljen! Pokušajte ponovno.";
                }
                return RedirectToAction("Index",new { godina = model.GODINA});
            }
        }
        public ActionResult ObrisiPlan(int id)
        {
            Mjesecni_plan plan = new Mjesecni_plan();
            plan = baza.MjesecniPlan.SingleOrDefault(s => s.ID_plan == id && s.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || plan==null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            return View("Obrisi", plan);
        }
        [HttpPost]
        public ActionResult ObrisiPlan(Mjesecni_plan plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int id=plan.ID_plan, god=plan.Ak_godina;
            using(var db = new BazaPodataka())
            {
                try
                {
                    TempData["poruka"] = "Plan nije obrisan! Pokušajte ponovno.";
                    var item = db.MjesecniPlan.SingleOrDefault(s => s.ID_plan == id && s.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId);
                    if (item != null)
                    {
                        god = item.Ak_godina;
                        db.MjesecniPlan.Remove(item);
                        db.SaveChanges();
                        TempData["poruka"] = "Plan je obrisan!";
                    }
                }
                catch
                {

                }
            }
            return RedirectToAction("Index",new { godina = god});
        }
        public ActionResult UrediPlan(int id)
        {
            MjesecniModel model = new MjesecniModel();
            model.SkolskaGodina = new List<Sk_godina>();
            model.SkolskaGodina = baza.SkolskaGodina.ToList();
            model.MjesecniPlan = new Mjesecni_plan();
            model.MjesecniPlan = baza.MjesecniPlan.SingleOrDefault(s => s.ID_plan == id && s.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if(model.MjesecniPlan==null || model.SkolskaGodina == null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            return View("UrediNoviPlan", model);
        }
        [HttpPost]
        public ActionResult UrediPlan(MjesecniModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || model.MjesecniPlan.ID_pedagog!=PlaniranjeSession.Trenutni.PedagogId)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.MjesecniPlan.Naziv == null || model.MjesecniPlan.Ak_godina==0)
            {
                return View(model);
            }
            int god = model.MjesecniPlan.Ak_godina;
            using(var db = new BazaPodataka())
            {
                try
                {
                    db.MjesecniPlan.Add(model.MjesecniPlan);
                    db.Entry(model.MjesecniPlan).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["poruka"] = "Plan je promijenjen!";
                }
                catch
                {
                    TempData["poruka"] = "Plan iz nekog razloga nije promijenjen! Pokušajte ponovno.";
                }
            }
            return RedirectToAction("Index",new { godina=god});
        }
        public ActionResult Detalji (int id)
        {
            MjesecniModel model = new MjesecniModel();
            model.MjesecniPlan = new Mjesecni_plan();
            model.MjesecniPlan = baza.MjesecniPlan.SingleOrDefault(s => s.ID_plan == id && s.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || model.MjesecniPlan==null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            model.MjesecniDetalji = new List<Mjesecni_detalji>();
            model.MjesecniDetalji = baza.MjesecniDetalji.Where(w => w.ID_plan == id).ToList();
            return View(model);
        }
        public ActionResult NoviDetalji (int idPlan)
        {            
            MjesecniModel model = new MjesecniModel();
            model.ID_PLAN = idPlan;
            model.MjesecniPlan = new Mjesecni_plan();
            model.MjesecniPlan = baza.MjesecniPlan.SingleOrDefault(s => s.ID_plan == idPlan && s.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || model.MjesecniPlan==null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            model.Aktivnosti = new List<Aktivnost>();
            model.Subjekti = new List<Subjekti>();
            model.PodrucjaRada = new List<Podrucje_rada>();
            model.Aktivnosti = aktivnosti.ReadAktivnost();
            model.Subjekti = subjekti.ReadSubjekti();
            model.PodrucjaRada = podrucja_rada.ReadPodrucjeRada();
            return View(model);
        }
        [HttpPost]
        public ActionResult NoviDetalji (MjesecniModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || model.mjesecniDetalj == null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.mjesecniDetalj.Aktivnost == null||model.mjesecniDetalj.Subjekti==null||model.mjesecniDetalj.Suradnici==null
                || model.mjesecniDetalj.Podrucje==null||model.mjesecniDetalj.Vrijeme==null||model.mjesecniDetalj.Br_sati==0
                || model.mjesecniDetalj.Biljeska==null)
            {
                model.Aktivnosti = aktivnosti.ReadAktivnost();
                model.Subjekti = subjekti.ReadSubjekti();
                model.PodrucjaRada = podrucja_rada.ReadPodrucjeRada();
                return View(model);
            }
            model.mjesecniDetalj.ID_plan = model.ID_PLAN;
            using(var db=new BazaPodataka())
            {
                try
                {
                    db.MjesecniDetalji.Add(model.mjesecniDetalj);
                    db.SaveChanges();
                    TempData["poruka"] = "Novi detalj je spremljen!";
                }
                catch
                {
                    TempData["poruka"] = "Novi detalj nije spremljen! Pokušajte ponovno.";
                }
            }
            return RedirectToAction("Detalji",new { id = model.ID_PLAN});
        }
        public ActionResult UrediDetalje(int id)
        {
            MjesecniModel model = new MjesecniModel();
            model.mjesecniDetalj = new Mjesecni_detalji();
            model.mjesecniDetalj = baza.MjesecniDetalji.SingleOrDefault(w => w.ID == id);
            if (model.mjesecniDetalj == null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idPlan = model.mjesecniDetalj.ID_plan;
            model.MjesecniPlan = new Mjesecni_plan();
            model.MjesecniPlan = baza.MjesecniPlan.SingleOrDefault(s => s.ID_plan == idPlan && s.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || model.MjesecniPlan == null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            model.PodrucjaRada = podrucja_rada.ReadPodrucjeRada();
            model.Subjekti = subjekti.ReadSubjekti();
            model.Aktivnosti = aktivnosti.ReadAktivnost();
            return View(model);
        }
        [HttpPost]
        public ActionResult UrediDetalje (MjesecniModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.mjesecniDetalj.Aktivnost == null || model.mjesecniDetalj.Subjekti == null || model.mjesecniDetalj.Suradnici == null
                || model.mjesecniDetalj.Podrucje == null || model.mjesecniDetalj.Vrijeme == null || model.mjesecniDetalj.Br_sati == 0
                || model.mjesecniDetalj.Biljeska == null)
            {
                model.Aktivnosti = aktivnosti.ReadAktivnost();
                model.Subjekti = subjekti.ReadSubjekti();
                model.PodrucjaRada = podrucja_rada.ReadPodrucjeRada();
                return View(model);
            }
            using (var db = new BazaPodataka())
            {
                try
                {
                    db.MjesecniDetalji.Add(model.mjesecniDetalj);
                    db.Entry(model.mjesecniDetalj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["poruka"] = "Detalji su promijenjeni!";
                }
                catch
                {
                    TempData["poruka"] = "Detalji nisu promijenjeni! Pokušajte ponovno.";
                }
            }
            return RedirectToAction("Detalji",new { id=model.MjesecniPlan.ID_plan});
        }
        public ActionResult ObrisiDetalj(int id)
        {
            MjesecniModel model = new MjesecniModel();            
            model.mjesecniDetalj = baza.MjesecniDetalji.SingleOrDefault(s => s.ID == id);            
            if (model.mjesecniDetalj == null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            int idPlan = model.mjesecniDetalj.ID_plan;
            model.MjesecniPlan = baza.MjesecniPlan.SingleOrDefault(s => s.ID_plan == idPlan && s.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || model.MjesecniPlan==null)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ObrisiDetalj(MjesecniModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }            
            int id = model.mjesecniDetalj.ID;
            using(var db = new BazaPodataka())
            {
                try
                {
                    var item = db.MjesecniDetalji.SingleOrDefault(s => s.ID == id);
                    if (item != null)
                    {
                        db.MjesecniDetalji.Remove(item);
                        db.SaveChanges();
                        TempData["poruka"] = "Detalj je obrisan!";
                    }
                }
                catch
                {
                    TempData["poruka"] = "Detalj nije obrisan! Pokušajte ponovno.";
                }
            }
            return RedirectToAction("Detalji", new { id = model.MjesecniPlan.ID_plan });
        }

        public FileStreamResult Ispis (int idPlan)
        {
            MjesecniModel model = new MjesecniModel();
            model.MjesecniDetalji = new List<Mjesecni_detalji>();
            model.MjesecniPlan = baza.MjesecniPlan.SingleOrDefault(s => s.ID_plan == idPlan && s.ID_pedagog == PlaniranjeSession.Trenutni.PedagogId);
            if (model.MjesecniPlan != null)
            {
                model.MjesecniDetalji = baza.MjesecniDetalji.Where(w => w.ID_plan == idPlan).ToList();
            }
            MjesecniPlanReport report = new MjesecniPlanReport(model);
            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
        }
    }
}
		