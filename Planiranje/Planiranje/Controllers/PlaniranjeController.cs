using System;
using System.Linq;
using System.Web.Mvc;
using Planiranje.Models;

namespace Planiranje.Controllers
{
	public class PlaniranjeController : Controller
    {
		private BazaPodataka baza = new BazaPodataka();
		private Mjesecni_plan_DBHandle mjesecni_planovi = new Mjesecni_plan_DBHandle();

		[HttpGet]
		public ActionResult Prijava()
		{
			Pedagog ped = new Pedagog();
			return View(ped);
		}

		[HttpPost]
		public ActionResult Prijava(Pedagog p)
		{
			Pedagog pedagog = baza.Pedagog.SingleOrDefault(ped => ped.Email == p.Email && ped.Lozinka == p.Lozinka);
			if (pedagog != null)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return View("Prijava");
			}
		}
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult ZaboravljenaLozinka()
		{
			return View();
		}
		public ActionResult Registracija()
		{
			return View();
		}
		public ActionResult MjesecniPlan()
		{
			/*ViewBag.CurrentSortOrder = Sorting_Order;
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
			}*/

			// List<Grad> Popis = gradDBHandle.GetGradove().ToList();
			/*int Size_Of_Page = 4;
			int No_Of_Page = (Page_No ?? 1);
			if (Search_Data == null || Search_Data.Length == 0)
			{
				var Popis = gradDBHandle.GetGradove().ToPagedList(No_Of_Page, Size_Of_Page);
				return View(Popis);
			}
			else
			{*/
			var mjesecni_p = mjesecni_planovi.DohvatiMjesecnePlanove().ToList();//  .GetGradove_2(Search_Data).ToPagedList(No_Of_Page, Size_Of_Page);
				return View(mjesecni_p);
			//}
		}
		public ActionResult GodisnjiPlan()
		{
			return View();
		}
		public ActionResult PodrucjaDjelovanja()
		{
			return View();
		}
		public ActionResult Aktivnosti()
		{
			return View();
		}
		public ActionResult Akcije()
		{
			return View();
		}
		public ActionResult ObliciMetode()
		{
			return View();
		}
		public ActionResult Subjekti()
		{
			return View();
		}
		public ActionResult Zadaci()
		{
			return View();
		}
		public ActionResult Ciljevi()
		{
			return View();
		}
	}
}