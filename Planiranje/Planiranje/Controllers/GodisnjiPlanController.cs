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
    public class GodisnjiPlanController : Controller
	{
        private Godisnji_plan_DBHandle godisnji_planovi = new Godisnji_plan_DBHandle();     
        int Page_No_Master = 1;

        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled godisnjih planova";
            ViewBag.CurrentSortOrder = Sort;
            ViewBag.SortingName = String.IsNullOrEmpty(Sort) ? "Naziv" : "";

            ViewBag.Message = "Grad";

            ViewBag.FilterValue = Search;
            if (Search != null)
            {
                Page_No = 1;
            }
            else
            {
                Search = Filter;
            }
            ViewBag.CurrentPage = 1;
            if (Page_No != null)
                ViewBag.CurrentPage = Page_No;


            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            if (Search == null || Search.Length == 0)
            {

                if (Request.IsAjaxRequest())
                {
                    int noP = (int)Page_No_Master;
                    var Popis2 = godisnji_planovi.ReadGodisnjePlanove().ToPagedList(No_Of_Page, Size_Of_Page);
                    return PartialView("_GradView", Popis2);
                }
                Page_No_Master = No_Of_Page;
                var Popis = godisnji_planovi.ReadGodisnjePlanove().ToPagedList(No_Of_Page, Size_Of_Page);
                return View(Popis);
            }
            else
            {
                Page_No_Master = No_Of_Page;
                var Popis = godisnji_planovi.ReadGodisnjePlanove(Search).ToPagedList(No_Of_Page, Size_Of_Page);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_GradView", Popis);
                }
                return View(Popis);
            }

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
        public ActionResult NoviPlan(Godisnji_plan gr)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Godisnji_plan godisnji_plan = new Godisnji_plan();
            godisnji_plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            godisnji_plan.Ak_godina = gr.Ak_godina;
            godisnji_plan.Br_dana_godina_odmor = gr.Br_dana_godina_odmor;
            godisnji_plan.Br_radnih_dana = gr.Br_radnih_dana;
            godisnji_plan.God_fond_sati = gr.God_fond_sati;
            godisnji_plan.Ukupni_rad_dana = gr.Ukupni_rad_dana;
            godisnji_plan.Id_god = gr.Id_god;
            
            if (godisnji_planovi.CreateGodisnjiPlan(godisnji_plan))
			{
				TempData["alert"] = "<script>alert('Novi godisnji plan je uspjesno spremljen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Novi godisnji plan nije spremljen');</script>";
			}
			return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Godisnji_plan godisnji_plan = new Godisnji_plan();
            godisnji_plan = godisnji_planovi.ReadGodisnjiPlan(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", godisnji_plan);
            }
            return View("Uredi", godisnji_plan);
        }

        [HttpPost]
        public ActionResult Edit(Godisnji_plan godisnji_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!godisnji_planovi.UpdateGodisnjiPlan(godisnji_plan))
			{
				TempData["alert"] = "<script>alert('Godisnji plan nije promjenjen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Godisnji plan je uspjesno promjenjen!');</script>";
			}
			return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Godisnji_plan godisnji_plan = new Godisnji_plan();
            godisnji_plan = godisnji_planovi.ReadGodisnjiPlan(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", godisnji_plan);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(Godisnji_plan godisnji_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!godisnji_planovi.DeleteGodisnjiPlan(godisnji_plan.Id_god))
			{
				TempData["alert"] = "<script>alert('Godisnji plan nije obrisan, dogodila se greska!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Godisnji plan je uspjesno obrisan!');</script>";
			}
			return RedirectToAction("Index");
		}

		public FileStreamResult Ispis()
		{
			List<Godisnji_plan> planovi = godisnji_planovi.ReadGodisnjePlanove();

			GodisnjiPlanReport report = new GodisnjiPlanReport(planovi);

			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}
	}
}