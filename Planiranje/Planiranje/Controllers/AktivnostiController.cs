using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class AktivnostiController : Controller
    {
        // GET: Aktivnosti
        public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Aktivnosti";
				return View();
			}
			return RedirectToAction("Prijava");
		}
    }
}