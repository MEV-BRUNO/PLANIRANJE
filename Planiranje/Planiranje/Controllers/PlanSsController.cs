using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Planiranje.Models;
using Planiranje.Reports;

namespace Planiranje.Controllers
{
    public class PlanSsController : Controller
    {

		private Mjesecni_plan_DBHandle mjesecni_planovi = new Mjesecni_plan_DBHandle();
		private Godisnji_plan_DBHandle godisnji_planovi = new Godisnji_plan_DBHandle();
		private Podrucje_rada_DBHandle podrucja_rada = new Podrucje_rada_DBHandle();
		private Subjekt_DBHandle subjekti = new Subjekt_DBHandle();
		private Aktivnost_DBHandle aktivnosti = new Aktivnost_DBHandle();
		private Zadaci_DBHandle zadaci = new Zadaci_DBHandle();
		private SS_Plan_DBHandle planovi_ss = new SS_Plan_DBHandle();
		private Oblici_DBHandle oblici = new Oblici_DBHandle();
		private Ciljevi_DBHandle ciljevi = new Ciljevi_DBHandle();

		public ActionResult Index(string Plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled srednjih skola";

			SSModel model = new SSModel();
			model.GodisnjiPlanovi = new List<SelectListItem>(godisnji_planovi.ReadGodisnjePlanove().Select(i => new SelectListItem()
			{
				Text = i.Ak_godina,
				Value = i.Id_god.ToString()
			}));

			int idPlan = 0;
			if (Plan != null)
			{
				idPlan = model.GodisnjiPlanovi.FindIndex(x => x.Value == Plan);
				model.ID_GODINA = Convert.ToInt32(model.GodisnjiPlanovi.ElementAt(idPlan).Value.ToString());
				ViewBag.HasGodPlan = true;
			}
			else
			{
				if (model.GodisnjiPlanovi.Count < 1)
				{
					ViewBag.HasGodPlan = false;
				}
				else
				{
					model.ID_GODINA = Convert.ToInt32(model.GodisnjiPlanovi.ElementAt(0).Value.ToString());
					idPlan = 0;
					ViewBag.HasGodPlan = true;
				}
			}
			if (model.GodisnjiPlanovi.Count > 0)
			{
				model.GodisnjiPlanovi.ElementAt(idPlan).Selected = true;
			}
			model.SS_Planovi = planovi_ss.ReadSSPlanove(model.ID_GODINA);
			return View("Index", model);
		}

        public ActionResult NoviPlan(int id_god)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			SSModel model = new SSModel();
			model.ID_GODINA = id_god;
			model.SS_Plan = new SS_Plan();
            if (Request.IsAjaxRequest())
            {
                return View("NoviPlan", model);
            }
            return View("NoviPlan");
        }

        [HttpPost]
        public ActionResult NoviPlan(SSModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SS_Plan ss_plan = new SS_Plan();
            ss_plan.Id_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            ss_plan.Id_godina = model.ID_GODINA;
            ss_plan.Naziv = model.SS_Plan.Naziv;
            ss_plan.Opis = model.SS_Plan.Opis;

			if (ss_plan.Naziv != null && ss_plan.Opis != null && planovi_ss.CreateSSPlan(ss_plan))
			{
				return RedirectToAction("Index", new { Plan = model.ID_GODINA });
			}
			model.SS_Plan = ss_plan;
			return View("NoviPlan", model);
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
			SSModel model = new SSModel();
			model.SS_Plan = planovi_ss.ReadSSPlan(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", model);
            }
            return View("Uredi");
        }
        [HttpPost]
        public ActionResult Edit(SSModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }

			if (model.SS_Plan.Naziv != null && model.SS_Plan.Opis != null && planovi_ss.UpdateSSPlan(model.SS_Plan))
			{
				return RedirectToAction("Index", new { Plan = model.SS_Plan.Id_godina });
			}
			return PartialView("Uredi", model);
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SS_Plan ss_plan = new SS_Plan();
            ss_plan = planovi_ss.ReadSSPlan(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", ss_plan);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(SS_Plan ss_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!planovi_ss.DeleteSSPlan(ss_plan.Id_plan))
			{
				TempData["alert"] = "<script>alert('Plan nije obrisan, dogodila se greska!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Plan je uspjesno obrisan!');</script>";
			}
			return RedirectToAction("Index");
		}

		public FileStreamResult Ispis(int id)
		{
			List<SS_Plan_podrucje> podrucja = planovi_ss.ReadSsPodrucja(id);

			PlanSsPodrucjaReport report = new PlanSsPodrucjaReport(podrucja);

			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}

		public ActionResult Detalji(int id, int id_god)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			SSModel model = new SSModel();
			model.SS_Podrucja = planovi_ss.ReadSsPodrucja(id);
			model.Ak_godina = godisnji_planovi.ReadGodisnjiPlan(id_god).Ak_godina;
			model.ID_PLAN = id;
			model.ID_GODINA = id_god;
			return View("Detalji", model);
		}
		[HttpPost]
		public ActionResult Detalji(Mjesecni_plan mjesecni_plan)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			if (!mjesecni_planovi.UpdateMjesecniPlan(mjesecni_plan))
			{
				TempData["alert"] = "<script>alert('Mjesecni plan nije promjenjen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Mjesecni plan je uspjesno promjenjen!');</script>";
			}
			return RedirectToAction("Detalji");
		}

		public ActionResult NoviDetalji(int id, int id_god)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			SSModel model = new SSModel();
			model.PodrucjaDjelovanja = new List<SelectListItem>(podrucja_rada.ReadPodrucjeRada().Select(i => new SelectListItem()
			{
				Text = i.Naziv,
				Value = i.Id_podrucje.ToString()
			}));
			model.Zadace = new List<SelectListItem>(zadaci.ReadZadaci().Select(i => new SelectListItem()
			{
				Text = i.Naziv,
				Value = i.ID_zadatak.ToString()
			}));
			model.Oblici = new List<SelectListItem>(oblici.ReadOblici().Select(i => new SelectListItem()
			{
				Text = i.Naziv,
				Value = i.Id_oblici.ToString()
			}));

			model.Suradnici = new List<SelectListItem>(subjekti.ReadSubjekti().Select(i => new SelectListItem()
			{
				Text = i.Naziv,
				Value = i.ID_subjekt.ToString()
			}));

			model.Ciljevi = new List<SelectListItem>(ciljevi.ReadCiljevi().Select(i => new SelectListItem()
			{
				Text = i.Naziv,
				Value = i.ID_cilj.ToString()
			}));

			model.ID_GODINA = id_god;
			model.ID_PLAN = id;

			return PartialView("NoviDetalji", model);
		}
		[HttpPost]
		public ActionResult NoviDetalji(SSModel model)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}

			if (model.SS_Plan_Podrucje.Ishodi == null ||
				model.SS_Plan_Podrucje.Mjesto == null ||
				model.SS_Plan_Podrucje.Oblici == null ||
				model.SS_Plan_Podrucje.Opis_podrucje == null ||
				model.SS_Plan_Podrucje.Sadrzaj == null ||
				model.SS_Plan_Podrucje.Sati < 1 ||
				model.SS_Plan_Podrucje.Suradnici == null ||
				model.SS_Plan_Podrucje.Svrha == null ||
				model.SS_Plan_Podrucje.Vrijeme <= DateTime.Now ||
				model.SS_Plan_Podrucje.Zadaca == null ||
				!planovi_ss.CreateSSPlanPodrucje(model))
			{
				model.PodrucjaDjelovanja = new List<SelectListItem>(podrucja_rada.ReadPodrucjeRada().Select(i => new SelectListItem()
				{
					Text = i.Naziv,
					Value = i.Id_podrucje.ToString()
				}));
				model.Zadace = new List<SelectListItem>(zadaci.ReadZadaci().Select(i => new SelectListItem()
				{
					Text = i.Naziv,
					Value = i.ID_zadatak.ToString()
				}));
				model.Oblici = new List<SelectListItem>(oblici.ReadOblici().Select(i => new SelectListItem()
				{
					Text = i.Naziv,
					Value = i.Id_oblici.ToString()
				}));

				model.Suradnici = new List<SelectListItem>(subjekti.ReadSubjekti().Select(i => new SelectListItem()
				{
					Text = i.Naziv,
					Value = i.ID_subjekt.ToString()
				}));

				model.Ciljevi = new List<SelectListItem>(ciljevi.ReadCiljevi().Select(i => new SelectListItem()
				{
					Text = i.Naziv,
					Value = i.ID_cilj.ToString()
				}));
				return PartialView("NoviDetalji", model);
			}
			else
			{
				return RedirectToAction("Detalji", new { id = model.ID_PLAN, id_god = model.ID_GODINA });
			}
		}
		
		public ActionResult CancelNewDetails(int id_godina, int id_plan)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			SSModel model = new SSModel();
			model.ID_GODINA = id_godina;
			model.ID_PLAN = id_plan;
			return View("ObrisiNoviDetalj", model);
		}
		[HttpPost]
		public ActionResult CancelNewDetails(SSModel model)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			return RedirectToAction("Detalji", new { id = model.ID_PLAN,  id_god = model.ID_GODINA });
		}
		public ActionResult DeleteDetails(int id, int id_plan, int id_god)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			SSModel model = new SSModel();
			model.ID_GODINA = id_god;
			model.ID_PLAN = id_plan;
			model.SS_Plan_Podrucje = planovi_ss.ReadSsPodrucje(id);
			if (Request.IsAjaxRequest())
			{
				return View("ObrisiDetalje", model);
			}
			return View("Obrisi");
		}

		[HttpPost]
		public ActionResult DeleteDetails(SSModel model)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			if (!planovi_ss.DeleteSSPlanPodrucje(model.SS_Plan_Podrucje.Id))
			{
				TempData["alert"] = "<script>alert('Plan nije obrisan, dogodila se greska!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Plan je uspjesno obrisan!');</script>";
			}
			return RedirectToAction("Detalji", new { id = model.ID_PLAN, id_god = model.ID_GODINA });
		}
	}
}