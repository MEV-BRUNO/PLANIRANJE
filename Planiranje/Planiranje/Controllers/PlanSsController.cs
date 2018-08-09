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
       private SS_Plan_DBHandle planovi_ss = new SS_Plan_DBHandle();

        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled srednjih skola";

			SSModel model = new SSModel();
			model.SS_Planovi = planovi_ss.ReadSSPlanove();
			
			return View(model);
		}

        public ActionResult NoviPlan()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("NoviPlan");
            }
            return View("NoviPlan");
        }

        [HttpPost]
        public ActionResult NoviPlan(SS_Plan gr)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SS_Plan ss_plan = new SS_Plan();
            ss_plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            ss_plan.Ak_godina = gr.Ak_godina;
            ss_plan.Naziv = gr.Naziv;
            ss_plan.Opis = gr.Opis;
            if (planovi_ss.CreateSSPlan(ss_plan))
			{
				TempData["alert"] = "<script>alert('Novi plan za srednju skolu je uspjesno spremljen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Novi plan nije spremljen');</script>";
			}
			return RedirectToAction("Index");
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

		public FileStreamResult Ispis()
		{
			List<SS_Plan> planovi = planovi_ss.ReadSSPlanove();

			PlanSsReport report = new PlanSsReport(planovi);

			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}
	}
}