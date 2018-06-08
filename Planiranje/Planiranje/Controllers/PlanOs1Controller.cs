using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class PlanOs1Controller : Controller
    {
        private OS_Plan_1_DBHandle os_plan_1 = new OS_Plan_1_DBHandle();

        

        // GET: PlanOs1
        public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
                var os_p_1 = os_plan_1.DohvatiOS_Plan_1().ToList();
                ViewBag.Title = "OS_Plan_1";
                return View(os_p_1);
            }
			return RedirectToAction("Prijava");
		}
    }
}