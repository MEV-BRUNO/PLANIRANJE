using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class MjesecniPlanController : Controller
	{
		private Mjesecni_plan_DBHandle mjesecni_planovi = new Mjesecni_plan_DBHandle();

		// GET: MjesecniPlan
		public ActionResult Index()
		{

			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				var mjesecni_p = mjesecni_planovi.DohvatiMjesecnePlanove().ToList();
				ViewBag.Title = "Mjesecni plan";
				return View(mjesecni_p);
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
    }
}