﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
        private BazaPodataka baza = new BazaPodataka();

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

		// INDEX
        public ActionResult Index()
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
			if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
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
            model.SkolskaGodina = new List<Sk_godina>();
            model.SkolskaGodina = baza.SkolskaGodina.ToList();
			return View("NoviPlan", model);
		}

		[HttpPost]
        public ActionResult NoviPlan(GodisnjiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.GodisnjiPlan.Ak_godina == 0 || model.GodisnjiPlan.Naziv == null)
            {
                model.GodisnjiDetalji = new List<Godisnji_detalji>();
                for (int i = 0; i < 12; i++)
                {
                    model.GodisnjiDetalji.Add(new Godisnji_detalji
                    {
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
                model.SkolskaGodina = baza.SkolskaGodina.ToList();
                return View("NoviPlan", model);
            }
			if (!godisnji_planovi.CreateGodisnjiPlan(model))
			{
				TempData["poruka"] = "Nije moguće spremiti, dogodila se greška!";
                return RedirectToAction("Index");
            }
			else
			{
                TempData["poruka"] = "Plan je spremljen!";
				return RedirectToAction("Index");
			}			
		}

		// UREĐIVANJE
        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
			GodisnjiModel detalji = godisnji_planovi.ReadGodisnjiDetalji(id);            
			ViewBag.Mjeseci = mjeseci;
			ViewBag.Title = "Uredi godišnji plan";
            detalji.SkolskaGodina = new List<Sk_godina>();
            detalji.SkolskaGodina = baza.SkolskaGodina.ToList();
			return View("Uredi", detalji);
        }

        [HttpPost]
        public ActionResult Edit(GodisnjiModel model)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (model.GodisnjiPlan.Ak_godina == 0 || model.GodisnjiPlan.Naziv == null)
            {
                ViewBag.Mjeseci = mjeseci;
                ViewBag.Title = "Uredi godišnji plan";
                model.SkolskaGodina = baza.SkolskaGodina.ToList();
                return View("Uredi", model);
            }
            if (!godisnji_planovi.UpdateGodisnjiPlan(model))
			{
				TempData["poruka"] = "Nije moguće promijeniti, dogodila se greška!";
                return RedirectToAction("Index");
            }
			else
			{
                TempData["poruka"] = "Uspješno promijenjeno!";
				return RedirectToAction("Index");
			}			
		}

		// BRISANJE
        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
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
			return RedirectToAction("Index");
		}

        [HttpPost]
        public ActionResult Delete(Godisnji_plan godisnji_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            
            if (!godisnji_planovi.DeleteGodisnjiPlan(godisnji_plan.Id_god))
			{
				return PartialView("Obrisi", godisnji_plan);
			}
			else
			{
                TempData["poruka"] = "Godišnji plan je obrisan";
                return RedirectToAction("Index");
			}
		}

		// DETALJI
        public ActionResult Details(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
			GodisnjiModel detalji = godisnji_planovi.ReadGodisnjiDetalji(id);
			ViewBag.Title = "Detalji " + detalji.GodisnjiPlan.Ak_godina.ToString();
			return View("Detalji", detalji);
		}
        public ActionResult Kopiraj (int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Godisnji_plan plan = godisnji_planovi.ReadGodisnjiPlan(id);
            ViewBag.naziv = plan.Naziv;
            plan.Naziv = null;

            ViewBag.select = VratiSelectList();
            return View(plan);
        }
        [HttpPost]
        public ActionResult Kopiraj (Godisnji_plan plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if(string.IsNullOrWhiteSpace(plan.Naziv) || plan.Ak_godina <= 0)
            {
                ViewBag.select = VratiSelectList();
                Godisnji_plan pl = godisnji_planovi.ReadGodisnjiPlan(plan.Id_god);
                ViewBag.naziv = pl.Naziv;
                return View(plan);
            }
            int id = plan.Id_god;
            Godisnji_plan planA = godisnji_planovi.ReadGodisnjiPlan(id);
            if(planA==null || planA.Id_pedagog != PlaniranjeSession.Trenutni.PedagogId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            planA.Naziv = plan.Naziv;
            planA.Ak_godina = plan.Ak_godina;
            planA.Id_god = 0;
            GodisnjiModel model = new GodisnjiModel();
            model = godisnji_planovi.ReadGodisnjiDetalji(id);
            model.GodisnjiPlan = planA;
            if (!godisnji_planovi.CreateGodisnjiPlan(model))
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            TempData["poruka"] = "Godišnji plan je kopiran";
            return RedirectToAction("Index");
        }

		// ISPIS
		public FileStreamResult Ispis(int id)
		{
			GodisnjiReport report = new GodisnjiReport(godisnji_planovi.ReadGodisnjiDetalji(id));
			return new FileStreamResult(new MemoryStream(report.Podaci), "application/pdf");
		}
        private SelectList VratiSelectList()
        {
            List<Sk_godina> skGodina = baza.SkolskaGodina.ToList();
            var selectListItem = new List<SelectListItem>();
            foreach (var item in skGodina)
            {
                selectListItem.Add(new SelectListItem { Value = item.Sk_Godina.ToString(), Text = item.Sk_Godina + "./" + (item.Sk_Godina + 1).ToString() + "." });
            }
            var select = new SelectList(selectListItem, "Value", "Text");
            return select;
        }
	}
}