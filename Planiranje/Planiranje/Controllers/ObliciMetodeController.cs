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
    public class ObliciMetodeController : Controller
    {
        private Oblici_DBHandle oblici = new Oblici_DBHandle();
        int Page_No_Master = 1;
        // GET: ObliciMetode
        public ActionResult Index(string Sort, string Search, string Filter, int? Page_No)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            ViewBag.Title = "Pregled oblika i metoda";
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
                    var Popis2 = oblici.ReadOblici().ToPagedList(No_Of_Page, Size_Of_Page);
                    return PartialView("_GradView", Popis2);
                }
                Page_No_Master = No_Of_Page;
                var Popis = oblici.ReadOblici().ToPagedList(No_Of_Page, Size_Of_Page);
                return View(Popis);
            }
            else
            {
                Page_No_Master = No_Of_Page;
                var Popis = oblici.ReadOblici(Search).ToPagedList(No_Of_Page, Size_Of_Page);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_GradView", Popis);
                }

                return View(Popis);
            }
        }

        public ActionResult NoviPlan()
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0 || !Request.IsAjaxRequest())
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
        public ActionResult NoviPlan(Oblici oblik)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            //Mjesecni_plan mjesecni_plan = new Mjesecni_plan();
            //mjesecni_plan.ID_pedagog = PlaniranjeSession.Trenutni.PedagogId;
            //mjesecni_plan.Ak_godina = _mjesecni_plan.Ak_godina;
            //mjesecni_plan.Naziv = _mjesecni_plan.Naziv;
            //mjesecni_plan.Opis = _mjesecni_plan.Opis;
            
            if (oblici.CreateOblici(oblik))
            {
                TempData["alert"] = "<script>alert('Novi oblik je uspjesno spremljen!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Novi oblik nije spremljen');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Oblici oblik = new Oblici();
            oblik = oblici.ReadOblici(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Uredi", oblik);
            }
            return View("Uredi", oblik);
        }

        [HttpPost]
        public ActionResult Edit(Oblici oblik)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!oblici.UpdateOblici(oblik))
            {
                TempData["alert"] = "<script>alert('Oblik nije promjenjen!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert('Oblik je uspjesno promjenjen!');</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            Oblici oblik = new Oblici();
            oblik = oblici.ReadOblici(id);
            if (Request.IsAjaxRequest())
            {
                ViewBag.IsUpdate = false;
                return View("Obrisi", oblik);
            }
            return View("Obrisi");
        }

        [HttpPost]
        public ActionResult Delete(Oblici oblik)
        {
            if (PlaniranjeSession.Trenutni.PedagogId <= 0)
            {
                return RedirectToAction("Index", "Planiranje");
            }
            if (!oblici.DeleteOblici(oblik.Id_oblici))
            {
                TempData["alert"] = "<script>alert('Oblik nije obrisan, dogodila se greska!');</script>";
            }
            else
            {
                TempData["alert"] = "<script>alert(Oblik je uspjesno obrisan!');</script>";
            }
            return RedirectToAction("Index");
        }
    }
}