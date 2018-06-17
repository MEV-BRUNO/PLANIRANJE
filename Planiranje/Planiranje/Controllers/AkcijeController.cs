using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class AkcijeController : Controller
    {
        // GET: Akcije
        public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Akcije";
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
    }
}