﻿using System;
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
    public class MjesecniPlanController : Controller
	{
		private Mjesecni_plan_DBHandle mjesecni_planovi = new Mjesecni_plan_DBHandle();
		private Godisnji_plan_DBHandle godisnji_planovi = new Godisnji_plan_DBHandle();
		private Podrucje_rada_DBHandle podrucja_rada = new Podrucje_rada_DBHandle();
		private Subjekt_DBHandle subjekti = new Subjekt_DBHandle();
		private Aktivnost_DBHandle aktivnosti = new Aktivnost_DBHandle();
		int Page_No_Master = 1;
		private int tmpId;

		public ActionResult Index(string Sort, string Search, string Plan, string Filter, int? Page_No)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			MjesecniModel mjesecniModel = new MjesecniModel();
			ViewBag.Title = "Pregled mjesečnih planova";
			ViewBag.CurrentSortOrder = Sort;
			ViewBag.SortingName = String.IsNullOrEmpty(Sort) ? "Naziv" : "";

			ViewBag.FilterValue = Search;
			if (Search != null)
			{
				Page_No = 1;
			}
			else
			{
				Search = Filter;
			}
			ViewBag.CurrentPage = 1;
			if (Page_No != null)
				ViewBag.CurrentPage = Page_No;

			
			int Size_Of_Page = 10;
			int No_Of_Page = (Page_No ?? 1);
			if (Search == null || Search.Length == 0)
			{
				Page_No_Master = No_Of_Page;
				int idPlan = 0;
				mjesecniModel.GodisnjiPlanovi = new List<SelectListItem>(godisnji_planovi.ReadGodisnjePlanove().Select(i => new SelectListItem()
				{
					Text = i.Ak_godina,
					Value = i.Id_god.ToString()
				}));

				if (Plan != null)
				{
					if (Plan == "0")
					{
						idPlan = 1;
					}
					Godisnji_plan plan = godisnji_planovi.ReadGodisnjiPlan((Int32.Parse(Plan)));
					int pln;
					Int32.TryParse(Plan, out pln);
					mjesecniModel.ID_GODINA = pln;
					idPlan = plan.Id_god;
				}
				else
				{
					idPlan = Convert.ToInt32(godisnji_planovi.ReadGodisnjiPlan(Convert.ToInt32(mjesecniModel.GodisnjiPlanovi.ElementAt(0).Value)).Id_god);
					mjesecniModel.ID_GODINA = idPlan;
				}


				mjesecniModel.MjesecniPlanovi = mjesecni_planovi.ReadMjesecnePlanove(idPlan).ToPagedList(No_Of_Page, Size_Of_Page).ToList();

				mjesecniModel.GodisnjiPlanovi.ElementAt(idPlan - 1).Selected = true;
				return View(mjesecniModel);
			}
			else
			{
				Page_No_Master = No_Of_Page;
				var Popis = mjesecni_planovi.ReadMjesecnePlanove(Search).ToPagedList(No_Of_Page, Size_Of_Page);

				return View(Popis);
			}
		}

		public ActionResult NoviPlan(int id_godina)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			ViewBag.Title = "Novi mjesečni plan";
			MjesecniModel mjesecniModel = new MjesecniModel();
			mjesecniModel.MjesecniPlan = new Mjesecni_plan();
			mjesecniModel.ID_GODINA = id_godina;
			mjesecniModel.GodisnjiPlanovi = new List<SelectListItem>(godisnji_planovi.ReadGodisnjePlanove().Select(i => new SelectListItem()
			{
				Text = i.Ak_godina,
				Value = i.Id_god.ToString()
			}));
			
			return PartialView("NoviPlan", mjesecniModel);
		}

		[HttpPost]
		public ActionResult NoviPlan(MjesecniModel _mjesecni_model)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			Mjesecni_plan mjesecni_plan = new Mjesecni_plan();
			mjesecni_plan.ID_pedagog = PlaniranjeSession.Trenutni.PedagogId;
			mjesecni_plan.Id_godina = _mjesecni_model.ID_GODINA;
			mjesecni_plan.Naziv = _mjesecni_model.MjesecniPlan.Naziv;
			mjesecni_plan.Opis = _mjesecni_model.MjesecniPlan.Opis;

			if (mjesecni_planovi.CreateMjesecniPlan(mjesecni_plan))
			{
				TempData["alert"] = "<script>alert('Novi mjesecni plan je uspjesno spremljen!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Novi mjesecni plan nije spremljen');</script>";
			}
			return RedirectToAction("Index", new { Plan = _mjesecni_model.ID_GODINA });
		}

		public ActionResult Detalji (int id, int id_god)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			tmpId = id;
			MjesecniModel model = new MjesecniModel();
			model.MjesecniPlan = mjesecni_planovi.ReadMjesecniPlan(id);
			model.MjesecniDetalji = mjesecni_planovi.ReadMjesecneDetalje(id);
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
		
		public ActionResult Delete(int id)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			Mjesecni_plan mjesecni_plan = new Mjesecni_plan();
			mjesecni_plan = mjesecni_planovi.ReadMjesecniPlan(id);
			if (Request.IsAjaxRequest())
			{
				ViewBag.IsUpdate = false;
				return View("Obrisi", mjesecni_plan);
			}
			return View("Obrisi");
		}

		[HttpPost]
		public ActionResult Delete(Mjesecni_plan mjesecni_plan)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			if (!mjesecni_planovi.DeleteMjesecniPlan(mjesecni_plan.ID_plan))
			{
				TempData["alert"] = "<script>alert('Mjesecni plan nije obrisan, dogodila se greska!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Mjesecni plan je uspjesno obrisan!');</script>";
			}
			return RedirectToAction("Detalji");
		}

        public FileStreamResult Ispis()
        {
            List<Mjesecni_plan> planovi = mjesecni_planovi.ReadMjesecnePlanove();

            MjesecniPlanReport report = new MjesecniPlanReport(planovi);

            return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}

		public ActionResult NoviDetalji(int id, int id_god)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			ViewBag.Title = "Novi detalji mjesečnog plana";
			MjesecniModel mjesecniModel = new MjesecniModel();
			mjesecniModel.MjesecniPlan = new Mjesecni_plan();
			mjesecniModel.PodrucjaRada = new List<SelectListItem>(podrucja_rada.ReadPodrucjeRada().Select(i => new SelectListItem()
			{
				Text = i.Naziv.ToString(),
				Value = i.Id_podrucje.ToString()
			}));
			mjesecniModel.Aktivnosti = new List<SelectListItem>(aktivnosti.ReadAktivnost().Select(i => new SelectListItem()
			{
				Text = i.Naziv.ToString(),
				Value = i.Id_aktivnost.ToString()
			}));
			mjesecniModel.Subjekti = new List<SelectListItem>(subjekti.ReadSubjekti().Select(i => new SelectListItem()
			{
				Text = i.Naziv.ToString(),
				Value = i.ID_subjekt.ToString()
			}));
			mjesecniModel.GodisnjiPlanovi = new List<SelectListItem>(godisnji_planovi.ReadGodisnjePlanove().Select(i => new SelectListItem()
			{
				Text = i.Ak_godina,
				Value = i.Id_god.ToString()
			}));
			mjesecniModel.MjesecniPlan = mjesecni_planovi.ReadMjesecniPlan(id);
			mjesecniModel.MjesecniDetalji = mjesecni_planovi.ReadMjesecneDetalje(id);
			mjesecniModel.ID_PLAN = id;
			mjesecniModel.ID_GODINA = id_god;

			return View("NoviDetalji", mjesecniModel);
		}

		[HttpPost]
		public ActionResult NoviDetalji(MjesecniModel _mjesecni_model)
		{
			_mjesecni_model.mjesecniDetalj.ID_plan = _mjesecni_model.ID_PLAN;
			if (mjesecni_planovi.CreateMjesecniDetalj(_mjesecni_model.mjesecniDetalj))
			{
				TempData["alert"] = "<script>alert('Mjesecni detalj je dodan!');</script>";
				_mjesecni_model.MjesecniDetalji = mjesecni_planovi.ReadMjesecneDetalje(_mjesecni_model.ID_PLAN);
				return View("Detalji", _mjesecni_model);
			}
			else
			{
				//TempData["alert"] = "<script>alert('Mjesecni detalj nije dodan, dogodila se greska!');</script>";
				_mjesecni_model.PodrucjaRada = new List<SelectListItem>(podrucja_rada.ReadPodrucjeRada().Select(i => new SelectListItem()
				{
					Text = i.Naziv.ToString(),
					Value = i.Id_podrucje.ToString()
				}));
				_mjesecni_model.Aktivnosti = new List<SelectListItem>(aktivnosti.ReadAktivnost().Select(i => new SelectListItem()
				{
					Text = i.Naziv.ToString(),
					Value = i.Id_aktivnost.ToString()
				}));
				_mjesecni_model.Subjekti = new List<SelectListItem>(subjekti.ReadSubjekti().Select(i => new SelectListItem()
				{
					Text = i.Naziv.ToString(),
					Value = i.ID_subjekt.ToString()
				}));
				_mjesecni_model.GodisnjiPlanovi = new List<SelectListItem>(godisnji_planovi.ReadGodisnjePlanove().Select(i => new SelectListItem()
				{
					Text = i.Ak_godina,
					Value = i.Id_god.ToString()
				}));
				return View("NoviDetalji", _mjesecni_model);
			}
		}

		public ActionResult DeleteDetails(int id, int id_god)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			MjesecniModel model = new MjesecniModel();
			//model.ID_PLAN = 2;
			model.mjesecniDetalj = mjesecni_planovi.ReadMjesecniDetalj(id);
			model.ID_GODINA = id_god;
			if (Request.IsAjaxRequest())
			{
				ViewBag.IsUpdate = false;
				return View("ObrisiDetalj", model);
			}
			return View("ObrisiDetalj");
		}

		[HttpPost]
		public ActionResult DeleteDetails(MjesecniModel model)
		{
			if (PlaniranjeSession.Trenutni.PedagogId <= 0)
			{
				return RedirectToAction("Index", "Planiranje");
			}
			if (!mjesecni_planovi.DeleteMjesecniDetalj(model.mjesecniDetalj.ID))
			{
				TempData["alert"] = "<script>alert('Mjesecni plan nije obrisan, dogodila se greska!');</script>";
			}
			else
			{
				TempData["alert"] = "<script>alert('Mjesecni plan je uspjesno obrisan!');</script>";
			}
			//MjesecniModel model = new MjesecniModel();
			model.MjesecniPlan = mjesecni_planovi.ReadMjesecniPlan(model.mjesecniDetalj.ID_plan);
			model.MjesecniDetalji = mjesecni_planovi.ReadMjesecneDetalje(model.mjesecniDetalj.ID_plan);
			model.ID_PLAN = model.mjesecniDetalj.ID_plan;
			return View("Detalji", model);
		}
	}
}
