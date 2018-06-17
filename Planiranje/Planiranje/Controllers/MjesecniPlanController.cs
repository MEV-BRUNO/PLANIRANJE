using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class MjesecniPlanController : Controller
	{
		private Mjesecni_plan_DBHandle mjesecni_planovi = new Mjesecni_plan_DBHandle();
		int Page_No_Master = 1;

		public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			ViewBag.Title = "Pregled mjesecnih planova";
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
					var Popis2 = mjesecni_planovi.ReadMjesecnePlanove().ToPagedList(No_Of_Page, Size_Of_Page);
					return PartialView("_GradView", Popis2);
				}
				Page_No_Master = No_Of_Page;
				var Popis = mjesecni_planovi.ReadMjesecnePlanove().ToPagedList(No_Of_Page, Size_Of_Page);
				return View(Popis);
			}
			else
			{
				Page_No_Master = No_Of_Page;
				var Popis = mjesecni_planovi.ReadMjesecnePlanove(Search).ToPagedList(No_Of_Page, Size_Of_Page);
				if (Request.IsAjaxRequest())
				{
					return PartialView("_GradView", Popis);
				}

				return View(Popis);
			}
		}

		public ActionResult NoviPlan()
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
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
		public ActionResult NoviPlan(Mjesecni_plan _mjesecni_plan)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			Mjesecni_plan mjesecni_plan = new Mjesecni_plan();
			mjesecni_plan.ID_pedagog = PlaniranjeSession.Trenutni.PedagogId;
			mjesecni_plan.Ak_godina = _mjesecni_plan.Ak_godina;
			mjesecni_plan.Naziv = _mjesecni_plan.Naziv;
			mjesecni_plan.Opis = _mjesecni_plan.Opis;
			if (mjesecni_planovi.CreateMjesecniPlan(mjesecni_plan))
			{
				TempData["alert"] = "<script>alert('Novi mjesecni plan je uspjesno spremljen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Novi mjesecni plan nije spremljen');</script>";
			}
			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			Mjesecni_plan mjesecni_plan = new Mjesecni_plan();
			mjesecni_plan = mjesecni_planovi.ReadMjesecniPlan(id);
			if (Request.IsAjaxRequest())
			{
				ViewBag.IsUpdate = false;
				return View("Uredi", mjesecni_plan);
			}
			return View("Uredi", mjesecni_plan);
		}
		[HttpPost]
		public ActionResult Edit(Mjesecni_plan mjesecni_plan)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			if (!mjesecni_planovi.UpdateMjesecniPlan(mjesecni_plan))
			{
				ModelState.Clear();
				return PartialView("Uredi", mjesecni_plan);
			}
			if (Request.IsAjaxRequest())
			{
				return new HttpStatusCodeResult(HttpStatusCode.OK);
			}
			return RedirectToAction("Index");
		}
		
		public ActionResult Delete(int id)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			Mjesecni_plan mjesecni_plan = new Mjesecni_plan();
			mjesecni_plan = mjesecni_planovi.ReadMjesecniPlan(id);
			if (Request.IsAjaxRequest())
			{
				ViewBag.IsUpdate = false;
				return View("Obrisi", mjesecni_plan);
			}
			return View("Obrisi");
		}

		[HttpPost]
		public ActionResult Delete(Mjesecni_plan mjesecni_plan)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			if (!mjesecni_planovi.DeleteMjesecniPlan(mjesecni_plan.ID_plan))
			{
				ModelState.Clear();
				return PartialView("Obrisi");
			}
			if (Request.IsAjaxRequest())
			{
				return new HttpStatusCodeResult(HttpStatusCode.OK);
			}
			return RedirectToAction("Index");
		}
	}
}
