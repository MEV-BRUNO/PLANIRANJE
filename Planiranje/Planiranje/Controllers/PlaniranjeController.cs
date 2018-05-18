using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class PlaniranjeController : Controller
    {
		private BazaPodataka baza = new BazaPodataka();

		[HttpGet]
		public ActionResult Prijava()
		{
			Pedagog ped = new Pedagog();
			return View(ped);
		}

		[HttpPost]
		public ActionResult Prijava(Pedagog p)
		{
			Pedagog pedagog = baza.Pedagog.SingleOrDefault(ped => ped.Email == p.Email && ped.Lozinka == p.Lozinka);
			if (pedagog != null)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return View("Prijava");
			}
		}
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult ZaboravljenaLozinka()
		{
			return View();
		}
		public ActionResult Registracija()
		{
			return View();
		}
		public ActionResult MjesecniPlan()
		{
			return View();
		}
		public ActionResult GodisnjiPlan()
		{
			return View();
		}
	}
}