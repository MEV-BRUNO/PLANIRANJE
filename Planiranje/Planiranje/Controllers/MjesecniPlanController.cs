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
    public class MjesecniPlanController : Controller
	{
		private Mjesecni_plan_DBHandle mjesecni_planovi = new Mjesecni_plan_DBHandle();
		private Godisnji_plan_DBHandle godisnji_planovi = new Godisnji_plan_DBHandle();
		int Page_No_Master = 1;

		public ActionResult Index(string Sort, string Search, string Plan, string Filter, int? Page_No)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			MjesecniModel mjesecniModel = new MjesecniModel();
			ViewBag.Title = "Pregled mjesecnih planova";
			ViewBag.CurrentSortOrder = Sort;
			ViewBag.SortingName = String.IsNullOrEmpty(Sort) ? "Naziv" : "";

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
				//var Popis = mjesecni_planovi.ReadMjesecnePlanove().ToPagedList(No_Of_Page, Size_Of_Page);
				string AkGodina = "";
				int idPlan = 0;
				mjesecniModel.GodisnjiPlanovi = new List<SelectListItem>(godisnji_planovi.ReadGodisnjePlanove().Select(i => new SelectListItem()
				{
					Text = i.Ak_godina,
					Value = i.Id_god.ToString()
				}));

				if (Plan != null)
				{
					Godisnji_plan plan = godisnji_planovi.ReadGodisnjiPlan((Int32.Parse(Plan)));
					AkGodina = plan.Ak_godina;
					idPlan = mjesecniModel.GodisnjiPlanovi.FindIndex(i => i.Text == AkGodina);
				}
				else
				{
					AkGodina = mjesecniModel.GodisnjiPlanovi.ElementAt(0).Text;
				}

				mjesecniModel.MjesecniPlanovi = mjesecni_planovi.ReadMjesecnePlanove(AkGodina).ToPagedList(No_Of_Page, Size_Of_Page).ToList();

				mjesecniModel.GodisnjiPlanovi.ElementAt(idPlan).Selected = true;
				return View(mjesecniModel);
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
				TempData["alert"] = "<script>alert('Mjesecni plan nije promjenjen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Mjesecni plan je uspjesno promjenjen!');</script>";
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
				TempData["alert"] = "<script>alert('Mjesecni plan nije obrisan, dogodila se greska!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Mjesecni plan je uspjesno obrisan!');</script>";
			}
			return RedirectToAction("Index");
		}

        public FileStreamResult Ispis()
        {
            List<Mjesecni_plan> planovi = mjesecni_planovi.ReadMjesecnePlanove();

            MjesecniPlanReport report = new MjesecniPlanReport(planovi);

            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
        }
	}
}
