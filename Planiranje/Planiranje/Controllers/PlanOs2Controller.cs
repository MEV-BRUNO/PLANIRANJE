using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class PlanOs1Controller : Controller
    {
        private OS_Plan_2_DBHandle os_plan_2 = new OS_Plan_2_DBHandle();



        // GET: PlanOs2
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId > 0)
            {
                var os_p_2 = os_plan_1.DohvatiOS_Plan_2().ToList();
                ViewBag.Title = "OS_Plan_2";
                return View(os_p_2);
            }
            return RedirectToAction("Prijava");
        }
    }
}