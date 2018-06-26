
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
    public class AktivnostiController : Controller
    {
        private Aktivnost_DBHandle aktivnosti = new Aktivnost_DBHandle();
        int Page_No_Master = 1;
        // GET: Aktivnosti
        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled aktivnosti";
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
                    var Popis2 = aktivnosti.ReadAktivnost().ToPagedList(No_Of_Page, Size_Of_Page);
                    return PartialView("_GradView", Popis2);
                }
                Page_No_Master = No_Of_Page;
                var Popis = aktivnosti.ReadAktivnost().ToPagedList(No_Of_Page, Size_Of_Page);
                return View(Popis);
            }
            else
            {
                Page_No_Master = No_Of_Page;
                var Popis = aktivnosti.ReadAktivnost(Search).ToPagedList(No_Of_Page, Size_Of_Page);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_GradView", Popis);
                }

                return View(Popis);
            }
        }

        public ActionResult NovaAktivnost()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("NovaAktivnost");
            }
            return View("NovaAktivnost");
        }

        [HttpPost]
        public ActionResult NovaAktivnost(Aktivnost aktivnost)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            //Aktivnost akt = new Aktivnost();
            //mjesecni_plan.ID_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            //mjesecni_plan.Ak_godina = _mjesecni_plan.Ak_godina;
            //mjesecni_plan.Naziv = _mjesecni_plan.Naziv;
            //mjesecni_plan.Opis = _mjesecni_plan.Opis;
            if (aktivnosti.CreateAktivnost(aktivnost))
            {
                TempData["alert"] = "<script>alert('Nova akitvnost je uspjesno spremljena!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Nova aktivnost nije spremljena');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Aktivnost aktivnost = new Aktivnost();
            aktivnost = aktivnosti.ReadAktivnost(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", aktivnost);
            }
            return View("Uredi", aktivnost);
        }

        [HttpPost]
        public ActionResult Edit(Aktivnost aktivnost)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!aktivnosti.UpdateAktivnost(aktivnost))
            {
                TempData["alert"] = "<script>alert('Aktivnost nije promjenjena!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Aktivnost je uspjesno promjenjena!');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Aktivnost aktivnost = new Aktivnost();
            aktivnost = aktivnosti.ReadAktivnost(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", aktivnost);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(Aktivnost aktivnost)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!aktivnosti.DeleteAktivnost(aktivnost.Id_aktivnost))
            {
                TempData["alert"] = "<script>alert('Aktivnost nije obrisana, dogodila se greska!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Aktivnost je uspjesno obrisana!');</script>";
            }
            return RedirectToAction("Index");
        }
    }
}