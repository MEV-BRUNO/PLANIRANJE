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
	}
}
