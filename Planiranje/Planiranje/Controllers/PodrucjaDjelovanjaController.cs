using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class PodrucjaDjelovanjaController : Controller
    {
        // GET: PodrucjaDjelovanja
        public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Podrucja djelovanja";
				return View();
			}
			return RedirectToAction("Prijava", "Planiranje");
		}
    }
}