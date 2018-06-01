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

		// GET: MjesecniPlan
		public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			ViewBag.Title = "Pregled mjesecnih planova";
			ViewBag.CurrentSortOrder = Sorting_Order;
			ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Naziv" : "";

			ViewBag.Message = "Grad";

			ViewBag.FilterValue = Search_Data;
			if (Search_Data != null)
			{
				Page_No = 1;
			}
			else
			{
				Search_Data = Filter_Value;
			}
			ViewBag.CurrentPage = 1;
			if (Page_No != null)
				ViewBag.CurrentPage = Page_No;

			
			int Size_Of_Page = 20;
			int No_Of_Page = (Page_No ?? 1);
			if (Search_Data == null || Search_Data.Length == 0)
			{

				if (Request.IsAjaxRequest())
				{
					int noP = (int)Page_No_Master;
					var Popis2 = mjesecni_planovi.DohvatiMjesecnePlanove().ToPagedList(No_Of_Page, Size_Of_Page);
					return PartialView("_GradView", Popis2);
				}
				Page_No_Master = No_Of_Page;
				var Popis = mjesecni_planovi.DohvatiMjesecnePlanove().ToPagedList(No_Of_Page, Size_Of_Page);
				return View(Popis);
			}
			else
			{
				Page_No_Master = No_Of_Page;
				var Popis = mjesecni_planovi.DohvatiMjesecnePlanove(Search_Data).ToPagedList(No_Of_Page, Size_Of_Page);
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
			else

				return View("NoviPlan");
		}

		[HttpPost]
		public ActionResult NoviPlan(Mjesecni_plan gr)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			Mjesecni_plan grObj = new Mjesecni_plan();
			grObj.ID_pedagog = PlaniranjeSession.Trenutni.PedagogId;
			grObj.Ak_godina = gr.Ak_godina;
			grObj.Naziv = gr.Naziv;
			grObj.Opis = gr.Opis;

			string IsSuccess = mjesecni_planovi.DodajMjesecniPlan(grObj);

			if (!IsSuccess.Equals("OK"))
			{
				ModelState.Clear();
				return PartialView("NoviPlan", gr);
			}
			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			Mjesecni_plan gr = new Mjesecni_plan();
			gr = mjesecni_planovi.DohvatiMjesecniPlan(id);
			if (Request.IsAjaxRequest())
			{
				ViewBag.IsUpdate = false;
				return View("Uredi", gr);
			}
			else

				return View("Uredi", gr);
		}
		[HttpPost]
		public ActionResult Edit(Mjesecni_plan gr)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			string IsSuccess = mjesecni_planovi.updateGrad(gr);

			if (!IsSuccess.Equals("OK"))
			{
				ModelState.Clear();
				return PartialView("Uredi", gr);
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
			Mjesecni_plan gr = new Mjesecni_plan();
			gr = mjesecni_planovi.DohvatiMjesecniPlan(id);
			if (Request.IsAjaxRequest())
			{
				ViewBag.IsUpdate = false;
				return View("Obrisi", gr);
			}
			else

				return View("Obrisi");
		}

		[HttpPost]
		public ActionResult Delete(Mjesecni_plan gr)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			string IsSuccess = mjesecni_planovi.deleteGrad(gr.ID_plan);
			if (!IsSuccess.Equals("OK"))
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