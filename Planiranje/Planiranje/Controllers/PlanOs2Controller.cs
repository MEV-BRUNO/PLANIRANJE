﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class PlanOs2Controller : Controller
    {
        private OS_Plan_2_DBHandle planovi_os2 = new OS_Plan_2_DBHandle();
        private BazaPodataka baza = new BazaPodataka();
        private Ciljevi_DBHandle ciljevi_db = new Ciljevi_DBHandle();
        private Zadaci_DBHandle zadaci_db = new Zadaci_DBHandle();
        private Subjekt_DBHandle subjekti_db = new Subjekt_DBHandle();
        private Oblici_DBHandle oblici_db = new Oblici_DBHandle();

        
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            List<OS_Plan_2> planovi = new List<OS_Plan_2>();
            planovi = planovi_os2.ReadOS_Plan_2();
            return View(planovi);
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
        public ActionResult NoviPlan(OS_Plan_2 gr)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			OS_Plan_2 os_plan_2 = new OS_Plan_2();
            os_plan_2.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            os_plan_2.Ak_godina = gr.Ak_godina;
            os_plan_2.Naziv = gr.Naziv;
            os_plan_2.Opis = gr.Opis;
            if (planovi_os2.CreateOS_Plan_2(os_plan_2))
			{
				TempData["alert"] = "<script>alert('Novi plan za osnovnu skolu 2 je uspjesno spremljen!');</script>";
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
            OS_Plan_2 os_plan_2 = new OS_Plan_2();
            os_plan_2 = planovi_os2.ReadOS_Plan_2(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", os_plan_2);
            }
            return View("Uredi", os_plan_2);
        }
        [HttpPost]
        public ActionResult Edit(OS_Plan_2 os_plan_2)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!planovi_os2.UpdateOS_Plan_2(os_plan_2))
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
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            OS_Plan_2 os_plan_2 = new OS_Plan_2();
			os_plan_2 = planovi_os2.ReadOS_Plan_2(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", os_plan_2);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(OS_Plan_2 os_plan_2)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!planovi_os2.DeleteOS_Plan_2(os_plan_2.Id_plan))
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
			List<OS_Plan_2> planovi = planovi_os2.ReadOS_Plan_2();

			PlanOs2Report report = new PlanOs2Report(planovi);

			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}
        public ActionResult Details(int id)
        {
            PlanOs2View plan = new PlanOs2View();
            plan.OsPlan2 = new OS_Plan_2();
            plan.OsPlan2 = planovi_os2.ReadOS_Plan_2(id);
            plan.OsPlan2Podrucja = new List<OS_Plan_2_podrucje>();
            plan.OsPlan2Podrucja = baza.OsPlan2Podrucje.Where(w => w.Id_glavni_plan == id).ToList();
            plan.OsPlan2Podrucja = plan.OsPlan2Podrucja.OrderBy(o => o.Red_br_podrucje).ToList();
            plan.Ciljevi = new List<Ciljevi>();
            plan.Ciljevi = ciljevi_db.ReadCiljevi();
            plan.Subjekti = new List<Subjekti>();
            plan.Subjekti = subjekti_db.ReadSubjekti();
            plan.Zadaci = new List<Zadaci>();
            plan.Zadaci = zadaci_db.ReadZadaci();
            plan.Oblici = new List<Oblici>();
            plan.Oblici = oblici_db.ReadOblici();
            return View(plan);
        }
        public ActionResult NoviPosao(int id)
        {

            return View();
        }
	}
}