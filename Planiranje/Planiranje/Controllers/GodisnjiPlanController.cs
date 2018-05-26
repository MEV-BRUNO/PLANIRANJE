using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class GodisnjiPlanController : Controller
	{
		// GET: GodisnjiPlan
		public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
    }
}