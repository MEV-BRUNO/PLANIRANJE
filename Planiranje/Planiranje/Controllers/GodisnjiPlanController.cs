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

		// NOVI PLAN
		public ActionResult NoviPlan()
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			ViewBag.Title = "Zaboravljena lozinka";
			return View("NoviPlan");
		}

		[HttpPost]
        public ActionResult NoviPlan(GodisnjiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            
            if (godisnji_planovi.CreateGodisnjiPlan(model))
			{
				TempData["alert"] = "<script>alert('Novi godisnji plan je uspjesno spremljen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Novi godisnji plan nije spremljen');</script>";
			}
			return RedirectToAction("Index");
        }

		// UREĐIVANJE
        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			GodisnjiModel detalji = godisnji_planovi.ReadGodisnjiDetalji(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", detalji);
            }
            return View("Uredi", detalji);
        }

        [HttpPost]
        public ActionResult Edit(GodisnjiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!godisnji_planovi.UpdateGodisnjiPlan(model))
			{
				TempData["alert"] = "<script>alert('Godisnji plan nije promjenjen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Godisnji plan je uspjesno promjenjen!');</script>";
			}
			return RedirectToAction("Index");
        }

		// BRISANJE
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
                if (!godisnji_planovi.DeleteGodisnjiDetalji(godisnji_plan.Id_god))
                {
                    TempData["alert"] = "<script>alert('Godisnji plan nije obrisan, dogodila se greska!');</script>";
                }
                TempData["alert"] = "<script>alert('Godisnji plan je uspjesno obrisan!');</script>";
			}
			return RedirectToAction("Index");
		}

		// DETALJI
        public ActionResult Details(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			GodisnjiModel detalji = godisnji_planovi.ReadGodisnjiDetalji(id);
			ViewBag.Title = "Detalji " + detalji.GodisnjiPlan.Ak_godina.ToString();
			return View("Detalji", detalji);
		}

		// ISPIS
		public FileStreamResult Ispis(int id)
		{
			GodisnjiReport report = new GodisnjiReport(godisnji_planovi.ReadGodisnjiDetalji(id));
			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}
	}
}