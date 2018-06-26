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
    public class ZadaciController : Controller
    {
        private Zadaci_DBHandle zadaci = new Zadaci_DBHandle();
        int Page_No_Master = 1;
        // GET: Zadaci
        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled zadataka";
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
                    var Popis2 = zadaci.ReadZadaci().ToPagedList(No_Of_Page, Size_Of_Page);
                    return PartialView("_GradView", Popis2);
                }
                Page_No_Master = No_Of_Page;
                var Popis = zadaci.ReadZadaci().ToPagedList(No_Of_Page, Size_Of_Page);
                return View(Popis);
            }
            else
            {
                Page_No_Master = No_Of_Page;
                var Popis = zadaci.ReadZadaci(Search).ToPagedList(No_Of_Page, Size_Of_Page);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_GradView", Popis);
                }

                return View(Popis);
            }
        }

        public ActionResult NoviZadatak()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("NoviZadatak");
            }
            return View("NoviZadatak");
        }

        [HttpPost]
        public ActionResult NoviZadatak(Zadaci zadatak)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            //Subjekti subj = new Subjekti();
            //mjesecni_plan.ID_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            //mjesecni_plan.Ak_godina = _mjesecni_plan.Ak_godina;
            //mjesecni_plan.Naziv = _mjesecni_plan.Naziv;
            //mjesecni_plan.Opis = _mjesecni_plan.Opis;
            if (zadaci.CreateZadaci(zadatak))
            {
                TempData["alert"] = "<script>alert('Novi zadatak je uspjesno spremljen!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Novi zadatak nije spremljen');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Zadaci zadatak = new Zadaci();
			zadatak = zadaci.ReadZadaci(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", zadatak);
            }
            return View("Uredi", zadatak);
        }

        [HttpPost]
        public ActionResult Edit(Zadaci zadatak)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!zadaci.UpdateZadaci(zadatak))
            {
                TempData["alert"] = "<script>alert('Zadatak nije promjenjen!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Zadatak je uspjesno promjenjen!');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Zadaci zadatak = new Zadaci();
            zadatak = zadaci.ReadZadaci(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", zadatak);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(Zadaci zadatak)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!zadaci.DeleteZadaci(zadatak.ID_zadatak))
            {
                TempData["alert"] = "<script>alert('Zadatak nije obrisan, dogodila se greska!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Zadatak je uspjesno obrisan!');</script>";
            }
            return RedirectToAction("Index");
        }
    }
}