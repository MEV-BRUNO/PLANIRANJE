using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models;

namespace Planiranje.Controllers
{
	public class PlaniranjeController : Controller
    {
		private BazaPodataka baza = new BazaPodataka();
		private Mjesecni_plan_DBHandle mjesecni_planovi = new Mjesecni_plan_DBHandle();

		[HttpGet]
		public ActionResult Prijava()
		{
			AppSession.Current.UserId = 0;
			return View();
		}

		[HttpPost]
		public ActionResult Prijava(Pedagog p)
		{
			Pedagog pedagog = baza.Pedagog.SingleOrDefault(ped => ped.Email == p.Email && ped.Lozinka == p.Lozinka);
			if (pedagog != null)
			{
				AppSession.Current.UserId = 1;
				return RedirectToAction("Index");
			}
			else
			{
				return View("Prijava");
			}
		}
		public ActionResult Index()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult ZaboravljenaLozinka()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult Registracija()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult MjesecniPlan()
		{
			if (AppSession.Current.UserId > 0)
			{
				var mjesecni_p = mjesecni_planovi.DohvatiMjesecnePlanove().ToList();
				return View(mjesecni_p);
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult GodisnjiPlan()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult PodrucjaDjelovanja()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult Aktivnosti()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult Akcije()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult ObliciMetode()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult Subjekti()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult Zadaci()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
		public ActionResult Ciljevi()
		{
			if (AppSession.Current.UserId > 0)
			{
				return View();
			}
			return RedirectToAction("Prijava");
		}
	}
	public class AppSession
	{
		public static AppSession Current
		{
			get
			{
				AppSession session = (AppSession)HttpContext.Current.Session["__Session__"];

				if (session == null)
				{
					session = new AppSession();
					HttpContext.Current.Session["__Session__"] = session;
				}
				return session;
			}
		}
		public int UserId { get; set; }
	}
}