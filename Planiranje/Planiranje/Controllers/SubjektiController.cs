using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class SubjektiController : Controller
    {
        // GET: Subjekti
        public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Subjekti";
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
    }
}