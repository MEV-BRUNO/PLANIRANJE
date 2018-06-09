using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Planiranje.Models;

namespace Planiranje.Controllers
{
    public class PlanSsController : Controller

    {
        // GET: PlanSs
        public ActionResult Index()
        {
            if (PlaniranjeSession.Trenutni.PedagogId > 0)
            {
                ViewBag.Title = "Plan - srednja skola";
                return View();
            }
            return RedirectToAction("Prijava");
        }
    }

    private SS_Plan_DBHandle SS_Plan = new SS_Plan_DBHandle();
        int Page_No_Master = 1;

        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled srednjih skola";
            ViewBag.CurrentSortOrder = Sort;
            ViewBag.SortingName = String.IsNullOrEmpty(Sort) ? "Naziv" : "";

            ViewBag.Message = "Grad";

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

                if (Request.IsAjaxRequest())
                {
                    int noP = (int)Page_No_Master;
                    var Popis2 = Ss_Plan.ReadSSPlan().ToPagedList(No_Of_Page, Size_Of_Page);
                    return PartialView("_GradView", Popis2);
                }
                Page_No_Master = No_Of_Page;
                var Popis = Ss_Plan.ReadSSPlan().ToPagedList(No_Of_Page, Size_Of_Page);
                return View(Popis);
            }
            else
            {
                Page_No_Master = No_Of_Page;
                var Popis = Ss_Plan.ReadSSPlan(Search).ToPagedList(No_Of_Page, Size_Of_Page);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_GradView", Popis);
                }

                return View(Popis);
            }
        }

        public ActionResult NoviPlan()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("NoviPlan");
            }
            return View("NoviPlan");
        }

        [HttpPost]
        public ActionResult NoviPlan(Ss_plan gr)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SS_Plan ss_plan = new Ss_plan();
            ss_plan.ID_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            ss_plan.Ak_godina = gr.Ak_godina;
            ss_plan.Naziv = gr.Naziv;
            ss_plan.Opis = gr.Opis;
            if (!ss_plan.CreateSsPlan(ss_plan))
            {
                ModelState.Clear();
                return PartialView("NoviPlan", gr);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Ss_Plan ss_plan = new SS_Plan();
            ss_plan = ss_planovi.ReadSSPlan(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", ss_plan);
            }
            return View("Uredi", ss_plan);
        }
        [HttpPost]
        public ActionResult Edit(SS_Plan ss_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!ss_planovi.UpdateSSPlan(ss_plan))
            {
                ModelState.Clear();
                return PartialView("Uredi", ss_plan);
            }
            if (Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            SS_Pla nss_plan = new SS_Plan();
            ss_plan = ss_planovi.ReadSSPlan(id);
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
            if (!ss_planovi.DeleteSSPlan(ss_plan.ID_plan))
            {
                ModelState.Clear();
                return PartialView("Obrisi");
            }
            if (Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            return RedirectToAction("Index");
        }
    }
}