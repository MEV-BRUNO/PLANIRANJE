using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class CiljeviController : Controller
    {
        // GET: Ciljevi
        public ActionResult Index()
        {
			if (PlaniranjeSession.Trenutni.PedagogId > 0)
			{
				ViewBag.Title = "Ciljevi";
				return View();
			}
			return RedirectToAction("Prijava");
		}
    }
}