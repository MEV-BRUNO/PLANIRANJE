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
    public class AkcijeController : Controller
    {
        private Aktivnost_akcija_DBHandle akcije_planovi = new Aktivnost_akcija_DBHandle();
        int Page_No_Master = 1;
        // GET: PodrucjaDjelovanja
		
        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled planova akcije";
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
                    var Popis2 = akcije_planovi.ReadAktivnostAkcija().ToPagedList(No_Of_Page, Size_Of_Page);
                    return PartialView("_GradView", Popis2);
                }
                Page_No_Master = No_Of_Page;
                var Popis = akcije_planovi.ReadAktivnostAkcija().ToPagedList(No_Of_Page, Size_Of_Page);
                return View(Popis);
            }
            else
            {
                Page_No_Master = No_Of_Page;
                var Popis = akcije_planovi.ReadAktivnostAkcija(Search).ToPagedList(No_Of_Page, Size_Of_Page);
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
        public ActionResult NoviPlan(Aktivnost_akcija akt)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Aktivnost_akcija akcija_plan = new Aktivnost_akcija();
            akcija_plan.Naziv = akt.Naziv;
            akcija_plan.Id_akcija = akt.Id_akcija;
            akcija_plan.Id_aktivnost = akt.Id_aktivnost;

            if (akcije_planovi.CreateAktivnostAkcija(akcija_plan))
            {
                TempData["alert"] = "<script>alert('Novi plan akcija je uspjesno spremljen!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Novi plan akcija nije spremljen');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Aktivnost_akcija akcija_plan = new Aktivnost_akcija();
            akcija_plan = akcije_planovi.ReadAktivnostAkcija(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", akcija_plan);
            }
            return View("Uredi", akcija_plan);
        }

        [HttpPost]
        public ActionResult Edit(Aktivnost_akcija akcija_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!akcije_planovi.UpdateAktivnostAkcija(akcija_plan))
            {
                TempData["alert"] = "<script>alert('Plan akcije nije promjenjen!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Plan akcije je uspjesno promjenjen!');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Aktivnost_akcija akcija_plan = new Aktivnost_akcija();
            akcija_plan = akcije_planovi.ReadAktivnostAkcija(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", akcija_plan);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(Aktivnost_akcija akcija_plan)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!akcije_planovi.DeleteAktivnostAkcija(akcija_plan.Id_akcija))
            {
                TempData["alert"] = "<script>alert('Plan akcije nije obrisan, dogodila se greska!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Plan akcije je uspjesno obrisan!');</script>";
            }
            return RedirectToAction("Index");
        }
    }
}