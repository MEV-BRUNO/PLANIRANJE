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

		private Mjesecni_plan_DBHandle mjesecni_planovi = new Mjesecni_plan_DBHandle();
		private Godisnji_plan_DBHandle godisnji_planovi = new Godisnji_plan_DBHandle();
		private Podrucje_rada_DBHandle podrucja_rada = new Podrucje_rada_DBHandle();
		private Subjekt_DBHandle subjekti = new Subjekt_DBHandle();
		private Aktivnost_DBHandle aktivnosti = new Aktivnost_DBHandle();
		private SS_Plan_DBHandle planovi_ss = new SS_Plan_DBHandle();

        public ActionResult Index(string Plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled srednjih skola";

			SSModel model = new SSModel();
			model.GodisnjiPlanovi = new List<SelectListItem>(godisnji_planovi.ReadGodisnjePlanove().Select(i => new SelectListItem()
			{
				Text = i.Ak_godina,
				Value = i.Id_god.ToString()
			}));

			int idPlan = 0;
			if (Plan != null)
			{
				idPlan = model.GodisnjiPlanovi.FindIndex(x => x.Value == Plan);
				model.ID_GODINA = Convert.ToInt32(model.GodisnjiPlanovi.ElementAt(idPlan).Value.ToString());
				ViewBag.HasGodPlan = true;
			}
			else
			{
				if (model.GodisnjiPlanovi.Count < 1)
				{
					ViewBag.HasGodPlan = false;
				}
				else
				{
					model.ID_GODINA = Convert.ToInt32(model.GodisnjiPlanovi.ElementAt(0).Value.ToString());
					idPlan = 0;
					ViewBag.HasGodPlan = true;
				}
			}
			if (model.GodisnjiPlanovi.Count > 0)
			{
				model.GodisnjiPlanovi.ElementAt(idPlan).Selected = true;
			}
			model.SS_Planovi = planovi_ss.ReadSSPlanove(model.ID_GODINA);
			return View("Index", model);
		}

        public ActionResult NoviPlan(int id_god)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			SSModel model = new SSModel();
			model.ID_GODINA = id_god;
			model.SS_Plan = new SS_Plan();
            if (Request.IsAjaxRequest())
            {
                return View("NoviPlan", model);
            }
            return View("NoviPlan");
        }

        [HttpPost]
        public ActionResult NoviPlan(SSModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SS_Plan ss_plan = new SS_Plan();
            ss_plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            ss_plan.Id_godina = model.ID_GODINA;
            ss_plan.Naziv = model.SS_Plan.Naziv;
            ss_plan.Opis = model.SS_Plan.Opis;

			if (ss_plan.Naziv != null && ss_plan.Opis != null && planovi_ss.CreateSSPlan(ss_plan))
			{
				return RedirectToAction("Index", new { Plan = model.ID_GODINA });
			}
			model.SS_Plan = ss_plan;
			return View("NoviPlan", model);
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SS_Plan ss_plan = new SS_Plan();
            ss_plan = planovi_ss.ReadSSPlan(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", ss_plan);
            }
            return View("Uredi", ss_plan);
        }
        [HttpPost]
        public ActionResult Edit(SS_Plan ss_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!planovi_ss.UpdateSSPlan(ss_plan))
			{
				TempData["alert"] = "<script>alert('Plan nije promjenjen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Plan je uspjesno promjenjen!');</script>";
			}
			return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SS_Plan ss_plan = new SS_Plan();
            ss_plan = planovi_ss.ReadSSPlan(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", ss_plan);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(SS_Plan ss_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!planovi_ss.DeleteSSPlan(ss_plan.Id_plan))
			{
				TempData["alert"] = "<script>alert('Plan nije obrisan, dogodila se greska!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Plan je uspjesno obrisan!');</script>";
			}
			return RedirectToAction("Index");
		}

		/*public FileStreamResult Ispis()
		{
			List<SS_Plan> planovi = planovi_ss.ReadSSPlanove();

			PlanSsReport report = new PlanSsReport(planovi);

			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}*/
	}
}