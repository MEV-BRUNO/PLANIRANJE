using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Planiranje.Models;
using Planiranje.Reports;

namespace Planiranje.Controllers
{
	public class GodisnjiPlanController : Controller
	{
		private Godisnji_plan_DBHandle godisnji_planovi = new Godisnji_plan_DBHandle();

		private List<KeyValuePair<int, String>> mjeseci = new List<KeyValuePair<int, String>>() {
			new KeyValuePair<int, String>(9, "Rujan"),
			new KeyValuePair<int, String>(10, "Listopad"),
			new KeyValuePair<int, String>(11, "Studeni"),
			new KeyValuePair<int, String>(12, "Prosinac"),
			new KeyValuePair<int, String>(1, "Siječanj"),
			new KeyValuePair<int, String>(2, "Veljača"),
			new KeyValuePair<int, String>(3, "Ožujak"),
			new KeyValuePair<int, String>(4, "Travanj"),
			new KeyValuePair<int, String>(5, "Svibanj"),
			new KeyValuePair<int, String>(6, "Lipanj"),
			new KeyValuePair<int, String>(7, "Srpanj"),
			new KeyValuePair<int, String>(8, "Kolovoz"),
		};

        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled godišnjih planova";
			List<Godisnji_plan> god_planovi = godisnji_planovi.ReadGodisnjePlanove();
			return View(god_planovi);

		}

		// NOVI PLAN
		public ActionResult NoviPlan()
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			GodisnjiModel model = new GodisnjiModel();
			model.GodisnjiDetalji = new List<Godisnji_detalji>();
			for (int i = 0; i < 12; i++)
			{
				model.GodisnjiDetalji.Add(new Godisnji_detalji{
					Radnih_dana = 0,
					Subota_dana = 0,
					Nedjelja_dana = 0,
					Blagdana_dana = 0,
					Nastavnih_dana = 0,
					Praznika_dana = 0,
					Odmor_dana = 0
				});
			}
			ViewBag.Mjeseci = mjeseci;
			ViewBag.Title = "Novi godišnji plan";
			return View("NoviPlan", model);
		}

		[HttpPost]
        public ActionResult NoviPlan(GodisnjiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }

			if (model.GodisnjiPlan.Ak_godina == null)
			{
				ViewBag.Mjeseci = mjeseci;
				ViewBag.Title = "Novi godišnji plan";
				return View("NoviPlan", model);
			}
			else
			{
				godisnji_planovi.CreateGodisnjiPlan(model);
				return RedirectToAction("Index");
			}
        }

		// UREĐIVANJE
        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			GodisnjiModel detalji = godisnji_planovi.ReadGodisnjiDetalji(id);
			ViewBag.Mjeseci = mjeseci;
			ViewBag.Title = "Uredi godišnji plan " + detalji.GodisnjiPlan.Ak_godina;
			return View("Uredi", detalji);
        }

        [HttpPost]
        public ActionResult Edit(GodisnjiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			
			if (model.GodisnjiPlan.Ak_godina == null)
			{
				ViewBag.Mjeseci = mjeseci;
				ViewBag.Title = "Uredi godišnji plan " + model.GodisnjiPlan.Ak_godina;
				return View("Uredi", model);
			}

			if (!godisnji_planovi.UpdateGodisnjiPlan(model))
			{
				TempData["alert"] = "<script>alert('Godišnji plan nije promjenjen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Godišnji plan je uspješno promjenjen!');</script>";
			}
			return RedirectToAction("Index");
        }

		// BRISANJE
        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Godisnji_plan godisnji_plan = new Godisnji_plan();
            godisnji_plan = godisnji_planovi.ReadGodisnjiPlan(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return PartialView("Obrisi", godisnji_plan);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(Godisnji_plan godisnji_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            
            if (!godisnji_planovi.DeleteGodisnjiPlan(godisnji_plan.Id_god))
			{
				TempData["alert"] = "<script>alert('Godišnji plan nije obrisan!');</script>";
			}
			else
			{
                //TempData["alert"] = "<script>alert('Godišnji plan je obrisan!');</script>";
			}
			return RedirectToAction("Index");
		}

		// DETALJI
        public ActionResult Details(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			GodisnjiModel detalji = godisnji_planovi.ReadGodisnjiDetalji(id);
			ViewBag.Title = "Detalji " + detalji.GodisnjiPlan.Ak_godina.ToString();
			return View("Detalji", detalji);
		}

		// ISPIS
		public FileStreamResult Ispis(int id)
		{
			GodisnjiReport report = new GodisnjiReport(godisnji_planovi.ReadGodisnjiDetalji(id));
			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}
	}
}