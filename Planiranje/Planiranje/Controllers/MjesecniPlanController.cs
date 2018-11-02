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
                mjesecniModel.GODINA = mjesecniModel.MjesecniPlanovi.Min(m => m.Ak_godina);
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
    }
}
		