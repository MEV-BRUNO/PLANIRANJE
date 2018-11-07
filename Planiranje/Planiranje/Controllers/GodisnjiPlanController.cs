using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
			if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled godišnjih planova";
			List<Godisnji_plan> god_planovi = godisnji_planovi.ReadGodisnjePlanove();
			return View(god_planovi);
		}


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
				return View("NoviPlan");
			}
			return RedirectToAction("NoviPlan", "GodisnjiPlan");
		}

		[HttpPost]
        public ActionResult NoviPlan(ViewModel model)
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
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
			ViewModel detalji = godisnji_planovi.ReadGodisnjiDetalji(id);
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
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
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
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Godisnji_plan godisnji_plan = new Godisnji_plan();
            godisnji_plan = godisnji_planovi.ReadGodisnjiPlan(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return PartialView("Obrisi", godisnji_plan);
            }
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Delete(Godisnji_plan godisnji_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            
            if (!godisnji_planovi.DeleteGodisnjiPlan(godisnji_plan.Id_god))
			{
				return PartialView("Obrisi", godisnji_plan);
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

		// DETALJI
        public ActionResult Details(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
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