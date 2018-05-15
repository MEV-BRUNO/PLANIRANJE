using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planiranje.Controllers
{
    public class PlaniranjeController : Controller
    {
		// GET: Planiranje
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Prijava()
		{
			return View();
		}
	}
}