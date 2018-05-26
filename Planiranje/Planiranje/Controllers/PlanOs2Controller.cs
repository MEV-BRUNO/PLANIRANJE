using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class PlanOs2Controller : Controller
    {
        // GET: PlanOs2
        public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Plan - osnovna skola 1";
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
    }
}