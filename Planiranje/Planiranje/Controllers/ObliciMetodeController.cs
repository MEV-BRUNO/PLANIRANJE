using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class ObliciMetodeController : Controller
    {
        // GET: ObliciMetode
        public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Oblici i metode";
				return View();
			}
			return RedirectToAction("Prijava");
		}
    }
}