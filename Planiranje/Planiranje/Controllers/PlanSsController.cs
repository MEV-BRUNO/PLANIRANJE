using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class PlanSsController : Controller
    {
        // GET: PlanSs
        public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Plan - srednja skola";
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
    }
}